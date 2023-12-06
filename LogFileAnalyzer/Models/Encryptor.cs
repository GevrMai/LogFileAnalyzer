using LogFileAnalyzer.Models.Interfaces;
using System.Text;

namespace LogFileAnalyzer.Models
{
    internal class Encryptor : IEncryptor
    {
        private readonly object _lock = new object();
        public void EncryptMail(ref string fileContent, ref DirectoryInfo directory, string fileName)
        {
            // каждая отдельная запись
            var splittedString = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var result = new StringBuilder();
            foreach (var line in splittedString)
            {
                if (line.Contains('@'))
                    EncryptMailInLine(line, result);
                else
                    result.AppendLine(line);
            }

            RewriteFile(ref directory, ref fileName, result);
        }
        private void EncryptMailInLine(string line, StringBuilder result)
        {
            // разделили по пробелу, далее будем смотреть есть ли в подстроке символ @
            var splittedLine = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splittedLine.Length; i++)
            {
                if (splittedLine[i].Contains('@'))
                {
                    int atSymbolIndex = splittedLine[i].IndexOf('@');

                    for (int j = 0; j < atSymbolIndex; j += 2)          // заменяем каждые 2 символа в строке
                    {
                        splittedLine[i] = splittedLine[i].Replace(splittedLine[i][j], '*');
                    }
                }
                result.Append($"{splittedLine[i]} ");       // склеиваем подстроки обратно
            }

            result.AppendLine();
        }
        private void RewriteFile(ref DirectoryInfo directory, ref string fileName, StringBuilder result)
        {
            // перезаписываем файл
            lock (_lock)
            {
                using StreamWriter writer = new($"{directory}\\{fileName}");
                writer.Write(result.ToString());
            }
        }
    }
}
