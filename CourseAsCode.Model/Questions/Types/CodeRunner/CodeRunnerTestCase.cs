using System.ComponentModel;

namespace CourseAsCode.Model.Questions.Types.CodeRunner;

public class CodeRunnerTestCase
{
    [Description("ID of the test case")]
    public int Id { get; set; }

    [Description("Test code or description")]
    public string TestCode { get; set; } = string.Empty;

    [Description("Test type (0 for normal)")]
    public int TestType { get; set; } = 0;

    [Description("Expected output from the test")]
    public string Expected { get; set; } = string.Empty;

    [Description("Use as example (0 for false)")]
    public bool UseAsExample { get; set; } = false;

    [Description("Display mode (SHOW, HIDE, HIDE_IF_FAIL, HIDE_IF_SUCCEED)")]
    public string Display { get; set; } = "SHOW";

    [Description("Hide rest if fail (0 for false)")]
    public bool HideRestIfFail { get; set; } = false;

    [Description("Mark for this test case")]
    public decimal Mark { get; set; } = 1.000m;

    [Description("Standard input for the test")]
    public string StdIn { get; set; } = string.Empty;

    [Description("Extra data for this test case")]
    public string Extra { get; set; } = string.Empty;
}