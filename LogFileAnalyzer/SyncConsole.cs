namespace LogFileAnalyzer
{
    class SyncConsole
    {
        private static readonly object syncLock = new();
        public static void WriteLine(string? message = null)
        {
            //lock(syncLock)
            //{
                Console.WriteLine(message);
            //}
        }
    }
}
