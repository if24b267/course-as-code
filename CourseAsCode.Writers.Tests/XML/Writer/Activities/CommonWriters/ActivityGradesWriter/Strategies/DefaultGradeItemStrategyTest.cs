using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    [TestFixture]
    public class DefaultGradeItemStrategyTests
    {
        private DefaultGradeItemStrategy _strategy;
        private IXmlHelper _xmlHelper;
        private XmlWriterFactoryTest _testFactory;

        [SetUp]
        public void SetUp()
        {
            _strategy = new DefaultGradeItemStrategy();
            _xmlHelper = Substitute.For<IXmlHelper>();
            _testFactory = new XmlWriterFactoryTest();

            // Set up XmlHelper to pass through the calls to the mocked writer from the factory
            _xmlHelper.When(x => x.WriteElements(Arg.Any<XmlWriter>(), Arg.Any<Dictionary<string, string>>()))
                .Do(callInfo =>
                {
                    var writer = callInfo.ArgAt<XmlWriter>(0);
                    var elements = callInfo.ArgAt<Dictionary<string, string>>(1);

                    foreach (var element in elements)
                    {
                        writer.WriteElementString(element.Key, element.Value);
                    }
                });

            _xmlHelper.When(x => x.WriteElementWithAttributes(Arg.Any<XmlWriter>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>()))
                .Do(callInfo =>
                {
                    var writer = callInfo.ArgAt<XmlWriter>(0);
                    var elementName = callInfo.ArgAt<string>(1);
                    var attributes = callInfo.ArgAt<Dictionary<string, string>>(2);

                    writer.WriteStartElement(elementName);
                    foreach (var attribute in attributes)
                    {
                        writer.WriteAttributeString(attribute.Key, attribute.Value);
                    }
                });
        }

        [Test]
        public void WriteGradeItems_DoesNotWriteAnything()
        {
            // Arrange
            var urlActivity = FakeUrlFactory.CreateBasicUrl(1);
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act
            _strategy.WriteGradeItems(writer, urlActivity);
            var output = _testFactory.StringWriter.ToString();

            // Assert
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void WriteGradeItems_DoesNotThrow_WithAnyActivityType()
        {
            // Arrange
            var quiz = FakeQuizFactory.CreateBasicQuiz(1);
            using var writer = _testFactory.Create("dummy.xml", new XmlWriterSettings());

            // Act & Assert
            Assert.DoesNotThrow(() => _strategy.WriteGradeItems(writer, quiz));
        }
    }
}
