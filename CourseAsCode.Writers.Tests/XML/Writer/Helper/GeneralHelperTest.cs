using System.Text.RegularExpressions;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class GeneralHelperTests
    {
        private GeneralHelper _generalHelper;

        // Dummy enum for testing EnumIntegerToString
        private enum DummyEnum
        {
            Zero,
            One,
            Two,
        }

        [SetUp]
        public void SetUp()
        {
            _generalHelper = GeneralHelper.Instance;
        }

        [Test]
        public void GenerateBackupName_ReturnsValidBackupNameFormat()
        {
            // Arrange: Create a dummy Course with known values
            var course = new Model.Course
            {
                ShortName = "TestCourse",
                SystemDefaults = new Model.Course.SystemDefaultsCourse { Id = 123 },
            };

            // Act
            var backupName = _generalHelper.GenerateBackupName(course);

            // Assert:
            // Expected format: backup-moodle2-course-<courseId>-<shortname>-<yyyyMMdd>-<HHmm>.mbz
            string pattern = $@"^backup-moodle2-course-{course.SystemDefaults.Id}-{course.ShortName}-\d{{8}}-\d{{4}}\.mbz$";
            Assert.IsTrue(
                Regex.IsMatch(backupName, pattern),
                $"Backup name '{backupName}' does not match the expected pattern");
        }

        [Test]
        public void DateTimeToUnixTimestamp_ConvertsCorrectly()
        {
            // Arrange: Use a fixed date in winter (CET, which is UTC+1)
            var dateTime = new DateTime(2022, 1, 1, 12, 0, 0);

            // Act
            var result = _generalHelper.DateTimeToUnixTimestamp(dateTime);

            // Expected: 2022-01-01 12:00 CET is 2022-01-01 11:00 UTC.
            // Unix timestamp for 2022-01-01 11:00 UTC is 1641034800.
            Assert.That(result, Is.EqualTo("1641034800"));
        }

        [Test]
        public void EnumIntegerToString_ReturnsCorrectString()
        {
            // Act
            var result = _generalHelper.EnumIntegerToString(DummyEnum.One);

            // Assert: DummyEnum.One has underlying value 1
            Assert.That(result, Is.EqualTo("1"));
        }

        [TestCase(Language.English, "en")]
        [TestCase(Language.German, "de")]
        [TestCase(null, "")]
        public void GetLanguageFromEnum_ReturnsExpectedValue(Language? language, string expected)
        {
            // Act
            var result = _generalHelper.GetLanguageFromEnum(language);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(TimeZone.EuropeBerlin, "Europe/Berlin")]
        [TestCase(TimeZone.EuropeVienna, "Europe/Vienna")]
        [TestCase(TimeZone.EuropeZurich, "Europe/Zurich")]
        [TestCase((TimeZone)999, "99")]
        public void GetTimeZoneFromEnum_ReturnsExpectedValue(TimeZone timeZone, string expected)
        {
            // Act
            var result = _generalHelper.GetTimeZoneFromEnum(timeZone);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
