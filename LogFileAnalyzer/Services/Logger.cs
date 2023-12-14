using LogFileAnalyzer.Services.Interfaces;

namespace LogFileAnalyzer.Services
{
    class Logger : ILogger
    {
        private static string LogFilePath => $"{Environment.CurrentDirectory}/logs.log";

        public void Log(string message)
        {
            var sw = new StreamWriter(LogFilePath, true);
            sw.WriteLine($"{DateTime.Now} | {message}");
            sw.Close();
        }
    }
}
