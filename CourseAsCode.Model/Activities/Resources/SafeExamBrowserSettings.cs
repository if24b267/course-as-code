using System.ComponentModel;
using CourseAsCode.Model.Enums.Quizzes;

namespace CourseAsCode.Model.Activities.Resources;

public class SafeExamBrowserSettings
{
    [Description("Safe Exam Browser requirement mode")]
    public SEBMode RequireSEB { get; set; } = SEBMode.Disabled;

    [Description("If enabled, a button is shown on the quiz start page that allows the student to download the Safe Exam Browser")]
    public bool ShowDownloadButton { get; set; } = false;

    [Description("If set, an 'Exit Safe Exam Browser' link is shown after submission, allowing the student to exit SEB without entering a password")]
    public string? QuitLinkButton { get; set; }

    [Description("If enabled, users must confirm quitting the Safe Exam Browser when a quit link is detected")]
    public bool ConfirmQuitLink { get; set; } = false;

    [Description("If enabled, users can quit SEB via the task bar quit button (also via Ctrl+Q or window close)")]
    public bool EnableQuit { get; set; } = false;

    [Description("If set, a password is required to quit the Safe Exam Browser")]
    public string? QuitPassword { get; set; }

    [Description("If enabled, page reload is allowed during the exam")]
    public bool EnableReload { get; set; } = false;

    [Description("If enabled, the SEB task bar is displayed in the Safe Exam Browser window")]
    public bool ShowTaskBar { get; set; } = false;

    [Description("If enabled, a reload button is shown in the SEB task bar")]
    public bool ShowReloadButton { get; set; } = false;

    [Description("If enabled, the current time is displayed in the SEB task bar")]
    public bool ShowTime { get; set; } = false;

    [Description("If enabled, the keyboard layout selector is displayed in the SEB task bar")]
    public bool ShowKeyboardLayout { get; set; } = false;

    [Description("If enabled, a Wi-Fi control button is shown in the SEB task bar")]
    public bool ShowWifiControl { get; set; } = false;

    [Description("If enabled, audio controls are shown in the SEB task bar")]
    public bool EnableAudioControls { get; set; } = false;

    [Description("If enabled, spell checking is enabled in the Safe Exam Browser")]
    public bool EnableSpellCheck { get; set; } = false;

    [Description("If enabled, URLs will be filtered when loading web pages in SEB")]
    public bool EnableURLFiltering { get; set; } = false;

    [Description("If enabled, embedded content will also be filtered using the defined URL filter")]
    public bool FilterEmbeddedContent { get; set; } = false;

    [Description("List of URLs allowed to be accessed in the Safe Exam Browser")]
    public List<string>? AllowedURLs { get; set; }

    [Description("List of regular expression patterns for URLs allowed in the Safe Exam Browser")]
    public List<string>? AllowedURLsRegex { get; set; }

    [Description("List of URLs blocked from being accessed in the Safe Exam Browser")]
    public List<string>? BlockedURLs { get; set; }

    [Description("List of regular expression patterns for URLs blocked in the Safe Exam Browser")]
    public List<string>? BlockedURLsRegex { get; set; }

    [Description("List of browser exam keys (SHA256-hashed) allowed to access the quiz; if none provided, any configuration can be used")]
    public List<string>? AllowedExamKeys { get; set; }
}