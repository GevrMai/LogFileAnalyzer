using LogFileAnalyzer.Models.Interfaces;
using System.Collections.Concurrent;
using System.Text;

namespace LogFileAnalyzer.Models
{
    internal class Analyzer : IAnalyzer
    {
        // guid запроса, его статус
        private readonly dynamic requestGuids = new ConcurrentDictionary<Guid, string>();

        public void RunAnalyzer()
        {
            Console.WriteLine("Введите название искомого сервиса и путь до директории с .log файлами");
            Console.WriteLine("Пример ввода:\tservice C:\\prog\\LogFileAnalyzer\\LogFileAnalyzer\\bin\\Debug\\net7.0\\analyzer_tests");
            Console.WriteLine("Ввод может подаваться неограниченное число раз, для выхода напишите 'exit'");
            Console.WriteLine("Запросу будет выдан guid. Можно ввести его в консоль - покажется статус запроса");
            Console.WriteLine("Команда 'Status' покажет guid всех запросов и их статусы'\n");


            var userInput = new string[] { "" };
            while (userInput[0] != "exit")
            {
                userInput = (Console.ReadLine() ?? "").Split();
                // проверка ввода
                if (!AreArgsCorrect(ref userInput))
                {
                    continue;
                }

                var currentGuid = GenerateGuid();
                
                var userInputData = new DirectoryPathAndService(userInput[0], userInput[1]);    // сохраняем название сервиса, путь запроса

                // поиск сервисов, для которых необходимо сделать отчет
                var servicesToAnalyze = ServicesSearcher.GetAllSuitableServices(userInputData.Service, userInputData.Path);
                if (servicesToAnalyze is null)
                {
                    requestGuids[currentGuid] = "Не выполнен, возникла ошибка";
                    continue;
                }

                MakeReports(servicesToAnalyze, userInputData, currentGuid);
            }
        }

        private void MakeReports(HashSet<string> servicesToAnalyze, DirectoryPathAndService userInputData, Guid currentGuid)
        {
            // делаем столько отчетов, сколько нашли подходящих сервисов
            var requestTask = new Task(() =>
            {
                // отчет по запросу, который запишем в файл
                var resultReport = new StringBuilder();
                foreach (var serviceName in servicesToAnalyze)
                {
                    var task = new Task(() =>
                    {
                        var report = new Report(serviceName, userInputData.Path);
                        resultReport.AppendLine(report.MakeReport());
                        requestGuids[currentGuid] = "Выполнен";
                    });
                    task.Start();
                    task.Wait();
                }
                SyncConsole.WriteLine(resultReport.ToString());
                WriteReportToFile(resultReport, currentGuid);
            });
            requestTask.Start();
        }
        private static void WriteReportToFile(StringBuilder reportText, Guid currentGuid)
        {
            using StreamWriter writer = new ($"{Environment.CurrentDirectory}\\Reports\\Report {currentGuid}.txt");
                writer.Write(reportText.ToString());
        }

        private Guid GenerateGuid()
        {
            var currentGuid = Guid.NewGuid();       // даем запросу свой guid
            requestGuids.TryAdd(currentGuid, "Не выполнен");
            SyncConsole.WriteLine($"Запросу выдан guid:\t{currentGuid}\n");

            return currentGuid;
        }

        // Метод проверяет Введенные пользователем параметры 
        private bool AreArgsCorrect(ref string[] userInput)
        {
            if (userInput.Length > 2)               // неверное количество аргументов
            {
                SyncConsole.WriteLine("Введено слишком много аргументов");
                return false;
            }

            if (userInput.Length == 1 && Guid.TryParse(userInput[0], out Guid parsedGuid))    // пользователем введен guid для уточнения статуса запроса
            {
                if (requestGuids.ContainsKey(parsedGuid))
                    SyncConsole.WriteLine($"Статус запроса: {requestGuids[parsedGuid]}\nВведите следующую инструкцию:\n");
                else
                    SyncConsole.WriteLine($"Запроса {parsedGuid} не существует");

                return false;
            }
            else if (userInput.Length == 1 && userInput[0] == "status")                // запрос на отслеживание всех статусов
            {
                foreach (var request in requestGuids)
                {
                    SyncConsole.WriteLine($"guid:\t{request.Key}, status:\t{request.Value}");
                }
                SyncConsole.WriteLine();
                return false;
            }
            else if (userInput.Length == 1 && userInput[0] != "exit")                // запрос пользователя не может состоять из 1 аргумента, кроме запроса guid
            {
                SyncConsole.WriteLine("Введено некорректное значение");
                return false;
            }

            return true;
        }
    }
}
