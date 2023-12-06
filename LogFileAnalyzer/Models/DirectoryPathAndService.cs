namespace LogFileAnalyzer.Models
{
    class DirectoryPathAndService
    {
        public readonly string Service;
        public readonly string Path;

        public DirectoryPathAndService(string service, string path)
        {
            Service = service;
            Path = path;
        }
    }
}
