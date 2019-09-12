using System.Threading.Tasks;
using System.IO;

namespace Observatory
{
    class JournalPoker
    {
        private bool Running = false;
        private DirectoryInfo DirectoryInfo;

        public JournalPoker(string dir)
        {
            DirectoryInfo = new DirectoryInfo(dir);
        }

        public async void Start()
        {
            Running = true;
            await Task.Run(() =>
            {
                while (Running)
                {
                    FileStream stream;
                    FileInfo fileToPoke = null;

                    foreach (var file in DirectoryInfo.GetFiles("Journal.????????????.??.log"))
                    {
                        if (fileToPoke == null || string.Compare(file.Name, fileToPoke.Name) > 0)
                        {
                            fileToPoke = file;
                        }
                    }

                    stream = fileToPoke.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    stream.Close();
                    System.Threading.Thread.Sleep(250);
                }
            });
        }

        public void Stop()
        {
            Running = false;
        }
    }
}
