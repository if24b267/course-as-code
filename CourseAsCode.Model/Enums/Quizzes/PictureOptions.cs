using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum PictureOptions
{
    [Description("no image")]
    NoImage = 0,
    [Description("small image")]
    SmallImage = 1,
    [Description("large image")]
    LargeImage = 2
}