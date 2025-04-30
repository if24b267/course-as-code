using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.XML.Writer.Sections;
using NLog;

namespace CourseAsCode.Writers;

public class Writer : IWriter
{
    // -- General Writers -- //
    private readonly CalendarWriter _calendarWriter = new CalendarWriter();
    private readonly CommentsWriter _commentsWriter = new CommentsWriter();
    private readonly CompetenciesWriter _competenciesWriter = new CompetenciesWriter();
    private readonly CompletiondefaultsWriter _completiondefaultsWriter = new CompletiondefaultsWriter();
    private readonly ContentbankWriter _contentbankWriter = new ContentbankWriter();
    private readonly CourseWriter _courseWriter = new CourseWriter();
    private readonly FilesWriter _filesWriter = new FilesWriter();
    private readonly GradehistoryWriter _gradehistoryWriter = new GradehistoryWriter();
    private readonly GradebookWriter _gradebookWriter = new GradebookWriter();
    private readonly GroupsWriter _groupsWriter = new GroupsWriter();
    private readonly MoodleBackupWriter _moodleBackupWriter = new MoodleBackupWriter();
    private readonly OutcomesWriter _outcomesWriter = new OutcomesWriter();
    private readonly QuestionsWriter _questionsWriter = new QuestionsWriter();
    private readonly RolesWriter _rolesWriter = new RolesWriter();
    private readonly ScalesWriter _scalesWriter = new ScalesWriter();
    private readonly UsersWriter _usersWriter = new UsersWriter();
    private readonly EnrolmentsWriter _enrolmentsWriter = new EnrolmentsWriter();
    private readonly FiltersWriter _filtersWriter = new FiltersWriter();
    private readonly InforefWriter _inforefWriter = new InforefWriter();
    private readonly RoleAssignmentsWriter _roleAssignmentsWriter = new RoleAssignmentsWriter();
    private readonly BadgesWriter _badgesWriter = new BadgesWriter();
    private readonly CompletionWriter _completionWriter = new CompletionWriter();
    private readonly SectionWriter _sectionWriter = new SectionWriter();

    // -- Activity Writers -- //
    // Common Activity Writers
    private readonly ActivityGradesWriter _activityGradesWriter = new ActivityGradesWriter();
    private readonly ActivityGradehistoryWriter _activityGradehistoryWriter = new ActivityGradehistoryWriter();
    private readonly ActivityInforefWriter _activityInforefWriter = new ActivityInforefWriter();
    private readonly ActivityRolesWriter _activityRolesWriter = new ActivityRolesWriter();
    private readonly ActivityXapistateWriter _activityXapistateWriter = new ActivityXapistateWriter();
    private readonly ActivityCalendarWriter _activityCalendarWriter = new ActivityCalendarWriter();
    private readonly ActivityCommentsWriter _activityCommentsWriter = new ActivityCommentsWriter();
    private readonly ActivityCompetenciesWriter _activityCompetenciesWriter = new ActivityCompetenciesWriter();
    private readonly ActivityCompletionWriter _activityCompletionWriter = new ActivityCompletionWriter();
    private readonly ActivityFiltersWriter _activityFiltersWriter = new ActivityFiltersWriter();

    // Text and Media Area (Label) Activity Writers
    private readonly LabelWriter _labelWriter = new LabelWriter();
    private readonly ModuleWriterLabel _moduleWriterLabel = new ModuleWriterLabel();

    // Assignment Activity Writers
    private readonly AssignmentWriter _assignmentWriter = new AssignmentWriter();
    private readonly ModuleWriterAssignment _moduleWriterAssignment = new ModuleWriterAssignment();

    // URL Activity Writers
    private readonly UrlWriter _urlWriter = new UrlWriter();
    private readonly ModuleWriterUrl _moduleWriterUrl = new ModuleWriterUrl();

    // File Activity Writers
    private readonly FileWriter _fileWriter = new FileWriter();
    private readonly ModuleWriterFile _moduleWriterFile = new ModuleWriterFile();

