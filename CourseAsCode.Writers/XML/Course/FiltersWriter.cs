using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class FiltersWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };
    
    public void WriteFilters(string filePath = "course/filters.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("filters"); // <filters> starts here

            writer.WriteStartElement("filter_actives"); // <filter_actives> starts here 
            writer.WriteEndElement(); // </filter_actives> ends here 

            writer.WriteStartElement("filter_configs"); // <filter_configs> starts here
            writer.WriteEndElement(); // </filter_configs> ends here 

            writer.WriteEndElement(); // </filters> ends here 
            writer.WriteEndDocument();
        }
    }
}