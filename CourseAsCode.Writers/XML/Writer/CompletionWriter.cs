using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class CompletionWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = false
    };


    /// <summary>
    /// stores the information about which users have completed the course; NOT IMPLEMENTED (yet?)
    /// </summary>
    public void WriteCompletion(Model.Course course, string outputPath = "completion.xml")
    {
        var filePath = Path.GetFullPath(Path.Combine(outputPath, "completion.xml"));
        using var writer = XmlWriter.Create(filePath, _settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("course_completion");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement();

        writer.WriteEndDocument();
    }
}