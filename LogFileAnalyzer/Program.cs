// Яценко Александр Владимирович
// alexander.iatsenko@gmail.com

using LogFileAnalyzer.Services;
using LogFileAnalyzer.Services.Interfaces;

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