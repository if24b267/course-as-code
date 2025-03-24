using CourseAsCode.Readers.Tests.Utils;

namespace CourseAsCode.Readers.Tests
{
	[TestFixture]
	public abstract class TestBase
	{
		protected string TestDataPath { get; private set; }
		protected string TempDirectory { get; private set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			TestDataPath = TestUtils.GetTestDataPath();
		}

		[SetUp]
		public void SetUp()
		{
			TempDirectory = TestUtils.CreateTempTestDirectory();
			TestUtils.ResetValueKeeperState();
		}

		[TearDown]
		public void TearDown()
		{
			TestUtils.CleanupTempDirectory(TempDirectory);
		}

		protected string CreateTestYamlFile(string fileName, string content)
		{
			return TestUtils.CreateYamlFile(TempDirectory, fileName, content);
		}

		protected string CreateTestDirectory(string directoryName)
		{
			var directory = Path.Combine(TempDirectory, directoryName);
			Directory.CreateDirectory(directory);
			return directory;
		}
	}
}