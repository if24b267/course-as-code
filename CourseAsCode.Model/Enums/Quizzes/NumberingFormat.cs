using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum NumberingFormat
{
    [Description("choices are numbered using letters, e.g. a, b, c, d")]
    AlphabeticLower,
    [Description("choices are numbered using letters, e.g. A, B, C, D")]
    AlphabeticUpper,
    [Description("choices are numbered using numbers, e.g. 1, 2, 3, 4")]
    Numeric,
    [Description("choices are numbered using roman numerals, e.g. i, ii, iii, iv")]
    RomanLower,
    [Description("choices are numbered using roman numerals, e.g. I, II, III, IV")]
    RomanUpper,
    [Description("choices are not numbered at all")]
    None
}