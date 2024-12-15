using System.Xml;

namespace CourseAsCode.Writers.XML;

public class QuestionsWriter : XmlWriterBase
{
    public void WriteQuestions(string filePath = "questions.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("question_categories");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <question_categories>
            writer.WriteEndDocument();
        }
    }
}
