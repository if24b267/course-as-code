using System.Xml;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class TagsWriterTests
    {
        private ILogger<TagsWriter> _testLogger;
        private ITagsWriter _tagsWriter;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath/dummyfile.xml";

        [SetUp]
        public void SetUp()
        {
            // Create a substitute for the logger
            _testLogger = Substitute.For<ILogger<TagsWriter>>();
            // Instantiate TagsWriter with the mock logger
            _tagsWriter = new TagsWriter(_testLogger);
        }


        /// <summary>
        /// Helper method to execute TagsWriter.WriteTags and capture its XML output
        /// </summary>
        /// <param name="tags">The list of tags to pass in</param>
        /// <returns>XML output as a string</returns>
        private string WriteTagsAndGetOutput(List<string>? tags)
        {
            // Use StringWriter to capture XML output in memory
            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    _tagsWriter.WriteTags(xmlWriter, tags, DEFAULT_DUMMY_OUTPUT_PATH);
                }

                return stringWriter.ToString();
            }
        }


        [Test]
        public void WriteTags_NullTags_GeneratesValidXml()
        {
            // Arrange
            List<string>? tags = null;

            // Act
            string output = WriteTagsAndGetOutput(tags);

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteTags_NullTags_WritesEmptyTagsElement()
        {
            // Arrange
            List<string>? tags = null;

            // Act
            string output = WriteTagsAndGetOutput(tags);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The XML should have a root <tags> element with no children
            XmlNode? tagsNode = doc.DocumentElement;
            Assert.IsNotNull(tagsNode, "The XML should have a root element");
            Assert.That(tagsNode.Name, Is.EqualTo("tags"), "The root element should be <tags>");
            Assert.That(tagsNode.ChildNodes.Count, Is.EqualTo(0), "No child nodes should be present");
        }

        [Test]
        public void WriteTags_EmptyTags_WritesEmptyTagsElement()
        {
            // Arrange
            List<string> tags = new List<string>(); // empty list

            // Act
            string output = WriteTagsAndGetOutput(tags);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The XML should have a root <tags> element with no children.
            XmlNode? tagsNode = doc.DocumentElement;
            Assert.IsNotNull(tagsNode, "The XML should have a root element");
            Assert.That(tagsNode.Name, Is.EqualTo("tags"), "The root element should be <tags>");
            Assert.That(tagsNode.ChildNodes.Count, Is.EqualTo(0), "No child nodes should be present");
        }

        [Test]
        public void WriteTags_WithTags_GeneratesValidXml()
        {
            // Arrange
            List<string> tags = new List<string> { "Tag1", "Tag2" };

            // Act
            string output = WriteTagsAndGetOutput(tags);

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteTags_WithTags_WritesCorrectXmlStructure()
        {
            // Arrange
            List<string> tags = new List<string> { "Tag1", "Tag2" };

            // Act
            string output = WriteTagsAndGetOutput(tags);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that the XML has a <tags> element with two <tag> child elements.
            XmlNode? tagsNode = doc.DocumentElement;
            Assert.IsNotNull(tagsNode, "The XML should have a root element");
            Assert.That(tagsNode.Name, Is.EqualTo("tags"), "The root element should be <tags>");
            Assert.That(tagsNode.ChildNodes.Count, Is.EqualTo(2), "There should be two <tag> elements");

            // Verify first <tag> element
            XmlElement? tagElement1 = tagsNode.ChildNodes[0] as XmlElement;
            Assert.IsNotNull(tagElement1, "The first <tag> element is missing.");
            Assert.That(tagElement1.Name, Is.EqualTo("tag"));
            Assert.That(tagElement1.GetAttribute("id"), Is.EqualTo("0"), "The id attribute of the first tag should be '0'");
            Assert.IsNotNull(tagElement1["name"], "The <name> element in the first tag is missing");
            Assert.That(tagElement1["name"].InnerText, Is.EqualTo("Tag1"));
            Assert.IsNotNull(tagElement1["rawname"], "The <rawname> element in the first tag is missing");
            Assert.That(tagElement1["rawname"].InnerText, Is.EqualTo("Tag1"));

            // Verify second <tag> element
            XmlElement? tagElement2 = tagsNode.ChildNodes[1] as XmlElement;
            Assert.IsNotNull(tagElement2, "The second <tag> element is missing.");
            Assert.That(tagElement2.Name, Is.EqualTo("tag"));
            Assert.That(tagElement2.GetAttribute("id"), Is.EqualTo("1"), "The id attribute of the second tag should be '1'");
            Assert.IsNotNull(tagElement2["name"], "The <name> element in the second tag is missing");
            Assert.That(tagElement2["name"].InnerText, Is.EqualTo("Tag2"));
            Assert.IsNotNull(tagElement2["rawname"], "The <rawname> element in the second tag is missing");
            Assert.That(tagElement2["rawname"].InnerText, Is.EqualTo("Tag2"));
        }

        [Test]
        public void WriteTags_WithTags_LogsTraceMessagesForEachTag()
        {
            // Arrange
            List<string> tags = new List<string> { "Tag1", "Tag2" };
            string expectedFileName = Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH); // "dummyfile.xml"

            // Act
            string output = WriteTagsAndGetOutput(tags);

            // Assert: Ensure that logger.Trace is called for each tag with the expected message.
            _testLogger.Received(1).LogTrace(Arg.Is<string>(msg => msg.Contains("Tag1") && msg.Contains(expectedFileName)));
            _testLogger.Received(1).LogTrace(Arg.Is<string>(msg => msg.Contains("Tag2") && msg.Contains(expectedFileName)));
        }
    }
}
