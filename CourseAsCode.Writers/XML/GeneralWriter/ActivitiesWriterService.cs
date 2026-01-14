using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter;
using CourseAsCode.Writers.XML.FileWriters.Activities.Forum;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Activities.Wiki;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.GeneralWriter
{
    public class ActivitiesWriterService : IActivitiesWriterService
    {
        private readonly ILogger<ActivitiesWriterService> _logger;
        private readonly IFileService _fileService;

        // -- Activity Writers -- //
        // Common Activity Writers
        private readonly ActivityGradesWriter _activityGradesWriter;
        private readonly ActivityGradehistoryWriter _activityGradehistoryWriter;
        private readonly ActivityInforefWriter _activityInforefWriter;
        private readonly ActivityRolesWriter _activityRolesWriter;
        private readonly ActivityXapistateWriter _activityXapistateWriter;
        private readonly ActivityCalendarWriter _activityCalendarWriter;
        private readonly ActivityCommentsWriter _activityCommentsWriter;
        private readonly ActivityCompetenciesWriter _activityCompetenciesWriter;
        private readonly ActivityCompletionWriter _activityCompletionWriter;
        private readonly ActivityFiltersWriter _activityFiltersWriter;

        // Text and Media Area (Label) Activity Writers
        private readonly LabelWriter _labelWriter;
        private readonly ModuleWriterLabel _moduleWriterLabel;

        // Assignment Activity Writers
        private readonly AssignmentWriter _assignmentWriter;
        private readonly ModuleWriterAssignment _moduleWriterAssignment;

        // URL Activity Writers
        private readonly UrlWriter _urlWriter;
        private readonly ModuleWriterUrl _moduleWriterUrl;

        // File Activity Writers
        private readonly FileWriter _fileWriter;
        private readonly ModuleWriterFile _moduleWriterFile;

        // Quiz Activity Writers
        private readonly ModuleWriterQuiz _moduleWriterQuiz;
        private readonly QuizWriter _quizWriter;

        // Wiki Activity Writers
        private readonly WikiWriter _wikiWriter;
        private readonly ModuleWriterWiki _moduleWriterWiki;

        // Forum Activity Writers
        private readonly ForumWriter _forumWriter;
        private readonly ModuleWriterForum _moduleWriterForum;

        public ActivitiesWriterService(
            ILogger<ActivitiesWriterService> logger,
            IFileService fileService,
            ActivityGradesWriter activityGradesWriter,
            ActivityGradehistoryWriter activityGradehistoryWriter,
            ActivityInforefWriter activityInforefWriter,
            ActivityRolesWriter activityRolesWriter,
            ActivityXapistateWriter activityXapistateWriter,
            ActivityCalendarWriter activityCalendarWriter,
            ActivityCommentsWriter activityCommentsWriter,
            ActivityCompetenciesWriter activityCompetenciesWriter,
            ActivityCompletionWriter activityCompletionWriter,
            ActivityFiltersWriter activityFiltersWriter,
            LabelWriter labelWriter,
            ModuleWriterLabel moduleWriterLabel,
            AssignmentWriter assignmentWriter,
            ModuleWriterAssignment moduleWriterAssignment,
            UrlWriter urlWriter,
            ModuleWriterUrl moduleWriterUrl,
            FileWriter fileWriter,
            ModuleWriterFile moduleWriterFile,
            ModuleWriterQuiz moduleWriterQuiz,
            QuizWriter quizWriter,
            WikiWriter wikiWriter,
            ModuleWriterWiki moduleWriterWiki,
            ForumWriter forumWriter,
            ModuleWriterForum moduleWriterForum)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));

            // -- Activity Writers Assignment -- //
            // Common Activity Writers
            _activityGradesWriter = activityGradesWriter ?? throw new ArgumentNullException(nameof(activityGradesWriter));
            _activityGradehistoryWriter = activityGradehistoryWriter ?? throw new ArgumentNullException(nameof(activityGradehistoryWriter));
            _activityInforefWriter = activityInforefWriter ?? throw new ArgumentNullException(nameof(activityInforefWriter));
            _activityRolesWriter = activityRolesWriter ?? throw new ArgumentNullException(nameof(activityRolesWriter));
            _activityXapistateWriter = activityXapistateWriter ?? throw new ArgumentNullException(nameof(activityXapistateWriter));
            _activityCalendarWriter = activityCalendarWriter ?? throw new ArgumentNullException(nameof(activityCalendarWriter));
            _activityCommentsWriter = activityCommentsWriter ?? throw new ArgumentNullException(nameof(activityCommentsWriter));
            _activityCompetenciesWriter = activityCompetenciesWriter ?? throw new ArgumentNullException(nameof(activityCompetenciesWriter));
            _activityCompletionWriter = activityCompletionWriter ?? throw new ArgumentNullException(nameof(activityCompletionWriter));
            _activityFiltersWriter = activityFiltersWriter ?? throw new ArgumentNullException(nameof(activityFiltersWriter));

            // Text and Media Area (Label) Activity Writers
            _labelWriter = labelWriter ?? throw new ArgumentNullException(nameof(labelWriter));
            _moduleWriterLabel = moduleWriterLabel ?? throw new ArgumentNullException(nameof(moduleWriterLabel));

            // Assignment Activity Writers
            _assignmentWriter = assignmentWriter ?? throw new ArgumentNullException(nameof(assignmentWriter));
            _moduleWriterAssignment = moduleWriterAssignment ?? throw new ArgumentNullException(nameof(moduleWriterAssignment));

            // URL Activity Writers
            _urlWriter = urlWriter ?? throw new ArgumentNullException(nameof(urlWriter));
            _moduleWriterUrl = moduleWriterUrl ?? throw new ArgumentNullException(nameof(moduleWriterUrl));

            // File Activity Writers
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
            _moduleWriterFile = moduleWriterFile ?? throw new ArgumentNullException(nameof(moduleWriterFile));

            // Quiz Activity Writers
            _moduleWriterQuiz = moduleWriterQuiz ?? throw new ArgumentNullException(nameof(moduleWriterQuiz));
            _quizWriter = quizWriter ?? throw new ArgumentNullException(nameof(quizWriter));

            // Wiki Activity Writers
            _wikiWriter = wikiWriter ?? throw new ArgumentNullException(nameof(wikiWriter));
            _moduleWriterWiki = moduleWriterWiki ?? throw new ArgumentNullException(nameof(moduleWriterWiki));

            // Forum Activity Writers
            _forumWriter = forumWriter ?? throw new ArgumentNullException(nameof(forumWriter));
            _moduleWriterForum = moduleWriterForum ?? throw new ArgumentNullException(nameof(moduleWriterForum));
        }

        /// <summary>
        /// Writes all activity data for a given course into their respective directories
        /// </summary>
        /// <param name="course">The course object containing the sections and activities to be written.</param>
        /// <param name="outputPath">The root directory path where activity files and directories will be created.</param>
        public void WriteActivities(Course course, string outputPath)
        {
            // Iterate through all sections of the course
            foreach (var section in course.Sections)
            {
                // Iterate through all activities of that section
                foreach (var activity in section.Activities)
                {
                    _logger.LogDebug($"Writing activity {activity.ActivityId}: {activity.Name}");
                    string activityPath = string.Empty;

                    // Assemble the path for the activity based on its type and write its specific files
                    if (activity is TextAndMediaArea textAndMediaArea)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "label");

                        _labelWriter.WriteLabel(activityPath, textAndMediaArea);
                        _moduleWriterLabel.WriteModule(activityPath, course, section, textAndMediaArea);
                    }
                    else if (activity is Assignment assignment)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "assign");

                        _assignmentWriter.WriteAssignment(activityPath, assignment);
                        _moduleWriterAssignment.WriteModule(activityPath, course, section, assignment);
                    }
                    else if (activity is URL urlObject)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "url");

                        _urlWriter.WriteUrl(activityPath, urlObject);
                        _moduleWriterUrl.WriteModule(activityPath, course, section, urlObject);
                    }
                    else if (activity is MoodleFile moodleFile)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "resource");

                        _fileWriter.WriteFile(activityPath, moodleFile);
                        _moduleWriterFile.WriteModule(activityPath, course, section, moodleFile);
                    }
                    else if (activity is Quiz quiz)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "quiz");

                        _quizWriter.WriteQuiz(activityPath, quiz);
                        _moduleWriterQuiz.WriteModule(activityPath, course, section, quiz);
                    }
                    else if (activity is Wiki wiki)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "wiki");

                        _wikiWriter.WriteWiki(activityPath, wiki);
                        _moduleWriterWiki.WriteModule(activityPath, course, section, wiki);
                    }
                    else if (activity is Forum forum)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "forum");

                        _forumWriter.WriteForum(activityPath, forum);
                        _moduleWriterForum.WriteModule(activityPath, course, section, forum);
                    }
                    else
                    {
                        // If the activity type is not handled, we log a warning and skip it
                        _logger.LogWarning($"Activity type {activity.GetType().Name} for {activity.Name} (ID: {activity.ActivityId}) unknown. Skipping specific writers...");
                    }

                    WriteCommonActivityFiles(activityPath, course, activity);
                }
            }
        }


        /// <summary>
        /// <para>Creates a new directory for storing the data belonging to a specific activity</para>
        /// </summary>
        /// <param name="outputPath">The root directory path where the activity directory will be created.</param>
        /// <param name="activity">The activity object containing information such as the unique activity ID.</param>
        /// <param name="prefix">A string prefix used to identify the type of activity (e.g., "label", "assign", etc.).</param>
        /// <returns>
        /// The full path of the created activity directory if successful; returns null if the directory creation fails.
        /// </returns>
        private string CreateActivityDirectory(string outputPath, Activity activity, string prefix)
        {
            string path = Path.Combine(outputPath, "activities", $"{prefix}_{activity.ActivityId}");

            if (!_fileService.CreateDirectory(path))
            {
                _logger.LogError($"Skipping activity {activity.ActivityId}: {activity.Name} due to directory creation failure at {path}");
                return null;
            }

            return path;
        }


        /// <summary>
        /// Writes common activity-related XML files such as calendar, comments, competencies, completion data, and more.
        /// </summary>
        /// <param name="activityPath">The directory path where the activity-related files will be written.</param>
        /// <param name="course">The course object containing metadata and context for the activity.</param>
        /// <param name="activity">The activity object containing details and attributes for a specific activity.</param>
        private void WriteCommonActivityFiles(string activityPath, Course course, Activity activity)
        {
            _activityCalendarWriter.WriteCalendar(activityPath);
            _activityCommentsWriter.WriteComments(activityPath);
            _activityCompetenciesWriter.WriteCompetencies(activityPath);
            _activityCompletionWriter.WriteCompletion(activityPath);
            _activityFiltersWriter.WriteFilters(activityPath);
            _activityGradesWriter.WriteGrade(activity, activityPath);
            _activityGradehistoryWriter.WriteGradeHistory(course, activityPath);
            _activityInforefWriter.WriteInforefLabel(course, activityPath, activity);
            _activityRolesWriter.WriteRoles(activityPath);
            _activityXapistateWriter.WriteState(activityPath);
        }
    }
}
