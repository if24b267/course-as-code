using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins;
using CourseAsCode.Writers.XML.Writer.Sections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers
{
    public class Writer : IWriter
    {
        // -- General Writers -- //
        private readonly CalendarWriter _calendarWriter;
        private readonly CommentsWriter _commentsWriter;
        private readonly CompetenciesWriter _competenciesWriter;
        private readonly CompletiondefaultsWriter _completiondefaultsWriter;
        private readonly ContentbankWriter _contentbankWriter;
        private readonly CourseWriter _courseWriter;
        private readonly FilesWriter _filesWriter;
        private readonly GradehistoryWriter _gradehistoryWriter;
        private readonly GradebookWriter _gradebookWriter;
        private readonly GroupsWriter _groupsWriter;
        private readonly MoodleBackupWriter _moodleBackupWriter;
        private readonly OutcomesWriter _outcomesWriter;
        private readonly QuestionsWriter _questionsWriter;
        private readonly RolesWriter _rolesWriter;
        private readonly ScalesWriter _scalesWriter;
        private readonly UsersWriter _usersWriter;
        private readonly EnrolmentsWriter _enrolmentsWriter;
        private readonly FiltersWriter _filtersWriter;
        private readonly InforefWriter _inforefWriter;
        private readonly RoleAssignmentsWriter _roleAssignmentsWriter;
        private readonly BadgesWriter _badgesWriter;
        private readonly CompletionWriter _completionWriter;
        private readonly SectionWriter _sectionWriter;
        private readonly InforefSectionsWriter _inforefSectionsWriter;

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

        // -- Additional Dependencies -- //
        private readonly ILogger<Writer> _logger;

        public Writer(
            CalendarWriter calendarWriter,
            CommentsWriter commentsWriter,
            CompetenciesWriter competenciesWriter,
            CompletiondefaultsWriter completiondefaultsWriter,
            ContentbankWriter contentbankWriter,
            CourseWriter courseWriter,
            FilesWriter filesWriter,
            GradehistoryWriter gradehistoryWriter,
            GradebookWriter gradebookWriter,
            GroupsWriter groupsWriter,
            MoodleBackupWriter moodleBackupWriter,
            OutcomesWriter outcomesWriter,
            QuestionsWriter questionsWriter,
            RolesWriter rolesWriter,
            ScalesWriter scalesWriter,
            UsersWriter usersWriter,
            EnrolmentsWriter enrolmentsWriter,
            FiltersWriter filtersWriter,
            InforefWriter inforefWriter,
            RoleAssignmentsWriter roleAssignmentsWriter,
            BadgesWriter badgesWriter,
            CompletionWriter completionWriter,
            SectionWriter sectionWriter,
            InforefSectionsWriter inforefSectionsWriter,
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
            ILogger<Writer> logger)
        {
            // -- General Writers Assignment -- //
            _calendarWriter = calendarWriter ?? throw new ArgumentNullException(nameof(calendarWriter));
            _commentsWriter = commentsWriter ?? throw new ArgumentNullException(nameof(commentsWriter));
            _competenciesWriter = competenciesWriter ?? throw new ArgumentNullException(nameof(competenciesWriter));
            _completiondefaultsWriter = completiondefaultsWriter ?? throw new ArgumentNullException(nameof(completiondefaultsWriter));
            _contentbankWriter = contentbankWriter ?? throw new ArgumentNullException(nameof(contentbankWriter));
            _courseWriter = courseWriter ?? throw new ArgumentNullException(nameof(courseWriter));
            _filesWriter = filesWriter ?? throw new ArgumentNullException(nameof(filesWriter));
            _gradehistoryWriter = gradehistoryWriter ?? throw new ArgumentNullException(nameof(gradehistoryWriter));
            _gradebookWriter = gradebookWriter ?? throw new ArgumentNullException(nameof(gradebookWriter));
            _groupsWriter = groupsWriter ?? throw new ArgumentNullException(nameof(groupsWriter));
            _moodleBackupWriter = moodleBackupWriter ?? throw new ArgumentNullException(nameof(moodleBackupWriter));
            _outcomesWriter = outcomesWriter ?? throw new ArgumentNullException(nameof(outcomesWriter));
            _questionsWriter = questionsWriter ?? throw new ArgumentNullException(nameof(questionsWriter));
            _rolesWriter = rolesWriter ?? throw new ArgumentNullException(nameof(rolesWriter));
            _scalesWriter = scalesWriter ?? throw new ArgumentNullException(nameof(scalesWriter));
            _usersWriter = usersWriter ?? throw new ArgumentNullException(nameof(usersWriter));
            _enrolmentsWriter = enrolmentsWriter ?? throw new ArgumentNullException(nameof(enrolmentsWriter));
            _filtersWriter = filtersWriter ?? throw new ArgumentNullException(nameof(filtersWriter));
            _inforefWriter = inforefWriter ?? throw new ArgumentNullException(nameof(inforefWriter));
            _roleAssignmentsWriter = roleAssignmentsWriter ?? throw new ArgumentNullException(nameof(roleAssignmentsWriter));
            _badgesWriter = badgesWriter ?? throw new ArgumentNullException(nameof(badgesWriter));
            _completionWriter = completionWriter ?? throw new ArgumentNullException(nameof(completionWriter));
            _sectionWriter = sectionWriter ?? throw new ArgumentNullException(nameof(sectionWriter));
            _inforefSectionsWriter = inforefSectionsWriter ?? throw new ArgumentNullException(nameof(inforefSectionsWriter));

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

            // -- Additional Dependencies Assignment -- //
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public bool WriteCourse(Course course, string outputPath)
        {
            _logger.LogDebug($"Writer started for course '{course.FullName}' with ID {course.SystemDefaults.Id}");
            _logger.LogInformation($"The course will be saved at {outputPath}");

            // Ensure sections list is not empty
            if (course.Sections.Count == 0)
            {
                _logger.LogInformation("No sections specified by user, adding default section");
                course.Sections = new List<Section> { SectionDefaults.GetDefaultSection() };
            }

            // -- Write course files -- //

            // Create course directory if it does not exist
            if (!SafeCreateDirectory(Path.Combine(outputPath, "course")))
            {
                Environment.Exit(1);
            }

            // Root directory
            _badgesWriter.WriteBadges(outputPath);
            _completionWriter.WriteCompletion(course, outputPath);
            _filesWriter.WriteFiles(course, outputPath);
            _gradebookWriter.WriteGradebook(course, outputPath);
            _gradehistoryWriter.WriteGradehistory(course, outputPath);
            _groupsWriter.WriteGroups(course, outputPath);
            _moodleBackupWriter.WriteMoodleBackup(course, outputPath);
            _outcomesWriter.WriteOutcomes(course, outputPath);
            _questionsWriter.WriteQuestions(course, outputPath);
            _rolesWriter.WriteRoles(course, outputPath);
            _scalesWriter.WriteScales(course, outputPath);
            _usersWriter.WriteUsers(course, outputPath);

            // /course directory
            _calendarWriter.WriteCalendar(course, outputPath);
            _commentsWriter.WriteComments(course, outputPath);
            _competenciesWriter.WriteCompetencies(course, outputPath);
            _completiondefaultsWriter.WriteCompletionDefaults(course, outputPath);
            _contentbankWriter.WriteContentbank(course, outputPath);
            _courseWriter.WriteCourse(course, outputPath);
            _enrolmentsWriter.WriteEnrolment(course, outputPath);
            _filtersWriter.WriteFilters(course, outputPath);
            _inforefWriter.WriteInforef(course, outputPath);
            _roleAssignmentsWriter.WriteRoleAssignments(course, outputPath);


            // -- Write section files -- //

            // Create sections directory if it does not exist
            if (!SafeCreateDirectory(Path.Combine(outputPath, "sections")))
            {
                Environment.Exit(1);
            }

            // Create section.xml for each section
            _sectionWriter.WriteSections(outputPath, course.Sections);

            // Create inforef.xml for each section
            _inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);

            // -- Write activity files -- //

            // Ensure there is an activities and a files directory (used by activities that require files)
            if (!SafeCreateDirectory(Path.Combine(outputPath, "activities")) || !SafeCreateDirectory(Path.Combine(outputPath, "files")))
            {
                Environment.Exit(1);
            }

            // Iterate through all sections of the course
            foreach (var section in course.Sections)
            {
                // Iterate through all activities of that section
                foreach (var activity in section.Activities)
                {
                    _logger.LogDebug($"Writing activity {activity.ActivityId}: {activity.Name}");

                    string? activityPath = string.Empty;

                    // Text and Media Area (Label)-specific XML files
                    if (activity is TextAndMediaArea textAndMediaArea)
                    {
                        activityPath = CreateTextAndMediaArea(course, outputPath, activity, textAndMediaArea, section);
                    }

                    // Assignment-specific XML files
                    if (activity is Assignment assignment)
                    {
                        activityPath = CreateAssignmentActivity(course, outputPath, activity, assignment, section);
                    }


                    // URL-specific XML files
                    if (activity is URL urlObject)
                    {
                        activityPath = CreateUrlActivity(course, outputPath, activity, urlObject, section);
                    }

                    // File-specific XML files
                    if (activity is MoodleFile moodleFile)
                    {
                        activityPath = CreateFileActivity(course, outputPath, activity, moodleFile, section);
                    }

                    if (activity is Quiz quiz)
                    {
                        activityPath = CreateQuizActivity(course, outputPath, activity, quiz, section);
                    }

                    // Skip if activityPath is null (i.e. directory creation failed)
                    if (activityPath == null)
                    {
                        _logger.LogError($"Skipping activity {activity.ActivityId}: {activity.Name} due to directory creation failure");
                        continue;
                    }

                    // General activity XML files (common for all activities)
                    _activityCalendarWriter.WriteCalendar(activityPath);
                    _activityCommentsWriter.WriteComments(activityPath);
                    _activityCompetenciesWriter.WriteCompetencies(activityPath);
                    _activityCompletionWriter.WriteCompletion(activityPath);
                    _activityFiltersWriter.WriteFilters(activityPath);
                    _activityGradesWriter.WriteGrade(activityPath);
                    _activityGradehistoryWriter.WriteGradeHistory(course, activityPath);
                    _activityInforefWriter.WriteInforefLabel(course, activityPath, activity);
                    _activityRolesWriter.WriteRoles(activityPath);
                    _activityXapistateWriter.WriteState(activityPath);
                }
            }

            return true;
        }

        /// <summary>
        /// writes the XML files specific to the File activity.
        /// </summary>
        /// <returns>
        /// <para> the path to the directory where the files for that activity were written. </para>
        /// <para> null if the directory couldn't be created.</para>
        /// </returns>
        private string? CreateFileActivity(Course course, string outputPath, Activity activity, MoodleFile moodleFile, Section section)
        {
            return CreateActivity(outputPath, activity, "resource", activityPath =>
            {
                _fileWriter.WriteFile(activityPath, moodleFile);
                _moduleWriterFile.WriteModule(activityPath, course, section, moodleFile);
            });
        }

        /// <summary>
        /// writes the XML files specific to the URL activity.
        /// </summary>
        /// <returns>
        /// <para> the path to the directory where the files for that activity were written. </para>
        /// <para> null if the directory couldn't be created.</para>
        /// </returns>
        private string? CreateUrlActivity(Course course, string outputPath, Activity activity, URL urlObject, Section section)
        {
            return CreateActivity(outputPath, activity, "url", activityPath =>
            {
                _urlWriter.WriteUrl(activityPath, urlObject);
                _moduleWriterUrl.WriteModule(activityPath, course, section, urlObject);
            });
        }

        /// <summary>
        /// writes the XML files specific to the Text and Media Area (Label) activity.
        /// </summary>
        /// <returns>
        /// <para> the path to the directory where the files for that activity were written. </para>
        /// <para> null if the directory couldn't be created.</para>
        /// </returns>
        private string? CreateTextAndMediaArea(Course course, string outputPath, Activity activity, TextAndMediaArea textAndMediaArea, Section section)
        {
            return CreateActivity(outputPath, activity, "label", activityPath =>
            {
                _labelWriter.WriteLabel(activityPath, textAndMediaArea);
                _moduleWriterLabel.WriteModule(activityPath, course, section, textAndMediaArea);
            });
        }

        /// <summary>
        /// writes the XML files specific to the Assignment activity.
        /// </summary>
        /// <returns>
        /// <para> the path to the directory where the files for that activity were written. </para>
        /// <para> null if the directory couldn't be created.</para>
        /// </returns>
        private string? CreateAssignmentActivity(Course course, string outputPath, Activity activity, Assignment assignment, Section section)
        {
            return CreateActivity(outputPath, activity, "assign", activityPath =>
            {
                _assignmentWriter.WriteAssignment(activityPath, assignment);
                _moduleWriterAssignment.WriteModule(activityPath, course, section, assignment);
            });
        }

        /// <summary>
        /// writes the XML files specific to the Quiz activity
        /// </summary>
        /// /// <returns>
        /// <para> the path to the directory where the files for that activity were written </para>
        /// <para> null if the directory couldn't be created</para>
        /// </returns>
        private string? CreateQuizActivity(Course course, string outputPath, Activity activity, Quiz quiz, Section section)
        {
            return CreateActivity(outputPath, activity, "quiz", activityPath =>
            {
                _quizWriter.WriteQuiz(activityPath, quiz);
                _moduleWriterQuiz.WriteModule(activityPath, course, section, quiz);
            });
        }


        /// <summary>
        /// Creates an activity directory using the specified prefix and processes it via the given action.
        /// </summary>
        /// <param name="outputPath">The base output directory.</param>
        /// <param name="activity">The activity instance.</param>
        /// <param name="prefix">prefix that distinguishes the activity type (e.g., "resource", "url", "label").</param>
        /// <param name="processActivity">A lambda that performs the writing logic for the activity.</param>
        /// <returns>The path to the created directory, or null if directory creation fails.</returns>
        private string? CreateActivity(string outputPath, Activity activity, string prefix, Action<string> processActivity)
        {
            string activityPath = Path.Combine(outputPath, "activities", $"{prefix}_{activity.ActivityId}");

            if (!SafeCreateDirectory(activityPath))
            {
                return null;
            }

            processActivity(activityPath);
            return activityPath;
        }

        private bool SafeCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Could not create directory {path}: {e.Message}");
                return false;
            }
        }


        public static void ConfigureXmlServices(IServiceCollection services)
        {
            // General XML services and helpers
            services.AddSingleton<IXmlWriterFactory, DefaultXmlWriterFactory>();
            services.AddSingleton<IGeneralHelper>(GeneralHelper.Instance);
            services.AddSingleton<IMoodleVersionHelper, MoodleVersionHelper>();
            services.AddSingleton<ICourseHelper, CourseHelper>();
            services.AddSingleton<ISequentialIdGenerator, SequentialIdGenerator>();
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IRolesHelper, RolesHelper>();
            services.AddSingleton<IUserHelper, UserHelper>();

            // Plugins for the QuestionWriter
            services.AddSingleton<IQuestionPluginWriter, DragDropTextPluginWriter>();
            services.AddSingleton<IQuestionPluginWriter, DropdownPluginWriter>();
            services.AddSingleton<IQuestionPluginWriter, MultiChoicePluginWriter>();

            // Register QuestionsWriter (depends on the plugins registered above)
            services.AddSingleton<QuestionsWriter>();

            // Register all other writers
            services.AddSingleton<CalendarWriter>();
            services.AddSingleton<CommentsWriter>();
            services.AddSingleton<CompetenciesWriter>();
            services.AddSingleton<CompletiondefaultsWriter>();
            services.AddSingleton<ContentbankWriter>();
            services.AddSingleton<CourseWriter>();
            services.AddSingleton<FilesWriter>();
            services.AddSingleton<GradehistoryWriter>();
            services.AddSingleton<GradebookWriter>();
            services.AddSingleton<GroupsWriter>();
            services.AddSingleton<MoodleBackupWriter>();
            services.AddSingleton<OutcomesWriter>();
            services.AddSingleton<RolesWriter>();
            services.AddSingleton<ScalesWriter>();
            services.AddSingleton<UsersWriter>();
            services.AddSingleton<EnrolmentsWriter>();
            services.AddSingleton<FiltersWriter>();
            services.AddSingleton<InforefWriter>();
            services.AddSingleton<RoleAssignmentsWriter>();
            services.AddSingleton<BadgesWriter>();
            services.AddSingleton<CompletionWriter>();
            services.AddSingleton<SectionWriter>();
            services.AddSingleton<InforefSectionsWriter>();
            services.AddSingleton<ActivityGradesWriter>();
            services.AddSingleton<ActivityGradehistoryWriter>();
            services.AddSingleton<ActivityInforefWriter>();
            services.AddSingleton<ActivityRolesWriter>();
            services.AddSingleton<ActivityXapistateWriter>();
            services.AddSingleton<ActivityCalendarWriter>();
            services.AddSingleton<ActivityCommentsWriter>();
            services.AddSingleton<ActivityCompetenciesWriter>();
            services.AddSingleton<ActivityCompletionWriter>();
            services.AddSingleton<ActivityFiltersWriter>();
            services.AddSingleton<LabelWriter>();
            services.AddSingleton<ModuleWriterLabel>();
            services.AddSingleton<AssignmentWriter>();
            services.AddSingleton<ModuleWriterAssignment>();
            services.AddSingleton<UrlWriter>();
            services.AddSingleton<ModuleWriterUrl>();
            services.AddSingleton<FileWriter>();
            services.AddSingleton<ModuleWriterFile>();
            services.AddSingleton<ModuleWriterQuiz>();
            services.AddSingleton<QuizWriter>();
            services.AddSingleton<ITagsWriter, TagsWriter>();

            services.AddSingleton<IWriter, Writer>();
        }
    }
}
