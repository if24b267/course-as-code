using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.CalculatedSimple;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class CalculatedSimpleStrategyTests
    {
        private ILogger<CalculatedSimpleStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IQuizHelper _mockQuizHelper;
        private IXmlService _mockXmlService;
        private CalculatedSimpleStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger<CalculatedSimpleStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockQuizHelper = Substitute.For<IQuizHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            _strategy = new CalculatedSimpleStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator, _mockQuizHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsCalculatedSimple()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("calculatedsimple"));
        }

        [Test]
        public void WritePluginData_WritesNumericalUnits()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicSimpleCalculatedQuestion(1, "Math");
            question.NumericalUnits = new List<CalculatedHelper.NumericalUnit>
            {
                new() { UnitName = "km", Multiplier = 1000.0f }
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("numerical_units");

            _mockXmlService.Received().WriteElementWithAttributes("numerical_unit", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("unit") && d["unit"] == "km" && 
                d.ContainsKey("multiplier") && d["multiplier"] == "1000.0000000"
            ));
        }

        [Test]
        public void WritePluginData_ParsesAndWritesDatasetDefinitions()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicSimpleCalculatedQuestion(1, "Math");

            question.DatasetDefinitions[0].Options = "uniform:5.0:15.0:2";
            question.DatasetDefinitions[0].Name = "x";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("dataset_definitions");

            _mockXmlService.Received().WriteElementWithAttributes("dataset_definition", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("name") && d["name"] == "x" &&
                d.ContainsKey("options") && d["options"] == "uniform:5.0:15.0:2"
            ));
        }

        [Test]
        public void WritePluginData_WritesAnswersAndCalculatedRecords()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicSimpleCalculatedQuestion(1, "Math");

            question.CalculatedRecords[0].Tolerance = 0.05f;
            question.CalculatedRecords[0].ToleranceType = 2;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert 1: Answers
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("answertext") && d["answertext"] == "{a} + {b}"
            ));

            // Assert 2: Calculated Records
            _mockXmlService.Received().WriteElement("calculated_records");
            _mockXmlService.Received().WriteElementWithAttributes("calculated_record", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("tolerance") && d["tolerance"] == "0.05" &&
                d.ContainsKey("tolerancetype") && d["tolerancetype"] == "2" &&
                d.ContainsKey("answer") 
            ));
        }

        [Test]
        public void WritePluginData_WritesNumericalOptions()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicSimpleCalculatedQuestion(1, "Math");
            question.NumericalOptions.ShowUnits = 3;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("numerical_options");
            _mockXmlService.Received().WriteElementWithAttributes("numerical_option", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("showunits") && d["showunits"] == "3"
            ));
        }
    }
}
