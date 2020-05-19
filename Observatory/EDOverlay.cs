using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Observatory
{
    public class EDOverlay
    {
        private readonly Properties.Observatory settings = Properties.Observatory.Default;

        private Socket sock;
        private string headerId = "obsHeader";
        private string bodyId = "obsBody";

        private IPEndPoint GetOverlayEndPoint()
        {
            IPEndPoint result = null;
            Process[] overlay = Process.GetProcessesByName("EDMCOverlay");
            if (overlay.Length == 0 )
            {
                return result;
            }

            foreach (TcpRow tcpRow in ManagedIpHelper.GetExtendedTcpTable(true))
            {
                foreach (Process overlayProc in overlay)
                {
                    if (overlayProc.Id == tcpRow.ProcessId)
                    {
                        result = tcpRow.LocalEndPoint;
                        break;
                    }
                }

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        private bool ConnectOverlay()
        {
            bool result = false;
            IPEndPoint overlayEndPoint = GetOverlayEndPoint();
            if (overlayEndPoint == null)
            {
                Log("EDOverlay plugin not listening. Aborting.");
                return false;
            }

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sock.Connect(overlayEndPoint);
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                Log($"Socket connected to {overlayEndPoint}");

                result = true;
            }
            catch (ArgumentNullException ane)
            {
                Log($"ArgumentNullException : {ane}");
            }
            catch (SocketException se)
            {
                Log($"SocketException : {se}");
            }
            catch (Exception e)
            {
                Log($"Unexpected exception : {e}");
            }

            return result;
        }

        public void Close() {
            if (sock != null)
            {
                try
                {
                    sock.Close();
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                }
                sock = null;
            }
        }

        public bool Send(string bodyName, string announceText)
        {
            bool socketReady = true;
            if (sock == null || !sock.Connected)
            {
                socketReady = ConnectOverlay();
            }

            if (socketReady)
            {
                JObject headerData = JObject.FromObject(
                    new
                    {
                        id = headerId,
                        text = bodyName,
                        size = "large",
                        color = settings.EDOHeaderColor,
                        x = settings.EDONotificationX,
                        y = settings.EDONotificationY,
                        ttl = settings.EDONotificationTimeout + 1
                    }); ;
                JObject bodyData = JObject.FromObject(
                    new
                    {
                        id = bodyId,
                        text = announceText,
                        size = "normal",
                        color = settings.EDOBodyColor,
                        x = settings.EDONotificationX,
                        y = settings.EDONotificationY + 30,
                        ttl = settings.EDONotificationTimeout,
                    });
                try
                {
                    Log(headerData.ToString(Formatting.None));
                    Log(bodyData.ToString(Formatting.None));
                    int i = sock.Send(System.Text.Encoding.UTF8.GetBytes(headerData.ToString(Formatting.None) + '\n'));
                    Log($"Sent {i} bytes.");
                    i = sock.Send(System.Text.Encoding.UTF8.GetBytes(bodyData.ToString(Formatting.None)+ '\n'));
                    Log($"Sent {i} bytes.");
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    socketReady = false;
                }
            }

            return socketReady;
        }

        private void Log(string text)
        {
            if (settings.EDODebug)
            {
                Observatory.Log("Overlay> " + text);
            }
        }
    }
}
