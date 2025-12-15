using System;
using System.Collections.Generic;
using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

using ChoiceModel = CourseAsCode.Model.Activities.Choice.Choice;

namespace CourseAsCode.Writers.XML.Writer.Activities.Choice
{
    public class ChoiceWriter : XmlWriterBase
    {
        private const string DEFAULT_CHOICE_RELATIVE_PATH = "choice.xml";
        private readonly IGeneralHelper _generalHelper;

        public ChoiceWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ChoiceWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WriteChoice(string outputPath, ChoiceModel choice)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CHOICE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, choice);

                    writer.WriteStartElement("choice");
                    writer.WriteAttributeString("id", choice.ActivityId.ToString());

                    // Core elements of <choice>
                    var elements = new Dictionary<string, string>
                    {
                        { "name", choice.Name },
                        { "intro", choice.Description ?? string.Empty },
                        { "introformat", _generalHelper.EnumIntegerToString(choice.DescriptionFormat) },
                        { "publish", "0" },
                        { "showresults", "0" },
                        { "display", "0" },
                        { "allowupdate", "0" },
                        { "allowmultiple", "0" },
                        { "showunanswered", "0" },
                        { "limitanswers", "0" },
                        { "timeopen", "0" },
                        { "timeclose", "0" },
                        { "timemodified", _generalHelper.DateTimeToUnixTimestamp(choice.TimeModified) },
                        { "completionsubmit", "0" },
                        { "showpreview", "0" },
                        { "includeinactive", "0" },
                        { "showavailable", "0" }
                    };

                    WriteElements(writer, elements);

                    // options
                    writer.WriteStartElement("options");
                    foreach (var opt in choice.Options)
                    {
                        writer.WriteStartElement("option");
                        writer.WriteAttributeString("id", opt.Id.ToString());

                        var optElements = new Dictionary<string, string>
                        {
                            { "text", opt.Text },
                            { "maxanswers", opt.MaxAnswers.ToString() },
                            { "timemodified", _generalHelper.DateTimeToUnixTimestamp(opt.TimeModified) }
                        };

                        WriteElements(writer, optElements);
                        writer.WriteEndElement(); // </option>
                    }
                    writer.WriteEndElement(); // </options>

                    writer.WriteStartElement("answers");
                    writer.WriteEndElement();

                    writer.WriteEndElement(); // </choice>
                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_CHOICE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_CHOICE_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, ChoiceModel choice)
        {
            writer.WriteAttributeString("id", choice.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", choice.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", choice.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(choice.Context));
        }
    }
}
