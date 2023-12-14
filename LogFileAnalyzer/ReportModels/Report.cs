using LogFileAnalyzer.Models.Interfaces;
using LogFileAnalyzer.Services;
using LogFileAnalyzer.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace LogFileAnalyzer.Models
{
    public class Report : IReport
    {
        private string ServiceName { get; set; }
        private DateTime FirstRecordDate { get; set; }
        private DateTime LastRecordDate { get; set; }
        private IUnifySeverityCategory SeveritiesStats { get; set; }
        private IUnifySeverityCategory CategoriesStats { get; set; }
        private int RotationsCount { get; set; }

        private readonly string path;
        private readonly ILogger logger;

        public Report(string serviceName, string path)
        {
            ServiceName = serviceName;
            this.path = path;

            SeveritiesStats = new Severities();
            CategoriesStats = new Category();
            logger = new Logger();
        }

        public string MakeReport()
        {
            try
            {
                var directory = new DirectoryInfo(path);
                var files = directory.GetFiles($"*{ServiceName}*");

                foreach (var file in files)
                {
                    var splittedFileName = file.Name.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                    // название сервиса, для которого нужно сделать отчет не совпадает с рассматриваемым сервисом
                    if (ServiceName != splittedFileName.First())
                        continue;

                    var fileContent = FileManager.ReadFileContent(path, file.Name);

                    // Определяем количество ротаций
                    CountRotation(ref splittedFileName[1]);
                    SetFirstLastDates(ref fileContent);
                    CountSeveritiesStats(ref fileContent);
                    CountCategoriesStats(ref fileContent);

                    Encryptor.EncryptMail(ref fileContent, ref directory, file.Name);
                }

                return new StringBuilder().AppendLine($"Отчет о сервисе '{ServiceName} по пути '{path}':")
                    .AppendLine($"Самая ранняя запись\t{FirstRecordDate}")
                    .AppendLine($"Самая последняя запись\t{LastRecordDate}")
                    .AppendLine($"Количество ротаций\t{RotationsCount}\n")
                    .AppendLine(SeveritiesStats.GetStats())
                    .AppendLine(CategoriesStats.GetStats()).ToString();
            }
            catch (InvalidOperationException ex)
            {
                logger.Log($"{ex.Message}, файл с логами не содержал необходимых данных");
                return $"При составлении отчета по сервису '{ServiceName}' возникла ошибка.";
            }
        }

        // Подсчет количества записей по типу ошибки
        private void CountCategoriesStats(ref string fileContent)
        {
            var splittedString = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in splittedString)
            {
                var match = Regex.Match(line, RegexPatterns.ExceptionTypePattern);

                if (match.Success)
                {
                    var exceptionType = match.Groups[1].Value;

                    CategoriesStats.SetStats(ref exceptionType);
                }
            }
        }

        // Подсчет количества записей по уровням логирования
        private void CountSeveritiesStats(ref string fileContent)
        {
            SeveritiesStats.SetStats(ref fileContent);
        }

        // По регулярному выражению определяем все даты в файле, устанавливаем first и last dates, если значения
        // больше/меньше текущих
        private void SetFirstLastDates(ref string fileContent)
        {
            MatchCollection matches = Regex.Matches(fileContent, RegexPatterns.DatePattern);

            // Самая первая запись в файле - самая ранняя
            // Последняя - самая свежая

            var firstDateInFileString = matches.First().Groups[1].Value;
            var firstDateInFile = DateTime.ParseExact(firstDateInFileString, "dd.MM.yyyy HH:mm:ss.fff", null);
            if (FirstRecordDate == DateTime.MinValue || FirstRecordDate > firstDateInFile)
                FirstRecordDate = firstDateInFile;

            var lastDateInFileString = matches.Last().Groups[1].Value;
            var lastDateInFile = DateTime.ParseExact(lastDateInFileString, "dd.MM.yyyy HH:mm:ss.fff", null);
            if (LastRecordDate == DateTime.MinValue || LastRecordDate < lastDateInFile)
                LastRecordDate = lastDateInFile;
        }

        private void CountRotation(ref string strToCheck)
        {
            if (int.TryParse(strToCheck, out int rotationNum))
                RotationsCount = Math.Max(RotationsCount, rotationNum);
        }
    }
}
