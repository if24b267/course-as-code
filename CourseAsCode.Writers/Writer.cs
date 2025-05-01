using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Logic;
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
using NLog;

namespace CourseAsCode.Writers
{
    public class Writer : IWriter
    {
        public Writer()
        {
            // TODO: Use a Dependency Injection framework to inject these dependencies | for all other writers as well
            var xmlWriterFactory = new DefaultXmlWriterFactory();

            var pluginWriters = new List<IQuestionPluginWriter>();

            // Register all plugin writers
            pluginWriters.Add(new DragDropTextPluginWriter(xmlWriterFactory, LogManager.GetCurrentClassLogger()));
            pluginWriters.Add(new DropdownPluginWriter(xmlWriterFactory, LogManager.GetCurrentClassLogger()));
            pluginWriters.Add(new MultiChoicePluginWriter(xmlWriterFactory, LogManager.GetCurrentClassLogger()));

            // Initialize the QuestionsWriter with the plugin writers
            _questionsWriter = new QuestionsWriter(xmlWriterFactory, LogManager.GetCurrentClassLogger(), GeneralHelper.Instance, new SequentialIdGenerator(), pluginWriters);
        }

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
        private readonly QuestionsWriter _questionsWriter;
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

            // -------------------------------------------------
            // QUIZ TESTING - REMOVE LATER
            // -------------------------------------------------

            // Add a dummy dropdown question to the course
            var q1 = new DropdownQuestion()
            {
                QuestionId = 12,
                Category = "Geography",
                QuestionName = "Democracies around the world",
                QuestionText = "<p>Please select the correct form of government for the given countries</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0.3333333f,
                Answers = new List<DropdownSubQuestion>
                {
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>Russia</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Electoral Autocracy",
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>Canada</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Parliamentary Democracy",
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>USA</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Illiberal Democracy",
                        WrongAnswers = new List<string> { "Federal Democracy", "Constitutional Democracy" },
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>China</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "One-Party Dictatorship",
                    },
                },
            };

            var q2 = new DropdownQuestion()
            {
                QuestionId = 13,
                Category = "Programming Languages",
                QuestionName = "Programming Paradigms",
                QuestionText = "<p>Select the primary programming paradigm associated with each language.</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0.25f,
                Answers = new List<DropdownSubQuestion>
                {
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>Haskell</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Functional",
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>Java</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Object-Oriented",
                        WrongAnswers = new List<string> { "Functional" },
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>C</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Procedural",
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "<p>Prolog</p>",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "Logic Programming",
                        WrongAnswers = new List<string> { "Markdown Language" },
                    },
                },
            };

            var q3 = new DragAndDropTextQuestion()
            {
                QuestionId = 14,
                Category = "Computer Science Basics",
                QuestionName = "Computer Science Basics Q1",
                QuestionText = "<p>The [[1]] is a type of system software that manages [[5]] and software resources, and provides common services for computer programs</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0.25f,
                Answers = new List<TextChoice>
                {
                    new TextChoice()
                    {
                        ID = 1,
                        Value = "Operating System",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "A",
                    },
                    new TextChoice()
                    {
                        ID = 2,
                        Value = "Compiler",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "A",
                    },
                    new TextChoice()
                    {
                        ID = 3,
                        Value = "Microsoft Word",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "A",
                    },
                    new TextChoice()
                    {
                        ID = 4,
                        Value = "Printer",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "A",
                    },
                    new TextChoice()
                    {
                        ID = 5,
                        Value = "Hardware",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "B",
                    },
                    new TextChoice()
                    {
                        ID = 6,
                        Value = "Internet",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "B",
                    },
                    new TextChoice()
                    {
                        ID = 7,
                        Value = "Printing",
                        ValueFormat = FormattingFormat.HTML,
                        Category = "B",
                    },
                },
            };


            var q4 = new DragAndDropTextQuestion()
            {
                QuestionId = 15,
                Category = "Linux Basics",
                QuestionName = "Linux File Permissions Q1",
                QuestionText = "In Linux, file permissions are controlled by three sets of access rights: [[3]], [[4]], and [[5]]. To view the permissions of a file, you can use the [[1]] command. Permissions are typically displayed as a string like `rwxr-xr--`, where each letter represents a different [[6]].",
                QuestionTextFormat = FormattingFormat.Markdown,
                DefaultMark = 1,
                HintPenalty = 0.25f,
                Answers = new List<TextChoice>
                {
                    new TextChoice()
                    {
                        ID = 1,
                        Value = "ls -l",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 2,
                        Value = "chmod",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 3,
                        Value = "owner",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 4,
                        Value = "group",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 5,
                        Value = "other",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 6,
                        Value = "permission",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 7,
                        Value = "cd",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 8,
                        Value = "shutdown",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 9,
                        Value = "cat",
                        ValueFormat = FormattingFormat.HTML,
                    },
                    new TextChoice()
                    {
                        ID = 10,
                        Value = "nano",
                        ValueFormat = FormattingFormat.HTML,
                    },
                },
            };


            // Create new quiz activity and add it to the course
            var testQuiz = new Quiz
            {
                ActivityId              = 6,
                Name                    = "New Question Types quiz",
                Description             = "<p>This quiz is hardcoded and was generated by Course‑as‑Code</p>",
                DescriptionFormat       = FormattingFormat.HTML,
                OpenTime                = null,
                CloseTime               = null,
                TimeLimit               = 3600,
                TimeExpiryHandling      = TimeExpiryHandle.AutoSubmit,
                QuestionBehaviour       = QuestionBehaviour.Deferred,
                AllowRedosDuringAttempt = true,
                AllowedAttempts         = 2,
                BuildOnPreviousAttempts = true,
                GradingMethod           = GradingMethod.LastAttempt,
                DecimalsToDisplayGrades = 2,
                DecimalsToDisplayMarks  = -1,
                ShuffleWithinQuestions  = true,
                NewPageThreshold        = 0,
                ShowPicture             = PictureOptions.SmallImage,
                Questions               = new() { q1, q2, q3, q4 },
                Feedback                = new List<QuizFeedback>()
                {
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>You did it!</p>",
                        GradeBoundary = 100f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                    new QuizFeedback()
                    {
                        FeedbackText  = "<p>Probably should go back to kindergarten</p>",
                        GradeBoundary = 50f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>Oh, wow that's just sad</p>",
                        GradeBoundary = 0f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                },
            };

            var testSection = new Section
            {
                SystemDefaults = new Section.SystemDefaultsSection
                {
                    SectionId = 5,
                    PathToSection = "sections/section_5",
                },
                Name = "Quiz Test Section",
                Activities = new List<Activity> { testQuiz },
            };

            course.Sections.Add(testSection);
            course.Questions.Add(q1);
            course.Questions.Add(q2);
            course.Questions.Add(q3);
            course.Questions.Add(q4);

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
}
