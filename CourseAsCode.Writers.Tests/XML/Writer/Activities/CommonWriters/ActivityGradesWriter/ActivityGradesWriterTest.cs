using System.Xml;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter
{
    [TestFixture]
    public class ActivityGradesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private IGradeItemStrategyResolver _mockStrategyResolver;
        private IGradeItemStrategy _mockStrategy;
        private ILogger<Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter> _testLogger;
        private Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter _activityGradesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest();
            _mockStrategyResolver = Substitute.For<IGradeItemStrategyResolver>();
            _mockStrategy = Substitute.For<IGradeItemStrategy>();
            _testLogger = Substitute.For<ILogger<Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter>>();

            // Setup resolver to return the mock strategy
            _mockStrategyResolver.GetStrategy(Arg.Any<Activity>()).Returns(_mockStrategy);

            _activityGradesWriter = new Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter(_testFactory, _mockStrategyResolver, _testLogger);
        }

        [Test]
        public void WriteGrade_GeneratesValidXml()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGrade_RootElementIsActivityGradebook()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("activity_gradebook"));
        }

        [Test]
        public void WriteGrade_ContainsGradeItemsElement()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItemsNode = doc.DocumentElement?.SelectSingleNode("grade_items");
            Assert.That(gradeItemsNode, Is.Not.Null, "The <grade_items> element is missing");
        }

        [Test]
        public void WriteGrade_ContainsGradeLettersElement()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeLettersNode = doc.DocumentElement?.SelectSingleNode("grade_letters");
            Assert.That(gradeLettersNode, Is.Not.Null, "The <grade_letters> element is missing");
        }

        [Test]
        public void WriteGrade_CallsStrategyResolverWithCorrectActivity()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _mockStrategyResolver.Received(1).GetStrategy(quiz);
        }

        [Test]
        public void WriteGrade_CallsStrategyWriteGradeItems()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _mockStrategy.Received(1).WriteGradeItems(Arg.Any<XmlWriter>(), quiz);
        }
    }
}
