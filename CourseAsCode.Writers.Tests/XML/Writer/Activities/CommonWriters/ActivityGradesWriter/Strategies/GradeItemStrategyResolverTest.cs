using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    [TestFixture]
    public class GradeItemStrategyResolverTests
    {
        private QuizGradeItemStrategy _quizStrategy;
        private DefaultGradeItemStrategy _defaultStrategy;
        private GradeItemStrategyResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            var generalHelper = Substitute.For<IGeneralHelper>();
            var xmlHelper = Substitute.For<IXmlHelper>();

            _quizStrategy = new QuizGradeItemStrategy(generalHelper, xmlHelper);
            _defaultStrategy = new DefaultGradeItemStrategy();
            _resolver = new GradeItemStrategyResolver(_quizStrategy, _defaultStrategy);
        }

        [Test]
        public void GetStrategy_ReturnsQuizStrategy_ForQuizActivity()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            var strategy = _resolver.GetStrategy(quiz);

            // Assert
            Assert.That(strategy, Is.SameAs(_quizStrategy));
            Assert.That(strategy, Is.InstanceOf<QuizGradeItemStrategy>());
        }

        [Test]
        public void GetStrategy_ReturnsDefaultStrategy_ForUrlActivity()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            var strategy = _resolver.GetStrategy(urlActivity);

            // Assert
            Assert.That(strategy, Is.SameAs(_defaultStrategy));
            Assert.That(strategy, Is.InstanceOf<DefaultGradeItemStrategy>());
        }

        [Test]
        public void GetStrategy_ReturnsDefaultStrategy_ForUnknownActivityType()
        {
            // Arrange
            var unknownActivity = Substitute.For<Activity>();

            // Act
            var strategy = _resolver.GetStrategy(unknownActivity);

            // Assert
            Assert.That(strategy, Is.SameAs(_defaultStrategy));
        }

        [Test]
        public void GetStrategy_ReturnsCorrectStrategy_ForMultipleActivityTypes()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var url = FakeUrlFactory.CreateBasicUrl(2);

            // Act
            var quizStrategy = _resolver.GetStrategy(quiz);
            var urlStrategy = _resolver.GetStrategy(url);

            // Assert
            Assert.That(quizStrategy, Is.InstanceOf<QuizGradeItemStrategy>());
            Assert.That(urlStrategy, Is.InstanceOf<DefaultGradeItemStrategy>());
            Assert.That(quizStrategy, Is.Not.SameAs(urlStrategy));
        }
    }
}
