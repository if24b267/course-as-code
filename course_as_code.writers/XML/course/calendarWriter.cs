using System;
using System.Xml;

namespace course_as_code.writers.XML.course;

public class CalendarWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };
    
    public void WriteCalendar(string filePath = "course/calendar.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            
            writer.WriteStartElement("events");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close the <events> tag
            
            writer.WriteEndDocument();
        }
    }
}