using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage
{
    public class DragAndDropTextAndImageQuestion : Question
    {
        [Description("list of the possible choices")]
        public required List<TextAndImageDrag> Drags { get; set; } = new();

        [Description("list of the correct choices and their location")]
        public required List<TextAndImageDrop> Drops { get; set; } = new();

        [Description("whether the possible choices for the cloze text are shuffled")]
        public bool ShuffleAnswers { get; set; }

        [Description("Feedback shown after the question was answered correctly")]
        public string? CorrectResponseFeedback { get; set; }
        public FormattingFormat CorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Feedback shown after the question was answered partially correct")]
        public string? PartiallyCorrectResponseFeedback { get; set; }
        public FormattingFormat PartiallyCorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Feedback shown after the question was answered incorrectly")]
        public string? IncorrectResponseFeedback { get; set; }
        public FormattingFormat IncorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Shows the amount of correct answers")]
        public bool ShowNumCorrect { get; set; }

        [Description("Should the background of the dropzone be visible? If visible, it hides the original image behind it. Transparent = true/1")]
        public bool DropZoneVisibility { get; set; }

        [Description("Path to the background/main image")]
        public string? BackgroundImagePath { get; set; }

        [Description("optional, name for the background image")]
        public string? BackgroundImageName { get; set; }
    }
}
