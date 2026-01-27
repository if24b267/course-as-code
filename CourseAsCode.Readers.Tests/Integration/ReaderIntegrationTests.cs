using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseAsCode.Readers.Tests.Integration
{
    [TestFixture]
    public class ReaderIntegrationTests : TestBase
    {
        private ServiceProvider _serviceProvider;
        private IActivityReader _activityReader;
        private ISectionReader _sectionReader;
        private ICourseReader _courseReader;

        [SetUp]
        public void SetUp()
        {
            // Call the base SetUp method
            base.SetUp();

            // Setup dependency injection
            var services = new ServiceCollection();
            CourseReader.ConfigureYamlServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Get readers from DI container
            _activityReader = _serviceProvider.GetRequiredService<IActivityReader>();
            _sectionReader = _serviceProvider.GetRequiredService<ISectionReader>();
            _courseReader = _serviceProvider.GetRequiredService<ICourseReader>();

            // Reset ValueKeeper state at the beginning of each test
            ValueKeeper.CurrentActivityId = 1;
            ValueKeeper.CurrentSectionId = 1;
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }

        [Test]
        public void YamlActivityReader_IntegratesWithYamlUrlReader()
        {
            // Arrange
            var urlFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "Activities", "url1.yaml");
            ValueKeeper.CurrentActivityId = 10;

            // Act
            var result = _activityReader.Read(urlFilePath);

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
            var sectionFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "section.yaml");
            ValueKeeper.CurrentActivityId = 1;
            ValueKeeper.CurrentSectionId = 5;

            // Act
            var result = _sectionReader.Read(sectionFilePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test Section 1"));
                Assert.That(result.Summary, Is.EqualTo("Test section 1 summary"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Activities, Has.Count.EqualTo(4));
                Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(5));
                Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_5"));

                // Activity count check - actual IDs may vary based on implementation
                Assert.That(result.Activities.Count, Is.EqualTo(4));

                // Verify the current activity ID was incremented
                Assert.That(ValueKeeper.CurrentActivityId, Is.GreaterThan(1));
            });
        }

        [Test]
        public void CourseReader_IntegratesWithSectionAndActivityReaders()
        {
            // Arrange
            ValueKeeper.CurrentActivityId = 1;
            ValueKeeper.CurrentSectionId = 1;

            // Act - Use CourseReader directly from DI
            var result = ((CourseReader)_courseReader).ReadYaml(TestDataPath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.FullName, Is.EqualTo("Test Course"));
                Assert.That(result.Sections, Has.Count.EqualTo(2));

                // Check section count
                Assert.That(result.Sections.Count, Is.EqualTo(2));

                // Check activity counts
                Assert.That(result.Sections[0].Activities.Count, Is.EqualTo(4));
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
            var labelFilePath = Path.Combine(TestDataPath, "Sections", "Section1", "Activities", "label1.yaml");
            ValueKeeper.CurrentActivityId = 20;

            // Act
            var result = _activityReader.Read(labelFilePath);

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
