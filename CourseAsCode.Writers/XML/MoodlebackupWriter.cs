using System.Xml;
using BackupModel =  CourseAsCode.Model.BackupMetadata;

using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML;

public class MoodleBackupWriter : XmlWriterBase
{
    public void WriteMoodleBackup(BackupModel backupMetadata, string filePath = "moodle_backup.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("moodle_backup");

            WriteInformation(writer, backupMetadata);
            WriteDetails(writer, backupMetadata);
            WriteContents(writer, backupMetadata);
            WriteSettings(writer, backupMetadata);

            writer.WriteEndElement(); // moodle_backup schlieﬂen
            writer.WriteEndDocument();
        }
    }

    private void WriteInformation(XmlWriter writer, BackupModel backupMetadata)
    {
        writer.WriteStartElement("information");

        var elements = new Dictionary<string, string>
        {
            { "name", backupMetadata.Name },
            { "moodle_version", backupMetadata.MoodleVersion.ToString() },
            { "moodle_release", GetMoodleRelease(backupMetadata.MoodleVersion) },
            { "backup_version", GetBackupVersion(backupMetadata.MoodleVersion) },
            { "backup_release", GetBackupRelease(backupMetadata.MoodleVersion) },
            { "backup_date", ((DateTimeOffset)backupMetadata.SystemDefaultsInstance.CreationDate).ToUnixTimeSeconds().ToString() },
            { "mnet_remoteusers", backupMetadata.SystemDefaultsInstance.MNetRemoteUsers.ToString() },
            { "include_files", backupMetadata.SensibleDefaultsInstance.FilesIncluded ? "1" : "0" },
            { "include_file_references_to_external_content", backupMetadata.SystemDefaultsInstance.IncludeFileReferencesToExternalContent ? "1" : "0" },
            { "original_wwwroot", backupMetadata.MoodleUrl.ToString() },
            { "original_site_identifier_hash", backupMetadata.SystemDefaultsInstance.BackupId },
            { "original_course_id", "4" }, // Anpassen entsprechend den Angaben
            { "original_course_format", "EmptyCourse" },
            { "original_course_fullname", backupMetadata.Name },
            { "original_course_shortname", backupMetadata.Name },
            { "original_course_startdate", "1731369600" }, // Anpassen entsprechend den Angaben
            { "original_course_enddate", "1734998400" }, // Anpassen entsprechend den Angaben
            { "original_course_contextid", "18" }, // Anpassen entsprechend den Angaben
            { "original_system_contextid", "1" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // information schlieﬂen
    }

    private void WriteDetails(XmlWriter writer, BackupModel backupMetadata)
    {
        writer.WriteStartElement("details");

        WriteStartElementWithAttributes(writer, "detail", new Dictionary<string, string>
        {
            { "backup_id", backupMetadata.SystemDefaultsInstance.BackupId },
            { "type", backupMetadata.SystemDefaultsInstance.WholeSiteBackup ? "site" : "course" }
        });

        var elements = new Dictionary<string, string>
        {
            { "format", backupMetadata.SystemDefaultsInstance.UsesNewBackupFormat ? "moodle2" : "moodle1" },
            { "interactive", "1" },
            { "mode", "70" },
            { "execution", "2" },
            { "executiontime", "0" }
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // detail schlieﬂen
        writer.WriteEndElement(); // details schlieﬂen
    }

    private void WriteContents(XmlWriter writer, BackupModel backupMetadata)
    {
        writer.WriteStartElement("contents");
        //Bei bedarf etwas einf¸gen
        writer.WriteEndElement(); // contents schlieﬂen
    }

    private void WriteSettings(XmlWriter writer, BackupModel backupMetadata)
    {
        writer.WriteStartElement("settings");

        // Root-Einstellungen
        WriteBackupSetting(writer, "root", "filename", backupMetadata.Name);
        WriteBackupSetting(writer, "root", "users", backupMetadata.BackupSettingsInstance.IncludeEnrolledUsers ? "1" : "0");
        WriteBackupSetting(writer, "root", "anonymize", backupMetadata.BackupSettingsInstance.AnonymizeUserInformation ? "1" : "0");
        WriteBackupSetting(writer, "root", "role_assignments", backupMetadata.BackupSettingsInstance.IncludeUserRoleAssignments ? "1" : "0");
        WriteBackupSetting(writer, "root", "activities", backupMetadata.BackupSettingsInstance.IncludeActivitiesAndResources ? "1" : "0");
        WriteBackupSetting(writer, "root", "blocks", backupMetadata.BackupSettingsInstance.IncludeBlocks ? "1" : "0");
        WriteBackupSetting(writer, "root", "files", backupMetadata.BackupSettingsInstance.IncludeFiles ? "1" : "0");
        WriteBackupSetting(writer, "root", "filters", backupMetadata.BackupSettingsInstance.IncludeFilters ? "1" : "0");
        WriteBackupSetting(writer, "root", "comments", backupMetadata.BackupSettingsInstance.IncludeComments ? "1" : "0");
        WriteBackupSetting(writer, "root", "badges", backupMetadata.BackupSettingsInstance.IncludeBadges ? "1" : "0");
        WriteBackupSetting(writer, "root", "calendarevents", backupMetadata.BackupSettingsInstance.IncludeCalendarEvents ? "1" : "0");
        WriteBackupSetting(writer, "root", "userscompletion", backupMetadata.BackupSettingsInstance.IncludeUserCompletionDetails ? "1" : "0");
        WriteBackupSetting(writer, "root", "logs", backupMetadata.BackupSettingsInstance.IncludeCourseLogs ? "1" : "0");
        WriteBackupSetting(writer, "root", "grade_histories", backupMetadata.BackupSettingsInstance.IncludeGradeHistory ? "1" : "0");
        WriteBackupSetting(writer, "root", "questionbank", backupMetadata.BackupSettingsInstance.IncludeQuestionBank ? "1" : "0");
        WriteBackupSetting(writer, "root", "groups", backupMetadata.BackupSettingsInstance.IncludeGroupsAndGroupings ? "1" : "0");
        WriteBackupSetting(writer, "root", "competencies", backupMetadata.BackupSettingsInstance.IncludeCompetencies ? "1" : "0");
        WriteBackupSetting(writer, "root", "customfield", backupMetadata.BackupSettingsInstance.IncludeCustomFields ? "1" : "0");
        WriteBackupSetting(writer, "root", "contentbankcontent", backupMetadata.BackupSettingsInstance.IncludeContentBankContent ? "1" : "0");
        WriteBackupSetting(writer, "root", "xapistate", backupMetadata.BackupSettingsInstance.IncludeUserStateInContent ? "1" : "0");
        WriteBackupSetting(writer, "root", "legacyfiles", backupMetadata.BackupSettingsInstance.IncludeLegacyCourseFiles ? "1" : "0");

        writer.WriteEndElement(); // settings schlieﬂen
    }

    private void WriteBackupSetting(XmlWriter writer, string level, string name, string value)
    {
        writer.WriteStartElement("setting");
        writer.WriteElementString("level", level);
        writer.WriteElementString("name", name);
        writer.WriteElementString("value", value);
        writer.WriteEndElement(); 
    }

    private string GetMoodleRelease(MoodleVersion version)
    {
        // Mappt MoodleVersion auf Moodle Release String
        return version switch
        {
            MoodleVersion.Moodle451 => "4.5.1 (Build: 20241007)",
            MoodleVersion.Moodle450 => "4.5.0 (Build: 20240601)",
            MoodleVersion.Moodle445 => "4.4.5 (Build: 20230427)",
            MoodleVersion.Moodle444 => "4.4.4 (Build: 20230309)",
            MoodleVersion.Moodle443 => "4.4.3 (Build: 20230228)",
            MoodleVersion.Moodle442 => "4.4.2 (Build: 20230209)",
            MoodleVersion.Moodle441 => "4.4.1 (Build: 20230112)",
            MoodleVersion.Moodle440 => "4.4.0 (Build: 20230101)",
            _ => "Unbekannt",
        };
    }


    private string GetBackupVersion(MoodleVersion version)
    {
        // Mappt MoodleVersion auf Backup Version
        return version switch
        {
            MoodleVersion.Moodle451 => "2024100700",
            MoodleVersion.Moodle450 => "2024060100",
            MoodleVersion.Moodle445 => "2023042700",
            MoodleVersion.Moodle444 => "2023030900",
            MoodleVersion.Moodle443 => "2023022800",
            MoodleVersion.Moodle442 => "2023020900",
            MoodleVersion.Moodle441 => "2023011200",
            MoodleVersion.Moodle440 => "2023010100",
            _ => "Unbekannt",
        };
    }


    private string GetBackupRelease(MoodleVersion version)
    {
        // Mappt MoodleVersion auf Backup Release
        return version switch
        {
            MoodleVersion.Moodle451 or MoodleVersion.Moodle450 => "4.5",
            MoodleVersion.Moodle445 or MoodleVersion.Moodle444 or MoodleVersion.Moodle443 or MoodleVersion.Moodle442 or MoodleVersion.Moodle441 or MoodleVersion.Moodle440 => "4.4",
          
            _ => "Unbekannt",
        };
    }

}