    // Quiz Activity Writers
    private readonly ModuleWriterQuiz _moduleWriterQuiz = new ModuleWriterQuiz();
    private readonly QuizWriter _quizWriter = new QuizWriter();

    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public bool WriteCourse(Course course, string outputPath)
    {
        _logger.Debug($"Writer started for course '{course.FullName}' with ID {course.SystemDefaults.Id}");
        _logger.Info($"The course will be saved at {outputPath}");

        // ---------------------------------------------
        // TEMPORARY CODE FOR TESTING - REMOVE LATER
        // ---------------------------------------------

        // Add a dummy assignment activity to the course
        var testAssignment = new Assignment
        {
            ActivityId = 10,
            Name = "Sample Assignment",
            Intro = "This is a sample assignment for testing purposes.",
            AlwaysShowDescription = true,
            SubmissionDrafts = true,
            SendNotifications = true,
            SendLateNotifications = false,
            SendStudentNotifications = true,
            DueDate = DateTime.UtcNow.AddDays(7), // 7 days in the future
            CutoffDate = DateTime.UtcNow.AddDays(10), // 10 days in the future
            GradingDueDate = DateTime.UtcNow.AddDays(14), // 14 days in the future
            AllowSubmissionsFromDate = DateTime.UtcNow, // Submissions allowed from now
            Grade = 100,
            CompletionSubmit = true,
            RequireSubmissionStatement = false,
            TeamSubmission = false,
            RequireAllTeamMembersSubmit = false,
            TeamSubmissionGroupingId = 0,
            BlindMarking = false,
            HideGrader = false,
            RevealIdentities = true,
            AttemptReopenMethod = "manual",
            MaxAttempts = 3,
            MarkingWorkflow = true,
            MarkingAllocation = false,
            MarkingAnonymous = false,
            PreventSubmissionNotInGroup = false,
            Submissions = new List<AssignmentSubmission>
    {
        new AssignmentSubmission
        {
            SubmissionId = 1,
            UserId = 101,
            TimeCreated = DateTime.UtcNow.AddDays(-1), // Created yesterday
            TimeModified = DateTime.UtcNow, // Modified today
            TimeStarted = DateTime.UtcNow.AddDays(-2).ToString("o"), // Started 2 days ago
            Status = "submitted",
            GroupId = 0,
            AttemptNumber = 1,
            IsLatest = true
        },
        new AssignmentSubmission
        {
            SubmissionId = 2,
            UserId = 102,
            TimeCreated = DateTime.UtcNow.AddDays(-3), // Created 3 days ago
            TimeModified = null, // Not modified yet
            TimeStarted = null, // Not started yet
            Status = "draft",
            GroupId = 0,
            AttemptNumber = 0,
            IsLatest = false
        }
    },
            PluginConfigs = new List<PluginConfig>
    {
        new PluginConfig
        {
            ConfigId = 1,
            PluginName = "assignsubmission_file",
            Subtype = "file",
            Name = "maxfilesubmissions",
            Value = "5"
        },
        new PluginConfig
        {
            ConfigId = 2,
            PluginName = "assignsubmission_onlinetext",
            Subtype = "onlinetext",
            Name = "wordlimit",
            Value = "1000"
        }
    },
            Overrides = new List<AssignmentOverride>
    {
        new AssignmentOverride
        {
            OverrideId = 1,
            UserId = 101,
            DueDate = DateTime.UtcNow.AddDays(8), // User has 1 extra day
            CutoffDate = DateTime.UtcNow.AddDays(11) // User has 1 extra day
        }
    }
        };

        // Add the assignment to a section
        var assignmentSection = new Section
        {
            SystemDefaults = new Section.SystemDefaultsSection
            {
                SectionId = 4,
                PathToSection = "sections/section_4"
            },
            Name = "Assignment Test Section",
            Activities = new List<Activity> { testAssignment }
        };

        course.Sections.Add(assignmentSection);

        /*
        // Add a few dummy questions to the course
        var q1 = new MultipleChoiceQuestion
        {
            Category            = "Default category",
            QuestionName        = "Shamir’s Theorem",
            QuestionText        = "<p>Which equality does Shamir’s Theorem prove?</p>",
            QuestionTextFormat  = FormattingFormat.HTML,
            DefaultMark         = 1,
            HintPenalty         = 0.3333333f,
            ChoiceNumbering     = NumberingFormat.AlphabeticLower,
            ShuffleAnswers      = true,
            SingleChoice        = true,
            Answers = new List<Answer>
            {
                new()
                {
                    AnswerText  = "<p>IP = NP</p>",
                    AnswerTextFormat = FormattingFormat.HTML,
                    AnswerGrade = 0,
                    AnswerFeedback =
                        "<p>Just seeing an NP somewhere doesn’t make it right.</p>",
                    AnswerFeedbackFormat = FormattingFormat.HTML
                },
                new()
                {
                    AnswerText  = "<p>IP = PSPACE</p>",
                    AnswerTextFormat = FormattingFormat.HTML,
                    AnswerGrade = 1
                }
            },
            Hints = new List<Hint>
            {
                new()
                {
                    HintText =
                        "<p>Recall that IP refers to interactive proofs …</p>",
                    HintTextFormat = FormattingFormat.HTML
                }
            }
        };


        course.Sections.Add(testSection);
        */
        // ---------------------------------------------
        // TEMPORARY CODE FOR TESTING - END
        // ---------------------------------------------

        // Ensure sections list is not empty
        if (course.Sections.Count == 0)
        {
            _logger.Info("No sections specified by user, adding default section");
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
        InforefSectionsWriter inforefSectionsWriter = new InforefSectionsWriter();
        inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);

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
                _logger.Debug($"Writing activity {activity.ActivityId}: {activity.Name}");

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
                    _logger.Error($"Skipping activity {activity.ActivityId}: {activity.Name} due to directory creation failure");
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
            _logger.Fatal($"Could not create directory {path}: {e.Message}");
            return false;
        }
    }
}
