using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter
{
    [TestFixture]
    public class ActivityGradesWriterTests
    {
        private IXmlService _xmlService;
        private IXmlServiceFactory _xmlServiceFactory;
        private IGradeItemStrategyResolver _mockStrategyResolver;
        private IGradeItemStrategy _mockStrategy;
        private ILogger<Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter> _testLogger;
        private Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter _activityGradesWriter;

        [SetUp]
        public void SetUp()
        {
            // Substitutes for dependencies
            _xmlService = Substitute.For<IXmlService>();
            _xmlServiceFactory = Substitute.For<IXmlServiceFactory>();
            _mockStrategyResolver = Substitute.For<IGradeItemStrategyResolver>();
            _mockStrategy = Substitute.For<IGradeItemStrategy>();
            _testLogger = Substitute.For<ILogger<Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter>>();

            // Set up the factory to return the mocked Xml Service
            _xmlServiceFactory.Create(Arg.Any<string>()).Returns(_xmlService);

            // Setup resolver to return the mock strategy
            _mockStrategyResolver.GetStrategy(Arg.Any<Activity>()).Returns(_mockStrategy);

            _activityGradesWriter = new Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.ActivityGradesWriter(
                _mockStrategyResolver,
                _xmlServiceFactory,
                _testLogger);
        }

        [TearDown]
        public void TearDown()
        {
            _xmlService.Dispose();
        }

        [Test]
        public void WriteGrade_CreatesXmlServiceWithCorrectPath()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var outputPath = "test/output/path";

            // Act
            _activityGradesWriter.WriteGrade(quiz, outputPath);

            // Assert
            _xmlServiceFactory.Received(1).Create(Arg.Is<string>(path =>
                path.EndsWith("grades.xml")));
        }

        [Test]
        public void WriteGrade_WritesRootActivityGradebookElement()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElement("activity_gradebook");
        }

        [Test]
        public void WriteGrade_WritesGradeItemsElement()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElement("grade_items");
        }

        [Test]
        public void WriteGrade_WritesGradeLettersElement()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElement("grade_letters");
        }

        [Test]
        public void WriteGrade_ClosesAllElements()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            // Should close: grade_items, grade_letters, activity_gradebook = 3 elements
            _xmlService.Received(3).CloseElement();
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
            _mockStrategy.Received(1).WriteGradeItems(_xmlService, quiz);
        }

        [Test]
        public void WriteGrade_PassesXmlServiceToStrategy()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _mockStrategy.Received(1).WriteGradeItems(
                Arg.Is<IXmlService>(s => s == _xmlService),
                quiz);
        }

        [Test]
        public void WriteGrade_DisposesXmlService()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);

            // Assert
            _xmlService.Received(1).Dispose();
        }
    }
}
