using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class RolesWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            XsdPath = PathHelper.GetAbsolutePath("..\\..\\..\\Schemas\\role.xsd");

            _outputPath = PathHelper.EnsureDirectoryExists("..\\..\\..\\..\\Documentation\\out\\moodleCourse");

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Roles = new List<Role>
        {
            new Role
            {
                ShortName = "student",
                LocalName = "Student",
                Description = "A student role",
                Archetype = Archetype.Student,
                SystemDefaults = new Role.SystemDefaultsRole
                {
                    RoleId = 1
                }
            },
            new Role
            {
                ShortName = "teacher",
                LocalName = "Teacher",
                Description = "A teacher role",
                Archetype = Archetype.EditingTeacher,
                SystemDefaults = new Role.SystemDefaultsRole
                {
                    RoleId = 2
                }
            }
        }
            };
        }


        [Test]
        public void WriteRoles_GeneratesValidXml()
        {
            // Arrange
            var writer = new RolesWriter();

            // Act
            writer.WriteRoles(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/roles.xml", XsdPath);
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