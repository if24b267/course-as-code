using System.ComponentModel.DataAnnotations;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlAssignment : YamlActivity
    {
        [Required]
        public string? Intro { get; set; }
        public FormattingFormat? AssignmentTextFormat { get; set; }
        public bool? AlwaysShowDescription { get; set; }
        public bool? AllowSubmissionDrafts { get; set; }
        public bool? SendNotifications { get; set; }
        public bool? SendLateNotifications { get; set; }
        public bool? SendStudentNotifications { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CutoffDate { get; set; }
        public DateTime? GradingDueDate { get; set; }
        public DateTime? AllowSubmissionsFromDate { get; set; }
        public int? Grade { get; set; }
        public bool? CompletionSubmit { get; set; }
        public bool? RequireSubmissionStatement { get; set; }
        public bool? AllowTeamSubmissions { get; set; }
        public bool? RequireAllTeamMembersSubmit { get; set; }
        public bool? BlindMarking { get; set; }
        public bool? HideGrader { get; set; }
        public bool? RevealIdentities { get; set; }
        public string? AttemptReopenMethod { get; set; }
        public int? MaxAttempts { get; set; }
        public bool? MarkingWorkflow { get; set; }
        public bool? MarkingAllocation { get; set; }
        public bool? MarkingAnonymous { get; set; }
        public bool? PreventSubmissionNotInGroup { get; set; }
    }
}
