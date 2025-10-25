using System.ComponentModel;
using CourseAsCode.Model.Enums.Grading;

namespace CourseAsCode.Model.Grading.Interfaces
{
    /// <summary>
    /// Interface for activities that can be graded
    /// </summary>
    public interface IGradeable
    {
        [Description("the name with which the activity will be displayed in the gradebook")]
        string? GradeName { get; set; }

        [Description("general information about the grade item; optional and will NOT be displayed in the gradebook")]
        string? GradeInfo { get; set; }

        [Description("Minimum required grade (in points) to pass the activity")]
        float? GradeToPass { get; set; }

        [Description("how the activity is graded")]
        GradeType GradeType { get; set; }

        [Description("specifies the number of decimal points to display for the grade | has no effect on grade calculations, which are made with an accuracy of 5 decimal places")]
        int? GradeDecimalPoints { get; set; }

        [Description("Factor by which the grade for this activity will be multiplied, with a maximum value of the maximum grade | For example, if the multiplicator is 2 and the maximum grade is 100, then all grades less than 50 are doubled, and all grades 50 and above are changed to 100")]
        float? GradeMultiplicator { get; set; }

        [Description("Number that will be added to every grade for this activity, after the Multiplicator is applied. (can be negative)")]
        float? GradeOffset { get; set; }

        [Description("whether this activity should be hidden from students in the gradebook")]
        bool GradeHidden { get; set; }

        [Description("An optional date after which the activity will no longer be hidden in the gradebook (i.e. a grades release date)")]
        DateTime? GradeHiddenUntil { get; set; }

        [Description("Whether or not to lock the grading for this activity. A locked grade item doesn't accept changes to its settings or grades from its related module")]
        bool GradingLocked { get; set; }

        [Description("A date after which the locking will be effective, usually after the grades release date.")]
        DateTime? GradingLockedAfter { get; set; }
    }
}
