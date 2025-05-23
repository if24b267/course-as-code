using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources;

public class TextAndMediaArea : Activity
{
    private static Random _rnd = new Random(); // This sucks, we should use a service class instead

    public string IndexTitle { get; set; }

    // TODO: how exactly is this saved in moodle
    public string Content { get; set; }

    public FormattingFormat FormattingFormat { get; set; } = FormattingFormat.MoodleAutoFormat;

    public SensibleDefaultsTextAndMediaArea SensibleDefaults { get; set; } = new SensibleDefaultsTextAndMediaArea();
    public SystemDefaultsTextAndMediaArea SystemDefaults { get; set; } = new SystemDefaultsTextAndMediaArea();

    public class SensibleDefaultsTextAndMediaArea
    {
        // TODO
    }

    public class SystemDefaultsTextAndMediaArea
    {
        public int ModuleId { get; set; } = _rnd.Next(1, 1000);
        public string ModuleName { get; set; } = "label";
        public string MoodleVersion { get; set; } = "2024100700";

        // TODO
    }
}
