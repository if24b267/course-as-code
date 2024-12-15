using System.Xml;

namespace CourseAsCode.Writers.XML;

public class MoodleBackupWriter : XmlWriterBase
{
    public void WriteMoodleBackup(string filePath = "moodle_backup.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("moodle_backup");

            WriteInformation(writer);
            WriteDetails(writer);
            WriteContents(writer);
            WriteSettings(writer);

            writer.WriteEndElement(); // Close moodle_backup
            writer.WriteEndDocument();
        }
    }

    private void WriteInformation(XmlWriter writer)
    {
        writer.WriteStartElement("information");

        var elements = new Dictionary<string, string>
        {
            { "name", "backup_test.mbz" },
            { "moodle_version", "2021051700" },
            { "moodle_release", "4.5 (Build: 20241007)" },
            { "backup_version", "2024100700" },
            { "backup_release", "4.5" },
            { "backup_date", "1731353758" },
            { "mnet_remoteusers", "0" },
            { "include_files", "1" },
            { "include_file_references_to_external_content", "0" },
            { "original_wwwroot", "https://moodle.sanctus.cx" },
            { "original_site_identifier_hash", "dccbb261ec5a2af4a6706a77387883ee" },
            { "original_course_id", "4" },
            { "original_course_format", "EmptyCourse" },
            { "original_course_fullname", "EmptyCourse" },
            { "original_course_shortname", "EmpCo" },
            { "original_course_startdate", "1731369600" },
            { "original_course_enddate", "1734998400" },
            { "original_course_contextid", "18" },
            { "original_system_contextid", "1" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close information
    }

    private void WriteDetails(XmlWriter writer)
    {
        writer.WriteStartElement("details");

        WriteStartElementWithAttributes(writer, "detail", new Dictionary<string, string>
        {
            { "backup_id", "6078ead34c57f691993f01c2e97accdd" },
            { "type", "course" }
        });

        var elements = new Dictionary<string, string>
        {
            { "format", "moodle2" },
            { "interactive", "1" },
            { "mode", "70" },
            { "execution", "2" },
            { "executiontime", "0" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close detail
        writer.WriteEndElement(); // Close details
    }

    private void WriteContents(XmlWriter writer)
    {
        writer.WriteStartElement("contents");

        writer.WriteStartElement("activities");

        WriteStartElementWithAttributes(writer, "activity", new Dictionary<string, string>
        {
            { "moduleid", "2" },
            { "modulename", "forum" }
        });

        var elements = new Dictionary<string, string>
        {
            { "sectionid", "13" },
            { "title", "Announcements" },
            { "directory", "ctivities/forum_2" },
            { "insubsection", "" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close activity
        writer.WriteEndElement(); // Close activities

        writer.WriteStartElement("sections");

        for (int i = 13; i <= 19; i++)
        {
            writer.WriteStartElement("section");
            var sectionElements = new Dictionary<string, string>
            {
                { "sectionid", i.ToString() },
                { "title", (i - 13).ToString() },
                { "directory", $"sections/section_{i}" },
                { "parentcmid", "" },
                { "modname", "" }
            };
            WriteElements(writer, sectionElements);
            writer.WriteEndElement(); // Close section
        }

        writer.WriteEndElement(); // Close sections

        writer.WriteStartElement("course");
        var courseElements = new Dictionary<string, string>
        {
            { "courseid", "4" },
            { "title", "EWC" },
            { "directory", "course" }
        };
        WriteElements(writer, courseElements);
        writer.WriteEndElement(); // Close course

        writer.WriteEndElement(); // Close contents
    }

    private void WriteSettings(XmlWriter writer)
    {
        writer.WriteStartElement("settings");

        string[][] settings = new string[][]
        {
            new string[] { "root", "filename", "backup_test_weekly.mbz" },
            new string[] { "root", "users", "1" },
            new string[] { "root", "anonymize", "0" },
            new string[] { "root", "role_assignments", "1" },
            new string[] { "root", "activities", "1" },
            new string[] { "root", "blocks", "1" },
            new string[] { "root", "files", "1" },
            new string[] { "root", "filters", "1" },
            new string[] { "root", "comments", "1" },
            new string[] { "root", "badges", "1" },
            new string[] { "root", "calendarevents", "1" },
            new string[] { "root", "userscompletion", "1" },
            new string[] { "root", "logs", "0" },
            new string[] { "root", "grade_histories", "0" },
            new string[] { "root", "questionbank", "1" },
            new string[] { "root", "groups", "1" },
            new string[] { "root", "competencies", "1" },
            new string[] { "root", "customfield", "1" },
            new string[] { "root", "contentbankcontent", "1" },
            new string[] { "root", "xapistate", "1" },
            new string[] { "root", "legacyfiles", "1" },
            new string[] { "section", "section_13", "section_13_included", "1" },
            new string[] { "section", "section_13", "section_13_userinfo", "1" },
            new string[] { "activity", "forum_2", "forum_2_included", "1" },
            new string[] { "activity", "forum_2", "forum_2_userinfo", "1" },
            new string[] { "section", "section_14", "section_14_included", "1" },
            new string[] { "section", "section_14", "section_14_userinfo", "1" },
            new string[] { "section", "section_15", "section_15_included", "1" },
            new string[] { "section", "section_15", "section_15_userinfo", "1" },
            new string[] { "section", "section_16", "section_16_included", "1" },
            new string[] { "section", "section_16", "section_16_userinfo", "1" },
            new string[] { "section", "section_17", "section_17_included", "1" },
            new string[] { "section", "section_17", "section_17_userinfo", "1" },
            new string[] { "section", "section_18", "section_18_included", "1" },
            new string[] { "section", "section_18", "section_18_userinfo", "1" },
            new string[] { "section", "section_19", "section_19_included", "1" },
            new string[] { "section", "section_19", "section_19_userinfo", "1" }
        };

        foreach (var setting in settings)
        {
            writer.WriteStartElement("setting");
            writer.WriteElementString("level", setting[0]);
            if (setting[0] == "section" || setting[0] == "activity")
            {
                writer.WriteElementString(setting[0], setting[1]);
                writer.WriteElementString("name", setting[2]);
                writer.WriteElementString("value", setting[3]);
            }
            else
            {
                writer.WriteElementString("name", setting[1]);
                writer.WriteElementString("value", setting[2]);
            }
            writer.WriteEndElement(); // Close setting
        }

        writer.WriteEndElement(); // Close settings
    }
}


