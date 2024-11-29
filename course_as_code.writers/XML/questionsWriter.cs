using System;
using System.Xml;

namespace course_as_code.writers.XML;

public class QuestionsWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteQuestions(string filePath = "questions.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("question_categories"); // Root element
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close 

            writer.WriteEndDocument();
        }
    }
}