using System.Xml;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface ITagsWriter
    {
        /// <summary>
        /// Writes the tags section of module.xml for an arbitrary activity
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write the tags to</param>
        /// <param name="tags">The tags to write</param>
        /// <param name="outputPath">The path to the output file</param>
        void WriteTags(XmlWriter xmlWriter, List<string>? tags, string outputPath);
    }
}
