using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GradebookWriter : XmlWriterBase
{
    public void WriteGradebook(string filePath = "gradebook.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("gradebook");

            WriteAttributes(writer);
            WriteGradeCategories(writer);
            WriteGradeItems(writer);
            WriteGradeLetters(writer);
            WriteGradeSettings(writer);

            writer.WriteEndElement(); // Close gradebook
            writer.WriteEndDocument();
        }
    }

    private void WriteAttributes(XmlWriter writer)
    {
        writer.WriteStartElement("attributes");
        writer.WriteString("");
        writer.WriteEndElement(); // Close attributes
    }

    private void WriteGradeCategories(XmlWriter writer)
    {
        writer.WriteStartElement("grade_categories");

        WriteStartElementWithAttributes(writer, "grade_category", new Dictionary<string, string>
        {
            { "id", "3" }
        });

        var elements = new Dictionary<string, string>
        {
            { "parent", "$@NULL@$" },
            { "depth", "1" },
            { "path", "/3/" },
            { "fullname", "?" },
            { "aggregation", "13" },
            { "keephigh", "0" },
            { "droplow", "0" },
            { "aggregateonlygraded", "1" },
            { "aggregateoutcomes", "0" },
            { "timecreated", "1731353585" },
            { "timemodified", "1731353585" },
            { "hidden", "0" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close grade_category
        writer.WriteEndElement(); // Close grade_categories
    }

    private void WriteGradeItems(XmlWriter writer)
    {
        writer.WriteStartElement("grade_items");

        WriteStartElementWithAttributes(writer, "grade_item", new Dictionary<string, string>
        {
            { "id", "3" }
        });

        var elements = new Dictionary<string, string>
        {
            { "categoryid", "$@NULL@$" },
            { "itemname", "$@NULL@$" },
            { "itemtype", "course" },
            { "itemmodule", "$@NULL@$" },
            { "iteminstance", "3" },
            { "itemnumber", "$@NULL@$" },
            { "iteminfo", "$@NULL@$" },
            { "idnumber", "$@NULL@$" },
            { "calculation", "$@NULL@$" },
            { "gradetype", "1" },
            { "grademax", "0.00000" },
            { "grademin", "0.00000" },
            { "scaleid", "$@NULL@$" },
            { "outcomeid", "$@NULL@$" },
            { "gradepass", "0.00000" },
            { "multfactor", "1.00000" },
            { "plusfactor", "0.00000" },
            { "aggregationcoef", "0.00000" },
            { "aggregationcoef2", "0.00000" },
            { "weightoverride", "0" },
            { "sortorder", "1" },
            { "display", "0" },
            { "decimals", "$@NULL@$" },
            { "hidden", "0" },
            { "locked", "0" },
            { "locktime", "0" },
            { "needsupdate", "0" },
            { "timecreated", "1731353585" },
            { "timemodified", "1731353585" }
        };

        WriteElements(writer, elements);

        writer.WriteStartElement("grade_grades");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close grade_grades

        writer.WriteEndElement(); // Close grade_item
        writer.WriteEndElement(); // Close grade_items
    }

    private void WriteGradeLetters(XmlWriter writer)
    {
        writer.WriteStartElement("grade_letters");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close grade_letters
    }

    private void WriteGradeSettings(XmlWriter writer)
    {
        writer.WriteStartElement("grade_settings");

        WriteStartElementWithAttributes(writer, "grade_setting", new Dictionary<string, string>
        {
            { "id", "" }
        });

        var elements = new Dictionary<string, string>
        {
            { "name", "minmaxtouse" },
            { "value", "1" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close grade_setting
        writer.WriteEndElement(); // Close grade_settings
    }
}


