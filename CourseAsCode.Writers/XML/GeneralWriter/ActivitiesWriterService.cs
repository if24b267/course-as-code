using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Activities.Wiki;
using CourseAsCode.Writers.XML.Writer.Activities.Choice;
using CourseAsCode.Writers.XML.Writer.Activities.Feedback;
using CourseAsCode.Writers.XML.Writer.Activities.Folder;
using CourseAsCode.Writers.XML.Writer.Activities.Pages;
using CourseAsCode.Writers.XML.Writer.Activities.Scheduler;
using CourseAsCode.Writers.XML.FileWriters.Activities.Lesson;
using Microsoft.Extensions.Logging;
using CourseAsCode.Model.Activities.Choice;
using LessonModel = CourseAsCode.Model.Activities.Resources.Lesson;

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

            //Choice Activity Writers
            private readonly ChoiceWriter _choiceWriter;
            private readonly ModuleWriterChoice _moduleWriterChoice;

            // Feedback Activity Writers
            private readonly FeedbackWriter _feedbackWriter;
            private readonly ModuleWriterFeedback _moduleWriterFeedback;

            // Folder Activity Writers
            private readonly FolderWriter _folderWriter;
            private readonly ModuleWriterFolder _moduleWriterFolder;

            // Page Activity Writers
            private readonly PageWriter _pageWriter;
            private readonly ModuleWriterPage _moduleWriterPage;

            // Scheduler Activity Writers
            private readonly SchedulerWriter _schedulerWriter;
            private readonly ModuleWriterScheduler _moduleWriterScheduler;

            //Lesson Activity Writers
            private readonly LessonWriter _lessonWriter;
            private readonly ModuleWriterLesson _moduleWriterLesson;

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
                ChoiceWriter choiceWriter,
                ModuleWriterChoice moduleWriterChoice,
                FeedbackWriter feedbackWriter,
                ModuleWriterFeedback moduleWriterFeedback,
                FolderWriter folderWriter,
                ModuleWriterFolder moduleWriterFolder,
                PageWriter pageWriter,
                ModuleWriterPage moduleWriterPage,
                SchedulerWriter schedulerWriter,
                ModuleWriterScheduler moduleWriterScheduler,
                LessonWriter lessonWriter,
                ModuleWriterLesson moduleWriterLesson)
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

                //Choice Activity Writers
                _choiceWriter = choiceWriter ?? throw new ArgumentNullException(nameof(choiceWriter));
                _moduleWriterChoice = moduleWriterChoice ?? throw new ArgumentNullException(nameof(moduleWriterChoice));

                // Feedback Activity Writers
                _feedbackWriter = feedbackWriter ?? throw new ArgumentNullException(nameof(feedbackWriter));
                _moduleWriterFeedback = moduleWriterFeedback ?? throw new ArgumentNullException(nameof(moduleWriterFeedback));

                // Folder Activity Writers
                _folderWriter = folderWriter ?? throw new ArgumentNullException(nameof(folderWriter));
                _moduleWriterFolder = moduleWriterFolder ?? throw new ArgumentNullException(nameof(moduleWriterFolder));

                // Page Activity Writers
                _pageWriter = pageWriter ?? throw new ArgumentNullException(nameof(pageWriter));
                _moduleWriterPage = moduleWriterPage ?? throw new ArgumentNullException(nameof(moduleWriterPage));

                // Scheduler Activity Writers
                _schedulerWriter = schedulerWriter ?? throw new ArgumentNullException(nameof(schedulerWriter));
                _moduleWriterScheduler = moduleWriterScheduler ?? throw new ArgumentNullException(nameof(moduleWriterScheduler));

                //Lesson Activity Writers
                _lessonWriter = lessonWriter ?? throw new ArgumentNullException(nameof(lessonWriter));
                _moduleWriterLesson = moduleWriterLesson ?? throw new ArgumentNullException(nameof(moduleWriterLesson));

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
                        else if (activity is Choice choice)
                        {
                            activityPath = CreateActivityDirectory(outputPath, activity, "choice");
                            _choiceWriter.WriteChoice(activityPath, choice);
                            _moduleWriterChoice.WriteModule(activityPath, course, section, choice);
                        }
                        else if (activity is Feedback feedback)
                        {
                            activityPath = CreateActivityDirectory(outputPath, activity, "feedback");
                            _feedbackWriter.WriteFeedback(activityPath, feedback);
                            _moduleWriterFeedback.WriteModule(activityPath, course, section, feedback);
                        }
                        else if (activity is Folder folder)
                        {
                            activityPath = CreateActivityDirectory(outputPath, activity, "folder");
                            _folderWriter.WriteFolder(activityPath, folder);
                            _moduleWriterFolder.WriteModule(activityPath, course, section, folder);
                        }
                        else if (activity is Page page)
                        {
                            activityPath = CreateActivityDirectory(outputPath, activity, "page");
                            _pageWriter.WritePage(activityPath, page);
                            _moduleWriterPage.WriteModule(activityPath, course, section, page);
                        }
                        else if (activity is Scheduler scheduler)
                        {
                            activityPath = CreateActivityDirectory(outputPath, activity, "scheduler");
                            _schedulerWriter.WriteScheduler(activityPath, scheduler);
                            _moduleWriterScheduler.WriteModule(activityPath, course, section, scheduler);
                        }
                    else if (activity is LessonModel lesson)
                    {
                        activityPath = CreateActivityDirectory(outputPath, activity, "lesson");
                        if (string.IsNullOrEmpty(activityPath)) { _logger.LogWarning("Skipping lesson {ActivityId} due to directory creation failure", activity.ActivityId); continue; }

                        _logger.LogDebug("Writing lesson files to {Path} for activity {ActivityId}", activityPath, activity.ActivityId);
                        _lessonWriter.WriteLesson(activityPath, lesson);
                        _moduleWriterLesson.WriteModule(activityPath, course, section, lesson);
                        _logger.LogDebug("Finished writing lesson {ActivityId}", activity.ActivityId);
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
