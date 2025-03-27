using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class FileHelperTests
    {
        private FileHelper _fileHelper;

        [SetUp]
        public void SetUp()
        {
            _fileHelper = FileHelper.Instance;
        }

        [TestCase(FileArea.Overview, "overviewfiles")]
        [TestCase(FileArea.CourseContent, "content")]
        [TestCase((FileArea)999, "content")] // unknown value should default to "content"
        public void GetFileAreaFromEnum_ReturnsExpectedValue(FileArea fileArea, string expected)
        {
            // Act
            var result = _fileHelper.GetFileAreaFromEnum(fileArea);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(Component.Course, "course")]
        [TestCase(Component.Resource, "mod_resource")]
        [TestCase(Component.Forum, "mod_forum")]
        [TestCase(Component.Glossary, "mod_glossary")]
        [TestCase(Component.HtmlBlock, "block_html")]
        [TestCase((Component)999, "course")] // unknown value should default to "course"
        public void GetComponentFromEnum_ReturnsExpectedValue(Component component, string expected)
        {
            // Act
            var result = _fileHelper.GetComponentFromEnum(component);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(License.None, "unknown")]
        [TestCase(License.AllRightsReserved, "allrightsreserved")]
        [TestCase(License.PublicDomain, "public")]
        [TestCase(License.CreativeCommons, "cc-4.0")]
        [TestCase(License.CreativeCommonsNonCommercial, "cc-nc-4.0")]
        [TestCase(License.CreativeCommonsNoDerivatives, "cc-nd-4.0")]
        [TestCase(License.CreativeCommonsNonCommercialNoDerivatives, "cc-nc-nd-4.0")]
        [TestCase(License.CreativeCommonsNonCommercialShareAlike, "cc-nc-sa-4.0")]
        [TestCase(License.CreativeCommonsShareAlike, "cc-sa-4.0")]
        [TestCase((License)999, "unknown")] // unknown value should default to "unknown"
        public void GetLicenseFromEnum_ReturnsExpectedValue(License license, string expected)
        {
            // Act
            var result = _fileHelper.GetLicenseFromEnum(license);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
