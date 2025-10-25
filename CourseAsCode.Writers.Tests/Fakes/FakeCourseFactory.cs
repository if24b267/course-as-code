using CourseAsCode.Model;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public static class FakeCourseFactory
    {
        /// <summary>
        /// Creates a basic course with predefined parameters and structure
        /// </summary>
        /// <returns>A Course object with basic settings</returns>
        public static Course CreateBasicCourse()
        {
            return new Course();
        }
    }
}
