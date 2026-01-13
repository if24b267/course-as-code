using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class TextAndImageDragModel
    {
        public int ID { get; set; }
        public int Number { get; set; } = 0;
        public int DragGroup { get; set; } = 1;
        public bool Infinite { get; set; } = false;
        public string? Label { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        public string? ImageName { get; set; } = string.Empty;
    }

    public class TextAndImageDropModel
    {
        public int ID { get; set; }
        public int Number { get; set; } = 0;
        public int XLeft { get; set; }
        public int YTop { get; set; }
        public int Choice { get; set; } = 0;
        public string? Label { get; set; } = string.Empty;
    }
}
