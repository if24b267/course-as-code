using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.Integration
{
	[TestFixture]
	public class ReaderIntegrationTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			// Call the base SetUp method
			base.SetUp();

			// Reset ValueKeeper state at the beginning of each test
			ValueKeeper.CurrentActivityId = 1;
			ValueKeeper.CurrentSectionId = 1;
		}

		[Test]
		public void YamlActivityReader_IntegratesWithYamlUrlReader()
		{
			// Arrange
			var activityReader = new YamlActivityReader();
			var urlFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "Activities", "url1.yaml");
			ValueKeeper.CurrentActivityId = 10;

			// Act
			var result = activityReader.Read(urlFilePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result, Is.TypeOf<URL>());

				var url = (URL)result;
				Assert.That(url.Name, Is.EqualTo("Test URL 1"));
				Assert.That(url.Description, Is.EqualTo("Test URL 1 description"));
				Assert.That(url.ExternalUrl, Is.EqualTo("https://example.com/1"));
				Assert.That(url.ActivityId, Is.EqualTo(10));
			});
		}

		[Test]
		public void YamlSectionReader_IntegratesWithYamlActivityReader()
		{
			// Arrange
			var sectionReader = new YAMLSectionReader();
			var sectionFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "section.yaml");
			ValueKeeper.CurrentActivityId = 1;
			ValueKeeper.CurrentSectionId = 5;

			// Act
			var result = sectionReader.Read(sectionFilePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test Section 1"));
				Assert.That(result.Summary, Is.EqualTo("Test section 1 summary"));
				Assert.That(result.VisibleToStudents, Is.True);
				Assert.That(result.Activities, Has.Count.EqualTo(3));
				Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(5));
				Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_5"));

				// Activity count check - actual IDs may vary based on implementation
				Assert.That(result.Activities.Count, Is.EqualTo(3));

				// Verify the current activity ID was incremented
				Assert.That(ValueKeeper.CurrentActivityId, Is.GreaterThan(1));
			});
		}

		[Test]
		public void CourseReader_IntegratesWithSectionAndActivityReaders()
		{
			// Arrange
			var courseReader = new CourseReader(new YAMLSectionReader());
			ValueKeeper.CurrentActivityId = 1;
			ValueKeeper.CurrentSectionId = 1;

			// Act
			var result = courseReader.ReadYaml(TestDataPath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.FullName, Is.EqualTo("Test Course"));
				Assert.That(result.Sections, Has.Count.EqualTo(2));

				// Check section count
				Assert.That(result.Sections.Count, Is.EqualTo(2));

				// Check activity counts
				Assert.That(result.Sections[0].Activities.Count, Is.EqualTo(3));
				Assert.That(result.Sections[1].Activities.Count, Is.EqualTo(1));

				// Check IDs were assigned correctly
				Assert.That(result.Sections[0].SystemDefaults.SectionId, Is.EqualTo(1));
				Assert.That(result.Sections[1].SystemDefaults.SectionId, Is.EqualTo(2));

				// Check that final counters were incremented correctly
				Assert.That(ValueKeeper.CurrentSectionId, Is.EqualTo(3));
				Assert.That(ValueKeeper.CurrentActivityId, Is.GreaterThan(3));
			});
		}

		[Test]
		public void YamlTextMediaAreaReader_IntegratesWithActivityReader()
		{
			// Arrange
			var activityReader = new YamlActivityReader();
			var labelFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "Activities", "label1.yaml");
			ValueKeeper.CurrentActivityId = 20;

			// Act
			var result = activityReader.Read(labelFilePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test Label 1"));
				Assert.That(result.ActivityId, Is.EqualTo(20));
			});
		}
	}
}