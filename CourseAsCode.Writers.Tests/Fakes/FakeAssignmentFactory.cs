using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums.Grading;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public class FakeAssignmentFactory
    {
        /// <summary>
        /// Creates a basic assignment with predefined parameters and structure
        /// </summary>
        /// <param name="id">The ActivityId for the assignment</param>
        /// <returns>An assignment object with basic settings</returns>
        public static Assignment CreateBasicAssignment(int id)
        {
            return new Assignment
            {
                ActivityId = id,
                Name = "Sample Assignment",
                GradeType = GradeType.Value,
                MaxGrade = 100,
                MinGrade = 0,
                GradeToPass = 50,
                GradeMultiplicator = 1,
                ModuleId = 9,
                ModuleName = "assignment",
            };
        }
    }
}
