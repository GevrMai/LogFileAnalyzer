// Яценко Александр Владимирович
// alexander.iatsenko@gmail.com

using LogFileAnalyzer.Models;
using LogFileAnalyzer.Models.Interfaces;

namespace LogFileAnalyzer
{
    internal class Program
    {
        static void Main()
        {
            IAnalyzer analyzer = new Analyzer();
            analyzer.RunAnalyzer();
        }
    }
}