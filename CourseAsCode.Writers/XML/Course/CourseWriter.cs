using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CourseWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    
    public void WriteCourse(string filePath = "course/course.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            
            // TODO: Refactor this code to use variables from the course object (internal representation of the course)
            // TODO: Implement default values using the ternary operator
            // e.g. writer.WriteAttributeString("id", course.Id ? 2);

            // root element <course> with attributes
            writer.WriteStartElement("course");
            writer.WriteAttributeString("id", "2");
            writer.WriteAttributeString("contextid", "14");

            // child elements of <course>
            writer.WriteElementString("shortname", "EmpCo");
            writer.WriteElementString("fullname", "EmptyCourse");
            writer.WriteElementString("idnumber", "");
            writer.WriteElementString("summary", "<p>Test Summary</p>");
            writer.WriteElementString("summaryformat", "1");
            writer.WriteElementString("format", "topics");
            writer.WriteElementString("showgrades", "1");
            writer.WriteElementString("newsitems", "0");
            writer.WriteElementString("startdate", "1731024000");
            writer.WriteElementString("enddate", "1762560000");
            writer.WriteElementString("marker", "0");
            writer.WriteElementString("maxbytes", "0");
            writer.WriteElementString("legacyfiles", "0");
            writer.WriteElementString("showreports", "0");
            writer.WriteElementString("visible", "1");
            writer.WriteElementString("groupmode", "0");
            writer.WriteElementString("groupmodeforce", "0");
            writer.WriteElementString("defaultgroupingid", "0");
            writer.WriteElementString("lang", "");
            writer.WriteElementString("theme", "");
            writer.WriteElementString("timecreated", "1731001588");
            writer.WriteElementString("timemodified", "1731001588");
            writer.WriteElementString("requested", "0");
            writer.WriteElementString("showactivitydates", "1");
            writer.WriteElementString("showcompletionconditions", "1");
            writer.WriteElementString("pdfexportfont", "$@NULL@$");
            writer.WriteElementString("enablecompletion", "1");
            writer.WriteElementString("completionnotify", "0");

            // <category> element
            // TODO: Check if multiple categories are allowed and refactor this code to use a loop
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", "Category 1");
            writer.WriteElementString("description", "$@NULL@$");
            writer.WriteEndElement(); // Close <category>

            // <tags> element
            // TODO: Refactor this code to use a loop
            writer.WriteStartElement("tags");
            writer.WriteStartElement("tag");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", "testtag");
            writer.WriteElementString("rawname", "testTag");
            writer.WriteEndElement(); // Close <tag>
            writer.WriteEndElement(); // Close <tags>
            
            writer.WriteStartElement("customfields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <customfields>

            // <courseformatoptions> element
            writer.WriteStartElement("courseformatoptions");
            
            // TODO: Refactor this code to use a loop
            // Add first <courseformatoption>
            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", "topics");
            writer.WriteElementString("sectionid", "0");
            writer.WriteElementString("name", "coursedisplay");
            writer.WriteElementString("value", "0");
            writer.WriteEndElement(); // Close first <courseformatoption>

            // Add second <courseformatoption>
            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", "topics");
            writer.WriteElementString("sectionid", "0");
            writer.WriteElementString("name", "hiddensections");
            writer.WriteElementString("value", "1");
            writer.WriteEndElement(); // Close second <courseformatoption>

            writer.WriteEndElement(); // Close <courseformatoptions>

            writer.WriteEndElement(); // Close <course>

            writer.WriteEndDocument();
        }
    }
}