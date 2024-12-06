namespace CourseAsCode.Model.Enums;

/// <summary>
/// A context in Moodle defines the "space" or "level" where specific roles, permissions, or settings apply. Contexts are hierarchical, meaning they form a tree-like structure, with permissions inheriting downwards unless overridden.
/// </summary>
public enum Context
{
    System = 10,
    User = 30,
    CourseCategory = 40,
    Course = 50,
    Module = 70,
    Block = 80
    // These values are based on the Moodle context levels, see https://docs.moodle.org/405/en/Context#Context_Level_constants
}