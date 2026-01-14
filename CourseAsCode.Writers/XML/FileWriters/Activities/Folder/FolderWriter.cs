using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using FolderModel = CourseAsCode.Model.Activities.Resources.Folder;

namespace CourseAsCode.Writers.XML.Writer.Activities.Folder
{
    public class FolderWriter : XmlWriterBase
    {
        private const string DEFAULT_FOLDER_RELATIVE_PATH = "folder.xml";
        private readonly IGeneralHelper _generalHelper;

        public FolderWriter(IXmlWriterFactory xmlWriterFactory, ILogger<FolderWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WriteFolder(string outputPath, FolderModel folder)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FOLDER_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, folder);

                    writer.WriteStartElement("folder");
                    writer.WriteAttributeString("id", folder.ActivityId.ToString());

                    var elements = new Dictionary<string, string>
                    {
                        { "name", folder.Name },
                        { "intro", folder.Intro ?? string.Empty },
                        { "introformat", _generalHelper.EnumIntegerToString(folder.DescriptionFormat) },
                        { "revision", folder.Revision.ToString() },
                        { "timemodified", _generalHelper.DateTimeToUnixTimestamp(folder.TimeModified) },
                        { "display", folder.Display.ToString() },
                        { "showexpanded", folder.ShowExpanded ? "1" : "0" },
                        { "showdownloadfolder", folder.ShowDownloadFolder ? "1" : "0" },
                        { "forcedownload", folder.ForceDownload ? "1" : "0" }
                    };

                    WriteElements(writer, elements);

                    writer.WriteEndElement(); // </folder>
                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FOLDER_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FOLDER_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, FolderModel folder)
        {
            writer.WriteAttributeString("id", folder.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", folder.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", folder.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", folder.ActivityId.ToString());
        }
    }
}
