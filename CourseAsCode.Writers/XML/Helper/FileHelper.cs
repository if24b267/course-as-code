using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides utility methods for handling file-related operations.
    /// </summary>
    public class FileHelper : IFileHelper
    {
        private readonly ILogger<FileHelper> _logger;

        /// <summary>
        /// Creates a new instance of the FileHelper class
        /// </summary>
        /// <param name="logger">Logger instance specific to FileHelper</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public FileHelper(ILogger<FileHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public string GetFileAreaFromEnum(FileArea fileArea)
        {
            return fileArea switch
            {
                FileArea.Overview => "overviewfiles",
                FileArea.CourseContent => "content",
                _ => "content",
            };
        }


        public string GetComponentFromEnum(Component component)
        {
            return component switch
            {
                Component.Course => "course",
                Component.Resource => "mod_resource",
                Component.Forum => "mod_forum",
                Component.Glossary => "mod_glossary",
                Component.HtmlBlock => "block_html",

                _ => "course",
            };
        }


        public string GetLicenseFromEnum(License license)
        {
            return license switch
            {
                License.None => "unknown",
                License.AllRightsReserved => "allrightsreserved",
                License.PublicDomain => "public",
                License.CreativeCommons => "cc-4.0",
                License.CreativeCommonsNonCommercial => "cc-nc-4.0",
                License.CreativeCommonsNoDerivatives => "cc-nd-4.0",
                License.CreativeCommonsNonCommercialNoDerivatives => "cc-nc-nd-4.0",
                License.CreativeCommonsNonCommercialShareAlike => "cc-nc-sa-4.0",
                License.CreativeCommonsShareAlike => "cc-sa-4.0",

                _ => "unknown",
            };
        }
    }
}
