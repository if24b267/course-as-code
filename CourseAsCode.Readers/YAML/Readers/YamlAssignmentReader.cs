using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlAssignmentReader : IAssignmentReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlFileReaderHelper;

        public YamlAssignmentReader(IYamlFileReaderHelper yamlFileReaderHelper)
        {
            _yamlFileReaderHelper = yamlFileReaderHelper;
        }

        public Assignment Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            var yamlAssignment = _yamlFileReaderHelper.DeserializeFromFile<YamlAssignment>(modelObjectPath);

            if (yamlAssignment == null)
            {
                _logger.Error($"Could not deserialize the file {modelObjectPath}.");
                return null;
            }

            var moodleAssignment = ConvertYamlAssignmentToMoodleAssignment(yamlAssignment);
            return moodleAssignment;
        }

        private Assignment ConvertYamlAssignmentToMoodleAssignment(YamlAssignment yamlAssignment)
        {
            return new Assignment()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlAssignment.Name,
                Description = yamlAssignment.Description,
                Intro = yamlAssignment.Intro ?? AssignmentDefaults.DEFAULT_INTRO,
                FormattingFormat = yamlAssignment.AssignmentTextFormat ?? AssignmentDefaults.DEFAULT_FORMATTING_FORMAT,
                AlwaysShowDescription =
                    yamlAssignment.AlwaysShowDescription ?? AssignmentDefaults.DEFAULT_ALWAYS_SHOW_DESCRIPTION,
                SubmissionDrafts = yamlAssignment.AllowSubmissionDrafts ?? AssignmentDefaults.DEFAULT_SUBMISSION_DRAFTS,
                SendNotifications = yamlAssignment.SendNotifications ?? AssignmentDefaults.DEFAULT_SEND_NOTIFICATIONS,
                SendLateNotifications =
                    yamlAssignment.SendLateNotifications ?? AssignmentDefaults.DEFAULT_SEND_LATE_NOTIFICATIONS,
                SendStudentNotifications =
                    yamlAssignment.SendStudentNotifications ?? AssignmentDefaults.DEFAULT_SEND_STUDENT_NOTIFICATIONS,
                DueDate = yamlAssignment.DueDate ?? AssignmentDefaults.DEFAULT_DUE_DATE,
                CutoffDate = yamlAssignment.CutoffDate ?? AssignmentDefaults.DEFAULT_CUTOFF_DATE,
                GradingDueDate = yamlAssignment.GradingDueDate ?? AssignmentDefaults.DEFAULT_GRADING_DUE_DATE,
                AllowSubmissionsFromDate = yamlAssignment.AllowSubmissionsFromDate ?? AssignmentDefaults.DEFAULT_ALLOW_SUBMISSIONS_FROM_DATE,
                MaxGrade = yamlAssignment.Grade ?? AssignmentDefaults.DEFAULT_GRADE_MAXIMUM,
                CompletionSubmit = yamlAssignment.CompletionSubmit ?? AssignmentDefaults.DEFAULT_COMPLETION_SUBMIT,
                RequireSubmissionStatement =
                    yamlAssignment.RequireSubmissionStatement ??
                    AssignmentDefaults.DEFAULT_REQUIRE_SUBMISSION_STATEMENT,
                TeamSubmission = yamlAssignment.AllowTeamSubmissions ?? AssignmentDefaults.DEFAULT_TEAM_SUBMISSION,
                RequireAllTeamMembersSubmit =
                    yamlAssignment.RequireAllTeamMembersSubmit ??
                    AssignmentDefaults.DEFAULT_REQUIRE_ALL_TEAM_MEMBERS_SUBMIT,
                BlindMarking = yamlAssignment.BlindMarking ?? AssignmentDefaults.DEFAULT_BLIND_MARKING,
                HideGrader = yamlAssignment.HideGrader ?? AssignmentDefaults.DEFAULT_HIDE_GRADER,
                RevealIdentities = yamlAssignment.RevealIdentities ?? AssignmentDefaults.DEFAULT_REVEAL_IDENTITIES,
                AttemptReopenMethod =
                    yamlAssignment.AttemptReopenMethod ?? AssignmentDefaults.DEFAULT_ATTEMPT_REOPEN_METHOD,
                MaxAttempts = yamlAssignment.MaxAttempts ?? AssignmentDefaults.DEFAULT_MAX_ATTEMPTS,
                MarkingWorkflow = yamlAssignment.MarkingWorkflow ?? AssignmentDefaults.DEFAULT_MARKING_WORKFLOW,
                MarkingAllocation = yamlAssignment.MarkingAllocation ?? AssignmentDefaults.DEFAULT_MARKING_ALLOCATION,
                MarkingAnonymous = yamlAssignment.MarkingAnonymous ?? AssignmentDefaults.DEFAULT_MARKING_ANONYMOUS,
                PreventSubmissionNotInGroup = yamlAssignment.PreventSubmissionNotInGroup ??
                                              AssignmentDefaults.DEFAULT_PREVENT_SUBMISSION_NOT_IN_GROUP,

                ModuleId = ValueKeeper.CurrentModuleId,
                ModuleName = "assign",
            };
        }
    }
}
