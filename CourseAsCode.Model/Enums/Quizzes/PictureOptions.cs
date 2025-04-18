using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum PictureOptions
{
    [Description("no image")]
    NoImage,
    [Description("small image")]
    SmallImage,
    [Description("large image")]
    LargeImage
}