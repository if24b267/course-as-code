using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Choice
{
    public class Choice : CourseAsCode.Model.Activities.Activity
    {
        private static readonly Random _rnd = new Random();

        [Description("format of the intro/description")]
        public FormattingFormat IntroFormat { get; set; } = FormattingFormat.HTML;

        [Description("publish choices to students")]
        public bool Publish { get; set; } = false;

        [Description("show results to students")]
        public bool ShowResults { get; set; } = false;

        [Description("display mode")]
        public int Display { get; set; } = 0;

        [Description("allow updating previous answers")]
        public bool AllowUpdate { get; set; } = false;

        [Description("allow multiple answers per user")]
        public bool AllowMultiple { get; set; } = false;

        [Description("show unanswered option")]
        public bool ShowUnanswered { get; set; } = false;

        [Description("limit number of answers per option")]
        public bool LimitAnswers { get; set; } = false;

        [Description("time when the choice opens")]
        public DateTime? TimeOpen { get; set; }

        [Description("time when the choice closes")]
        public DateTime? TimeClose { get; set; }

        [Description("time the choice was last modified")]
        public DateTime TimeModified { get; set; } = DateTime.Now;

        [Description("whether completion requires a submission")]
        public bool CompletionSubmit { get; set; } = false;

        [Description("show preview to students")]
        public bool ShowPreview { get; set; } = false;

        [Description("include inactive options in results")]
        public bool IncludeInactive { get; set; } = false;

        [Description("show available information")]
        public bool ShowAvailable { get; set; } = false;

        [Description("options available in this choice activity")]
        public List<ChoiceOption> Options { get; set; } = new();

        [Description("recorded answers - typically empty in backups")]
        public List<ChoiceAnswer> Answers { get; set; } = new();

        public SensibleDefaultsChoice SensibleDefaults { get; set; } = new SensibleDefaultsChoice();
        public SystemDefaultsChoice SystemDefaults { get; set; } = new SystemDefaultsChoice();

        public class SensibleDefaultsChoice
        {
            public string? SomeSetting { get; set; } = null; 
        }

        public class SystemDefaultsChoice
        {
            public int ModuleId { get; set; } = _rnd.Next(1, 1000);
            public string ModuleName { get; set; } = "choice";
        }

        public class ChoiceOption
        {
            [Description("unique id of the option")]
            public int Id { get; set; }

            [Description("text shown for the option")]
            public string Text { get; set; } = string.Empty;

            [Description("maximum answers for this option (0 = unlimited)")]
            public int MaxAnswers { get; set; } = 0;

            [Description("time the option was last modified")]
            public DateTime TimeModified { get; set; } = DateTime.Now;
        }

        public class ChoiceAnswer
        {
            [Description("unique id of the answer record")]
            public int Id { get; set; }

            [Description("selected option id")]
            public int OptionId { get; set; }

            [Description("user id who answered")]
            public int UserId { get; set; }

            [Description("time the answer was created")]
            public DateTime TimeCreated { get; set; } = DateTime.Now;
        }
    }
}
