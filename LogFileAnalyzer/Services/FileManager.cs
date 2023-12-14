using System.Text;

namespace LogFileAnalyzer.Services
{
    internal class FileManager
    {
        private static readonly object _lock = new();

        public static void RewriteFile(ref DirectoryInfo directory, ref string fileName, StringBuilder result)
        {
            lock (_lock)
            {
                using StreamWriter writer = new($"{directory}\\{fileName}");
                writer.Write(result.ToString());
            }
        }
        public static string ReadFileContent(string path, string fileName)
        {
            lock (_lock)
            {
                return File.ReadAllText($"{path}\\{fileName}");
            }
        }
        public static void WriteReportToFile(StringBuilder reportText, Guid currentGuid)
        {
            // синхронизация не нужна, так как на 1 запрос создается 1 файл (название уникальное)
            using StreamWriter writer = new($"{Environment.CurrentDirectory}\\Reports\\Report {currentGuid}.txt");
            writer.Write(reportText.ToString());
        }
    }
}
