using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.Calculated;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class CalculatedStrategyTests
    {
        private ILogger<CalculatedStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IQuizHelper _mockQuizHelper;
        private IXmlService _mockXmlService;
        private CalculatedStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";
        private int _idCounter;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger<CalculatedStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockQuizHelper = Substitute.For<IQuizHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            _idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => _idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            _strategy = new CalculatedStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator, _mockQuizHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsCalculated()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("calculated"));
        }

        [Test]
        public void WritePluginData_WritesNumericalUnits()
        {
            // Arrange
            var question = CreateBasicCalculatedQuestion();
            question.NumericalUnits = new List<CalculatedHelper.NumericalUnit>
            {
                new() { UnitName = "cm", Multiplier = 1.0f },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("numerical_units");

            _mockXmlService.Received().WriteElementWithAttributes("numerical_unit", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("unit") && d["unit"] == "cm" &&
                d.ContainsKey("multiplier") && d["multiplier"] == "1.0000000"
            ));
        }

        [Test]
        public void WritePluginData_WritesAnswersAndCalculatedRecords()
        {
            // Arrange
            var question = CreateBasicCalculatedQuestion();
            question.CalculatedRecords.Add(new CalculatedHelper.CalculatedRecord
            {
                Tolerance = 0.01f,
                ToleranceType = 1,
                CorrectAnswerLength = 1,
                CorrectAnswerFormat = 1,
            });

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
                d.ContainsKey("tolerance") && d["tolerance"] == "0.01" &&
                d.ContainsKey("tolerancetype") && d["tolerancetype"] == "1"
            ));
        }

        [Test]
        public void WritePluginData_WritesNumericalOptions()
        {
            // Arrange
            var question = CreateBasicCalculatedQuestion();
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

        [Test]
        public void WritePluginData_SharedDataset_UsesQuestionCategoryId()
        {
            // Arrange
            var question = CreateBasicCalculatedQuestion();
            question.CategoryId = 55;

            string uniqueVarName = $"shared_var_{Guid.NewGuid()}";

            question.DatasetDefinitions = new List<CalculatedHelper.DatasetDefinition>
            {
                new()
                {
                    Name = uniqueVarName,
                    Category = 1,
                    Options = "uniform:1:10:1",
                    Items = new List<CalculatedHelper.DatasetItem> { new() { Number = 1, Value = 5.0f } },
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("dataset_definitions");
            _mockXmlService.Received().WriteElementWithAttributes("dataset_definition", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("name") && d["name"] == uniqueVarName &&
                d.ContainsKey("category") && d["category"] == "55"
            ));
        }

        [Test]
        public void WritePluginData_PrivateDataset_UsesZero()
        {
            // Arrange
            var question = CreateBasicCalculatedQuestion();
            question.CategoryId = 55;
            string uniqueVarName = $"private_var_{Guid.NewGuid()}";

            question.DatasetDefinitions = new List<CalculatedHelper.DatasetDefinition>
            {
                new()
                {
                    Name = uniqueVarName,
                    Category = 0,
                    Options = "uniform:1:10:1",
                    Items = new List<CalculatedHelper.DatasetItem> { new() { Number = 1, Value = 5.0f } },
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("name") && d["name"] == uniqueVarName &&
                d.ContainsKey("category") && d["category"] == "0"
            ));
        }

        // Helper to mimic FakeQuestionFactory behavior locally
        private CalculatedQuestion CreateBasicCalculatedQuestion()
        {
            return new CalculatedQuestion
            {
                QuestionText = "Testt",
                QuestionName = "Calc Test",
                Category = "test",
                Answers = new List<CalculatedHelper.CalculatedAnswer>
                {
                    new() { AnswerText = "{a} + {b}", Fraction = 1.0f },
                },
                NumericalOptions = new CalculatedHelper.NumericalOption(),
                NumericalUnits = new List<CalculatedHelper.NumericalUnit>(),
                DatasetDefinitions = new List<CalculatedHelper.DatasetDefinition>(),
                CalculatedRecords = new List<CalculatedHelper.CalculatedRecord>(),
                CalculatedOptions = new CalculatedHelper.CalculatedOption { Synchronize = 1 },
            };
        }
    }
}
