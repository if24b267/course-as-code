using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Wiki
{
    public class WikiWriter : XmlWriterBase
    {
        private const string DEFAULT_WIKI_RELATIVE_PATH = "wiki.xml";
        private readonly IGeneralHelper _generalHelper;

        public WikiWriter(IXmlWriterFactory xmlWriterFactory, ILogger<WikiWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WriteWiki(string outputPath, Model.Activities.Resources.Wiki wiki)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_WIKI_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, wiki);
                    WriteWikiTag(writer, wiki);

                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_WIKI_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_WIKI_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, Model.Activities.Resources.Wiki wiki)
        {
            writer.WriteAttributeString("id", wiki.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", wiki.ModuleId.ToString());
            writer.WriteAttributeString("modulename", wiki.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(wiki.Context));
        }

        private void WriteWikiTag(XmlWriter writer, Model.Activities.Resources.Wiki wiki)
        {
            writer.WriteStartElement("wiki");
            writer.WriteAttributeString("id", wiki.ActivityId.ToString());

            var wikiElements = new Dictionary<string, string>
            {
                { "name", wiki.Name ?? "Wiki" },
                { "intro", wiki.Intro ?? "" },
                { "introformat", _generalHelper.EnumIntegerToString(wiki.IntroFormat) },
                { "timecreated", wiki.TimeCreated != default
                    ? _generalHelper.DateTimeToUnixTimestamp(wiki.TimeCreated)
                    : "0" },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(wiki.TimeModified) },
                { "firstpagetitle", wiki.FirstPageTitle ?? "First Page" },
                { "wikimode", ConvertWikiModeToString(wiki.WikiMode) },
                { "defaultformat", ConvertWikiFormatToString(wiki.DefaultFormat) },
                { "forceformat", wiki.ForceFormat ? "1" : "0" },
                { "editbegin", wiki.EditBegin.ToString() },
                { "editend", wiki.EditEnd.ToString() }
            };

            WriteElements(writer, wikiElements);

            WriteSubwikis(writer, wiki);

            writer.WriteEndElement(); // </wiki>
        }

        private void WriteSubwikis(XmlWriter writer, Model.Activities.Resources.Wiki wiki)
        {
            writer.WriteStartElement("subwikis");

            var subwikis = wiki.Subwikis;
            if (subwikis == null || subwikis.Count == 0)
            {
                // Create default subwiki with first page if none exist
                subwikis = new List<Model.Activities.Resources.WikiSubwiki>
                {
                    new Model.Activities.Resources.WikiSubwiki
                    {
                        SubwikiId = 1,
                        GroupId = 0,
                        UserId = 0,
                        Pages = new List<Model.Activities.Resources.WikiPage>
                        {
                            new Model.Activities.Resources.WikiPage
                            {
                                PageId = 1,
                                Title = wiki.FirstPageTitle ?? "First Page",
                                CachedContent = "",
                                TimeCreated = DateTime.Now,
                                TimeModified = DateTime.Now,
                                TimeRendered = DateTime.Now,
                                UserId = 2,
                                PageViews = 0,
                                ReadOnly = false,
                                Versions = new List<Model.Activities.Resources.WikiPageVersion>
                                {
                                    new Model.Activities.Resources.WikiPageVersion
                                    {
                                        VersionId = 1,
                                        Content = "",
                                        ContentFormat = wiki.DefaultFormat,
                                        Version = 0,
                                        TimeCreated = DateTime.Now,
                                        UserId = 2
                                    }
                                }
                            }
                        }
                    }
                };
            }

            foreach (var subwiki in subwikis)
            {
                writer.WriteStartElement("subwiki");
                writer.WriteAttributeString("id", subwiki.SubwikiId.ToString());

                var subwikiElements = new Dictionary<string, string>
                {
                    { "groupid", subwiki.GroupId.ToString() },
                    { "userid", subwiki.UserId.ToString() }
                };

                WriteElements(writer, subwikiElements);

                WritePages(writer, subwiki);
                WriteEmptyContainer(writer, "synonyms");
                WriteEmptyContainer(writer, "links");

                writer.WriteEndElement(); // </subwiki>
            }

            writer.WriteEndElement(); // </subwikis>
        }

        private void WritePages(XmlWriter writer, Model.Activities.Resources.WikiSubwiki subwiki)
        {
            writer.WriteStartElement("pages");

            foreach (var page in subwiki.Pages)
            {
                writer.WriteStartElement("page");
                writer.WriteAttributeString("id", page.PageId.ToString());

                var pageElements = new Dictionary<string, string>
                {
                    { "title", page.Title ?? "Page" },
                    { "cachedcontent", page.CachedContent ?? "" },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(page.TimeCreated) },
                    { "timemodified", _generalHelper.DateTimeToUnixTimestamp(page.TimeModified) },
                    { "timerendered", _generalHelper.DateTimeToUnixTimestamp(page.TimeRendered) },
                    { "userid", page.UserId.ToString() },
                    { "pageviews", page.PageViews.ToString() },
                    { "readonly", page.ReadOnly ? "1" : "0" }
                };

                WriteElements(writer, pageElements);

                WriteVersions(writer, page);
                WriteEmptyContainer(writer, "tags");

                writer.WriteEndElement(); // </page>
            }

            writer.WriteEndElement(); // </pages>
        }

        private void WriteVersions(XmlWriter writer, Model.Activities.Resources.WikiPage page)
        {
            writer.WriteStartElement("versions");

            foreach (var version in page.Versions)
            {
                writer.WriteStartElement("version");
                writer.WriteAttributeString("id", version.VersionId.ToString());

                var versionElements = new Dictionary<string, string>
                {
                    { "content", version.Content ?? "" },
                    { "contentformat", ConvertWikiFormatToString(version.ContentFormat) },
                    { "version", version.Version.ToString() },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(version.TimeCreated) },
                    { "userid", version.UserId.ToString() }
                };

                WriteElements(writer, versionElements);

                writer.WriteEndElement(); // </version>
            }

            writer.WriteEndElement(); // </versions>
        }

        private void WriteEmptyContainer(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteEndElement();
        }

        private string ConvertWikiModeToString(Model.Enums.Wikis.WikiMode wikiMode)
        {
            return wikiMode switch
            {
                Model.Enums.Wikis.WikiMode.Collaborative => "collaborative",
                Model.Enums.Wikis.WikiMode.Individual => "individual",
                _ => "collaborative"
            };
        }

        private string ConvertWikiFormatToString(Model.Enums.Wikis.WikiFormat wikiFormat)
        {
            return wikiFormat switch
            {
                Model.Enums.Wikis.WikiFormat.Html => "html",
                Model.Enums.Wikis.WikiFormat.Creole => "creole",
                Model.Enums.Wikis.WikiFormat.Nwiki => "nwiki",
                _ => "html"
            };
        }
    }
}
