using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.Integration
{
	[TestFixture]
	public class CourseReaderIntegrationTests : TestBase
	{
		private CourseReader _reader;

		[SetUp]
		public void Setup()
		{
			_reader = new CourseReader(new YAMLSectionReader());
		}

		[Test]
		public void Read_TestDataCourse_ReturnsValidCourse()
		{
			// Act
			var result = _reader.Read(TestDataPath, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.FullName, Is.EqualTo("Test Course"));
				Assert.That(result.ShortName, Is.EqualTo("TC"));
				Assert.That(result.Sections, Has.Count.EqualTo(2));

				// Verify first section
				var section1 = result.Sections[0];
				Assert.That(section1.Name, Is.EqualTo("Test Section 1"));
				Assert.That(section1.Summary, Is.EqualTo("Test section 1 summary"));
				Assert.That(section1.VisibleToStudents, Is.True);
				Assert.That(section1.Activities, Has.Count.EqualTo(3));

				// Verify second section
				var section2 = result.Sections[1];
				Assert.That(section2.Name, Is.EqualTo("Test Section 2"));
				Assert.That(section2.Summary, Is.EqualTo("Test section 2 summary"));
				Assert.That(section2.VisibleToStudents, Is.False);
				Assert.That(section2.Activities, Has.Count.EqualTo(1));
			});
		}

		[Test]
		public void Read_TestDataCourse_ReturnsCorrectActivityTypes()
		{
			// Act
			var result = _reader.Read(TestDataPath, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);

				// Verify activity types in first section
				var section1Activities = result.Sections[0].Activities;
				Assert.That(section1Activities[0].GetType().Name, Is.EqualTo("URL"));
				Assert.That(section1Activities[1].GetType().Name, Is.EqualTo("MoodleFile"));

				// The third activity might be TextAndMediaArea depending on implementation
				Assert.That(section1Activities[2].Name, Is.EqualTo("Test Label 1"));

				// Verify URL in second section
				var section2Activities = result.Sections[1].Activities;
				Assert.That(section2Activities[0].GetType().Name, Is.EqualTo("URL"));
				Assert.That(section2Activities[0].Name, Is.EqualTo("Test URL 2"));
			});
		}

		[Test]
		public void Read_TestDataCourse_SetsCorrectIds()
		{
			// Act
			var result = _reader.Read(TestDataPath, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);

				// Verify section IDs
				Assert.That(result.Sections[0].SystemDefaults.SectionId, Is.EqualTo(1));
				Assert.That(result.Sections[1].SystemDefaults.SectionId, Is.EqualTo(2));

				// Verify activity IDs are sequential
				var allActivities = result.Sections.SelectMany(s => s.Activities).ToList();
				for (int i = 0; i < allActivities.Count; i++)
				{
					Assert.That(allActivities[i].ActivityId, Is.EqualTo(i + 1));
				}
			});
		}

		[Test]
		public void Read_TestDataCourse_SetsCorrectURLProperties()
		{
			// Act
			var result = _reader.Read(TestDataPath, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);

				// Find the URL activities
				var url1 = result.Sections[0].Activities
						 .FirstOrDefault(a => a.Name == "Test URL 1");
				var url2 = result.Sections[1].Activities
						 .FirstOrDefault(a => a.Name == "Test URL 2");

				Assert.That(url1, Is.Not.Null);
				Assert.That(url2, Is.Not.Null);

				// Cast and verify properties
				var typedUrl1 = url1 as Model.Activities.Resources.URL;
				var typedUrl2 = url2 as Model.Activities.Resources.URL;

				Assert.That(typedUrl1.Description, Is.EqualTo("Test URL 1 description"));
				Assert.That(typedUrl1.ExternalUrl, Is.EqualTo("https://example.com/1"));

				Assert.That(typedUrl2.Description, Is.EqualTo("Test URL 2 description"));
				Assert.That(typedUrl2.ExternalUrl, Is.EqualTo("https://example.com/2"));
			});
		}
	}
}