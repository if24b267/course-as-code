using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class GradebookWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for GradebookWriter
        private IXmlServiceFactory _xmlServiceFactory;
        private IGeneralHelper _generalHelper;

        private ILogger<GeneralHelper> _generalHelperLogger;
        private ILogger<GradebookWriter> _gradebookLogger;
        private ILogger<XmlServiceFactory> _xmlServiceFactoryLogger;
        private ILogger<XmlService> _xmlServiceLogger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "gradebook.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlServiceFactoryLogger = NullLogger<XmlServiceFactory>.Instance;
            _xmlServiceLogger = NullLogger<XmlService>.Instance;
            _gradebookLogger = NullLogger<GradebookWriter>.Instance; // Use NullLogger to avoid log output during tests
            _generalHelperLogger = NullLogger<GeneralHelper>.Instance;
            _generalHelper = new GeneralHelper(_generalHelperLogger);

            _xmlServiceFactory = new XmlServiceFactory(_xmlServiceFactoryLogger, _xmlServiceLogger);
            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        [Ignore("Needs rework for new grading implementation")]
        public void WriteGradebook_GeneratesValidXml()
        {
            // Arrange
            var writer = new GradebookWriter(_generalHelper, _xmlServiceFactory, _gradebookLogger);

            // Act
            writer.WriteGradebook(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/gradebook.xml", XsdPath);
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
