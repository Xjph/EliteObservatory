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
                return false;
            }

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sock.Connect(overlayEndPoint);
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                Console.WriteLine("Socket connected to {0}",
                    overlayEndPoint.ToString());

                result = true;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
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
                    Console.WriteLine(e.ToString());
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
                    Console.WriteLine(headerData.ToString(Formatting.None));
                    Console.WriteLine(bodyData.ToString(Formatting.None));
                    int i = sock.Send(System.Text.Encoding.UTF8.GetBytes(headerData.ToString(Formatting.None) + '\n'));
                    Console.WriteLine("Sent {0} bytes.", i);
                    i = sock.Send(System.Text.Encoding.UTF8.GetBytes(bodyData.ToString(Formatting.None)+ '\n'));
                    Console.WriteLine("Sent {0} bytes.", i);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    socketReady = false;
                }
            }

            return socketReady;
        }
    }
}
