using LogFileAnalyzer.Models.Interfaces;

namespace LogFileAnalyzer.Models
{
    class Logger : ILogger
    {
        private string LogFilePath => $"{Environment.CurrentDirectory}/logs.log";

        public void Log(string message)
        {
            var sw = new StreamWriter(LogFilePath, true);
            sw.WriteLine($"{DateTime.Now} | {message}");
            sw.Close();
        }
    }
}
