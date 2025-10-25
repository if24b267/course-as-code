using CourseAsCode.Model.Activities.Resources;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public class FakeUrlFactory
    {
        /// <summary>
        /// Creates a basic URL with predefined parameters and structure
        /// </summary>
        /// <param name="id">The ActivityId for the URL</param>
        /// <returns>A URL object with preset properties</returns>
        public static URL CreateBasicUrl(int id)
        {
            return new URL
            {
                ActivityId = id,
                Name = "Test URL",
                Description = "This is a test URL description",
                ExternalUrl = "https://example.com",
                ModuleId = 101,
                ModuleName = "url",
            };
        }
    }
}
