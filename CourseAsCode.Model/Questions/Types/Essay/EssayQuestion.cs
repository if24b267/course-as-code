using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.Essay
{
    public class EssayQuestion : Question
    {
        [Description("Format of the response box")]
        public string ResponseFormat { get; set; } = "editor";

        [Description("Are we required to enter the text?")]
        public bool ResponseRequired { get; set; } = true;

        [Description("Size of the input box in lines")]
        public int ResponseFieldLines { get; set; } = 15;

        [Description("Minimum word count")]
        public int? MinWordLimit { get; set; }

        [Description("Maximum word count")]
        public int? MaxWordLimit { get; set; }

        [Description("Number of allowed attachments (0, 1, 2, 3, unlimited)")]
        public int AttachmentsAllowed { get; set; } = 0;

        [Description("Number of attachments required to submit")]
        public int AttachmentsRequired { get; set; } = 0;

        [Description("Comma-separated list of allowed file types")]
        public string FileTypesList { get; set; } = string.Empty;

        [Description("Max size of attachments in bytes")]
        public long MaxBytes { get; set; } = 0;

        [Description("Information for graders")]
        public string GraderInfo { get; set; }

        public FormattingFormat GraderInfoFormat { get; set; } = FormattingFormat.HTML;

        [Description("Template text that appears in the editor for the student")]
        public string ResponseTemplate { get; set; }

        public FormattingFormat ResponseTemplateFormat { get; set; } = FormattingFormat.HTML;
    }
}
