using CourseAsCode.Model.Questions;

namespace CourseAsCode.Readers.Helper
{
    // TODO: change this so values are being dependency injected
    public static class ValueKeeper
    {
        public static string? CoursePath { get; set; }

        // currently only being counted up from 1
        public static int CurrentActivityId { get; set; }

        // currently only being counted up from 1
        public static int CurrentSectionId { get; set; }

        // currently only being counted up from 1
        public static int CurrentModuleId { get; set; }
        public static int CurrentSubwikiId { get; set; } = 1;
        public static int CurrentWikiPageId { get; set; } = 1;

        public static List<Question> Questions { get; set; }
    }
}
