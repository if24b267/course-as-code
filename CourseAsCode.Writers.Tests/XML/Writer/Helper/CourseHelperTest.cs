using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class CourseHelperTests
    {
        private CourseHelper _courseHelper;

        [SetUp]
        public void SetUp()
        {
            _courseHelper = CourseHelper.Instance;
        }

        [TestCase(CourseFormat.CustomSections, "topics")]
        [TestCase(CourseFormat.WeeklySections, "weeks")]
        [TestCase(CourseFormat.SingleActivity, "singleactivity")]
        [TestCase(CourseFormat.Social, "social")]
        [TestCase((CourseFormat)999, "topics")] // Unknown value should default to "topics"
        public void GetCourseFormatFromEnum_ReturnsExpectedValue(CourseFormat courseFormat, string expected)
        {
            // Act
            var result = _courseHelper.GetCourseFormatFromEnum(courseFormat);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
