using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions
{
    public abstract class Question
    {
        // ────────────────────────────────────────────────────────────────────────────
        //  Core – shared by all question types
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Unique identifier for the question")]
        public int QuestionId { get; set; }

        [Description("Absolute path of the YAML file defining this question")]
        public string? SourcePath { get; set; }

        [Description("Identifier for the context")]
        public int ContextId { get; set; }

        [Description("Unique identifier of the question bank to which the question belongs")]
        public int BankEntryId { get; set; }

        [Description("Category the question belongs to")]
        public required string Category { get; set; }

        [Description("Unique identifier of the category to which the question belongs")]
        public int CategoryId { get; set; }

        [Description("Name of the question as shown in the question bank")]
        public required string QuestionName { get; set; }

        [Description("Main body of the question presented to the learner")]
        public required string QuestionText { get; set; }

        [Description("Format in which QuestionText is provided")]
        public FormattingFormat QuestionTextFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Whether the question is ready for use (true) or a draft (false)")]
        public bool QuestionReady { get; set; } = true;

        [Description("Default mark if not overridden in a specific quiz activity")]
        public float DefaultMark { get; set; } = 1.0f;

        [Description("General feedback shown after the question is completed (identical for all responses)")]
        public string? GeneralFeedback { get; set; }

        [Description("Format in which GeneralFeedback is provided")]
        public FormattingFormat GeneralFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Hints offered when using interactive/adaptive behaviour")]
        public List<Hint>? Hints { get; set; }

        [Description("Penalty applied for each subsequent try in interactive/adaptive modes (0‑1)")]
        public float HintPenalty { get; set; } = 0.0f;

        [Description("Optional tags associated with the question for search/organisation")]
        public List<string>? Tags { get; set; }
    }
}
