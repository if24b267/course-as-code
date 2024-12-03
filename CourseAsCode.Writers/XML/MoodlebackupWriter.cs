using System.Xml;

namespace CourseAsCode.Writers.XML;

public class MoodleBackupWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    public void WriteMoodleBackup(string filePath = "moodle_backup.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // root element <moodle_backup> with attributes
            writer.WriteStartElement("moodle_backup");

            writer.WriteStartElement("information");
            writer.WriteElementString("name", "backup_test.mbz");
            writer.WriteElementString("moodle_version", "2021051700");
            writer.WriteElementString("moodle_release", "4.5 (Build: 20241007)");
            writer.WriteElementString("backup_version", "2024100700");
            writer.WriteElementString("backup_release", "4.5");
            writer.WriteElementString("backup_date", "1731353758");
            writer.WriteElementString("mnet_remoteusers", "0");
            writer.WriteElementString("include_files", "1");
            writer.WriteElementString("include_file_references_to_external_content", "0");
            writer.WriteElementString("original_wwwroot", "https://moodle.sanctus.cx");
            writer.WriteElementString("original_site_identifier_hash", "dccbb261ec5a2af4a6706a77387883ee");
            writer.WriteElementString("original_course_id", "4");
            writer.WriteElementString("original_course_format", "EmptyCourse");
            writer.WriteElementString("original_course_fullname", "EmptyCourse");
            writer.WriteElementString("original_course_shortname", "EmpCo");
            writer.WriteElementString("original_course_startdate", "1731369600");
            writer.WriteElementString("original_course_enddate", "1734998400");
            writer.WriteElementString("original_course_contextid", "18");
            writer.WriteElementString("original_system_contextid", "1");

            writer.WriteStartElement("details");
            writer.WriteStartElement("detail");
            writer.WriteAttributeString("backup_id", "6078ead34c57f691993f01c2e97accdd");
            writer.WriteAttributeString("type", "course");
            writer.WriteElementString("format", "moodle2");
            writer.WriteElementString("interactive", "1");
            writer.WriteElementString("mode", "70");
            writer.WriteElementString("execution", "2");
            writer.WriteElementString("executiontime", "0");
            writer.WriteEndElement(); // Close detail
            writer.WriteEndElement(); // Close details

            writer.WriteStartElement("contents");
            writer.WriteStartElement("activities");

            writer.WriteStartElement("activity");
            writer.WriteAttributeString("moduleid", "2");
            writer.WriteElementString("sectionid", "13");
            writer.WriteAttributeString("modulename", "forum");
            writer.WriteElementString("title", "Announcements");
            writer.WriteElementString("directory", "ctivities/forum_2");
            writer.WriteElementString("insubsection", "");
            writer.WriteEndElement(); // Close activity

            writer.WriteEndElement(); // Close activities

            writer.WriteStartElement("sections");

            for (int i = 13; i <= 19; i++)
            {
                writer.WriteStartElement("section");
                writer.WriteElementString("sectionid", i.ToString());
                writer.WriteElementString("title", (i - 13).ToString());
                writer.WriteElementString("directory", $"sections/section_{i}");
                writer.WriteElementString("parentcmid", "");
                writer.WriteElementString("modname", "");
                writer.WriteEndElement(); // Close section
            }

            writer.WriteEndElement(); // Close sections

            writer.WriteStartElement("course");
            writer.WriteElementString("courseid", "4");
            writer.WriteElementString("title", "EWC");
            writer.WriteElementString("directory", "course");
            writer.WriteEndElement(); // Close course

            writer.WriteEndElement(); // Close contents

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

            writer.WriteEndElement(); // Close information
            writer.WriteEndElement(); // Close moodle_backup

            writer.WriteEndDocument();
        }
    }
}