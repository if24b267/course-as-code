using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage
{
    public class TextAndImageDrop
    {
        [Description("Rrefers to the drags, used to map correct answers and images")]
        public int ID { get; set; }

        [Description("Each Drag has a number, starting from 1, then 2, 3, 4 etc")]
        public int Number { get; set; }

        [Description("Determines the position of the correct answer in the X axis, starting from the left e.g. value 50 would move the answer 50px closer towards the right")]
        public int XLeft { get; set; }

        [Description("Determines the position of the correct answer in the Y axis, starting from the top e.g. value 50 would move the answer 50px closer towards the botttom")]
        public int YTop { get; set; }

        [Description("Uses the Drags assigned Number from the other model to map a correct answer")]
        public int Choice { get; set; }

        [Description("Simply creates a Label, mostly irrelevant, but can find a use in case an image cannot load")]
        public string? Label { get; set; } = string.Empty;
    }
}
