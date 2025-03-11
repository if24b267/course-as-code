using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Static;

namespace CourseAsCode.Writers.XML.Writer;

public class FilesWriter : XmlWriterBase
{
	private readonly FileHelper _fileHelper = FileHelper.Instance;
	private readonly GeneralHelper _generalHelper = new GeneralHelper();
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	
	public void WriteFiles(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "files.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);

		writer.WriteStartDocument();

		writer.WriteStartElement("files");

		foreach (Section section in course.Sections)
		{
			foreach (Activity activity in section.Activities)
			{
				if (activity is MoodleFile)
				{
					WriteSingleFile(writer, outputPath, (MoodleFile)activity);
				}
			}
		}

		writer.WriteEndElement(); // Close <files>
		
		writer.WriteEndDocument();
	}


	/// <summary>
	/// Writes a single file to the XML file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="file">File to be written</param>
	private void WriteSingleFile(XmlWriter writer, string outputPath, MoodleFile file)
	{
		// Check if file exists / try to open the file
		FileStream? fileStream = null;
		try
		{
			if (!File.Exists(file.FilePath))
			{
				_logger.Warn($"File {file.FilePath} does not exist. Skipping file.");
				return;
			}

			fileStream = File.OpenRead(file.FilePath);
			_logger.Debug($"Opened file {file.FilePath} successfully.");
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error while trying to open file {file.FilePath}. Skipping file.");
			return;
		}


		// Write XML
		WriteStartElementWithAttributes(writer, "file", new Dictionary<string, string>
		{
			{ "id", file.ActivityId.ToString() }
		});

		var elements = new Dictionary<string, string>
		{
			{ "contenthash", GetMoodleContentHash(fileStream) },
			{ "contextid", _generalHelper.EnumIntegerToString(file.Context) },
			{ "component", file.SystemDefaults.Component != null ? _fileHelper.GetComponentFromEnum((Component)file.SystemDefaults.Component) : "course" },
			{ "filearea", file.SystemDefaults.FileArea != null ? _fileHelper.GetFileAreaFromEnum((FileArea)file.SystemDefaults.FileArea) : "content" },
			{ "itemid", "0" }, // If there is only one of a file area per context, the itemid is 0, see https://moodledev.io/docs/5.0/apis/subsystems/files#file-areas
			{ "filepath", "/" }, // TODO: Why is this always "/"?
			{ "filename", file.FileName },
			{ "userid", file.SensibleDefaults.Author?.SystemDefaults.Id.ToString() ?? GeneralDefaults.DEFAULT_ADMIN_USER_ID.ToString() },
			{ "filesize", fileStream.Length.ToString() },
			{ "mimetype", MimeTypes.GetMimeType(file.FilePath) },
			{ "status", "0" }, // TODO: Determine function of this field
			{ "timecreated", _generalHelper.DateTimeToUnixTimestamp(file.TimeCreated)},
			{ "timemodified", _generalHelper.DateTimeToUnixTimestamp(file.TimeModified)},
			{ "source", file.FileName },
			{ "author", file.SensibleDefaults.Author?.Username ?? "$@NULL@$" },
			{ "license", file.SensibleDefaults.License != null ? _fileHelper.GetLicenseFromEnum((License)file.SensibleDefaults.License) : "unknown" }, // TODO: Add function that maps enum value to correct moodle values
			{ "sortorder", "0" }, // TODO: Determine function of this field
			{ "repositorytype", "$@NULL@$" }, // TODO: Determine function of this field
			{ "repositoryid", "$@NULL@$" }, // TODO: Determine function of this field
			{ "reference", "$@NULL@$" } // TODO: Determine function of this field
		};

		WriteElements(writer, elements);
		CopyFileToBackupFolder(outputPath, fileStream);
		fileStream.Dispose();
	}


	/// <summary>
	/// copies the file currently being processed from the input folder to the moodle backup folder
	/// </summary>
	/// <param name="outputPath"></param>
	/// <param name="fileStream"></param>
	private void CopyFileToBackupFolder(string outputPath, FileStream fileStream)
	{
		string fileHash = GetMoodleContentHash(fileStream);
		string targetDirectory = Path.Combine(outputPath, "files", fileHash.Substring(0, 2));

		Directory.CreateDirectory(targetDirectory);

		string targetFilePath = Path.Combine(targetDirectory, fileHash);
       
		File.Copy(fileStream.Name, targetFilePath,true);
	}


	/// <summary>
	/// Calculates the SHA1 hash of the file content
	/// </summary>
	/// <param name="fileStream">FileStream of the file to be hashed</param>
	/// <returns>SHA1 hash of the file content</returns>
	private string GetMoodleContentHash(FileStream fileStream)
	{
		using var sha1 = SHA1.Create();
		fileStream.Seek(0, SeekOrigin.Begin);

		byte[] hashBytes = sha1.ComputeHash(fileStream);

		fileStream.Seek(0, SeekOrigin.Begin);

		return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
	}
}