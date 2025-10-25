using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public static class FakeGradeCategoryFactory
    {
        /// <summary>
        /// Creates a basic grade category with predefined parameters and structure
        /// </summary>
        /// <param name="id">The unique identifier for the grade category</param>
        /// <param name="name">The name of the grade category</param>
        /// <param name="maxGrade">The maximum number of points acheivable in the grade category</param>
        /// <param name="gradeToPass">The required number of points in the grade category to pass</param>
        /// <returns></returns>
        public static GradeCategory CreateBasicGradeCategory(int id, string name = "Unit Test Grade Category", int maxGrade = 100, int gradeToPass = 60)
        {
            return new GradeCategory()
            {
                GradeCategoryId = id,
                GradeName = name,
                AggregationType = GradeAggregation.Natural,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                MaxGrade = maxGrade,
                GradeToPass = gradeToPass,
            };
        }
    }
}
