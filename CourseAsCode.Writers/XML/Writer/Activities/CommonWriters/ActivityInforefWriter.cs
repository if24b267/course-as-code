using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityInforefWriter : XmlWriterBase
{
    /// <summary>
    /// writes the inforef.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
    public void WriteInforefLabel(Model.Course course, string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "inforef.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("inforef");

        foreach (Section section in course.Sections)
        {
            foreach (Activity activity in section.Activities)
            {
                if (activity is MoodleFile moodleFile)
                {
                    // TODO: Refactor this to only write fileref once - even if there are multiple files
                    writer.WriteStartElement("fileref");
                    writer.WriteStartElement("file");
                    
                    writer.WriteElementString("id", moodleFile.ActivityId.ToString());
                    
                    writer.WriteEndElement(); // Close <file>
                    writer.WriteEndElement(); // Close <fileref>
                }
            }
        }

        writer.WriteEndElement(); // Close <events>

        writer.WriteEndDocument();
    }
}