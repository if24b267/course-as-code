using CourseAsCode.Readers.Helper;

namespace CourseAsCode.Readers.Tests.Helper
{
	[TestFixture]
	public class ValueKeeperTests
	{
		[SetUp]
		public void Setup()
		{
			// Reset ValueKeeper state before each test
			ValueKeeper.CoursePath = null;
			ValueKeeper.CurrentActivityId = 0;
			ValueKeeper.CurrentSectionId = 0;
		}

		[Test]
		public void CoursePath_CanSetAndGet()
		{
			// Arrange
			var testPath = "/test/path";

			// Act
			ValueKeeper.CoursePath = testPath;

			// Assert
			Assert.That(ValueKeeper.CoursePath, Is.EqualTo(testPath));
		}

		[Test]
		public void CurrentActivityId_CanSetAndGet()
		{
			// Arrange
			var testId = 42;

			// Act
			ValueKeeper.CurrentActivityId = testId;

			// Assert
			Assert.That(ValueKeeper.CurrentActivityId, Is.EqualTo(testId));
		}

		[Test]
		public void CurrentSectionId_CanSetAndGet()
		{
			// Arrange
			var testId = 42;

			// Act
			ValueKeeper.CurrentSectionId = testId;

			// Assert
			Assert.That(ValueKeeper.CurrentSectionId, Is.EqualTo(testId));
		}

		[Test]
		public void CoursePath_CanSetToNull()
		{
			// Arrange
			ValueKeeper.CoursePath = "/test/path";

			// Act
			ValueKeeper.CoursePath = null;

			// Assert
			Assert.That(ValueKeeper.CoursePath, Is.Null);
		}

		[Test]
		public void IdValues_CanIncrement()
		{
			// Arrange
			ValueKeeper.CurrentActivityId = 5;
			ValueKeeper.CurrentSectionId = 10;

			// Act
			ValueKeeper.CurrentActivityId++;
			ValueKeeper.CurrentSectionId++;

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(ValueKeeper.CurrentActivityId, Is.EqualTo(6));
				Assert.That(ValueKeeper.CurrentSectionId, Is.EqualTo(11));
			});
		}
	}
}