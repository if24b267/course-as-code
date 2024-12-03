using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GradebookWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteGradebook(string filePath = "gradebook.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("gradebook"); // Root element
            
            writer.WriteStartElement("attributes");
            writer.WriteString("");
            writer.WriteEndElement(); // Close </attributes>

            writer.WriteStartElement("grade_categories");
            
            writer.WriteStartElement("grade_category");
            writer.WriteAttributeString("id", "3");

            writer.WriteElementString("parent", "$@NULL@$");
            writer.WriteElementString("depth", "1");
            writer.WriteElementString("path", "/3/");
            writer.WriteElementString("fullname", "?");
            writer.WriteElementString("aggregation", "13");
            writer.WriteElementString("keephigh", "0");
            writer.WriteElementString("droplow", "0");
            writer.WriteElementString("aggregateonlygraded", "1");
            writer.WriteElementString("aggregateoutcomes", "0");
            writer.WriteElementString("timecreated", "1731353585");
            writer.WriteElementString("timemodified", "1731353585");
            writer.WriteElementString("hidden", "0");
            
            writer.WriteEndElement(); //close </grade_category>
            writer.WriteEndElement(); // Close </grade_categories>

            writer.WriteStartElement("grade_items");

            writer.WriteStartElement("grade_item");
            writer.WriteAttributeString("id", "3");
            writer.WriteElementString("categoryid", "$@NULL@$");
            writer.WriteElementString("itemname", "$@NULL@$");
            writer.WriteElementString("itemtype", "course");
            writer.WriteElementString("itemmodule", "$@NULL@$");
            writer.WriteElementString("iteminstance", "3");
            writer.WriteElementString("itemnumber", "$@NULL@$");
            writer.WriteElementString("iteminfo", "$@NULL@$");
            writer.WriteElementString("idnumber", "$@NULL@$");
            writer.WriteElementString("calculation", "$@NULL@$");
            writer.WriteElementString("gradetype", "1");
            writer.WriteElementString("grademax", "0.00000");
            writer.WriteElementString("grademin", "0.00000");
            writer.WriteElementString("scaleid", "$@NULL@$");
            writer.WriteElementString("outcomeid", "$@NULL@$");
            writer.WriteElementString("gradepass", "0.00000");
            writer.WriteElementString("multfactor", "1.00000");
            writer.WriteElementString("plusfactor", "0.00000");
            writer.WriteElementString("aggregationcoef", "0.00000");
            writer.WriteElementString("aggregationcoef2", "0.00000");
            writer.WriteElementString("weightoverride", "0");
            writer.WriteElementString("sortorder", "1");
            writer.WriteElementString("display", "0");
            writer.WriteElementString("decimals", "$@NULL@$");
            writer.WriteElementString("hidden", "0");
            writer.WriteElementString("locked", "0");
            writer.WriteElementString("locktime", "0");
            writer.WriteElementString("needsupdate", "0");
            writer.WriteElementString("timecreated", "1731353585");
            writer.WriteElementString("timemodified", "1731353585");
            writer.WriteStartElement("grade_grades");
            writer.WriteString(" ");
            writer.WriteEndElement();

            writer.WriteEndElement(); // Close </grade_item>
            writer.WriteEndElement(); // Close </grade_items>

            writer.WriteStartElement("grade_letters");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </grade_letters>

            writer.WriteStartElement("grade_settings");
            writer.WriteStartElement("grade_setting");
            writer.WriteAttributeString("id", "");
            writer.WriteElementString("name", "minmaxtouse");
            writer.WriteElementString("value", "1");

            writer.WriteEndElement(); // Close </grade_setting>
            writer.WriteEndElement(); // Close </grade_settings>

            writer.WriteEndElement(); // Close </gradebook>
            writer.WriteEndDocument();
        }
    }
}