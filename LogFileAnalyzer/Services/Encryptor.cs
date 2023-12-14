using System.Text;

namespace LogFileAnalyzer.Services
{
    internal static class Encryptor
    {
        public static void EncryptMail(ref string fileContent, ref DirectoryInfo directory, string fileName)
        {
            var result = new StringBuilder();
            // каждый отдельный лог
            var splittedString = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in splittedString)
            {
                if (line.Contains('@'))
                    EncryptMailInLine(line, result);
                else
                    result.AppendLine(line);
            }

            FileManager.RewriteFile(ref directory, ref fileName, result);
        }
        private static void EncryptMailInLine(string line, StringBuilder result)
        {
            // разделили по пробелу, далее будем смотреть есть ли в подстроке символ @
            var splittedLine = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var str = splittedLine.Where(x => x.Contains('@')).ToList();
            for (int i = 0; i < str.Count; i++)
            {
                var outputEmail = str[i].ToCharArray();
                for (int j = 0; j < str[i].IndexOf('@'); j += 2)
                {
                    outputEmail[j] = '*';
                }

                line = line.Replace(str[i], new string(outputEmail));
            }

            result.AppendLine(line);
        }
    }
}
