using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Model;
using System.Collections.Generic;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class FiltersWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "filters.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteFilters_GeneratesValidXml()
        {
            // Arrange
            var writer = new FiltersWriter();

            // Sicherstellen, dass das Verzeichnis existiert
            var filtersDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(filtersDirectory))
            {
                Directory.CreateDirectory(filtersDirectory);
            }

            // Act
            writer.WriteFilters(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{filtersDirectory}/filters.xml", XsdPath);
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

