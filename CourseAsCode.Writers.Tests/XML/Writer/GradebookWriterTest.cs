using System.Xml;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class GradebookWriterTest
    {
        private XmlWriterFactoryTest _testFactory;
        private IGeneralHelper _generalHelper;
        private ILogger<GradebookWriter> _testLogger;
        private GradebookWriter _gradebookWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _testLogger = Substitute.For<ILogger<GradebookWriter>>();

            // Setup default mock behaviors
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("13");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            _gradebookWriter = new GradebookWriter(_testFactory, _generalHelper, _testLogger);
        }

        [Test]
        public void WriteGradebook_GeneratesValidXml()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGradebook_RootElementIsGradebook()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("gradebook"));
        }

        [Test]
        public void WriteGradebook_ContainsAllRequiredElements()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var requiredElements = new[] { "attributes", "grade_categories", "grade_items", "grade_letters", "grade_settings" };

            foreach (var elementName in requiredElements)
            {
                var element = doc.DocumentElement?.SelectSingleNode(elementName);
                Assert.That(element, Is.Not.Null, $"The <{elementName}> element is missing");
            }
        }

        [Test]
        public void WriteGradebook_WithSingleGradeCategory_WritesGradeCategory()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeCategoryNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category");
            Assert.That(gradeCategoryNode, Is.Not.Null, "The <grade_category> element is missing");
            Assert.That(gradeCategoryNode.Attributes["id"].Value, Is.EqualTo("1"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var categoryNodes = doc.DocumentElement?.SelectNodes("grade_categories/grade_category");
            Assert.That(categoryNodes?.Count, Is.EqualTo(3));
        }

        [Test]
        public void WriteGradebook_WithSingleGradeCategory_WritesGradeItem()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var category = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Test Category");
            course.GradeCategories.Add(category);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItemNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item");
            Assert.That(gradeItemNode, Is.Not.Null, "The <grade_item> element is missing");
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeCategoryNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category");

            var requiredElements = new[]
            {
                "parent", "depth", "path", "fullname", "aggregation",
                "keephigh", "droplow", "aggregateonlygraded", "aggregateoutcomes",
                "timecreated", "timemodified", "hidden",
            };

            foreach (var elementName in requiredElements)
            {
                var element = gradeCategoryNode?.SelectSingleNode(elementName);
                Assert.That(element, Is.Not.Null, $"The <{elementName}> element is missing in grade_category");
            }
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
                "locked", "locktime", "needsupdate", "timecreated", "timemodified",
            };

            foreach (var elementName in requiredElements)
            {
                var element = gradeItemNode?.SelectSingleNode(elementName);
                Assert.That(element, Is.Not.Null, $"The <{elementName}> element is missing in grade_item");
            }
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var parentNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category/parent");
            Assert.That(parentNode?.InnerText, Is.EqualTo("$@NULL@$"));
        }

        [Test]
        public void WriteGradebook_GradeCategory_WithParent_WritesParentId()
        {
            // Arrange
            var course = Fakes.FakeCourseFactory.CreateBasicCourse();
            var parentCategory = FakeGradeCategoryFactory.CreateBasicGradeCategory(1, "Parent Category");
            var childCategory = FakeGradeCategoryFactory.CreateBasicGradeCategory(2, "Child Category");
            childCategory.Parent = parentCategory;
            course.GradeCategories.Add(parentCategory);
            course.GradeCategories.Add(childCategory);
            var dummyOutputPath = "dummyPath";

            // Act
            _gradebookWriter.WriteGradebook(course, dummyOutputPath);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var categoryNodes = doc.DocumentElement?.SelectNodes("grade_categories/grade_category");
            var childNode = categoryNodes?[1]; // Second category is the child
            var parentNode = childNode?.SelectSingleNode("parent");
            Assert.That(parentNode?.InnerText, Is.EqualTo("1"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var depthNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category/depth");
            Assert.That(depthNode?.InnerText, Is.EqualTo("1"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var pathNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category/path");
            Assert.That(pathNode?.InnerText, Is.EqualTo("/5/"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var categoryNodes = doc.DocumentElement?.SelectNodes("grade_categories/grade_category");
            var childDepthNode = categoryNodes?[1]?.SelectSingleNode("depth");
            Assert.That(childDepthNode?.InnerText, Is.EqualTo("2"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var categoryNodes = doc.DocumentElement?.SelectNodes("grade_categories/grade_category");
            var grandchildPathNode = categoryNodes?[2]?.SelectSingleNode("path");
            Assert.That(grandchildPathNode?.InnerText, Is.EqualTo("/4/2/1/"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var dropLowNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category/droplow");
            Assert.That(dropLowNode?.InnerText, Is.EqualTo("2"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var dropLowNode = doc.DocumentElement?.SelectSingleNode("grade_categories/grade_category/droplow");
            Assert.That(dropLowNode?.InnerText, Is.EqualTo("0"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var itemTypeNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/itemtype");
            Assert.That(itemTypeNode?.InnerText, Is.EqualTo("course"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var itemNodes = doc.DocumentElement?.SelectNodes("grade_items/grade_item");
            var childItemTypeNode = itemNodes?[1]?.SelectSingleNode("itemtype");
            Assert.That(childItemTypeNode?.InnerText, Is.EqualTo("category"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeMaxNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/grademax");
            Assert.That(gradeMaxNode?.InnerText, Is.EqualTo("150.00000"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradePassNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/gradepass");
            Assert.That(gradePassNode?.InnerText, Is.EqualTo("75.00000"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var aggregationCoef2Node = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/aggregationcoef2");
            Assert.That(aggregationCoef2Node?.InnerText, Is.EqualTo("0.70000"));
            var weightOverrideNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/weightoverride");
            Assert.That(weightOverrideNode?.InnerText, Is.EqualTo("1"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var aggregationCoef2Node = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/aggregationcoef2");
            Assert.That(aggregationCoef2Node?.InnerText, Is.EqualTo("0.00000"));
            var weightOverrideNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/weightoverride");
            Assert.That(weightOverrideNode?.InnerText, Is.EqualTo("0"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var hiddenNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/hidden");
            Assert.That(hiddenNode?.InnerText, Is.EqualTo("9876543210"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var hiddenNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/hidden");
            Assert.That(hiddenNode?.InnerText, Is.EqualTo("1735689600"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var lockedNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/locked");
            Assert.That(lockedNode?.InnerText, Is.EqualTo("9876543210"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var decimalsNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/decimals");
            Assert.That(decimalsNode?.InnerText, Is.EqualTo("3"));
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
            var output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var decimalsNode = doc.DocumentElement?.SelectSingleNode("grade_items/grade_item/decimals");
            Assert.That(decimalsNode?.InnerText, Is.EqualTo("$@NULL@$"));
        }
    }
}
