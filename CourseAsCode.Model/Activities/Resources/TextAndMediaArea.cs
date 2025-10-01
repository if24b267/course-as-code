using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources;

public class TextAndMediaArea : Activity
{
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
        public string MoodleVersion { get; set; } = "2024100700";

        // TODO
    }
}
