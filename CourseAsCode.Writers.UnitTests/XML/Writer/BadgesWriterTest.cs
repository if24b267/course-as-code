using System.Xml;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.UnitTests.XML.Writer
{
    [TestFixture]
    public class BadgesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        BadgesWriter _testBadgesWriter;
        
        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();
            _testBadgesWriter = new BadgesWriter(_testFactory, _testLogger); // Use DI to inject the test factory and logger
        }
        
        
        [Test]
        public void WriteBadges_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _testBadgesWriter.WriteBadges(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteBadges_RootElementIsCorrect()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _testBadgesWriter.WriteBadges(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element should be "badges"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement);
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("badges"));
        }

        [Test]
        public void WriteBadges_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _testBadgesWriter.WriteBadges(dummyOutputPath);

            // Assert: Verify that logger.Debug is called with a message containing "badges.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("badges.xml")));
        }

        [Test]
        public void WriteBadges_LogsErrorOnException()
        {
            // Arrange
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _testBadgesWriter = new BadgesWriter(throwingFactory, _testLogger);
            
            string dummyOutputPath = "dummyPath";

            // Act & Assert: An exception should be thrown
            Assert.Throws<InvalidOperationException>(() => _testBadgesWriter.WriteBadges(dummyOutputPath));
            
            // Verify that logger.Error is called with a message containing "badges.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("badges.xml")));
        }
    }
}
