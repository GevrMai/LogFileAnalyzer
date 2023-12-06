using LogFileAnalyzer.Models.Interfaces;

namespace LogFileAnalyzer.Models
{
    public class ServicesSearcher
    {
        private static readonly ILogger logger = new Logger();


        // Метод возвращает названия сервисов, для которых нужно сделать отчет (например, если первым параметром в качестве имени сервиса подается 'ser',
        // то подходящими сервисами для составления отчета будут 'service' и 'serv')
        public static HashSet<string>? GetAllSuitableServices(string searchedService, string path)
        {
            try
            {
                string directoryPath = $"{path}\\";
                string partialFileName = searchedService;

                var directory = new DirectoryInfo(directoryPath);
                var files = directory.GetFiles($"*{partialFileName}*");

                var suitableServices = new HashSet<string>();

                foreach (var file in files)
                {
                    var logFileName = file.FullName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                    var serviceName = logFileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).First();

                    suitableServices.Add(serviceName);
                }

                if (suitableServices.Count == 0)
                    throw new Exception($"В директории {path} нет сервисов, содержащих '{searchedService}' в названии");

                return suitableServices;
            }
            catch(DirectoryNotFoundException ex)
            {
                SyncConsole.WriteLine($"{ex.Message}\n");
                logger.Log(ex.Message);
            }
            catch(Exception ex)
            {
                SyncConsole.WriteLine($"{ex.Message}\n");
                logger.Log(ex.Message);
            }
            return null;
        }
    }
}
