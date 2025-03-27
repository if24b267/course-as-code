using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class RolesHelperTests
    {
        private RolesHelper _rolesHelper;

        [SetUp]
        public void SetUp()
        {
            _rolesHelper = new RolesHelper();
        }

        [TestCase(Archetype.Manager, "manager")]
        [TestCase(Archetype.CourseCreator, "coursecreator")]
        [TestCase(Archetype.EditingTeacher, "editingteacher")]
        [TestCase(Archetype.NonEditingTeacher, "teacher")]
        [TestCase(Archetype.Student, "student")]
        [TestCase(Archetype.Guest, "guest")]
        [TestCase(Archetype.AuthenticatedUser, "user")]
        [TestCase(Archetype.AuthenticatedUserOnFrontpage, "frontpage")]
        [TestCase((Archetype)999, "user")] // invalid enum value should default to "user"
        public void GetArchetypeFromEnum_ReturnsExpectedValue(Archetype archetype, string expected)
        {
            // Act
            var result = _rolesHelper.GetArchetypeFromEnum(archetype);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}