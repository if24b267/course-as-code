using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum SEBMode
{
    [Description("safe exam browser is not required for this quiz")]
    Disabled,
    [Description("safe exam browser is required for this quiz, the config is done manually in Moodle")]
    EnabledManualConfig,
    [Description("safe exam browser is required with uploaded configuration file")]
    EnabledUploadConfig,
    [Description("no specific configuration of SEB is enforced – the quiz can be attempted with any configuration")]
    EnabledClientConfig
}