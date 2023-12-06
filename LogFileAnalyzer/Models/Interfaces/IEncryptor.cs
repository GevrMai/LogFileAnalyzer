namespace LogFileAnalyzer.Models.Interfaces
{
    internal interface IEncryptor
    {
        void EncryptMail(ref string fileContent, ref DirectoryInfo directory, string fileName);
    }
}
