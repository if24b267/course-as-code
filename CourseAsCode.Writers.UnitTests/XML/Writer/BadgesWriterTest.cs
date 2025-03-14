using System.Xml;
using CourseAsCode.Writers.XML.Writer;

namespace CourseAsCode.Writers.UnitTests.XML.Writer
{
    [TestFixture]
    public class BadgesWriterTests
    {
        /// <summary>
        /// A test subclass of BadgesWriter that overrides protected methods to avoid file I/O and capture logging
        /// </summary>
        private class TestableBadgesWriter : BadgesWriter
        {
            public StringWriter StringWriter { get; private set; } = new StringWriter();
            public string CapturedDebugLog { get; private set; } = String.Empty;
            public (Exception Exception, string Message) CapturedErrorLog { get; private set; }

            
            /// <summary>
            /// Overrides the XML writer creation to use an in-memory StringWriter instead
            /// </summary>
            /// <param name="filePath">Ignored in this override</param>
            /// <returns>An XmlWriter writing to the StringWriter</returns>
            protected override XmlWriter CreateXmlWriter(string filePath)
            {
                return XmlWriter.Create(StringWriter, Settings);
            }

            
            /// <summary>
            /// Captures the debug log message instead of logging it externally
            /// </summary>
            /// <param name="message">The debug message</param>
            protected override void LogDebug(string message)
            {
                CapturedDebugLog = message;
            }

            
            /// <summary>
            /// Captures the error log details instead of logging them externally
            /// </summary>
            /// <param name="ex">The exception to log</param>
            /// <param name="message">The error message</param>
            protected override void LogError(Exception ex, string message)
            {
                CapturedErrorLog = (ex, message);
            }
        }
        
        
        /// <summary>
        /// A subclass of TestableBadgesWriter that simulates an exception during XmlWriter creation
        /// </summary>
        private class ExceptionThrowingBadgesWriter : TestableBadgesWriter
        {
            protected override XmlWriter CreateXmlWriter(string filePath)
            {
                throw new InvalidOperationException("Test exception");
            }
        }
        
        
        /// <summary>
        /// Verifies that the generated XML is well-formed.
        /// </summary>
        [Test]
        public void WriteBadges_GeneratesValidXml()
        {
            // Arrange
            var writer = new TestableBadgesWriter();
            var dummyOutputPath = "dummyPath";

            // Act
            writer.WriteBadges(dummyOutputPath);
            string output = writer.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception.
            Assert.DoesNotThrow(() =>
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(output);
            });
        }

        
        /// <summary>
        /// Checks that the root element of the generated XML is "badges".
        /// </summary>
        [Test]
        public void WriteBadges_RootElementIsCorrect()
        {
            // Arrange
            var writer = new TestableBadgesWriter();
            var dummyOutputPath = "dummyPath";

            // Act
            writer.WriteBadges(dummyOutputPath);
            string output = writer.StringWriter.ToString();

            // Assert: The root element should be "badges".
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);
            Assert.IsNotNull(xmlDoc.DocumentElement);
            Assert.That(xmlDoc.DocumentElement.Name, Is.EqualTo("badges"));
        }

        
        /// <summary>
        /// Verifies that a debug log message is captured upon successful writing
        /// </summary>
        [Test]
        public void WriteBadges_LogsDebugOnSuccess()
        {
            // Arrange
            var writer = new TestableBadgesWriter();
            var dummyOutputPath = "dummyPath";

            // Act
            writer.WriteBadges(dummyOutputPath);

            // Assert: Verify that the debug log message was captured.
            Assert.IsNotNull(writer.CapturedDebugLog);
            StringAssert.Contains("badges.xml", writer.CapturedDebugLog);
        }

        
        /// <summary>
        /// Verifies that an error is logged when an exception occurs during XML writer creation
        /// </summary>
        [Test]
        public void WriteBadges_LogsErrorOnException()
        {
            // Arrange
            var writer = new ExceptionThrowingBadgesWriter();
            var dummyOutputPath = "dummyPath";

            // Act
            Assert.Throws<InvalidOperationException>(() => writer.WriteBadges(dummyOutputPath));

            // Assert: Verify that the error log was captured.
            Assert.IsNotNull(writer.CapturedErrorLog);
            StringAssert.Contains("badges.xml", writer.CapturedErrorLog.Message);
        }
    }
}
