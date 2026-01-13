using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using PageModel = CourseAsCode.Model.Activities.Resources.Page;

namespace CourseAsCode.Writers.XML.Writer.Activities.Pages
{
    public class PageWriter : XmlWriterBase
    {
        private const string DEFAULT_PAGE_RELATIVE_PATH = "page.xml";
        private readonly IGeneralHelper _generalHelper;

        public PageWriter(IXmlWriterFactory xmlWriterFactory, ILogger<PageWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WritePage(string outputPath, PageModel page)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_PAGE_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, page);

                    writer.WriteStartElement("page");
                    writer.WriteAttributeString("id", page.ActivityId.ToString());

                    var elements = new Dictionary<string, string>
                    {
                        { "name", page.Name },
                        { "intro", page.Intro ?? string.Empty },
                        { "introformat", _generalHelper.EnumIntegerToString(page.DescriptionFormat) },
                        { "content", page.Content ?? string.Empty },
                        { "contentformat", _generalHelper.EnumIntegerToString(page.ContentFormat) },
                        { "legacyfiles", page.LegacyFiles.ToString() },
                        { "legacyfileslast", page.LegacyFilesLast ?? "$@NULL@$" },
                        { "display", page.Display.ToString() },
                        { "displayoptions", page.DisplayOptions ?? string.Empty },
                        { "revision", page.Revision.ToString() },
                        { "timemodified", _generalHelper.DateTimeToUnixTimestamp(page.TimeModified) }
                    };

                    WriteElements(writer, elements);

                    writer.WriteEndElement(); // </page>
                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_PAGE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_PAGE_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, PageModel page)
        {
            writer.WriteAttributeString("id", page.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", page.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", page.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(page.Context));
        }
    }
}
