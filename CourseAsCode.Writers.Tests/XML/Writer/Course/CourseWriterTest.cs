using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Static;
using CourseAsCode.Writers.XML.Writer.Course;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Course
{
    [TestFixture]
    public class CourseWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ICourseHelper _fakeCourseHelper;
        private IGeneralHelper _fakeGeneralHelper;
        private CourseWriter _courseWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up fake XML writer factory and logger.
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create substitutes for helper dependencies.
            _fakeCourseHelper = Substitute.For<ICourseHelper>();
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();

            // Configure fake helper methods with predictable return values
            _fakeCourseHelper.GetCourseFormatFromEnum(Arg.Any<CourseFormat>()).Returns("topics");
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");

            // Instantiate the CourseWriter with injected dependencies.
            _courseWriter = new CourseWriter(_testFactory, _testLogger, _fakeCourseHelper, _fakeGeneralHelper);

            // Create a dummy course with necessary properties
            _dummyCourse = FakeCourseFactory.CreateCourseWithDetails();
        }

        [Test]
        public void WriteCourse_GeneratesValidXml()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCourse_RootElementHasExpectedAttributes()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Root element is <course> and contains "id" and "contextid" attributes
            var root = doc.DocumentElement;
            Assert.IsNotNull(root, "XML should have a root element");
            Assert.That(root.Name, Is.EqualTo("course"));
            Assert.That(root.GetAttribute("id"), Is.EqualTo(_dummyCourse.SystemDefaults.Id.ToString()));
            Assert.That(root.GetAttribute("contextid"), Is.EqualTo(GeneralDefaults.COURSECONTEXTID.ToString()));
        }

        [Test]
        public void WriteCourse_ContainsGeneralInformationSection()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Check that the general information elements exist inside <course>
            // Full schema validation should be done in integration tests
            var shortnameNode = doc.DocumentElement?.SelectSingleNode("shortname");
            var fullnameNode = doc.DocumentElement?.SelectSingleNode("fullname");

            Assert.IsNotNull(shortnameNode, "<shortname> element is missing");
            Assert.IsNotNull(fullnameNode, "<fullname> element is missing");
            Assert.That(shortnameNode.InnerText, Is.EqualTo(_dummyCourse.ShortName));
            Assert.That(fullnameNode.InnerText, Is.EqualTo(_dummyCourse.FullName));
        }

        [Test]
        public void WriteCourse_ContainsCategorySection()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that the <category> section exists and contains the expected elements
            var categoryNode = doc.DocumentElement?.SelectSingleNode("category");
            Assert.IsNotNull(categoryNode, "<category> element is missing");

            var nameNode = categoryNode["name"];
            var descriptionNode = categoryNode["description"];

            Assert.IsNotNull(nameNode, "Category <name> element is missing");
            Assert.IsNotNull(descriptionNode, "Category <description> element is missing");
            Assert.That(nameNode.InnerText, Is.EqualTo(_dummyCourse.SensibleDefaults.CategoryName));
            Assert.That(descriptionNode.InnerText, Is.EqualTo(_dummyCourse.SensibleDefaults.CategoryDescription ?? "$@NULL@$"));
        }

        [Test]
        public void WriteCourse_ContainsTagsSection_WhenTagsExist()
        {
            // Arrange: Ensure dummy course has tags
            _dummyCourse.Tags = new List<string> { "tag1", "tag2" };

            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that the <tags> element exists and contains tag elements
            var tagsNode = doc.DocumentElement?.SelectSingleNode("tags");
            Assert.IsNotNull(tagsNode, "<tags> element should exist when course has tags");

            var tagNodes = tagsNode.SelectNodes("tag");
            Assert.IsNotNull(tagNodes, "No <tag> elements found in <tags> section");
            Assert.That(tagNodes.Count, Is.EqualTo(_dummyCourse.Tags.Count));
        }

        [Test]
        public void WriteCourse_SkipsTagsSection_WhenNoTagsExist()
        {
            // Arrange: Ensure dummy course has no tags
            _dummyCourse.Tags = null;

            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that the <tags> element is not present
            var tagsNode = doc.DocumentElement?.SelectSingleNode("tags");
            Assert.IsNull(tagsNode, "Tags section should be skipped when there are no tags.");
        }

        [Test]
        public void WriteCourse_ContainsCustomFields()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that <customfields> exists
            var customFieldsNode = doc.DocumentElement?.SelectSingleNode("customfields");
            Assert.IsNotNull(customFieldsNode, "<customfields> element is missing");
        }

        [Test]
        public void WriteCourse_ContainsCourseFormatOptions()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that <courseformatoptions> exists
            var optionsNode = doc.DocumentElement?.SelectSingleNode("courseformatoptions");
            Assert.IsNotNull(optionsNode, "<courseformatoptions> element is missing");
        }

        [Test]
        public void WriteCourse_LogsDebugOnSuccess()
        {
            // Act
            _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing the course file path
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("course/course.xml")));
        }

        [Test]
        public void WriteCourse_LogsErrorOnException()
        {
            // Arrange: Use a factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _courseWriter = new CourseWriter(throwingFactory, _testLogger, _fakeCourseHelper, _fakeGeneralHelper);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _courseWriter.WriteCourse(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded containing "course/course.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/course.xml")));
        }
    }

    #region Fake Course Factory
    /// <summary>
    /// A factory class that creates a dummy course with necessary details for testing
    /// </summary>
    internal static partial class FakeCourseFactory
    {
        public static Model.Course CreateCourseWithDetails()
        {
            var course = new Model.Course
            {
                SensibleDefaults = new Model.Course.SensibleDefaultsCourse()
                {
                    CategoryName = "Category A",
                    CategoryDescription = "A sample category",
                },
                ShortName = "COURSE101",
                FullName = "Sample Course 101",
                Tags = new List<string> { "education", "sample" }
            };

            return course;
        }
    }

    #endregion
}
