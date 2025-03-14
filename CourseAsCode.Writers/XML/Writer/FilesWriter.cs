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
using NLog;
using NLog.Fluent;

namespace CourseAsCode.Writers.XML.Writer;

public class FilesWriter : XmlWriterBase
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
	private const string DEFAULT_FILE_RELATIVE_PATH = "files.xml";
	
	private readonly FileHelper _fileHelper;
	private readonly GeneralHelper _generalHelper;
	
	
	// Constructor
	public FilesWriter()
	{
		_fileHelper = FileHelper.Instance;
		_generalHelper = GeneralHelper.Instance;
	}
	
	
	// Constructor supporting Dependency Injection for Unit Testing
	public FilesWriter(FileHelper fileHelper, GeneralHelper generalHelper)
	{
		_fileHelper = fileHelper;
		_generalHelper = generalHelper;
	}
	
	
	public void WriteFiles(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILE_RELATIVE_PATH));

		try
		{
			using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
			{
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

			Logger.Debug($"Created {DEFAULT_FILE_RELATIVE_PATH}");
		}
		catch (Exception e)
		{
			Logger.Error(e, $"Error writing {DEFAULT_FILE_RELATIVE_PATH}");
			throw;
		}
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
				Logger.Warn($"{file.FilePath} does not exist. Skipping file");
				return;
			}

			fileStream = File.OpenRead(file.FilePath);
			Logger.Debug($"Found {file.FilePath}");
		}
		catch (Exception ex)
		{
			Logger.Error(ex, $"Error opening {file.FilePath}. Skipping file");
			return;
		}

		string? contentHash = null;
		try
		{
			contentHash = GetMoodleContentHash(fileStream);
		}
		catch (Exception ex)
		{
			Logger.Error(ex, $"Skipping file {Path.GetFileName(fileStream.Name)}");
			return;
		}

		// Write XML
		WriteStartElementWithAttributes(writer, "file", new Dictionary<string, string>
		{
			{ "id", file.ActivityId.ToString() }
		});

		var elements = new Dictionary<string, string>
		{
			{ "contenthash", contentHash },
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
		CopyFileToBackupFolder(outputPath, fileStream, contentHash);
		fileStream.Dispose();
	}


	/// <summary>
	/// copies the file currently being processed from the input folder to the moodle backup folder
	/// </summary>
	/// <param name="outputPath"></param>
	/// <param name="fileStream"></param>
	private void CopyFileToBackupFolder(string outputPath, FileStream fileStream, string contentHash)
	{
		string? targetFilePath = null;
		
		try
		{
			string targetDirectory = Path.Combine(outputPath, "files", contentHash.Substring(0, 2));

			Directory.CreateDirectory(targetDirectory);

			targetFilePath = Path.Combine(targetDirectory, contentHash);
       
			File.Copy(fileStream.Name, targetFilePath,true);
		
			Logger.Debug($"Copied {Path.GetFileName(fileStream.Name)} to {targetFilePath}");
		}
		catch (Exception ex)
		{
			Logger.Error(ex, $"Error copying file {Path.GetFileName(fileStream.Name)} to {targetFilePath ?? "undefined"}");
		}
	}


	/// <summary>
	/// Calculates the SHA1 hash of the file content
	/// </summary>
	/// <param name="fileStream">FileStream of the file to be hashed</param>
	/// <returns>SHA1 hash of the file content</returns>
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
			Logger.Warn(ex, $"Error calculating hash for file {Path.GetFileName(fileStream.Name)}");
			throw;
		}
		
		Logger.Debug($"Calculated hash {hash} for file {Path.GetFileName(fileStream.Name)}");
		return hash;
	}
}