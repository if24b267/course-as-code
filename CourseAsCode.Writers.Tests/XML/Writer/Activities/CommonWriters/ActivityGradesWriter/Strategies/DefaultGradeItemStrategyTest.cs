using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    [TestFixture]
    public class DefaultGradeItemStrategyTests
    {
        private DefaultGradeItemStrategy _strategy;
        private IXmlService _xmlService;

        [SetUp]
        public void SetUp()
        {
            // Substitutes for Dependencies
            _strategy = new DefaultGradeItemStrategy();
            _xmlService = Substitute.For<IXmlService>();
        }

        [TearDown]
        public void TearDown()
        {
            _xmlService.Dispose();
        }

        [Test]
        public void WriteGradeItems_DoesNotWriteAnything()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _strategy.WriteGradeItems(_xmlService, urlActivity);

            // Assert
            _xmlService.DidNotReceive().WriteElementWithAttributes(
                Arg.Any<string>(),
                Arg.Any<Dictionary<string, string>>());
            _xmlService.DidNotReceive().WriteElements(
                Arg.Any<Dictionary<string, string>>());
            _xmlService.DidNotReceive().WriteElement(Arg.Any<string>());
            _xmlService.DidNotReceive().CloseElement();
        }

        [Test]
        public void WriteGradeItems_DoesNotThrow_WithUrlActivity()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);

            // Act & Assert
            Assert.DoesNotThrow(() => _strategy.WriteGradeItems(_xmlService, urlActivity));
        }

        [Test]
        public void WriteGradeItems_DoesNotThrow_WithQuizActivity()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act & Assert
            Assert.DoesNotThrow(() => _strategy.WriteGradeItems(_xmlService, quiz));
        }

        [Test]
        public void WriteGradeItems_DoesNotThrow_WithAssignmentActivity()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);

            // Act & Assert
            Assert.DoesNotThrow(() => _strategy.WriteGradeItems(_xmlService, assignment));
        }
    }
}
