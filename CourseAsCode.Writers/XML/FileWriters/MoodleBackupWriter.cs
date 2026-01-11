using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Static;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class MoodleBackupWriter : XmlWriterBase
    {
        private const string DEFAULT_MOODLE_BACKUP_RELATIVE_PATH = "moodle_backup.xml";

        private readonly IGeneralHelper _helper;
        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly ICourseHelper _courseHelper;


        /// <summary>
        /// Creates a new instance of the MoodleBackupWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to MoodleBackupWriter</param>
        /// <param name="helper">General Helper providing various helper methods</param>
        /// <param name="moodleVersionHelper">Helper method providing specific helper methods focused on Moodle version management</param>
        /// <param name="courseHelper">Helper method providing specific helper methods focused on course management</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public MoodleBackupWriter(IXmlWriterFactory xmlWriterFactory, ILogger<MoodleBackupWriter> logger, IGeneralHelper helper, IMoodleVersionHelper moodleVersionHelper, ICourseHelper courseHelper)
            : base(xmlWriterFactory, logger)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _courseHelper = courseHelper ?? throw new ArgumentNullException(nameof(courseHelper));
        }


        public void WriteMoodleBackup(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MOODLE_BACKUP_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("moodle_backup");
                    writer.WriteStartElement("information");

                    WriteGeneralSettings(writer, course);
                    WriteDetails(writer, course);
                    WriteContents(writer, course);
                    WriteSettings(writer, course);

                    writer.WriteEndElement(); // Closes <information>
                    writer.WriteEndElement(); // Closes <moodle_backup>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// writes the general settings within the information section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteGeneralSettings(XmlWriter writer, Model.Course course)
        {
            var elements = new Dictionary<string, string>
            {
                { "name", _helper.GenerateBackupName(course) },
                { "moodle_version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion) },
                { "moodle_release", _moodleVersionHelper.GetMoodleReleaseStringFromEnum(course.BackupMetadata.MoodleVersion) },
                { "backup_version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion) },
                { "backup_release", _moodleVersionHelper.GetMoodleReleaseStringFromEnum(course.BackupMetadata.MoodleVersion) },
                { "backup_date", _helper.DateTimeToUnixTimestamp(course.BackupMetadata.SystemDefaults.CreationDate) },
                { "mnet_remoteusers", course.BackupMetadata.SystemDefaults.MNetRemoteUsers.ToString() ?? "0" },
                { "include_files", course.BackupMetadata.SensibleDefaults.FilesIncluded ? "1" : "0" },
                { "include_file_references_to_external_content", course.BackupMetadata.SystemDefaults.IncludeFileReferencesToExternalContent ? "1" : "0" },
                { "original_wwwroot", course.BackupMetadata.MoodleUrl.ToString() },
                { "original_site_identifier_hash", "0" },
                { "original_course_id", course.SystemDefaults.Id.ToString() },
                { "original_course_format", _courseHelper.GetCourseFormatFromEnum(course.SensibleDefaults.Format) },
                { "original_course_fullname", course.FullName },
                { "original_course_shortname", course.ShortName },
                { "original_course_startdate", _helper.DateTimeToUnixTimestamp(course.StartDate) },
                { "original_course_enddate", course.EndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)course.EndDate) : "0" },
                { "original_course_contextid", GeneralDefaults.COURSECONTEXTID.ToString() },
                { "original_system_contextid", GeneralDefaults.SYSTEMCONTEXTID.ToString() },
            };

            WriteElements(writer, elements);
        }

        /// <summary>
        /// writes the information section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteDetails(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("details");

            WriteStartElementWithAttributes(writer, "detail", new Dictionary<string, string>
            {
                { "backup_id", course.BackupMetadata.SystemDefaults.BackupId },
            });

            var elements = new Dictionary<string, string>
            {
                { "type", course.BackupMetadata.SystemDefaults.WholeSiteBackup ? "site" : "course" },
                { "format", course.BackupMetadata.SystemDefaults.UsesNewBackupFormat ? "moodle2" : "moodle1" },
                { "interactive", "1" },
                { "mode", "10" },
                { "execution", "1" },
                { "executiontime", "0" },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Closes <detail>
            writer.WriteEndElement(); // Closes <details>
        }

        /// <summary>
        /// writes the contents section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteContents(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("contents");

            WriteActivities(writer, course);
            WriteSections(writer, course);
            WriteCourseSubsection(writer, course);

            writer.WriteEndElement(); // Closes <contents>
        }

        /// <summary>
        /// Writes the activities subsection of the contents section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteActivities(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("activities");
            foreach (var section in course.Sections)
            {
                foreach (var activity in section.Activities)
                {
                    // TODO: Refactor this to avoid duplicate code
                    if (activity is TextAndMediaArea textAndMediaArea)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", textAndMediaArea.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", textAndMediaArea.ModuleName);
                        writer.WriteElementString("title", textAndMediaArea.Name);
                        writer.WriteElementString("directory", $"activities/label_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is URL urlObject)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", urlObject.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", urlObject.ModuleName);
                        writer.WriteElementString("title", urlObject.Name);
                        writer.WriteElementString("directory", $"activities/url_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is MoodleFile moodleFile)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", moodleFile.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", moodleFile.ModuleName);
                        writer.WriteElementString("title", moodleFile.FileName);
                        writer.WriteElementString("directory", $"activities/resource_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is Quiz quiz)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", quiz.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", quiz.ModuleName);
                        writer.WriteElementString("title", quiz.Name);
                        writer.WriteElementString("directory", $"activities/quiz_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is Assignment assignment)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", assignment.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", assignment.ModuleName);
                        writer.WriteElementString("title", assignment.Name);
                        writer.WriteElementString("directory", $"activities/assign_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is Wiki wiki)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", wiki.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", wiki.ModuleName);
                        writer.WriteElementString("title", wiki.Name);
                        writer.WriteElementString("directory", $"activities/wiki_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // Closes <activity>
                    }

                    if (activity is Lesson lesson)
                    {
                        writer.WriteStartElement("activity");

                        writer.WriteElementString("moduleid", lesson.ModuleId.ToString());
                        writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                        writer.WriteElementString("modulename", lesson.ModuleName);
                        writer.WriteElementString("title", lesson.Name);
                        writer.WriteElementString("directory", $"activities/lesson_{activity.ActivityId}");
                        writer.WriteElementString("insubsection", string.Empty);

                        writer.WriteEndElement(); // </activity>
                    }

                    LogTrace($"Added reference for 'Activity {activity.ActivityId}: {activity.Name}' to {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
                }
            }

            writer.WriteEndElement(); // Closes <activities>
        }

        /// <summary>
        /// Writes the sections subsection of the contents section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteSections(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("sections");
            foreach (var section in course.Sections)
            {
                writer.WriteStartElement("section");

                writer.WriteElementString("sectionid", section.SystemDefaults.SectionId.ToString());
                writer.WriteElementString("title", section.Name ?? "0");
                writer.WriteElementString("directory", section.SystemDefaults.PathToSection);
                writer.WriteElementString("parentcmid", string.Empty);
                writer.WriteElementString("modname", string.Empty);

                writer.WriteEndElement(); // Closes <section>

                LogTrace($"Added reference for 'Section {section.SystemDefaults.SectionId}: {section.Name}' to {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
            }

            writer.WriteEndElement(); // Closes <sections>
        }

        /// <summary>
        /// writes the course subsection of the contents section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteCourseSubsection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("course");

            var elements = new Dictionary<string, string>
            {
                { "courseid", course.SystemDefaults.Id.ToString() },
                { "title", course.ShortName },
                { "directory", "course" },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Closes <course>
        }

        /// <summary>
        /// writes the settings section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteSettings(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("settings");

            WriteRootLevelSettings(writer, course);
            WriteSectionSettings(writer, course);
            WriteActivitySettings(writer, course);

            writer.WriteEndElement(); // Closes <settings>
        }

        /// <summary>
        /// writes the root level settings of the settings section of moodle_backup.xml, corresponds to the general backup settings selected by the user in the Moodle UI.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteRootLevelSettings(XmlWriter writer, Model.Course course)
        {
            var settings = new Dictionary<string, string>
            {
                { "filename", _helper.GenerateBackupName(course) },
                { "users", course.BackupMetadata.BackupSettings.IncludeEnrolledUsers ? "1" : "0" },
                { "anonymize", course.BackupMetadata.BackupSettings.AnonymizeUserInformation ? "1" : "0" },
                { "role_assignments", course.BackupMetadata.BackupSettings.IncludeUserRoleAssignments ? "1" : "0" },
                { "activities", course.BackupMetadata.BackupSettings.IncludeActivitiesAndResources ? "1" : "0" },
                { "blocks", course.BackupMetadata.BackupSettings.IncludeBlocks ? "1" : "0" },
                { "files", course.BackupMetadata.BackupSettings.IncludeFiles ? "1" : "0" },
                { "filters", course.BackupMetadata.BackupSettings.IncludeFilters ? "1" : "0" },
                { "comments", course.BackupMetadata.BackupSettings.IncludeComments ? "1" : "0" },
                { "badges", course.BackupMetadata.BackupSettings.IncludeBadges ? "1" : "0" },
                { "calendarevents", course.BackupMetadata.BackupSettings.IncludeCalendarEvents ? "1" : "0" },
                { "userscompletion", course.BackupMetadata.BackupSettings.IncludeUserCompletionDetails ? "1" : "0" },
                { "logs", course.BackupMetadata.BackupSettings.IncludeCourseLogs ? "1" : "0" },
                { "grade_histories", course.BackupMetadata.BackupSettings.IncludeGradeHistory ? "1" : "0" },
                { "questionbank", course.BackupMetadata.BackupSettings.IncludeQuestionBank ? "1" : "0" },
                { "groups", course.BackupMetadata.BackupSettings.IncludeGroupsAndGroupings ? "1" : "0" },
                { "competencies", course.BackupMetadata.BackupSettings.IncludeCompetencies ? "1" : "0" },
                { "customfield", course.BackupMetadata.BackupSettings.IncludeCustomFields ? "1" : "0" },
                { "contentbankcontent", course.BackupMetadata.BackupSettings.IncludeContentBankContent ? "1" : "0" },
                { "xapistate", course.BackupMetadata.BackupSettings.IncludeUserStateInContent ? "1" : "0" },
                { "legacyfiles", course.BackupMetadata.BackupSettings.IncludeLegacyCourseFiles ? "1" : "0" },
            };

            foreach (var setting in settings)
            {
                WriteSingleBackupSettingTag(writer, "root", setting.Key, setting.Value);
            }
        }

        /// <summary>
        /// Writes the section settings of the settings section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteSectionSettings(XmlWriter writer, Model.Course course)
        {
            foreach (var section in course.Sections)
            {
                WriteSingleBackupSettingTag(writer, "section", $"section_{section.SystemDefaults.SectionId}", $"section_{section.SystemDefaults.SectionId}_included", "1");
                WriteSingleBackupSettingTag(writer, "section", $"section_{section.SystemDefaults.SectionId}", $"section_{section.SystemDefaults.SectionId}_userinfo", "1");

                LogTrace($"Added setting for 'Section {section.SystemDefaults.SectionId}: {section.Name}' to {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
            }
        }

        /// <summary>
        /// Writes the activity settings of the settings section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteActivitySettings(XmlWriter writer, Model.Course course)
        {
            foreach (var section in course.Sections)
            {
                foreach (var activity in section.Activities)
                {
                    // TODO: Refactor this to avoid duplicate code
                    if (activity is TextAndMediaArea textAndMediaArea)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"label_{activity.ActivityId}", $"label_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"label_{activity.ActivityId}", $"label_{activity.ActivityId}_userinfo", "1");
                    }

                    if (activity is URL urlObject)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"url_{activity.ActivityId}", $"url_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"url_{activity.ActivityId}", $"url_{activity.ActivityId}_userinfo", "1");
                    }

                    if (activity is MoodleFile moodleFile)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"resource_{activity.ActivityId}", $"resource_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"resource_{activity.ActivityId}", $"resource_{activity.ActivityId}_userinfo", "1");
                    }

                    if (activity is Quiz quiz)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"quiz_{activity.ActivityId}", $"quiz_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"quiz_{activity.ActivityId}", $"quiz_{activity.ActivityId}_userinfo", "1");
                    }

                    if (activity is Assignment assignment)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"assign_{activity.ActivityId}", $"assign_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"assign_{activity.ActivityId}", $"assign_{activity.ActivityId}_userinfo", "1");
                    }

                    if (activity is Wiki wiki)
                    {
                        WriteSingleBackupSettingTag(writer, "activity", $"wiki_{activity.ActivityId}", $"wiki_{activity.ActivityId}_included", "1");
                        WriteSingleBackupSettingTag(writer, "activity", $"wiki_{activity.ActivityId}", $"wiki_{activity.ActivityId}_userinfo", "1");
                    }

                    LogTrace($"Added setting for 'Activity {activity.ActivityId}: {activity.Name}' to {DEFAULT_MOODLE_BACKUP_RELATIVE_PATH}");
                }
            }
        }

        /// <summary>
        /// Writes a single setting tag with the specified level, element, name, and value.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="level">Value of the level tag inside the setting tag.</param>
        /// <param name="element">Value of the activity or section tag inside the setting tag.</param>
        /// <param name="name">Value of the name tag inside the setting tag.</param>
        /// <param name="value">Value of the value tag inside the setting tag.</param>
        private void WriteSingleBackupSettingTag(XmlWriter writer, string level, string element, string name, string value)
        {
            writer.WriteStartElement("setting");

            writer.WriteElementString("level", level);
            if (level == "activity")
            {
                writer.WriteElementString("activity", element);
            }
            else if (level == "section")
            {
                writer.WriteElementString("section", element);
            }

            writer.WriteElementString("name", name);
            writer.WriteElementString("value", value);

            writer.WriteEndElement(); // Closes <setting>
        }

        /// <summary>
        /// writes a single setting tag, for use inside the settings section of moodle_backup.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="level">value of the level tag inside the setting tag.</param>
        /// <param name="name">value of the name tag inside the setting tag.</param>
        /// <param name="value">value of the value tag inside the setting tag.</param>
        private void WriteSingleBackupSettingTag(XmlWriter writer, string level, string name, string value)
        {
            writer.WriteStartElement("setting");

            writer.WriteElementString("level", level);
            writer.WriteElementString("name", name);
            writer.WriteElementString("value", value);

            writer.WriteEndElement(); // Closes <setting>
        }
    }
}
