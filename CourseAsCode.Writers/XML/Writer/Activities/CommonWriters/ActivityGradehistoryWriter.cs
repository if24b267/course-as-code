using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityGradehistoryWriter : XmlWriterBase
{
    /// <summary>
    /// writes the grade_history.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
    public void WriteGradehistory(Model.Course course, string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "grade_history.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);
        
        writer.WriteStartDocument();
        writer.WriteStartElement("grade_history");

        WriteGradeGradesSection(writer, course);

        writer.WriteEndElement(); // Close <grade_history>
        writer.WriteEndDocument();
    }


    /// <summary>
    /// writes the `grade_grades` section of the grade_history.xml file required for an activity
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="course">initialized Moodle Course</param>
    private void WriteGradeGradesSection(XmlWriter writer, Model.Course course)
    {
        writer.WriteStartElement("grade_grades");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close <grade_grades>
    }
}