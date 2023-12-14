namespace LogFileAnalyzer.Models.Interfaces
{
    // Общий интерфейс для категорий ошибок category и видов логгирования severity
    // Подсчет количества записей, вычислений процента для каждой из записи
    internal interface IUnifySeverityCategory
    {
        string GetStats();
        void SetStats(ref string fileContent);
    }
}
