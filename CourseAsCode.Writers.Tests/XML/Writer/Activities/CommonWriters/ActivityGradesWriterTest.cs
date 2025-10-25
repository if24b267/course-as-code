using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityGradesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private IGeneralHelper _generalHelper;
        private ILogger<ActivityGradesWriter> _testLogger;
        private ActivityGradesWriter _activityGradesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _testLogger = Substitute.For<ILogger<ActivityGradesWriter>>();

            // Setup default mock behaviors
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("1");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            _activityGradesWriter = new ActivityGradesWriter(_testFactory, _generalHelper, _testLogger);
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
        public void WriteGrade_WithQuiz_CreatesGradeItemWithCorrectId()
        {
            // Arrange
            int quizId = 123;
            var quiz = FakeQuizFactory.CreateBasicQuiz(quizId);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItemNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item");
            Assert.That(gradeItemNode, Is.Not.Null, "The <grade_item> element is missing");
            Assert.That(gradeItemNode.Attributes["id"].Value, Is.EqualTo(quizId.ToString()));
        }

        [Test]
        public void WriteGrade_WithQuiz_WritesAllRequiredElements()
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
            var gradeItemNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item");

            var requiredElements = new[]
            {
                "categoryid", "itemname", "itemtype", "itemmodule", "iteminstance",
                "itemnumber", "iteminfo", "idnumber", "calculation", "gradetype",
                "grademax", "grademin", "scaleid", "outcomeid", "gradepass",
                "multfactor", "plusfactor", "aggregationcoef", "aggregationcoef2",
                "weightoverride", "sortorder", "display", "decimals", "hidden",
                "locked", "locktime", "needsupdate", "timecreated", "timemodified"
            };

            foreach (var elementName in requiredElements)
            {
                var element = gradeItemNode?.SelectSingleNode(elementName);
                Assert.That(element, Is.Not.Null, $"The <{elementName}> element is missing");
            }
        }

        [Test]
        public void WriteGrade_WithQuiz_UsesGradeNameWhenAvailable()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeName = "Custom Grade Name";
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var itemNameNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/itemname");
            Assert.That(itemNameNode?.InnerText, Is.EqualTo("Custom Grade Name"));
        }

        [Test]
        public void WriteGrade_WithQuiz_FallsBackToActivityNameWhenGradeNameIsNull()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeName = null;
            quiz.Name = "Fallback Name";
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var itemNameNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/itemname");
            Assert.That(itemNameNode?.InnerText, Is.EqualTo("Fallback Name"));
        }

        [Test]
        public void WriteGrade_WithQuiz_WritesMaxGradeWithCorrectFormat()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.MaxGrade = 100.12345f;
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeMaxNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/grademax");
            Assert.That(gradeMaxNode?.InnerText, Is.EqualTo("100.12345"));
        }

        [Test]
        public void WriteGrade_WithQuiz_WritesWeightOverrideCorrectly()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingWeight = 75;
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var weightOverrideNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/weightoverride");
            var aggregationcoef2Node = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/aggregationcoef2");
            Assert.That(aggregationcoef2Node?.InnerText, Is.EqualTo("0.75000"));
            Assert.That(weightOverrideNode?.InnerText, Is.EqualTo("1"));
        }

        [Test]
        public void WriteGrade_WithQuiz_WritesWeightOverrideAsZeroWhenNull()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingWeight = null;
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var weightOverrideNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/weightoverride");
            var aggregationcoef2Node = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/aggregationcoef2");
            Assert.That(aggregationcoef2Node?.InnerText, Is.EqualTo("0.00000"));
            Assert.That(weightOverrideNode?.InnerText, Is.EqualTo("0"));
        }

        [Test]
        public void WriteGrade_WithQuizAndGradeHiddenTrue_WritesCurrentTimestamp()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradeHidden = true;
            quiz.GradeHiddenUntil = null;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var hiddenNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/hidden");
            Assert.That(hiddenNode?.InnerText, Is.EqualTo("9876543210"));
        }

        [Test]
        public void WriteGrade_WithQuizAndGradeHiddenUntil_WritesSpecificTimestamp()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            var specificDate = new DateTime(2025, 1, 1);
            quiz.GradeHiddenUntil = specificDate;
            _generalHelper.DateTimeToUnixTimestamp(specificDate).Returns("1735689600");
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var hiddenNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/hidden");
            Assert.That(hiddenNode?.InnerText, Is.EqualTo("1735689600"));
            _generalHelper.Received(1).DateTimeToUnixTimestamp(specificDate);
        }

        [Test]
        public void WriteGrade_WithQuizAndGradingLocked_WritesCurrentTimestamp()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            quiz.GradingLocked = true;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(quiz, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var lockedNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/locked");
            Assert.That(lockedNode?.InnerText, Is.EqualTo("9876543210"));
        }

        [Test]
        public void WriteGrade_WithNonQuizActivity_DoesNotWriteGradeItem()
        {
            // Arrange
            URL urlActivity = FakeUrlFactory.CreateBasicUrl(1);
            var dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(urlActivity, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItemNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item");
            Assert.That(gradeItemNode, Is.Null, "No grade_item should be written for non-Quiz activities");
        }
    }
}
