using CourseAsCode.Readers.Helper;

namespace CourseAsCode.Readers.Tests.Utils
{
    public static class TestUtils
    {
        public static string GetTestDataPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        }

        public static string CreateTempTestDirectory()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        public static void CleanupTempDirectory(string tempPath)
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }

        public static void ResetValueKeeperState()
        {
            ValueKeeper.CoursePath = null;
            ValueKeeper.CurrentActivityId = 0;
            ValueKeeper.CurrentSectionId = 0;
        }

        public static string CreateYamlFile(string directoryPath, string fileName, string content)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, content);
            return filePath;
        }
    }
}
