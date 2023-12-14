namespace LogFileAnalyzer.Services
{
    class RegexPatterns
    {
        // Паттерн для поиска дат в .log файле в формате dd.MM.yyyy HH:mm:ss.fff
        public readonly static string DatePattern = @"\[(\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d{3})\]";

        // Паттерны для поиска подстрок '[Severity]' в .log файлах
        public readonly static string TracePattern = @"\[Trace\]";
        public readonly static string DebugPattern = @"\[Debug\]";
        public readonly static string InformationPattern = @"\[Information\]";
        public readonly static string WarningPattern = @"\[Warning\]";
        public readonly static string ErrorPattern = @"\[Error\]";
        public readonly static string CriticalPattern = @"\[Critical\]";

        // Паттерн для взятия содержимого из третьей пары скобок [ ], где хранится название ошибки
        //public readonly static string ExceptionTypePattern = @"\[[^\]]*\].*\[[^\]]*\].*\[(.*?)\]";
        public readonly static string ExceptionTypePattern = @"\[[^\]]*\][^\[]*\[[^\]]*\][^\[]*\[(.*?)\]";
    }
}
