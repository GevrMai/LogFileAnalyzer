using LogFileAnalyzer.Models.Interfaces;
using System.Text;

namespace LogFileAnalyzer.Models
{
    class Category : IUnifySeverityCategory
    {
        private Dictionary<string, int> RecordsCount { get; set; }
        public Category()
        {
            RecordsCount = new Dictionary<string, int>();
        }
        public string GetStats()
        {
            var sb = new StringBuilder();
            var totalRecords = RecordsCount.Sum(x => x.Value);

            sb.AppendLine("CATEGORY STATS:");
            foreach (var record in RecordsCount)
            {
                sb.AppendLine($"{record.Key}: {record.Value}\t{Math.Round(record.Value * 100.0 / totalRecords, 2)}%");
            }
            return sb.ToString();
        }

        public void SetStats(ref string exceptionType)
        {
            if (!RecordsCount.ContainsKey(exceptionType))
                RecordsCount.Add(exceptionType, 1);
            else
                RecordsCount[exceptionType]++;
        }
    }
}
