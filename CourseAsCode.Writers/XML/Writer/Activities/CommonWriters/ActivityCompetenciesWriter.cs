using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityCompetenciesWriter : XmlWriterBase
{
    /// <summary>
    /// writes the competencies.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
    public void WriteCompetencies(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "competencies.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("course_module_competencies");
        writer.WriteStartElement("competencies");
        
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        
        writer.WriteEndElement(); // Close <competencies>
        writer.WriteEndElement(); // Close <course_module_competenciesS>

        writer.WriteEndDocument();
    }
}