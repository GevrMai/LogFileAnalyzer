using LogFileAnalyzer.Services;

namespace LogFileAnalyzerTests.Encryptor_Tests
{
    [TestFixture]
    internal class EncryptorTests
    {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void FileWithNoEmailAdresses_NoChangesExpected()
        {
            var sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\FileToTestNoEmailAdresses.txt");
            var fileContent = sr.ReadToEnd();
            sr.Close();
            var directoryInfo = new DirectoryInfo("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files");
            
            Encryptor.EncryptMail(ref fileContent, ref directoryInfo, "FileToTestNoEmailAdresses.txt");

            sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\ExpectedFileNoEmailAdresses.txt");
            var expectedFileContent = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\FileToTestNoEmailAdresses.txt");
            var finalFileContent = sr.ReadToEnd();
            sr.Close();

            if (finalFileContent[finalFileContent.Length - 1] == '\r')
                finalFileContent = finalFileContent.Remove(finalFileContent.Length - 1);

            Assert.That(finalFileContent, Is.EqualTo(expectedFileContent));
        }
        [Test]
        public void FileWithEmailAdresses_ChangesExpected()
        {
            var sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\FileToTestWithEmailAdresses.txt");
            var fileContent = sr.ReadToEnd();
            sr.Close();
            var directoryInfo = new DirectoryInfo("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files");

            Encryptor.EncryptMail(ref fileContent, ref directoryInfo, "FileToTestWithEmailAdresses.txt");

            sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\ExpectedFileWithEmailAdresses.txt");
            var expectedFileContent = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\FileToTestWithEmailAdresses.txt");
            var finalFileContent = sr.ReadToEnd();
            sr.Close();

            if (finalFileContent[finalFileContent.Length - 1] == '\r')
                finalFileContent = finalFileContent.Remove(finalFileContent.Length - 1);

            Assert.That(finalFileContent, Is.EqualTo(expectedFileContent));

            // Восстановление содержимого тестового файла
            var sw = new StreamWriter("C:\\prog\\LogFileAnalyzer\\LogFileAnalyzerTests\\Encryptor_Tests\\Files\\FileToTestWithEmailAdresses.txt");
            sw.Write(fileContent);
            sw.Close();
        }
    }
}
