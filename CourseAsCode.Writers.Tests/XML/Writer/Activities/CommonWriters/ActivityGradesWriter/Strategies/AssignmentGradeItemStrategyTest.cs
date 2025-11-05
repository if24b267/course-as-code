using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    [TestFixture]
    public class AssignmentGradeItemStrategyTest
    {
        private IGeneralHelper _generalHelper;
        private IXmlService _xmlService;
        private AssignmentGradeItemStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            // Substitutes for Dependencies
            _generalHelper = Substitute.For<IGeneralHelper>();
            _xmlService = Substitute.For<IXmlService>();

            // Setup default mock behaviors
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("1");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            _strategy = new AssignmentGradeItemStrategy(_generalHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _xmlService.Dispose();
        }

        [Test]
        public void WriteGradeItems_CreatesGradeItemWithCorrectId()
        {
            // Arrange
            int assignmentId = 123;
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(assignmentId);

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElementWithAttributes("grade_item",
                Arg.Is<Dictionary<string, string>>(d => d["id"] == assignmentId.ToString()));
        }

        [Test]
        public void WriteGradeItems_WritesAllRequiredElements()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d =>
                    d.ContainsKey("categoryid") &&
                    d.ContainsKey("itemname") &&
                    d.ContainsKey("itemtype") &&
                    d.ContainsKey("itemmodule") &&
                    d.ContainsKey("iteminstance") &&
                    d.ContainsKey("itemnumber") &&
                    d.ContainsKey("iteminfo") &&
                    d.ContainsKey("idnumber") &&
                    d.ContainsKey("calculation") &&
                    d.ContainsKey("gradetype") &&
                    d.ContainsKey("grademax") &&
                    d.ContainsKey("grademin") &&
                    d.ContainsKey("scaleid") &&
                    d.ContainsKey("outcomeid") &&
                    d.ContainsKey("gradepass") &&
                    d.ContainsKey("multfactor") &&
                    d.ContainsKey("plusfactor") &&
                    d.ContainsKey("aggregationcoef") &&
                    d.ContainsKey("aggregationcoef2") &&
                    d.ContainsKey("weightoverride") &&
                    d.ContainsKey("sortorder") &&
                    d.ContainsKey("display") &&
                    d.ContainsKey("decimals") &&
                    d.ContainsKey("hidden") &&
                    d.ContainsKey("locked") &&
                    d.ContainsKey("locktime") &&
                    d.ContainsKey("needsupdate") &&
                    d.ContainsKey("timecreated") &&
                    d.ContainsKey("timemodified")));
        }

        [Test]
        public void WriteGradeItems_UsesGradeNameWhenAvailable()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeName = "Custom Grade Name";

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["itemname"] == "Custom Grade Name"));
        }

        [Test]
        public void WriteGradeItems_FallsBackToActivityNameWhenGradeNameIsNull()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeName = null;
            assignment.Name = "Fallback Name";

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["itemname"] == "Fallback Name"));
        }

        [Test]
        public void WriteGradeItems_WritesGradeInfoWhenAvailable()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeInfo = "Important grading information";

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["iteminfo"] == "Important grading information"));
        }

        [Test]
        public void WriteGradeItems_FormatsMaxGradeCorrectly()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.MaxGrade = 100.12345f;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["grademax"] == "100.12345"));
        }

        [Test]
        public void WriteGradeItems_FormatsMinGradeCorrectly()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.MinGrade = 10.5f;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["grademin"] == "10.50000"));
        }

        [Test]
        public void WriteGradeItems_UsesZeroForNullMinGrade()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.MinGrade = null;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["grademin"] == "0.00000"));
        }

        [Test]
        public void WriteGradeItems_FormatsGradeToPassCorrectly()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeToPass = 60.5f;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["gradepass"] == "60.50000"));
        }

        [Test]
        public void WriteGradeItems_FormatsGradeMultiplicatorCorrectly()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeMultiplicator = 1.5f;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["multfactor"] == "1.50000"));
        }

        [Test]
        public void WriteGradeItems_FormatsGradeOffsetCorrectly()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeOffset = 5.5f;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["plusfactor"] == "5.50000"));
        }

        [Test]
        public void WriteGradeItems_CalculatesWeightOverrideCorrectly_WhenGradingWeightIsSet()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradingWeight = 75;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d =>
                    d["weightoverride"] == "1" &&
                    d["aggregationcoef2"] == "0.75000"));
        }

        [Test]
        public void WriteGradeItems_SetsWeightOverrideToZero_WhenGradingWeightIsNull()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradingWeight = null;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d =>
                    d["weightoverride"] == "0" &&
                    d["aggregationcoef2"] == "0.00000"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradeHiddenUntil()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            var specificDate = new DateTime(2025, 1, 1);
            assignment.GradeHiddenUntil = specificDate;
            _generalHelper.DateTimeToUnixTimestamp(specificDate).Returns("1735689600");

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _generalHelper.Received(1).DateTimeToUnixTimestamp(specificDate);
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["hidden"] == "1735689600"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradeHiddenTrue()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeHidden = true;
            assignment.GradeHiddenUntil = null;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["hidden"] == "9876543210"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradeNotHidden()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradeHidden = false;
            assignment.GradeHiddenUntil = null;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["hidden"] == "0"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradingLocked()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradingLocked = true;
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["locked"] == "9876543210"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradingNotLocked()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            assignment.GradingLocked = false;

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["locked"] == "0"));
        }

        [Test]
        public void WriteGradeItems_HandlesGradingLockedAfter()
        {
            // Arrange
            var assignment = FakeAssignmentFactory.CreateBasicAssignment(1);
            var lockDate = new DateTime(2025, 6, 1);
            assignment.GradingLockedAfter = lockDate;
            _generalHelper.DateTimeToUnixTimestamp(lockDate).Returns("1748736000");

            // Act
            _strategy.WriteGradeItems(_xmlService, assignment);

            // Assert
            _generalHelper.Received(1).DateTimeToUnixTimestamp(lockDate);
            _xmlService.Received(1).WriteElements(
                Arg.Is<Dictionary<string, string>>(d => d["locktime"] == "1748736000"));
        }

        [Test]
        public void WriteGradeItems_DoesNothing_WhenActivityIsNotAssignment()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _strategy.WriteGradeItems(_xmlService, urlActivity);

            // Assert
            _xmlService.DidNotReceive().WriteElementWithAttributes(
                Arg.Any<string>(),
                Arg.Any<Dictionary<string, string>>());
            _xmlService.DidNotReceive().WriteElements(
                Arg.Any<Dictionary<string, string>>());
            _xmlService.DidNotReceive().CloseElement();
        }
    }
}
