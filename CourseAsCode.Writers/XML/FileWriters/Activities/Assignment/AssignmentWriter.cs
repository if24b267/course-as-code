using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Assignment
{
    public class AssignmentWriter : XmlWriterBase
    {
        private const string DEFAULT_ASSIGNMENT_RELATIVE_PATH = "assign.xml";
        private readonly IGeneralHelper _generalHelper;

        public AssignmentWriter(IXmlWriterFactory xmlWriterFactory, ILogger<AssignmentWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

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

        private void WriteActivityAttributes(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteAttributeString("id", assignment.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", assignment.ModuleId.ToString());
            writer.WriteAttributeString("modulename", assignment.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(assignment.Context));
        }

        private void WriteAssignmentTag(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("assign");
            writer.WriteAttributeString("id", assignment.ActivityId.ToString());

            var sensibleDefaults = assignment.SensibleDefaults ?? new Model.Activities.Resources.Assignment.SensibleDefaultsAssignment
            {
                Activity = "",
                ActivityFormat = 1,
                TimeLimit = 0,
                SubmissionAttachments = 0
            };

            var assignmentElements = new Dictionary<string, string>
            {
                { "name", assignment.Name ?? "Assignment" },
                { "intro", assignment.Intro ?? "" },
                { "introformat", _generalHelper.EnumIntegerToString(assignment.FormattingFormat) },
                { "alwaysshowdescription", assignment.AlwaysShowDescription ? "1" : "0" },
                { "submissiondrafts", assignment.SubmissionDrafts ? "1" : "0" },
                { "sendnotifications", assignment.SendNotifications ? "1" : "0" },
                { "sendlatenotifications", assignment.SendLateNotifications ? "1" : "0" },
                { "sendstudentnotifications", assignment.SendStudentNotifications ? "1" : "0" },
                { "duedate", assignment.DueDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.DueDate.Value)
                    : "1746050400" },
                { "cutoffdate", assignment.CutoffDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.CutoffDate.Value)
                    : "0" },
                { "gradingduedate", assignment.GradingDueDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.GradingDueDate.Value)
                    : "1746655200" },
                { "allowsubmissionsfromdate", assignment.AllowSubmissionsFromDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.AllowSubmissionsFromDate.Value)
                    : "1745445600" },
                { "grade", assignment.Grade != 0 ? assignment.Grade.ToString() : "100" },
                { "timemodified", assignment.TimeModified != default
                    ? _generalHelper.DateTimeToUnixTimestamp(assignment.TimeModified)
                    : "1745515519" },
                { "completionsubmit", assignment.CompletionSubmit ? "1" : "0" },
                { "requiresubmissionstatement", assignment.RequireSubmissionStatement ? "1" : "0" },
                { "teamsubmission", assignment.TeamSubmission ? "1" : "0" },
                { "requireallteammemberssubmit", assignment.RequireAllTeamMembersSubmit ? "1" : "0" },
                { "teamsubmissiongroupingid", assignment.TeamSubmissionGroupingId != 0 ? assignment.TeamSubmissionGroupingId.ToString() : "0" },
                { "blindmarking", assignment.BlindMarking ? "1" : "0" },
                { "hidegrader", assignment.HideGrader ? "1" : "0" },
                { "revealidentities", assignment.RevealIdentities ? "1" : "0" },
                { "attemptreopenmethod", !string.IsNullOrEmpty(assignment.AttemptReopenMethod) ? assignment.AttemptReopenMethod : "untilpass" },
                { "maxattempts", assignment.MaxAttempts != 0 ? assignment.MaxAttempts.ToString() : "1" },
                { "markingworkflow", assignment.MarkingWorkflow ? "1" : "0" },
                { "markingallocation", assignment.MarkingAllocation ? "1" : "0" },
                { "markinganonymous", assignment.MarkingAnonymous ? "1" : "0" },
                { "preventsubmissionnotingroup", assignment.PreventSubmissionNotInGroup ? "1" : "0" },
                { "activity", sensibleDefaults.Activity ?? "" },
                { "activityformat", sensibleDefaults.ActivityFormat?.ToString() ?? "1" },
                { "timelimit", sensibleDefaults.TimeLimit?.ToString() ?? "0" },
                { "submissionattachments", sensibleDefaults.SubmissionAttachments?.ToString() ?? "0" }
            };

            WriteElements(writer, assignmentElements);

            writer.WriteStartElement("userflags");
            writer.WriteEndElement();

            WriteSubmissions(writer, assignment);
            WriteGrades(writer, assignment);
            WritePluginConfigs(writer, assignment);
            WriteOverrides(writer, assignment);

            writer.WriteEndElement(); // </assign>
        }

        private void WriteSubmissions(XmlWriter writer, Model.Activities.Resources.Assignment assignment)
        {
            writer.WriteStartElement("submissions");

            var submissions = assignment.Submissions;
            if (submissions == null || submissions.Count == 0)
            {
                submissions = new List<Model.Activities.Resources.AssignmentSubmission>
                {
                    new Model.Activities.Resources.AssignmentSubmission
                    {
                        SubmissionId = 3,
                        UserId = 2,
                        TimeCreated = DateTimeOffset.FromUnixTimeSeconds(1745515520).UtcDateTime,
                        TimeModified = DateTimeOffset.FromUnixTimeSeconds(1745515520).UtcDateTime,
                        TimeStarted = null,
                        Status = "new",
                        GroupId = 0,
                        AttemptNumber = 0,
                        IsLatest = true
                    }
                };
            }

            foreach (var submission in submissions)
            {
                writer.WriteStartElement("submission");
                writer.WriteAttributeString("id", submission.SubmissionId.ToString());

                var submissionElements = new Dictionary<string, string>
                {
                    { "userid", submission.UserId.ToString() },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(submission.TimeCreated) },
                    { "timemodified", submission.TimeModified.HasValue
                        ? _generalHelper.DateTimeToUnixTimestamp(submission.TimeModified.Value)
                        : "1745515520" },
                    { "timestarted", submission.TimeStarted ?? "$@NULL@$" },
                    { "status", submission.Status ?? "new" },
                    { "groupid", submission.GroupId.ToString() },
                    { "attemptnumber", submission.AttemptNumber.ToString() },
                    { "latest", submission.IsLatest ? "1" : "0" }
                };

                WriteElements(writer, submissionElements);

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

            var pluginConfigs = assignment.PluginConfigs;
            if (pluginConfigs == null || pluginConfigs.Count == 0)
            {
                pluginConfigs = new List<Model.Activities.Resources.PluginConfig>
                {
                    new Model.Activities.Resources.PluginConfig { ConfigId = 23, PluginName = "onlinetext", Subtype = "assignsubmission", Name = "enabled", Value = "0" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 24, PluginName = "file", Subtype = "assignsubmission", Name = "enabled", Value = "1" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 25, PluginName = "file", Subtype = "assignsubmission", Name = "maxfilesubmissions", Value = "20" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 26, PluginName = "file", Subtype = "assignsubmission", Name = "maxsubmissionsizebytes", Value = "0" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 27, PluginName = "file", Subtype = "assignsubmission", Name = "filetypeslist", Value = "*" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 28, PluginName = "comments", Subtype = "assignsubmission", Name = "enabled", Value = "1" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 29, PluginName = "comments", Subtype = "assignfeedback", Name = "enabled", Value = "1" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 30, PluginName = "comments", Subtype = "assignfeedback", Name = "commentinline", Value = "0" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 31, PluginName = "editpdf", Subtype = "assignfeedback", Name = "enabled", Value = "0" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 32, PluginName = "offline", Subtype = "assignfeedback", Name = "enabled", Value = "0" },
                    new Model.Activities.Resources.PluginConfig { ConfigId = 33, PluginName = "file", Subtype = "assignfeedback", Name = "enabled", Value = "0" }
                };
            }

            foreach (var pluginConfig in pluginConfigs)
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

            var overrides = assignment.Overrides ?? new List<Model.Activities.Resources.AssignmentOverride>();

            foreach (var overrideItem in overrides)
            {
                writer.WriteStartElement("override");
                writer.WriteAttributeString("id", overrideItem.OverrideId.ToString());
                writer.WriteAttributeString("userid", overrideItem.UserId.ToString());
                writer.WriteAttributeString("duedate", overrideItem.DueDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(overrideItem.DueDate.Value)
                    : "0");
                writer.WriteAttributeString("cutoffdate", overrideItem.CutoffDate.HasValue
                    ? _generalHelper.DateTimeToUnixTimestamp(overrideItem.CutoffDate.Value)
                    : "0");
                writer.WriteEndElement(); // </override>
            }

            writer.WriteEndElement(); // </overrides>
        }

        private void WriteEmptyContainer(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteEndElement();
        }
    }
}
