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
    public class RoleAssignmentsWriterIntegrationTests
    {
        private const string XsdPath = "..\\..\\..\\Schemas\\course\\roles.xsd";
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                EnrolledUsers = new List<User>
                {
                    new User
                    {
                        Username = "user1",
                        SystemDefaults = new User.SystemDefaultsUser
                        {
                            Id = 1,
                            RoleChange = DateTime.UtcNow
                        },
                        Role = new Role
                        {
                            ShortName = "student",
                            SystemDefaults = new Role.SystemDefaultsRole
                            {
                                RoleId = 1
                            }
                        }
                    }
                }
            };
            _outputPath = "..\\..\\..\\..\\Documentation\\out\\moodleCourse\\";
        }

        [Test]
        public void WriteRoleAssignments_GeneratesValidXml()
        {
            // Arrange
            var writer = new RoleAssignmentsWriter();

            // Act
            writer.WriteRoleAssignments(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/course/roles.xml", XsdPath);
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

