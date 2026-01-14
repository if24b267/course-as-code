using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface ITagsWriter
    {
        /// <summary>
        /// Writes the tags section of module.xml for an arbitrary activity using an XmlWriter; used for backwards compatibility
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write the tags to</param>
        /// <param name="tags">The tags to write</param>
        /// <param name="outputPath">The path to the output file</param>
        void WriteTags(XmlWriter xmlWriter, List<string>? tags, string outputPath);

        /// <summary>
        /// Writes the tags using the provided XML helper
        /// </summary>
        /// <param name="xmlHelper">The XML helper to use</param>
        /// <param name="tags">The tags to write</param>
        void WriteTags(IXmlService xmlHelper, List<string>? tags);
    }
}
