using LogFileAnalyzer.Models.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace LogFileAnalyzer.Models
{
    public class Severities : IUnifySeverityCategory
    {
        private Dictionary<string, int> RecordsCount { get; set; }
        public Severities()
        {
            RecordsCount = new Dictionary<string, int>()
            {
                { "Trace", 0 },
                { "Debug", 0 },
                { "Information", 0 },
                { "Warning", 0 },
                { "Error", 0 },
                { "Critical", 0 }
            };
        }
        public string GetStats()
        {
            var sb = new StringBuilder();
            var totalRecords = RecordsCount.Sum(x => x.Value);

            sb.AppendLine("SEVERITY STATS:");
            foreach (var record in RecordsCount)
            {
                sb.AppendLine($"{record.Key}: {record.Value}\t{Math.Round(record.Value * 100.0 / totalRecords, 2)}%");
            }
            return sb.ToString();
        }

        public void SetStats(ref string fileContent)
        {
            RecordsCount["Trace"] += Regex.Matches(fileContent, RegexPatterns.TracePattern).Count;
            RecordsCount["Debug"] += Regex.Matches(fileContent, RegexPatterns.DebugPattern).Count;
            RecordsCount["Information"] += Regex.Matches(fileContent, RegexPatterns.InformationPattern).Count;
            RecordsCount["Warning"] += Regex.Matches(fileContent, RegexPatterns.WarningPattern).Count;
            RecordsCount["Error"] += Regex.Matches(fileContent, RegexPatterns.ErrorPattern).Count;
            RecordsCount["Critical"] += Regex.Matches(fileContent, RegexPatterns.CriticalPattern).Count;
        }
    }
}
