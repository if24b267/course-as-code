using System.Security.Cryptography;
using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Static;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters
{
    public class FilesWriter : XmlWriterBase
    {
        private const string DEFAULT_FILE_RELATIVE_PATH = "files.xml";
        private readonly IFileHelper _fileHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly IFileService _fileService;


        /// <summary>
        /// Creates a new instance of the FilesWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to FilesWriter</param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="fileHelper">Helper method providing specific helper methods focused on file management</param>
        /// <param name="fileService">Service that handles operations on the file system</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public FilesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<FilesWriter> logger, IFileHelper fileHelper, IGeneralHelper generalHelper, IFileService fileService)
            : base(xmlWriterFactory, logger)
        {
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }



        public void WriteFiles(Model.Course course, string outputPath)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILE_RELATIVE_PATH));

            try
            {
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("files");

                    foreach (Section section in course.Sections)
                    {
                        foreach (Activity activity in section.Activities)
                        {
                            if (activity is MoodleFile file)
                            {
                                WriteSingleFile(writer, outputPath, file, contextIdOverride: null);
                            }

                            if (activity is Folder folder && folder.Files != null)
                            {
                                foreach (var folderFile in folder.Files)
                                {
                                    WriteSingleFile(writer, outputPath, folderFile, contextIdOverride: folder.ActivityId);
                                }
                            }
                        }
                    }

                    writer.WriteEndElement(); // </files>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_FILE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_FILE_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Writes a single file entry to the XML document.
        /// </summary>
        private void WriteSingleFile(XmlWriter writer, string outputPath, MoodleFile file)
        {
            FileStream? fileStream = null;
            try
            {
                if (!_fileService.Exists(file.FilePath))
                {
                    LogWarn($"{file.FilePath} does not exist. Skipping file");
                    return;
                }

                fileStream = _fileService.OpenRead(file.FilePath);
                LogDebug($"Found {file.FilePath}");
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error opening {file.FilePath}. Skipping file");
                return;
            }

            string? contentHash = null;
            try
            {
                contentHash = GetMoodleContentHash(fileStream);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Skipping file {Path.GetFileName(fileStream.Name)}");
                return;
            }

            WriteStartElementWithAttributes(writer, "file", new Dictionary<string, string>
            {
                { "id", file.ActivityId.ToString() },
            });

            var elements = new Dictionary<string, string>
        {
            { "contenthash", contentHash },
            { "contextid", file.ActivityId.ToString() },
            { "component", file.SystemDefaults.Component != null ? _fileHelper.GetComponentFromEnum((Component)file.SystemDefaults.Component) : "course" },
            { "filearea", file.SystemDefaults.FileArea != null ? _fileHelper.GetFileAreaFromEnum((FileArea)file.SystemDefaults.FileArea) : "content" },
            { "itemid", "0" }, // If there is only one of a file area per context, the itemid is 0, see https://moodledev.io/docs/5.0/apis/subsystems/files#file-areas
            { "filepath", "/" }, // TODO: Why is this always "/"?
            { "filename", file.FileName },
            { "userid", file.SensibleDefaults.Author?.SystemDefaults.Id.ToString() ?? GeneralDefaults.DEFAULTADMINUSERID.ToString() },
            { "filesize", fileStream.Length.ToString() },
            { "mimetype", MimeTypes.GetMimeType(file.FilePath) },
            { "status", "0" }, // TODO: Determine function of this field
            { "timecreated", _generalHelper.DateTimeToUnixTimestamp(file.TimeCreated) },
            { "timemodified", _generalHelper.DateTimeToUnixTimestamp(file.TimeModified) },
            { "source", file.FileName },
            { "author", file.SensibleDefaults.Author?.Username ?? "$@NULL@$" },
            { "license", file.SensibleDefaults.License != null ? _fileHelper.GetLicenseFromEnum((License)file.SensibleDefaults.License) : "unknown" }, // TODO: Add function that maps enum value to correct moodle values
            { "sortorder", "0" }, // TODO: Determine function of this field
            { "repositorytype", "$@NULL@$" }, // TODO: Determine function of this field
            { "repositoryid", "$@NULL@$" }, // TODO: Determine function of this field
            { "reference", "$@NULL@$" }, // TODO: Determine function of this field
        };

            WriteElements(writer, elements);
            writer.WriteEndElement(); // Close <file>
            CopyFileToBackupFolder(outputPath, fileStream, contentHash);
            fileStream.Dispose();
        }

        /// <summary>
        /// Writes a single file entry to the XML document, with an option to override the context ID.
        /// </summary>
        private void WriteSingleFile(XmlWriter writer, string outputPath, MoodleFile file, int? contextIdOverride)
        {
            FileStream? fileStream = null;
            try
            {
                if (!_fileService.Exists(file.FilePath))
                {
                    LogWarn($"{file.FilePath} does not exist. Skipping file");
                    return;
                }

                fileStream = _fileService.OpenRead(file.FilePath);
                LogDebug($"Found {file.FilePath}");
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error opening {file.FilePath}. Skipping file");
                return;
            }

            string? contentHash = null;
            try
            {
                contentHash = GetMoodleContentHash(fileStream);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Skipping file {Path.GetFileName(fileStream.Name)}");
                return;
            }

            var contextId = (contextIdOverride ?? file.ActivityId).ToString();

            WriteStartElementWithAttributes(writer, "file", new Dictionary<string, string>
            {
                { "id", file.ActivityId.ToString() },
            });

            var elements = new Dictionary<string, string>
        {
            { "contenthash", contentHash },
            { "contextid", contextId },
            { "component", file.SystemDefaults.Component != null ? _fileHelper.GetComponentFromEnum((Component)file.SystemDefaults.Component) : "course" },
            { "filearea", file.SystemDefaults.FileArea != null ? _fileHelper.GetFileAreaFromEnum((FileArea)file.SystemDefaults.FileArea) : "content" },
            { "itemid", "0" },
            { "filepath", "/" },
            { "filename", file.FileName },
            { "userid", file.SensibleDefaults.Author?.SystemDefaults.Id.ToString() ?? GeneralDefaults.DEFAULTADMINUSERID.ToString() },
            { "filesize", fileStream.Length.ToString() },
            { "mimetype", MimeTypes.GetMimeType(file.FilePath) },
            { "status", "0" },
            { "timecreated", _generalHelper.DateTimeToUnixTimestamp(file.TimeCreated) },
            { "timemodified", _generalHelper.DateTimeToUnixTimestamp(file.TimeModified) },
            { "source", file.FileName },
            { "author", file.SensibleDefaults.Author?.Username ?? "$@NULL@$" },
            { "license", file.SensibleDefaults.License != null ? _fileHelper.GetLicenseFromEnum((License)file.SensibleDefaults.License) : "unknown" },
            { "sortorder", "0" },
            { "repositorytype", "$@NULL@$" },
            { "repositoryid", "$@NULL@$" },
            { "reference", "$@NULL@$" },
        };

            WriteElements(writer, elements);
            writer.WriteEndElement(); // </file>

            CopyFileToBackupFolder(outputPath, fileStream, contentHash);
            fileStream.Dispose();
        }

        /// <summary>
        /// Copies the file currently being processed from the input folder to the Moodle backup folder.
        /// </summary>
        private void CopyFileToBackupFolder(string outputPath, FileStream fileStream, string contentHash)
        {
            string? targetFilePath = null;

            try
            {
                string targetDirectory = Path.Combine(outputPath, "files", contentHash.Substring(0, 2));
                _fileService.CreateDirectory(targetDirectory);
                targetFilePath = Path.Combine(targetDirectory, contentHash);

                _fileService.Copy(fileStream.Name, targetFilePath, true);
                LogDebug($"Copied {Path.GetFileName(fileStream.Name)} to {targetFilePath}");
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error copying file {Path.GetFileName(fileStream.Name)} to {targetFilePath ?? "undefined"}");
            }
        }

        /// <summary>
        /// Calculates the SHA1 hash of the file content.
        /// </summary>
        private string GetMoodleContentHash(FileStream fileStream)
        {
            string hash = string.Empty;

            try
            {
                using var sha1 = SHA1.Create();
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] hashBytes = sha1.ComputeHash(fileStream);
                fileStream.Seek(0, SeekOrigin.Begin);
                hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                LogWarn(ex, $"Error calculating hash for file {Path.GetFileName(fileStream.Name)}");
                throw;
            }

            LogDebug($"Calculated hash {hash} for file {Path.GetFileName(fileStream.Name)}");
            return hash;
        }
    }
}
