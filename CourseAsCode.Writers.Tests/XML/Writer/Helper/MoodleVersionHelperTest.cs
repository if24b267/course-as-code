using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class MoodleVersionHelperTests
    {
        private MoodleVersionHelper _helper;

        [SetUp]
        public void SetUp()
        {
            _helper = MoodleVersionHelper.Instance;
        }

        [TestCase(MoodleVersion.Moodle451, "2024100701")]
        [TestCase(MoodleVersion.Moodle450, "2024100700")]
        [TestCase(MoodleVersion.Moodle445, "2024042205")]
        [TestCase(MoodleVersion.Moodle444, "2024042204")]
        [TestCase(MoodleVersion.Moodle443, "2024042203")]
        [TestCase(MoodleVersion.Moodle442, "2024042202")]
        [TestCase(MoodleVersion.Moodle441, "2024042201")]
        [TestCase(MoodleVersion.Moodle440, "2024042200")]
        [TestCase((MoodleVersion)999, "0000000000")] // invalid enum value
        public void GetMoodleVersionStringFromEnum_ReturnsExpectedString(MoodleVersion version, string expected)
        {
            // Act
            var result = _helper.GetMoodleVersionStringFromEnum(version);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(MoodleVersion.Moodle451, "4.5.1 (Build: 2024100701)")]
        [TestCase(MoodleVersion.Moodle450, "4.5 (Build: 2024100700)")]
        [TestCase(MoodleVersion.Moodle445, "4.4.5 (Build: 2024042205)")]
        [TestCase(MoodleVersion.Moodle444, "4.4.4 (Build: 2024042204)")]
        [TestCase(MoodleVersion.Moodle443, "4.4.3 (Build: 2024042203)")]
        [TestCase(MoodleVersion.Moodle442, "4.4.2 (Build: 2024042202)")]
        [TestCase(MoodleVersion.Moodle441, "4.4.1 (Build: 2024042201)")]
        [TestCase(MoodleVersion.Moodle440, "4.4 (Build: 2024042200)")]
        [TestCase((MoodleVersion)999, "1.0 (Build: 0000000000)")] // invalid enum value
        public void GetMoodleReleaseStringFromEnum_ReturnsExpectedString(MoodleVersion version, string expected)
        {
            // Act
            var result = _helper.GetMoodleReleaseStringFromEnum(version);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
