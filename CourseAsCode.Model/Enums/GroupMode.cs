namespace CourseAsCode.Model.Enums;

public enum GroupMode
{
    NoGroups = 0, // no groups are used in the course
    SeparateGroups = 1, // students are divided into separate groups and can only see their own groups' work
    VisibleGroups = 2 // students are divided into separate groups but can see other groups' work
}