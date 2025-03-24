using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.AbstractClasses;

namespace CourseAsCode.Readers.Tests.AbstractClasses
{
	[TestFixture]
	public class AbstractReaderTests
	{
		[Test]
		public void ActivityReader_ImplementsIModelObjectReader()
		{
			// Act & Assert
			Assert.That(typeof(ActivityReader<Activity>).GetInterfaces()
				 .Any(i => i.Name.Contains("IModelObjectReader")), Is.True);
		}

		[Test]
		public void SectionReader_ImplementsIModelObjectReader()
		{
			// Act & Assert
			Assert.That(typeof(SectionReader<Section>).GetInterfaces()
				 .Any(i => i.Name.Contains("IModelObjectReader")), Is.True);
		}

		[Test]
		public void ConcreteActivityReader_CallsOverriddenReadMethod()
		{
			// Arrange
			var reader = new TestActivityReader();
			var path = "test/path";

			// Act
			var result = reader.Read(path);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test Activity"));
				Assert.That(reader.CalledPath, Is.EqualTo(path));
			});
		}

		[Test]
		public void ConcreteSectionReader_CallsOverriddenReadMethod()
		{
			// Arrange
			var reader = new TestSectionReader();
			var path = "test/path";

			// Act
			var result = reader.Read(path);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test Section"));
				Assert.That(reader.CalledPath, Is.EqualTo(path));
			});
		}

		// Test implementation of ActivityReader
		private class TestActivityReader : ActivityReader<TestActivity>
		{
			public string CalledPath { get; private set; }

			public override TestActivity Read(string modelObjectPath)
			{
				CalledPath = modelObjectPath;
				return new TestActivity { Name = "Test Activity" };
			}
		}

		// Test implementation of SectionReader
		private class TestSectionReader : SectionReader<Section>
		{
			public string CalledPath { get; private set; }

			public override Section Read(string modelObjectPath)
			{
				CalledPath = modelObjectPath;
				return new Section { Name = "Test Section" };
			}
		}

		// Test activity class
		private class TestActivity : Activity
		{
			public TestActivity()
			{
				Name = "Default Test Activity";
			}
		}
	}
}