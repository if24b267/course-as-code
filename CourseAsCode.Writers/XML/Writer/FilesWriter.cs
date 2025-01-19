using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Helper;
using CourseAsCode.Writers.Static;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class FilesWriter : XmlWriterBase
{
	private int _fileIdCounter = 1;
	private readonly FileHelper _fileHelper = FileHelper.Instance;
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
					WriteSingleFile(writer, (MoodleFile)activity);
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
	private void WriteSingleFile(XmlWriter writer, MoodleFile file)
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
			{ "id", _fileIdCounter++.ToString() }
		});

		var elements = new Dictionary<string, string>
		{
			{ "contenthash", file.SystemDefaults.ContentHash ?? GetSha1Hash(fileStream) },
			{ "contextid", file.SystemDefaults.Component == Component.Course ? "14" : "35" }, // TODO: It probably makes sense to make this value dependent on the value of `Component`
			{ "component", file.SystemDefaults.Component != null ? _fileHelper.GetComponentFromEnum((Component)file.SystemDefaults.Component) : "course" },
			{ "filearea", file.SystemDefaults.FileArea != null ? _fileHelper.GetFileAreaFromEnum((FileArea)file.SystemDefaults.FileArea) : "content" },
			{ "itemid", "0" }, // If there is only one of a file area per context, the itemid is 0, see https://moodledev.io/docs/5.0/apis/subsystems/files#file-areas
			{ "filepath", file.FilePath },
			{ "filename", file.FileName },
			{ "userid", file.SensibleDefaults.Author?.SystemDefaults.Id.ToString() ?? GeneralDefaults.DEFAULT_ADMIN_USER_ID.ToString() },
			{ "filesize", fileStream.Length.ToString() },
			{ "mimetype", MimeTypes.GetMimeType(file.FilePath) },
			{ "status", "0" }, // TODO: Determine function of this field
			{ "source", file.FileName },
			{ "author", file.SensibleDefaults.Author?.Username ?? "$@NULL@$" },
			{ "license", file.SensibleDefaults.License != null ? _fileHelper.GetLicenseFromEnum((License)file.SensibleDefaults.License) : "unknown" }, // TODO: Add function that maps enum value to correct moodle values
			{ "sortorder", "0" }, // TODO: Determine function of this field
			{ "repositorytype", "$@NULL@$" }, // TODO: Determine function of this field
			{ "repositoryid", "$@NULL@$" }, // TODO: Determine function of this field
			{ "reference", "$@NULL@$" } // TODO: Determine function of this field
		};

		WriteElements(writer, elements);
		fileStream.Dispose();
	}


	/// <summary>
	/// Calculates the SHA1 hash of the file content
	/// </summary>
	/// <param name="fileStream">FileStream of the file to be hashed</param>
	/// <returns>SHA1 hash of the file content</returns>
	private string GetSha1Hash(FileStream fileStream)
	{
		using SHA1 sha1 = SHA1.Create();
		byte[] hashBytes = sha1.ComputeHash(fileStream);

		StringBuilder sb = new();
		foreach (byte b in hashBytes)
		{
			sb.Append(b.ToString("X2"));
		}

		return sb.ToString();
	}
}