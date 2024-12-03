using System.Xml;

namespace CourseAsCode.Writers.XML;

public class UsersWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteUsers(string filePath = "users.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // root element <users>
            writer.WriteStartElement("users");

            // child elements of <users>
            writer.WriteStartElement("user");
            writer.WriteAttributeString("id", "2");
            writer.WriteAttributeString("contextid", "5");

            writer.WriteElementString("username", "course_as_code");
            writer.WriteElementString("idnumber", "");
            writer.WriteElementString("email", "user.example.com");
            writer.WriteElementString("phone1", "");
            writer.WriteElementString("phone2", "");
            writer.WriteElementString("institution", "");
            writer.WriteElementString("department", "");
            writer.WriteElementString("address", "");
            writer.WriteElementString("city", "");
            writer.WriteElementString("country", "");
            writer.WriteElementString("lastip", "127.21.0.1");
            writer.WriteElementString("picture", "0");
            writer.WriteElementString("description", "$@NULL@$");
            writer.WriteElementString("descriptionformat", "1");
            writer.WriteElementString("imagealt", "$@NULL@$");
            writer.WriteElementString("auth", "manual");
            writer.WriteElementString("firstnamephonetic", "$@NULL@$");
            writer.WriteElementString("lastnamephonetic", "$@NULL@$");
            writer.WriteElementString("middlename", "$@NULL@$");
            writer.WriteElementString("alternatename", "$@NULL@$");
            writer.WriteElementString("firstname", "Admin");
            writer.WriteElementString("lastname", "User");
            writer.WriteElementString("confirmed", "1");
            writer.WriteElementString("policyagreed", "0");
            writer.WriteElementString("deleted", "0");
            writer.WriteElementString("lang", "en");
            writer.WriteElementString("theme", "");
            writer.WriteElementString("timezone", "99");
            writer.WriteElementString("firstaccess", "1731096393");
            writer.WriteElementString("lastaccess", "1731353758");
            writer.WriteElementString("lastlogin", "1731350894");
            writer.WriteElementString("currentlogin", "1731351035");
            writer.WriteElementString("mailformat", "1");
            writer.WriteElementString("maildigest", "0");
            writer.WriteElementString("maildisplay", "1");
            writer.WriteElementString("autosubscribe", "1");
            writer.WriteElementString("trackforums", "0");
            writer.WriteElementString("timecreated", "0");
            writer.WriteElementString("timemodified", "1731096239");
            writer.WriteElementString("trustbitmask", "0");

            writer.WriteStartElement("custom_fields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <customfields>

            writer.WriteStartElement("tags");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <tags>

            writer.WriteStartElement("preferences");

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", "core_message_migrate_data");
            writer.WriteElementString("value", "0");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "2");
            writer.WriteElementString("name", "core_user_welcome");
            writer.WriteElementString("value", "1731096394");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "7");
            writer.WriteElementString("name", "filepicker_recentlicense");
            writer.WriteElementString("value", "unknown");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "6");
            writer.WriteElementString("name", "filepicker_recentrepository");
            writer.WriteElementString("value", "5");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "21");
            writer.WriteElementString("name", "ifirst");
            writer.WriteElementString("value", "");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "22");
            writer.WriteElementString("name", "ilast");
            writer.WriteElementString("value", "");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "3");
            writer.WriteElementString("name", "login_failed_count_since_success");
            writer.WriteElementString("value", "0");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "20");
            writer.WriteElementString("name", "tool_usertours_tour_completion_time_3");
            writer.WriteElementString("value", "1731351490");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "4");
            writer.WriteElementString("name", "tool_usertours_tour_completion_time_4");
            writer.WriteElementString("value", "1731096405");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteStartElement("preference");
            writer.WriteAttributeString("id", "5");
            writer.WriteElementString("name", "tool_usertours_tour_completion_time_5");
            writer.WriteElementString("value", "1731098404");
            writer.WriteEndElement(); // Close <preference>

            writer.WriteEndElement(); // Close <preferences>

            writer.WriteStartElement("roles");

            writer.WriteStartElement("role_overrides");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <role_overrides>

            writer.WriteStartElement("role_assignments");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <role_assignments>

            writer.WriteEndElement(); // Close <roles>

            writer.WriteEndElement(); // Close <user>
            writer.WriteEndElement(); // Close <users>
            writer.WriteEndDocument();
        }
    }
}


