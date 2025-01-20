using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class GradesWriterLabel : XmlWriterBase
{
    public void WriteGrade(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "grades.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("activity_gradebook");
        writer.WriteStartElement("grade_items");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <grade_items>
        writer.WriteStartElement("grade_letters");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <grade_letters>
        writer.WriteEndElement(); // Close <activity_gradebook>
        writer.WriteEndDocument();
    }
}