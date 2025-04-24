using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;


namespace CourseAsCode.Writers.XML.Writer.Activities.Assignment
{
    public class AssignmentWriter : XmlWriterBase
    {
        private const string DEFAULT_ASSIGNMENT_RELATIVE_PATH = "assign.xml";
        private readonly IGeneralHelper _generalHelper;

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public AssignmentWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
            _generalHelper = GeneralHelper.Instance;
        }

        /// <summary>
        /// Constructor using Dependency Injection for Unit Testing.
        /// </summary>
        /// <param name="xmlWriterFactory">Injected XML writer factory.</param>
        /// <param name="logger">Injected logger.</param>
        /// <param name="generalHelper">Injected general helper.</param>
        public AssignmentWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper;
        }

        /// <summary>
        /// Writes the assignment activity to an XML file.
        /// </summary>
        /// <param name="outputPath">The output path where the assignment XML file will be saved.</param>
        /// <param name="assignment">The Assignment activity to write.</param>
        public void WriteAssignment(string outputPath, Model.Activities.Resources.Assignment assignment)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ASSIGNMENT_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, assignment);
                    WriteAssignmentTag(writer, assignment);

                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_ASSIGNMENT_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_ASSIGNMENT_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Writes the attributes of the activity element.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="assignment">The Assignment activity to write.</param>
        private void WriteActivityAttributes(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteAttributeString("id", assignment.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", assignment.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", assignment.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(assignment.Context));
        }

        /// <summary>
        /// Writes the assignment element and its child elements.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="assignment">The Assignment activity to write.</param>
        private void WriteAssignmentTag(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("assign");
            writer.WriteAttributeString("id", assignment.ActivityId.ToString());

            var assignmentElements = new Dictionary<string, string>
            {
                { "name", assignment.Name },
                { "intro", assignment.Intro ?? string.Empty },
                { "introformat", _generalHelper.EnumIntegerToString(assignment.FormattingFormat) },

                { "alwaysshowdescription", assignment.AlwaysShowDescription ? "1" : "0" },
                { "submissiondrafts", assignment.SubmissionDrafts ? "1" : "0" },
                { "sendnotifications", assignment.SendNotifications ? "1" : "0" },
                { "sendlatenotifications", assignment.SendLateNotifications ? "1" : "0" },
                { "sendstudentnotifications", assignment.SendStudentNotifications ? "1" : "0" },

                // Convert nullable DateTime? fields safely:
                { "duedate", assignment.DueDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.DueDate.Value)
                    : "$@NULL@$" },
                { "cutoffdate", assignment.CutoffDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.CutoffDate.Value)
                    : "$@NULL@$" },
                { "gradingduedate", assignment.GradingDueDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.GradingDueDate.Value)
                    : "$@NULL@$" },
                { "allowsubmissionsfromdate", assignment.AllowSubmissionsFromDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.AllowSubmissionsFromDate.Value)
                    : "$@NULL@$" },

                { "grade", assignment.Grade.ToString() },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(assignment.TimeModified) },
                { "completionsubmit", assignment.CompletionSubmit ? "1" : "0" },
                { "requiresubmissionstatement", assignment.RequireSubmissionStatement ? "1" : "0" },
                { "teamsubmission", assignment.TeamSubmission ? "1" : "0" },
                { "requireallteammemberssubmit", assignment.RequireAllTeamMembersSubmit ? "1" : "0" },
                { "teamsubmissiongroupingid", assignment.TeamSubmissionGroupingId.ToString() },
                { "blindmarking", assignment.BlindMarking ? "1" : "0" },
                { "hidegrader", assignment.HideGrader ? "1" : "0" },
                { "revealidentities", assignment.RevealIdentities ? "1" : "0" },
                { "attemptreopenmethod", assignment.AttemptReopenMethod },
                { "maxattempts", assignment.MaxAttempts.ToString() },
                { "markingworkflow", assignment.MarkingWorkflow ? "1" : "0" },
                { "markingallocation", assignment.MarkingAllocation ? "1" : "0" },
                { "markinganonymous", assignment.MarkingAnonymous ? "1" : "0" },
                { "preventsubmissionnotingroup", assignment.PreventSubmissionNotInGroup ? "1" : "0" }
            };

            WriteElements(writer, assignmentElements);

            // Write nested elements
            WriteSubmissions(writer, assignment);
            WriteGrades(writer, assignment);
            WritePluginConfigs(writer, assignment);
            WriteOverrides(writer, assignment);

            writer.WriteEndElement(); // </assign>
        }

        private void WriteSubmissions(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("submissions");

            foreach (var submission in assignment.Submissions)
            {
                writer.WriteStartElement("submission");
                writer.WriteAttributeString("id", submission.SubmissionId.ToString());

                var submissionElements = new Dictionary<string, string>
           {
               { "userid", submission.UserId.ToString() },
               { "timecreated", _generalHelper.DateTimeToUnixTimestamp(submission.TimeCreated) },
               { "timemodified", submission.TimeModified.HasValue
                   ? _generalHelper.DateTimeToUnixTimestamp(submission.TimeModified.Value)
                   : "0" }, // Standardwert, wenn null
               { "timestarted", submission.TimeStarted ?? "$@NULL@$" }, // Standardwert, wenn null
               { "status", submission.Status },
               { "groupid", submission.GroupId.ToString() },
               { "attemptnumber", submission.AttemptNumber.ToString() },
               { "latest", submission.IsLatest ? "1" : "0" }
           };

                WriteElements(writer, submissionElements);

                // Write subplugin elements
                WriteEmptyContainer(writer, "subplugin_assignsubmission_file_submission");
                WriteEmptyContainer(writer, "subplugin_assignsubmission_onlinetext_submission");

                writer.WriteEndElement(); // </submission>
            }

            writer.WriteEndElement(); // </submissions>
        }


        private void WriteGrades(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("grades");
            // Add logic for grades if needed
            writer.WriteEndElement();
        }

        private void WritePluginConfigs(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("plugin_configs");

            foreach (var pluginConfig in assignment.PluginConfigs)
            {
                writer.WriteStartElement("plugin_config");
                writer.WriteAttributeString("id", pluginConfig.ConfigId.ToString());

                var pluginConfigElements = new Dictionary<string, string>
                {
                    { "plugin", pluginConfig.PluginName },
                    { "subtype", pluginConfig.Subtype },
                    { "name", pluginConfig.Name },
                    { "value", pluginConfig.Value }
                };

                WriteElements(writer, pluginConfigElements);

                writer.WriteEndElement(); // </plugin_config>
            }

            writer.WriteEndElement(); // </plugin_configs>
        }

        private void WriteOverrides(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("overrides");
            // Add logic for overrides if needed
            writer.WriteEndElement();
        }

        private void WriteEmptyContainer(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteEndElement();
        }
    }
}
