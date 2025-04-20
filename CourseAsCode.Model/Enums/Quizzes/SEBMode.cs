using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum SEBMode
{
    [Description("safe exam browser is not required for this quiz")]
    Disabled = 0,
    [Description("safe exam browser is required for this quiz, the config is done manually in Moodle")]
    EnabledManualConfig = 1,
    [Description("safe exam browser is required with uploaded configuration file")]
    EnabledUploadConfig = 3, // Educated Guess - not confirmed
    [Description("no specific configuration of SEB is enforced – the quiz can be attempted with any configuration")]
    EnabledClientConfig = 4
}