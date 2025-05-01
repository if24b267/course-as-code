using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class QuestionsWriterIntegrationTests
    {
        private string _xsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "questions.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
            };
        }

        [Test]
        [Ignore("Needs to be refactored due to QuestionsWriter now requiring certain dependencies")]
        public void WriteQuestions_GeneratesValidXml()
        {
            /*
            // Arrange
            var writer = new QuestionsWriter();

            // Act
            writer.WriteQuestions(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/questions.xml", _xsdPath);
            */
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

