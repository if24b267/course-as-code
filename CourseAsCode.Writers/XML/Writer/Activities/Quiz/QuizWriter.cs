using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.Quiz;

public class QuizWriter : XmlWriterBase
{
    private const string DEFAULT_QUIZ_RELATIVE_PATH = "quiz.xml";
    private readonly IGeneralHelper _generalHelper;
    
    private const string DEFAULT_CONTEXT_ID = "383"; // Every value >> 100 seems to be working fine | Needs to be the same as in `QuestionsWriter.cs`

    /// <summary>
    /// Default constructor using default implementations for dependencies
    /// </summary>
    public QuizWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
    {
        _generalHelper = GeneralHelper.Instance;
    }

    /// <summary>
    /// Constructor using Dependency Injection for Unit Testing
    /// </summary>
    /// <param name="xmlWriterFactory">Injected XML writer factory</param>
    /// <param name="logger">Injected logger instance</param>
    /// <param name="generalHelper">Injected general helper</param>
    public QuizWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger, IGeneralHelper generalHelper)
        : base(xmlWriterFactory, logger)
    {
        _generalHelper = generalHelper;
    }

    /// <summary>
    /// Writes the quiz activity to an XML file
    /// </summary>
    /// <param name="outputPath">the output path where the quiz.xml file will be saved</param>
    /// <param name="quizObject">the Quiz activity to write</param>
    public void WriteQuiz(string outputPath, Model.Activities.Resources.Quiz quizObject)
    {
        try
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_QUIZ_RELATIVE_PATH));

            using (XmlWriter writer = CreateXmlWriter(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("activity");

                WriteActivityAttributes(writer, quizObject);
                WriteQuizTag(writer, quizObject);

                writer.WriteEndElement(); // </activity>
                writer.WriteEndDocument();
            }

            LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_QUIZ_RELATIVE_PATH}");
        }
        catch (Exception e)
        {
            LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_QUIZ_RELATIVE_PATH}");
            throw;
        }
    }


    /// <summary>
    /// writes the activity tag and its attributes
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="quizObject">initialized quiz activity to write</param>
    private void WriteActivityAttributes(XmlWriter writer, Model.Activities.Resources.Quiz quizObject)
    {
        writer.WriteAttributeString("id", quizObject.ActivityId.ToString());
        writer.WriteAttributeString("moduleid", quizObject.SystemDefaults.ModuleId.ToString());
        writer.WriteAttributeString("modulename", quizObject.SystemDefaults.ModuleName);
        writer.WriteAttributeString("contextid", DEFAULT_CONTEXT_ID);
    }

    
    /// <summary>
    /// writes the quiz element and its child elements
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="quiz">The Quiz object to write</param>
    private void WriteQuizTag(XmlWriter writer, Model.Activities.Resources.Quiz quiz)
    {
        writer.WriteStartElement("quiz");
        writer.WriteAttributeString("id", quiz.ActivityId.ToString());

        // ──────────────────────────────────────────────────────────────────────
        // Basic elements – direct mapping from the data model
        // ──────────────────────────────────────────────────────────────────────
        var elements = new Dictionary<string, string>
        {
            { "name", quiz.Name },
            { "intro", quiz.Description ?? string.Empty },
            { "introformat", _generalHelper.EnumIntegerToString(quiz.DescriptionFormat) },
            { "timeopen", quiz.OpenTime != null ? _generalHelper.DateTimeToUnixTimestamp((DateTime)quiz.OpenTime) : "0" },
            { "timeclose", quiz.CloseTime != null ? _generalHelper.DateTimeToUnixTimestamp((DateTime)quiz.CloseTime) : "0" },
            { "timelimit", quiz.TimeLimit.ToString() },
            { "overduehandling", EnumMappers.MapTimeExpiryHandle(quiz.TimeExpiryHandling) },
            { "graceperiod", quiz.GracePeriod.ToString() },
            { "preferredbehaviour", EnumMappers.MapQuestionBehaviour(quiz.QuestionBehaviour) },
            { "canredoquestions", quiz.AllowRedosDuringAttempt ? "1" : "0" },
            { "attempts_number", quiz.AllowedAttempts.ToString() },
            { "attemptonlast", quiz.BuildOnPreviousAttempts ? "1" : "0" },
            { "grademethod", _generalHelper.EnumIntegerToString(quiz.GradingMethod) },
            { "decimalpoints", quiz.DecimalsToDisplayGrades.ToString() },
            { "questiondecimalpoints", quiz.DecimalsToDisplayMarks.ToString() },
            // Review options – currently the same mask for all reviewX elements | TODO: Find out how the bit-mask works
            { "reviewattempt", quiz.ReviewOptions.ToString() },
            { "reviewcorrectness", quiz.ReviewOptions.ToString() },
            { "reviewmaxmarks", quiz.ReviewOptions.ToString() },
            { "reviewmarks", quiz.ReviewOptions.ToString() },
            { "reviewspecificfeedback", quiz.ReviewOptions.ToString() },
            { "reviewgeneralfeedback", quiz.ReviewOptions.ToString() },
            { "reviewrightanswer", quiz.ReviewOptions.ToString() },
            { "reviewoverallfeedback", quiz.ReviewOptions.ToString() },
            // End review options
            { "questionsperpage", quiz.NewPageThreshold.ToString() },
            { "navmethod", "free" }, // TODO: make configurable
            { "shuffleanswers", quiz.ShuffleWithinQuestions ? "1" : "0" },
            { "sumgrades", (quiz.Questions?.Sum(q => q.DefaultMark) ?? 0.0f).ToString("0.00000", CultureInfo.InvariantCulture) }, // TODO: Check if this is correct
            { "grade", (10.0f).ToString(format:"0.00000", CultureInfo.InvariantCulture) }, // TODO: Find out what this value is for (Maximum grade achievable?)
            { "timecreated", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeCreated) },
            { "timemodified", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeModified) },
            { "password", quiz.Password ?? string.Empty },
            { "subnet", string.Empty },
            { "browsersecurity", "-" }, // TODO: Find out what this is for (seemingly has nothing to do with SEB)
            { "delay1", "0" },
            { "delay2", "0" },
            { "showuserpicture", _generalHelper.EnumIntegerToString(quiz.ShowPicture) },
            { "showblocks", "0" },
            { "completionattemptsexhausted", "0" },
            { "completionminattempts", "0" },
            { "allowofflineattempts", "0" }
        };

        WriteElements(writer, elements);

        // ──────────────────────────────────────────────────────────────────────
        // Complex sub‑structures
        // ──────────────────────────────────────────────────────────────────────
        WriteSEBSubPlugin(writer, quiz);
        WriteGradeItems(writer);
        WriteQuestionInstances(writer, quiz);
        WriteSections(writer, quiz);
        WriteFeedbacks(writer, quiz);
        // Empty containers, currently not applicable
        WriteEmptyContainer(writer, "overrides");
        WriteEmptyContainer(writer, "grades");
        WriteEmptyContainer(writer, "attempts");

        writer.WriteEndElement(); // </quiz>
    }
    
    
    private void WriteSEBSubPlugin(XmlWriter writer, Model.Activities.Resources.Quiz quiz)
    {
        writer.WriteStartElement("subplugin_quizaccess_seb_quiz"); // Empty tag if SEB is not required
        
        if (quiz.SafeExamBrowser.RequireSEB == SEBMode.EnabledManualConfig)
        {
            writer.WriteStartElement("quizaccess_seb_quizsettings");

            var sebElements = new Dictionary<string, string>
            {
                { "quizid", quiz.ActivityId.ToString() },
                { "cmid", "227" }, // TODO: Determine what this value is
                { "templateid", "0" },
                { "requiresafeexambrowser", _generalHelper.EnumIntegerToString(quiz.SafeExamBrowser.RequireSEB) },
                { "showsebtaskbar", quiz.SafeExamBrowser.ShowTaskBar ? "1" : "0" },
                { "showwificontrol", quiz.SafeExamBrowser.ShowWifiControl ? "1" : "0" },
                { "showreloadbutton", quiz.SafeExamBrowser.ShowReloadButton ? "1" : "0" },
                { "showtime", quiz.SafeExamBrowser.ShowTime ? "1" : "0" },
                { "showkeyboardlayout", quiz.SafeExamBrowser.ShowKeyboardLayout ? "1" : "0" },
                { "allowuserquitseb", quiz.SafeExamBrowser.EnableQuit ? "1" : "0" },
                { "quitpassword", quiz.SafeExamBrowser.QuitPassword ?? string.Empty },
                { "linkquitseb", quiz.SafeExamBrowser.QuitLinkButton ?? string.Empty },
                { "userconfirmquit", quiz.SafeExamBrowser.ConfirmQuitLink ? "1" : "0" },
                { "enableaudiocontrol", quiz.SafeExamBrowser.EnableAudioControls ? "1" : "0" },
                { "muteonstartup", "0" }, // TODO: Determine what this does
                { "allowspellchecking", quiz.SafeExamBrowser.EnableSpellCheck ? "1" : "0" },
                { "allowreloadinexam", quiz.SafeExamBrowser.EnableReload ? "1" : "0" },
                { "activateurlfiltering", quiz.SafeExamBrowser.EnableURLFiltering ? "1" : "0" },
                { "filterembeddedcontent", quiz.SafeExamBrowser.FilterEmbeddedContent ? "1" : "0" },
                { "expressionsallowed", JoinOrEmpty(quiz.SafeExamBrowser.AllowedURLs) },
                { "regexallowed", JoinOrEmpty(quiz.SafeExamBrowser.AllowedURLsRegex) },
                { "expressionsblocked", JoinOrEmpty(quiz.SafeExamBrowser.BlockedURLs) },
                { "regexblocked", JoinOrEmpty(quiz.SafeExamBrowser.BlockedURLsRegex) },
                { "showsebdownloadlink", quiz.SafeExamBrowser.ShowDownloadButton ? "1" : "0" },
                { "allowedbrowserexamkeys", quiz.SafeExamBrowser.AllowedExamKeys != null ? JoinOrEmpty(quiz.SafeExamBrowser.AllowedExamKeys) : "$@NULL@$" },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeModified) }
            };
            WriteElements(writer, sebElements);

            writer.WriteEndElement(); // </quizaccess_seb_quizsettings>
        }
        
        writer.WriteEndElement(); // </subplugin_quizaccess_seb_quiz>
    }
    
    
    private void WriteGradeItems(XmlWriter writer)
    {
        writer.WriteStartElement("quiz_grade_items");
        
        // TODO: Write grade items
        
        writer.WriteEndElement();
    }

    
    private void WriteQuestionInstances(XmlWriter writer, Model.Activities.Resources.Quiz quiz)
    {
        writer.WriteStartElement("question_instances");

        if (quiz.Questions?.Count > 0)
        {
            int slot = 1; // Ascending slot number (1-based)
            foreach (var question in quiz.Questions)
            {
                writer.WriteStartElement("question_instance");
                writer.WriteAttributeString("id", question.QuestionId.ToString());

                var qiElements = new Dictionary<string, string>
                {
                    { "quizid", quiz.ActivityId.ToString() },
                    { "slot", slot.ToString() },
                    { "page", CalculatePageNumber(slot, quiz.NewPageThreshold).ToString() }, // TODO: Check if this is correct
                    { "displaynumber", "$@NULL@$" },
                    { "requireprevious", "0" },
                    { "maxmark", question.DefaultMark.ToString(format: "0.0000000", CultureInfo.InvariantCulture) },
                    { "quizgradeitemid", "$@NULL@$" }
                };
                WriteElements(writer, qiElements);

                // question_reference sub‑element
                writer.WriteStartElement("question_reference");
                writer.WriteAttributeString("id", question.QuestionId.ToString());
                WriteElements(writer, new Dictionary<string, string>
                {
                    { "usingcontextid", DEFAULT_CONTEXT_ID },
                    { "component", "mod_quiz" },
                    { "questionarea", "slot" },
                    { "questionbankentryid", question.BankEntryId.ToString() },
                    { "version", "$@NULL@$" }
                });
                writer.WriteEndElement(); // </question_reference>

                writer.WriteEndElement(); // </question_instance>
                slot++;
            }
        }

        writer.WriteEndElement(); // </question_instances>
    }

    
    private void WriteSections(XmlWriter writer, Model.Activities.Resources.Quiz quiz)
    {
        writer.WriteStartElement("sections");
        writer.WriteStartElement("section");
        writer.WriteAttributeString("id", quiz.ActivityId.ToString());

        WriteElements(writer, new Dictionary<string, string>
        {
            { "firstslot", "1" },
            { "heading", string.Empty },
            { "shufflequestions", "0" }
        });

        writer.WriteEndElement(); // </section>
        writer.WriteEndElement(); // </sections>
    }

    
    private void WriteFeedbacks(XmlWriter writer, Model.Activities.Resources.Quiz quiz)
    {
        writer.WriteStartElement("feedbacks");

        if (quiz.Feedback is { Count: > 0 })
        {
            int id = 1;
            
            // Sort by descending grade boundary (probably not necessary, but easier to read and the way moodle structures it)
            foreach (var feedback in quiz.Feedback.OrderByDescending(f => f.GradeBoundary))
            {
                writer.WriteStartElement("feedback");
                writer.WriteAttributeString("id", id.ToString());

                var feedbackElements = new Dictionary<string, string>
                {
                    { "feedbacktext", feedback.FeedbackText },
                    { "feedbacktextformat", _generalHelper.EnumIntegerToString(feedback.OverallFeedbackFormat) },
                    { "mingrade", feedback.GradeBoundary.ToString(format: "0.00000", CultureInfo.InvariantCulture) },
                    { "maxgrade", GetNextHigherBoundary(feedback.GradeBoundary, quiz.Feedback).ToString(format: "0.00000", CultureInfo.InvariantCulture) }
                };
                WriteElements(writer, feedbackElements);

                writer.WriteEndElement(); // </feedback>
                id++;
            }
        }

        writer.WriteEndElement(); // </feedbacks>
    }

    
    private void WriteEmptyContainer(XmlWriter writer, string elementName)
    {
        writer.WriteStartElement(elementName);
        writer.WriteEndElement();
    }
    
    
    /// <summary>
    /// Calculates the 1‑based page number that an item falls on, given its sequential number and the number of items per page
    /// </summary>
    /// <param name="itemNumber">The 1‑based sequence number of the item (must be ≥1).</param>
    /// <param name="itemsPerPage">How many items fit on one page</param>
    /// <returns>The page number (≥1) on which the specified item appears</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="itemNumber"/> is less than 1</exception>
    private int CalculatePageNumber(int itemNumber, int itemsPerPage)
    {
        // If itemsPerPage is 0 (or negative - theoretically invalid), all items are on page 1
        if (itemsPerPage <= 0)
        {
            return 1;
        }

        // Item numbers below 1 don’t make sense
        if (itemNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(itemNumber), "Item number must be a positive integer");
        }

        // Calculate the page number, rounding up to the next whole number
        double rawPage = (double)itemNumber / itemsPerPage;
        int pageNumber = (int)Math.Ceiling(rawPage);

        return pageNumber;
    }
    
    
    /// <summary>
    /// Finds the next higher grade boundary above the given current value
    /// </summary>
    /// <param name="current">The current grade boundary to compare against</param>
    /// <param name="all">A sequence of <see cref="QuizFeedback"/> objects containing grade boundaries</param>
    /// <returns>
    /// <para>The smallest grade boundary strictly greater than <paramref name="current"/></para>
    /// <para>or <c>DefaultBoundary</c> if none are found</para>
    /// </returns>
    private float GetNextHigherBoundary(float current, List<QuizFeedback> all)
    {
        const float defaultBoundary = 11f;

        // Extract all grade boundaries from the feedback items
        var boundaries = all.Select(fb => fb.GradeBoundary);

        // Filter only those strictly greater than the current boundary
        var higherBoundaries = boundaries
            .Where(boundary => boundary > current);

        // If there are no higher boundaries, return the default (i.e., the max possible score)
        if (!higherBoundaries.Any())
        {
            return defaultBoundary;
        }

        // Otherwise, return the smallest of the higher boundaries
        float nextHigher = higherBoundaries.Min();
        return nextHigher;
    }


    /// <summary>
    /// Joins the elements of the sequence into a single string with the specified separator, or returns <see cref="String.Empty"/> if the sequence is null or contains no elements
    /// </summary>
    /// <param name="items">The sequence of strings to join. May be null</param>
    /// <param name="separator">The string to use as a separator. If null, defaults to <see cref="Environment.NewLine"/></param>
    /// <returns>A single concatenated string, or <see cref="String.Empty"/> if <paramref name="items"/> is null or empty.</returns>
    private string JoinOrEmpty(List<string>? items, string? separator = null)
    {
        if (items == null || items.Count == 0)
        {
            return string.Empty;
        }

        var sep = separator ?? Environment.NewLine;
        return string.Join(sep, items);
    }
}