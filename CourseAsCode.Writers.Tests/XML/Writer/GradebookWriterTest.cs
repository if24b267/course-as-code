using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class GradebookWriterTest
    {
        private IXmlService _xmlService;
        private IXmlServiceFactory _xmlServiceFactory;
        private IGeneralHelper _generalHelper;
        private ILogger<GradebookWriter> _testLogger;
        private GradebookWriter _gradebookWriter;

        [SetUp]
        public void SetUp()
        {
            // Substitutes for dependencies
            _xmlService = Substitute.For<IXmlService>();
            _xmlServiceFactory = Substitute.For<IXmlServiceFactory>();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _testLogger = Substitute.For<ILogger<GradebookWriter>>();

            // Set up the factory to return the mocked Xml Service
            _xmlServiceFactory.Create(Arg.Any<string>()).Returns(_xmlService);

            // Setup default mock behaviors
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("1");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            _gradebookWriter = new GradebookWriter(_generalHelper, _xmlServiceFactory, _testLogger);
        }

        [TearDown]
        public void TearDown()
        {
            _xmlService.Dispose();
        }

        [Test]
        public void WriteGradebook_CreatesXmlServiceWithCorrectPath()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var outputPath = "test/output/path";

            // Act
            _gradebookWriter.WriteGradebook(course, outputPath);

            // Assert
            _xmlServiceFactory.Received(1).Create(Arg.Is<string>(path =>
                path.EndsWith("gradebook.xml")));
        }

        [Test]
        public void WriteGradebook_WritesRootGradebookElement()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElement("gradebook");
        }

        [Test]
        public void WriteGradebook_WritesAllRequiredSections()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElement("attributes");
            _xmlService.Received(1).WriteElement("grade_categories");
            _xmlService.Received(1).WriteElement("grade_items");
            _xmlService.Received(1).WriteElement("grade_letters");
            _xmlService.Received(1).WriteElement("grade_settings");
        }

        [Test]
        public void WriteGradebook_ClosesAllOpenedElements()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            // Should close: attributes, grade_categories, grade_items, grade_letters,
            // grade_setting, grade_settings, gradebook = 7 elements
            _xmlService.Received(7).CloseElement();
        }

        [Test]
        public void WriteGradebook_WithSingleGradeCategory_WritesGradeCategoryElement()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElementWithAttributes("grade_category",
                Arg.Is<Dictionary<string, string>>(dict => dict["id"] == "1"));
        }

        [Test]
        public void WriteGradebook_WithMultipleGradeCategories_WritesAllCategories()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            course.GradeCategories.Add(FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Category 1"));
            course.GradeCategories.Add(FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Category 2"));
            course.GradeCategories.Add(FakeGradeCategoryFactory.CreateBasicGradeCategory(3, "Category 3"));
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(3).WriteElementWithAttributes("grade_category", Arg.Any<Dictionary<string, string>>());
        }

        [Test]
        public void WriteGradebook_WithSingleGradeCategory_WritesGradeItemElement()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(1).WriteElementWithAttributes("grade_item",
                Arg.Is<Dictionary<string, string>>(dict => dict["id"] == "1"));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WritesAllRequiredElements()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("parent") &&
                dict.ContainsKey("depth") &&
                dict.ContainsKey("path") &&
                dict.ContainsKey("fullname") &&
                dict.ContainsKey("aggregation") &&
                dict.ContainsKey("keephigh") &&
                dict.ContainsKey("droplow") &&
                dict.ContainsKey("aggregateonlygraded") &&
                dict.ContainsKey("aggregateoutcomes") &&
                dict.ContainsKey("timecreated") &&
                dict.ContainsKey("timemodified") &&
                dict.ContainsKey("hidden")
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WritesAllRequiredElements()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("categoryid") &&
                dict.ContainsKey("itemname") &&
                dict.ContainsKey("itemtype") &&
                dict.ContainsKey("itemmodule") &&
                dict.ContainsKey("iteminstance") &&
                dict.ContainsKey("itemnumber") &&
                dict.ContainsKey("iteminfo") &&
                dict.ContainsKey("idnumber") &&
                dict.ContainsKey("calculation") &&
                dict.ContainsKey("gradetype") &&
                dict.ContainsKey("grademax") &&
                dict.ContainsKey("grademin") &&
                dict.ContainsKey("scaleid") &&
                dict.ContainsKey("outcomeid") &&
                dict.ContainsKey("gradepass") &&
                dict.ContainsKey("multfactor") &&
                dict.ContainsKey("plusfactor") &&
                dict.ContainsKey("aggregationcoef") &&
                dict.ContainsKey("aggregationcoef2") &&
                dict.ContainsKey("weightoverride") &&
                dict.ContainsKey("sortorder") &&
                dict.ContainsKey("display") &&
                dict.ContainsKey("decimals") &&
                dict.ContainsKey("hidden") &&
                dict.ContainsKey("locked") &&
                dict.ContainsKey("locktime") &&
                dict.ContainsKey("needsupdate") &&
                dict.ContainsKey("timecreated") &&
                dict.ContainsKey("timemodified")
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithNullParent_WritesNullPlaceholder()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.Parent = null;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict["parent"] == "$@NULL@$"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithParent_WritesParentId()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var parentCategory = FakeGradeCategoryFactory.CreateBasicGradeCategory(5, "Parent Category");
            var childCategory = FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Child Category");
            childCategory.Parent = parentCategory;
            course.GradeCategories.Add(parentCategory);
            course.GradeCategories.Add(childCategory);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("parent") && dict["parent"] == "5"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_CalculatesCorrectDepthForRootCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Root Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("depth") && dict["depth"] == "1"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_CalculatesCorrectPathForRootCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(5, "Root Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("path") && dict["path"] == "/5/"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_CalculatesCorrectDepthForNestedCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var parent = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Parent");
            var child = FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Child");
            child.Parent = parent;
            course.GradeCategories.Add(parent);
            course.GradeCategories.Add(child);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("depth") && dict["depth"] == "2"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_CalculatesCorrectPathForNestedCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var parent = FakeGradeCategoryFactory.CreateBasicGradeCategory(4, "Parent");
            var child = FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Child");
            var grandchild = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Grandchild");
            child.Parent = parent;
            grandchild.Parent = child;
            course.GradeCategories.Add(parent);
            course.GradeCategories.Add(child);
            course.GradeCategories.Add(grandchild);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("path") && dict["path"] == "/4/2/1/"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithDropLowestGrades_WritesValue()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.DropLowestGrades = 2;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("droplow") && dict["droplow"] == "2"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithNullDropLowestGrades_WritesZero()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.DropLowestGrades = null;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("droplow") && dict["droplow"] == "0"
            ));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithNonNaturalAggregation_LogsWarning()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.AggregationType = GradeAggregation.WeightedMeanOfGrades;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _testLogger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Test Category") && o.ToString()!.Contains("Natural")),
                null,
                Arg.Any<Func<object, Exception?, string>>()!);
        }

        [Test]
        public void WriteGradebook_GradeItem_RootCategory_HasItemTypeCourse()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Root Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("itemtype") && dict["itemtype"] == "course"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_NestedCategory_HasItemTypeCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var parent = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Parent");
            var child = FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Child");
            child.Parent = parent;
            course.GradeCategories.Add(parent);
            course.GradeCategories.Add(child);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("itemtype") && dict["itemtype"] == "category"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WritesMaxGradeWithCorrectFormat()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category", maxGrade: 150);
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("grademax") && dict["grademax"] == "150.00000"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WritesGradeToPassWithCorrectFormat()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category", gradeToPass: 75);
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("gradepass") && dict["gradepass"] == "75.00000"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WithGradingWeight_WritesWeightOverrideAndAggregationCoef2()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradingWeight = 70;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("aggregationcoef2") && dict["aggregationcoef2"] == "0.70000" &&
                dict.ContainsKey("weightoverride") && dict["weightoverride"] == "1"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WithNullGradingWeight_WritesWeightOverrideAsZero()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradingWeight = null;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("aggregationcoef2") && dict["aggregationcoef2"] == "0.00000" &&
                dict.ContainsKey("weightoverride") && dict["weightoverride"] == "0"
            ));
        }


        [Test]
        public void WriteGradebook_GradeItem_WithGradeHidden_WritesCurrentTimestamp()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradeHidden = true;
            category.GradeHiddenUntil = null;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("hidden") && dict["hidden"] == "9876543210"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WithGradeHiddenUntil_WritesSpecificTimestamp()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            var specificDate = new DateTime(2025, 1, 1);
            category.GradeHiddenUntil = specificDate;
            _generalHelper.DateTimeToUnixTimestamp(specificDate).Returns("1735689600");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("hidden") && dict["hidden"] == "1735689600"
            ));
            _generalHelper.Received(1).DateTimeToUnixTimestamp(specificDate);
        }

        [Test]
        public void WriteGradebook_GradeItem_WithGradingLocked_WritesCurrentTimestamp()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradingLocked = true;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("locked") && dict["locked"] == "9876543210"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WithGradeDecimalPoints_WritesValue()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradeDecimalPoints = 3;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("decimals") && dict["decimals"] == "3"
            ));
        }

        [Test]
        public void WriteGradebook_GradeItem_WithNullGradeDecimalPoints_WritesNullPlaceholder()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            category.GradeDecimalPoints = null;
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(dict =>
                dict.ContainsKey("decimals") && dict["decimals"] == "$@NULL@$"
            ));
        }

        [Test]
        public void WriteGradebook_DisposesXmlService()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);

            // Assert
            _xmlService.Received(1).Dispose();
        }
    }
}
