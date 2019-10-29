using System.Threading.Tasks;
using System.IO;

namespace Observatory
{
    // Some explanation is probably in order here, since this on it own probably doesn't appear to do anything.
    // The journal files don't reliably fire the filesystem created/changed events when Elite Dangerous writes to them.
    // This will poke the most recent file in the journal folder once every 250ms by opening it and immediately closing
    // it again, forcing the filesystem to commit whatever changes are pending, since it looks like something else is
    // about to read the file, and fire the delayed events.

    class JournalPoker
    {
        private bool Running = false;
        private DirectoryInfo directoryInfo;

        public JournalPoker(string dir)
        {
            directoryInfo = new DirectoryInfo(dir);
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

                    foreach (var file in directoryInfo.GetFiles("Journal.????????????.??.log"))
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
