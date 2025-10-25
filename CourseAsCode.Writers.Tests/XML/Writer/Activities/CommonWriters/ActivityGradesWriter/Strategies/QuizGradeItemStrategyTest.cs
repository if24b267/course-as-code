using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    [TestFixture]
    public class QuizGradeItemStrategyTests
    {
        private IGeneralHelper _generalHelper;
        private IXmlHelper _xmlHelper;
        private XmlWriterFactoryTest _testFactory;
        private QuizGradeItemStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _generalHelper = Substitute.For<IGeneralHelper>();
            _xmlHelper = Substitute.For<IXmlHelper>();
            _testFactory = new XmlWriterFactoryTest();
            _strategy = new QuizGradeItemStrategy(_generalHelper, _xmlHelper);

            // Setup default mock behaviors
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("1");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Set up XmlHelper to pass through the calls to the mocked writer from the factory
            _xmlHelper.When(x => x.WriteElements(Arg.Any<XmlWriter>(), Arg.Any<Dictionary<string, string>>()))
                .Do(callInfo =>
                {
                    var writer = callInfo.ArgAt<XmlWriter>(0);
                    var elements = callInfo.ArgAt<Dictionary<string, string>>(1);

                    foreach (var element in elements)
                    {
                        writer.WriteElementString(element.Key, element.Value);
                    }
                });

            _xmlHelper.When(x => x.WriteElementWithAttributes(Arg.Any<XmlWriter>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>()))
                .Do(callInfo =>
                {
                    var writer = callInfo.ArgAt<XmlWriter>(0);
                    var elementName = callInfo.ArgAt<string>(1);
                    var attributes = callInfo.ArgAt<Dictionary<string, string>>(2);

                    writer.WriteStartElement(elementName);
                    foreach (var attribute in attributes)
                    {
                        writer.WriteAttributeString(attribute.Key, attribute.Value);
                    }
                });
        }

        [Test]
        public void WriteGradeItems_CreatesGradeItemWithCorrectId()
        {
            // Arrange
            int quizId = 123;
            var quiz = FakeQuizFactory.CreateBasicQuiz(quizId);
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElementWithAttributes(
                Arg.Any<XmlWriter>(),
                "grade_item",
                Arg.Is<Dictionary<string, string>>(d => d["id"] == quizId.ToString()));
        }

        [Test]
        public void WriteGradeItems_WritesAllRequiredElements()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d =>
                    d.ContainsKey("categoryid") &&
                    d.ContainsKey("itemname") &&
                    d.ContainsKey("itemtype") &&
                    d.ContainsKey("itemmodule") &&
                    d.ContainsKey("iteminstance") &&
                    d.ContainsKey("itemnumber") &&
                    d.ContainsKey("calculation") &&
                    d.ContainsKey("gradetype") &&
                    d.ContainsKey("grademax") &&
                    d.ContainsKey("grademin") &&
                    d.ContainsKey("scaleid") &&
                    d.ContainsKey("outcomeid") &&
                    d.ContainsKey("gradepass") &&
                    d.ContainsKey("multfactor") &&
                    d.ContainsKey("plusfactor") &&
                    d.ContainsKey("aggregationcoef") &&
                    d.ContainsKey("aggregationcoef2") &&
                    d.ContainsKey("weightoverride") &&
                    d.ContainsKey("sortorder") &&
                    d.ContainsKey("display") &&
                    d.ContainsKey("decimals") &&
                    d.ContainsKey("hidden") &&
                    d.ContainsKey("locked") &&
                    d.ContainsKey("locktime") &&
                    d.ContainsKey("needsupdate") &&
                    d.ContainsKey("timecreated") &&
                    d.ContainsKey("timemodified")));
        }

        [Test]
        public void WriteGradeItems_UsesGradeNameWhenAvailable()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeName = "Custom Grade Name";
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["itemname"] == "Custom Grade Name"));
        }

        [Test]
        public void WriteGradeItems_FallsBackToActivityNameWhenGradeNameIsNull()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeName = null;
            quiz.Name = "Fallback Name";
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["itemname"] == "Fallback Name"));
        }

        [Test]
        public void WriteGradeItems_FormatsMaxGradeCorrectly()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.MaxGrade = 100.12345f;
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["grademax"] == "100.12345"));
        }

        [Test]
        public void WriteGradeItems_CalculatesWeightOverrideCorrectly_WhenGradingWeightIsSet()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingWeight = 75;
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d =>
                    d["weightoverride"] == "1" &&
                    d["aggregationcoef2"] == "0.75000"));
        }

        [Test]
        public void WriteGradeItems_SetsWeightOverrideToZero_WhenGradingWeightIsNull()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingWeight = null;
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d =>
                    d["weightoverride"] == "0" &&
                    d["aggregationcoef2"] == "0.00000"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradeHiddenUntil()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var specificDate = new DateTime(2025, 1, 1);
            quiz.GradeHiddenUntil = specificDate;
            _generalHelper.DateTimeToUnixTimestamp(specificDate).Returns("1735689600");
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _generalHelper.Received(1).DateTimeToUnixTimestamp(specificDate);
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["hidden"] == "1735689600"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradeHiddenTrue()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeHidden = true;
            quiz.GradeHiddenUntil = null;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["hidden"] == "9876543210"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradingLocked()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingLocked = true;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, quiz);

            // Assert
            _xmlHelper.Received(1).WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Is<Dictionary<string, string>>(d => d["locked"] == "9876543210"));
        }

        [Test]
        public void WriteGradeItems_DoesNothing_WhenActivityIsNotQuiz()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, urlActivity);

            // Assert
            _xmlHelper.DidNotReceive().WriteElementWithAttributes(
                Arg.Any<XmlWriter>(),
                Arg.Any<string>(),
                Arg.Any<Dictionary<string, string>>());
            _xmlHelper.DidNotReceive().WriteElements(
                Arg.Any<XmlWriter>(),
                Arg.Any<Dictionary<string, string>>());
        }
    }
}
