using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.CalculatedMulti;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class CalculatedMultiChoiceStrategyTests
    {
        private ILogger<CalculatedMultiStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IQuizHelper _mockQuizHelper;
        private IXmlService _mockXmlService;
        private CalculatedMultiStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";
        private int _idCounter;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger<CalculatedMultiStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockQuizHelper = Substitute.For<IQuizHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            _idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => _idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            _strategy = new CalculatedMultiStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator, _mockQuizHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsCalculatedMulti()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("calculatedmulti"));
        }

        [Test]
        public void WritePluginData_WritesNumericalUnits()
        {
            // Arrange
            var question = CreateBasicCalculatedMultiChoiceQuestion();
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
            var question = CreateBasicCalculatedMultiChoiceQuestion();

            question.Answers.Add(new CalculatedHelper.CalculatedAnswer { AnswerText = "{a}-{b}" });

            question.CalculatedRecords.Add(new CalculatedHelper.CalculatedRecord { Tolerance = 0.01f });
            question.CalculatedRecords.Add(new CalculatedHelper.CalculatedRecord { Tolerance = 0.05f });

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert 1: Answers
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received(2).WriteElementWithAttributes("answer", Arg.Any<Dictionary<string, string>>());

            // Assert 2: Calculated Records
            _mockXmlService.Received().WriteElement("calculated_records");
            _mockXmlService.Received(2).WriteElementWithAttributes("calculated_record", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("tolerance") && d["tolerance"] == "0.01"));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("tolerance") && d["tolerance"] == "0.05"));
        }

        [Test]
        public void WritePluginData_WritesNumericalOptions()
        {
            // Arrange
            var question = CreateBasicCalculatedMultiChoiceQuestion();
            question.NumericalOptions.ShowUnits = 3;
            question.NumericalOptions.UnitsLeft = true;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("numerical_options");
            _mockXmlService.Received().WriteElementWithAttributes("numerical_option", Arg.Any<Dictionary<string, string>>());

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("showunits") && d["showunits"] == "3" &&
                d.ContainsKey("unitsleft") && d["unitsleft"] == "1"
            ));
        }

        [Test]
        public void WritePluginData_SharedDataset_UsesQuestionCategoryId()
        {
            // Arrange
            var question = CreateBasicCalculatedMultiChoiceQuestion();
            question.CategoryId = 99;

            string uniqueVarName = $"shared_var_multi_{Guid.NewGuid()}";

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
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("name") && d["name"] == uniqueVarName &&
                d.ContainsKey("category") && d["category"] == "99"
            ));
        }

        [Test]
        public void WritePluginData_PrivateDataset_UsesZero()
        {
            // Arrange
            var question = CreateBasicCalculatedMultiChoiceQuestion();
            question.CategoryId = 99;
            string uniqueVarName = $"private_var_multi_{Guid.NewGuid()}";

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

        private CalculatedMultiChoiceQuestion CreateBasicCalculatedMultiChoiceQuestion()
        {
            return new CalculatedMultiChoiceQuestion
            {
                QuestionText = "Multi Test",
                QuestionName = "Calc Multi Test",
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
