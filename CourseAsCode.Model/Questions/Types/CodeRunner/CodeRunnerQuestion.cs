using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.CodeRunner;

public class CodeRunnerQuestion : Question
{
    [Description("The type of code runner (e.g. java_program, c_program)")]
    public required string CodeRunnerType { get; set; }

    [Description("Prototype type (0 for standard)")]
    public int PrototypeType { get; set; } = 0;

    [Description("All or nothing grading (1 for true)")]
    public bool AllOrNothing { get; set; } = true;

    [Description("Penalty regime for subsequent tries (e.g. '10, 20, ...')")]
    public string PenaltyRegime { get; set; } = "10, 20, ...";

    // TODO: convert to enum?
    [Description("Enable precheck (0 = none, 1 = examples, 2 = selected, 3 = all)")]
    public int Precheck { get; set; } = 0;

    [Description("Hide check button (0 for false)")]
    public int HideCheck { get; set; } = 0;

    [Description("Show source code of prototypes (0 for false)")]
    public int ShowSource { get; set; } = 0;

    [Description("Number of lines in answer box")]
    public int AnswerBoxLines { get; set; } = 18;

    [Description("Number of columns in answer box")]
    public int AnswerBoxColumns { get; set; } = 100;

    [Description("Text to preload in answer box")]
    public string AnswerPreload { get; set; } = string.Empty;

    [Description("Extra data that doesn't vary per test case")]
    public string GlobalExtra { get; set; } = string.Empty;

    [Description("Solution code for the problem")]
    public string Answer { get; set; } = string.Empty;

    [Description("Validate on save (1 for true)")]
    public bool ValidateOnSave { get; set; } = true;

    [Description("Template parameters as JSON")]
    public string TemplateParams { get; set; } = string.Empty;

    [Description("Hoist template parameters (1 for true)")]
    public bool HoistTemplateParams { get; set; } = true;

    [Description("Extract code from JSON (1 for true)")]
    public bool ExtractCodeFromJson { get; set; } = true;

    [Description("Template parameters language")]
    public string TemplateParamsLang { get; set; } = "None";

    [Description("Template parameters evaluated per try (0 for false)")]
    public bool TemplateParamsEvalPerTry { get; set; } = false;

    [Description("Template parameters evaluated")]
    public string TemplateParamsEvald { get; set; } = "{}";

    [Description("Twig all (0 for false)")]
    public bool TwigAll { get; set; } = false;

    [Description("UI parameters")]
    public string UiParameters { get; set; } = string.Empty;

    [Description("Allow file attachments (0 for false)")]
    public int Attachments { get; set; } = 0;

    [Description("Required file attachments (0 for false)")]
    public int AttachmentsRequired { get; set; } = 0;

    [Description("Maximum file size (in bytes)")]
    public int MaxFileSize { get; set; } = 10240;

    [Description("File names regex")]
    public string FilenamesRegex { get; set; } = string.Empty;

    [Description("File names explanation")]
    public string FilenamesExplain { get; set; } = string.Empty;

    [Description("Display feedback (1 for true)")]
    public bool DisplayFeedback { get; set; } = true;

    [Description("Give up allowed (0 for false)")]
    public bool GiveUpAllowed { get; set; } = false;

    [Description("Test cases for the CodeRunner question")]
    public List<CodeRunnerTestCase> TestCases { get; set; } = new();
}
