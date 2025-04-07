using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using System.Collections.Generic;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class CourseWriterIntegrationTests
    {
        private const string XsdPath = "..\\..\\..\\Schemas\\course\\course.xsd";
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Summary = "This is a test course.",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                GroupMode = GroupMode.SeparateGroups,
                CourseLanguage = Language.English,
                Tags = new List<string> { "test", "course" },
                Sections = new List<Section>(),
                Roles = new List<Role>(),
                EnrolmentMethods = new List<EnrolmentMethod>(),
                EnrolledUsers = new List<User>(),
                SensibleDefaults = new Course.SensibleDefaultsCourse
                {
                    NumberOfAnnouncementsToDisplay = 5,
                    Format = CourseFormat.WeeklySections,
                    ShowGrades = true,
                    MaximumUploadSize = 1048576,
                    VisibleToStudents = true,
                    ForceGroupMode = false,
                    Theme = "default",
                    ShowActivityDates = true,
                    ShowActivityCompletionConditions = true,
                    EnableCompletionTracking = true,
                    CategoryName = "Test Category",
                    CategoryDescription = "This is a test category."
                },
                SystemDefaults = new Course.SystemDefaultsCourse
                {
                    Id = 1,
                    SummaryFormat = FormattingFormat.HTML,
                    UsesLegacyFiles = false,
                    TimeCreated = DateTime.UtcNow,
                    TimeModified = DateTime.UtcNow,
                    PdfExportFont = "Arial",
                    CustomFields = "Custom Field Data"
                }
            };
            _outputPath = "..\\..\\..\\..\\Documentation\\out\\moodleCourse\\";
        }

        [Test]
        public void WriteCourse_GeneratesValidXml()
        {
            // Arrange
            var writer = new CourseWriter();

            // Act
            writer.WriteCourse(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/course/course.xml", XsdPath);
        }

        private void ValidateXml(string xmlPath, string xsdPath)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;

            using (var reader = XmlReader.Create(xmlPath, settings))
            {
                var document = new XmlDocument();
                document.Load(reader);
                document.Validate((sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        Assert.Fail($"XML Validation Error: {e.Message}");
                    }
                });
            }
        }
    }
}