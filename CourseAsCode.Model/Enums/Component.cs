namespace CourseAsCode.Model.Enums;

/// <summary>
/// the component refers to the subsystem or plugin responsible for the file
/// </summary>
public enum Component
{
    Course, // files managed by the course itself, e.g. the course logo
    Resource, // files managed by a file resource within the course
    Forum, // files managed by a forum within the course
    Glossary, // files managed by a glossary within the course
    HtmlBlock, // files managed by an HTML Block within the course
    Folder, // files managed by a folder resource within the course
}
