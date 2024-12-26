using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.Static;

namespace CourseAsCode.Writers.XML.Writer;

public class FilesWriter : XmlWriterBase
{
	private int _fileIdCounter = 1;
	
	public void WriteFiles(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "files.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		
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


	private void WriteSingleFile(XmlWriter writer, MoodleFile file)
	{
		WriteStartElementWithAttributes(writer, "file", new Dictionary<string, string>
		{
			{ "id", _fileIdCounter.ToString() }
		});
		_fileIdCounter++;
		
		var elements = new Dictionary<string, string>
		{
			{ "contenthash", file.SystemDefaults.ContentHash },
			{ "contextid", "14" }, // TODO: It probably makes sense to make this value dependent on the value of `Component`
			{ "component", file.SystemDefaults.Component.ToString() }, // TODO: Add function that maps enum value to correct moodle values
			{ "filearea", file.SystemDefaults.FileArea.ToString() }, // TODO: Add function that maps enum value to correct moodle values
			{ "itemid", "0" }, // TODO: Determine function of this field
			{ "filepath", file.FilePath  ?? "/"},
			{ "filename", file.FileName },
			{ "userid", file.SensibleDefaults.Author?.SystemDefaults.Id.ToString() ?? GeneralDefaults.DEFAULT_ADMIN_USER_ID.ToString() },
			{ "filesize", new System.IO.FileInfo(file.FilePath ?? string.Empty).Length.ToString() },
			{ "mimetype", "TODO" },
			{ "status", "0" }, // TODO: Determine function of this field
			{ "source", file.FilePath ?? "$@NULL@$" },
			{ "author", file.SensibleDefaults.Author?.Username ?? "$@NULL@$" },
			{ "license", file.SensibleDefaults.License?.ToString() ?? "unknown" }, // TODO: Add function that maps enum value to correct moodle values
			{ "sortorder", "0" }, // TODO: Determine function of this field
			{ "repositorytype", "$@NULL@$" }, // TODO: Determine function of this field
			{ "repositoryid", "$@NULL@$" }, // TODO: Determine function of this field
			{ "reference", "$@NULL@$" } // TODO: Determine function of this field
		};

		WriteElements(writer, elements);
	}
}