using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using FeedbackModel = CourseAsCode.Model.Activities.Resources.Feedback;

namespace CourseAsCode.Writers.XML.Writer.Activities.Feedback;

public class FeedbackWriter : XmlWriterBase
{
    private const string DEFAULT_FEEDBACK_RELATIVE_PATH = "feedback.xml";
    private readonly IGeneralHelper _generalHelper;

    public FeedbackWriter(IXmlWriterFactory xmlWriterFactory, ILogger<FeedbackWriter> logger, IGeneralHelper generalHelper)
        : base(xmlWriterFactory, logger)
    {
        _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
    }

    public void WriteFeedback(string outputPath, FeedbackModel feedback)
    {
        try
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FEEDBACK_RELATIVE_PATH));

            using (XmlWriter writer = CreateXmlWriter(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("activity");

                WriteActivityAttributes(writer, feedback);

                writer.WriteStartElement("feedback");
                writer.WriteAttributeString("id", feedback.ActivityId.ToString());

                var elements = new Dictionary<string, string>
                {
                    { "name", feedback.Name },
                    { "intro", feedback.Description ?? string.Empty },
                    { "introformat", _generalHelper.EnumIntegerToString(feedback.DescriptionFormat) },
                    { "anonymous", feedback.Anonymous.ToString() },
                    { "email_notification", feedback.EmailNotification.ToString() },
                    { "multiple_submit", feedback.MultipleSubmit.ToString() },
                    { "autonumbering", feedback.Autonumbering.ToString() },
                    { "site_after_submit", feedback.SiteAfterSubmit ?? string.Empty },
                    { "page_after_submit", feedback.PageAfterSubmit ?? string.Empty },
                    { "page_after_submitformat", _generalHelper.EnumIntegerToString(feedback.PageAfterSubmitFormat) },
                    { "publish_stats", feedback.PublishStats.ToString() },
                    { "timeopen", feedback.TimeOpen != null ? _generalHelper.DateTimeToUnixTimestamp((DateTime)feedback.TimeOpen) : "0" },
                    { "timeclose", feedback.TimeClose != null ? _generalHelper.DateTimeToUnixTimestamp((DateTime)feedback.TimeClose) : "0" },
                    { "timemodified", _generalHelper.DateTimeToUnixTimestamp(feedback.TimeModified) },
                    { "completionsubmit", feedback.CompletionSubmit ? "1" : "0" },
                };

                WriteElements(writer, elements);

                // items
                writer.WriteStartElement("items");
                foreach (var item in feedback.Items)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("id", item.Id.ToString());

                    var itemElements = new Dictionary<string, string>
                    {
                        { "template", item.Template.ToString() },
                        { "name", item.Name },
                        { "label", item.Label ?? string.Empty },
                        { "presentation", item.Presentation ?? string.Empty },
                        { "typ", item.Type },
                        { "hasvalue", item.HasValue.ToString() },
                        { "position", item.Position.ToString() },
                        { "required", item.Required.ToString() },
                        { "dependitem", item.DependItem.ToString() },
                        { "dependvalue", item.DependValue ?? string.Empty },
                        { "options", item.Options ?? string.Empty }
                    };

                    WriteElements(writer, itemElements);
                    writer.WriteEndElement(); // </item>
                }
                writer.WriteEndElement(); // </items>

                // completeds placeholder
                writer.WriteStartElement("completeds");
                writer.WriteEndElement();

                writer.WriteEndElement(); // </feedback>
                writer.WriteEndElement(); // </activity>
                writer.WriteEndDocument();
            }

            LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FEEDBACK_RELATIVE_PATH}");
        }
        catch (Exception e)
        {
            LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FEEDBACK_RELATIVE_PATH}");
            throw;
        }
    }

    private void WriteActivityAttributes(XmlWriter writer, FeedbackModel feedback)
    {
        writer.WriteAttributeString("id", feedback.ActivityId.ToString());
        writer.WriteAttributeString("moduleid", feedback.SystemDefaults.ModuleId.ToString());
        writer.WriteAttributeString("modulename", feedback.SystemDefaults.ModuleName);
        writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(feedback.Context));
    }
}
