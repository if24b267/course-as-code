using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class CompletionWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";

        public CompletionWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CompletionWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteCompletion(Model.Course course, string outputPath = "completion.xml")
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_completion");

                    if (course.Completions != null)
                    {
                        foreach (var completion in course.Completions)
                        {
                            WriteSingleCompletion(writer, completion);
                        }
                    }

                    writer.WriteEndElement(); // </course_completion>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteSingleCompletion(XmlWriter writer, Model.CourseCompletion completion)
        {
            WriteStartElementWithAttributes(writer, "course_completions", new Dictionary<string, string>
            {
                { "id", completion.Id.ToString() }
            });

            var elements = new Dictionary<string, string>
            {
                { "userid", completion.UserId.ToString() },
                { "course", completion.CourseId.ToString() },
                { "timeenrolled", completion.TimeEnrolled.ToString() },
                { "timestarted", completion.TimeStarted.ToString() },
                { "timecompleted", completion.TimeCompleted?.ToString() ?? "$@NULL@$" },
                { "reaggregate", completion.Reaggregate.ToString() }
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // </course_completions>
            LogTrace($"Added completion {completion.Id} for user {completion.UserId} to {DEFAULT_COMPLETION_RELATIVE_PATH}");
        }
    }
}
