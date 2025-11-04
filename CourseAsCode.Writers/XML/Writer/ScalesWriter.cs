using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class ScalesWriter : XmlWriterBase
    {
        private const string DEFAULT_SCALES_RELATIVE_PATH = "scales.xml";

        public ScalesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ScalesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteScales(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_SCALES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("scales_definition");

                    if (course.Scales != null)
                    {
                        foreach (var scale in course.Scales)
                        {
                            WriteSingleScale(writer, scale);
                        }
                    }

                    writer.WriteEndElement(); // </scales_definition>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_SCALES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_SCALES_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteSingleScale(XmlWriter writer, Model.Scale scale)
        {
            WriteStartElementWithAttributes(writer, "scale", new Dictionary<string, string>
            {
                { "id", scale.Id.ToString() }
            });

            WriteScaleElements(writer, scale);

            writer.WriteEndElement(); // </scale>
            LogTrace($"Added scale {scale.Id}: '{scale.Name}' to {DEFAULT_SCALES_RELATIVE_PATH}");
        }

        private void WriteScaleElements(XmlWriter writer, Model.Scale scale)
        {
            var elements = new Dictionary<string, string>
            {
                { "courseid", scale.CourseId.ToString() },
                { "userid", scale.UserId.ToString() },
                { "name", scale.Name ?? string.Empty },
                { "scale", scale.ScaleValues ?? string.Empty },
                { "description", scale.Description ?? string.Empty },
                { "descriptionformat", scale.DescriptionFormat.ToString() },
                { "timemodified", scale.TimeModified.ToString() }
            };

            WriteElements(writer, elements);
        }
    }
}
