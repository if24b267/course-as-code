using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions
{
    /// <summary>
    /// Handles the creation and writing of the questions.xml file for a course.
    /// </summary>
    public class QuestionsWriter
    {
        private const string DEFAULT_QUESTIONS_RELATIVE_PATH = "questions.xml";
        private const string DEFAULT_SERVER_ADDRESS = "172.21.0.3";
        private const int DEFAULT_SERVER_PORT = 8080;

        private const int DEFAULT_TOP_CATEGORY_ID = 1;
        private const int DEFAULT_CONTEXT_ID = 383; // Every value >> 100 seems to be working fine | Needs to be the same as in `QuizWriter.cs`
        private const int DEFAULT_CONTEXT_LEVEL = 50; // 50 = course, 70 = module
        private const int DEFAULT_CONTEXT_INSTANCE_ID = 135; // Honestly, no idea what this is for

        private readonly IXmlServiceFactory _xmlServiceFactory;
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;
        private readonly ITagsWriter _tagsWriter;
        private readonly ILogger<QuestionsWriter> _logger;
        private readonly IQuestionStrategyResolver _strategyResolver;


        /// <summary>
        /// Creates a new instance of the QuestionsWriter class
        /// </summary>
        /// <param name="xmlServiceFactory">Factory for creating XML services</param>
        /// <param name="logger">Logger instance specific to QuestionsWriter</param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Helper for generating sequential IDs for various entities</param>
        /// <param name="quizHelper">Helper providing quiz-related utility methods</param>
        /// <param name="tagsWriter">Helper method providing specific helper methods focused on tag management</param>
        /// <param name="strategyResolver">Resolver for obtaining the appropriate question strategy based on question type</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public QuestionsWriter(IXmlServiceFactory xmlServiceFactory, ILogger<QuestionsWriter> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper, ITagsWriter tagsWriter, IQuestionStrategyResolver strategyResolver)
        {
            _xmlServiceFactory = xmlServiceFactory ?? throw new ArgumentNullException(nameof(xmlServiceFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
            _strategyResolver = strategyResolver ?? throw new ArgumentNullException(nameof(strategyResolver));
        }


        /// <summary>
        /// writes the questions.xml file
        /// </summary>
        /// <param name="course">The course to write, containing all questions that should be included</param>
        /// <param name="outputPath">the output path where the questions.xml file will be saved</param>
        public void WriteQuestions(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_QUESTIONS_RELATIVE_PATH));

                using (var xmlHelper = _xmlServiceFactory.Create(filePath))
                {
                    xmlHelper.WriteElement("question_categories");

                    // Moodle requires a top-level category, which is the parent of all other categories
                    WriteTopCategory(xmlHelper, DEFAULT_TOP_CATEGORY_ID, DEFAULT_CONTEXT_ID, DEFAULT_CONTEXT_LEVEL, DEFAULT_CONTEXT_INSTANCE_ID);

                    // Questions are grouped by category | Each entry for a category contains the entries for all questions belonging to that category
                    foreach (var categoryGrouping in course.Questions.GroupBy(q => q.Category))
                    {
                        int categoryId = _sequentialIdGenerator.GetNextCategoryId();
                        WriteQuestionCategory(xmlHelper, categoryId, categoryGrouping.Key, categoryGrouping.ToList());

                        _logger.LogTrace($"Created question category {categoryId}: '{categoryGrouping.Key}' with {categoryGrouping.Count()} questions");
                    }

                    xmlHelper.CloseElement(); // </question_categories>
                }

                _logger.LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_QUESTIONS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_QUESTIONS_RELATIVE_PATH}");
                throw;
            }
        }


        /// <summary>
        /// Writes the `question_category` element of questions.xml
        /// </summary>
        /// <param name="xmlHelper">Service used for writing to the XML file</param>
        /// <param name="categoryId">the category ID</param>
        /// <param name="categoryName">the category name</param>
        /// <param name="questions">the list of questions belonging to that category</param>
        private void WriteQuestionCategory(IXmlService xmlHelper, int categoryId, string categoryName, List<Question> questions)
        {
            xmlHelper.WriteElementWithAttributes("question_category", new Dictionary<string, string>
            {
                { "id", categoryId.ToString() },
            });

            // Metadata for the category
            var categoryElements = new Dictionary<string, string>
            {
                { "name", categoryName },
                { "contextid", DEFAULT_CONTEXT_ID.ToString() },
                { "contextlevel", DEFAULT_CONTEXT_LEVEL.ToString() },
                { "contextinstanceid", DEFAULT_CONTEXT_INSTANCE_ID.ToString() },
                { "info", string.Empty }, // Description of the category, not implemented (yet?)
                { "infoformat", "0" },
                { "stamp", GenerateStamp() },
                { "parent", DEFAULT_TOP_CATEGORY_ID.ToString() },
                { "sortorder", "0" },
                { "idnumber", "$@NULL@$" },
            };
            xmlHelper.WriteElements(categoryElements);

            xmlHelper.WriteElement("question_bank_entries");
            foreach (var question in questions)
            {
                // Write all questions belonging to this category
                WriteQuestionBankEntry(xmlHelper, categoryId, question);

                // Store the ID of the category the question was saved to, so that other Writers (specifically InfoRefWriter) can reference it later
                question.CategoryId = categoryId;
            }

            xmlHelper.CloseElement(); // </question_bank_entries>
            xmlHelper.CloseElement(); // </question_category>
        }


        /// <summary>
        /// Writes the `question_bank_entry` element of questions.xml
        /// </summary>
        /// <param name="xmlHelper">Service used for writing to the XML file</param>
        /// <param name="categoryId">the category ID of the category to which the question / questionbank belongs</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionBankEntry(IXmlService xmlHelper, int categoryId, Question question)
        {
            int bankEntryId = _sequentialIdGenerator.GetNextBankEntryId();
            question.BankEntryId = bankEntryId; // Store the ID of the question bank entry the question was saved to, so that QuizWriter can reference it later

            xmlHelper.WriteElementWithAttributes("question_bank_entry", new Dictionary<string, string>
            {
                { "id", bankEntryId.ToString() },
            });

            xmlHelper.WriteElements(new Dictionary<string, string>
            {
                { "questioncategoryid", categoryId.ToString() },
                { "idnumber", "$@NULL@$" },
                { "ownerid", "0" },
            });

            // Moodle uses an internal version control system for questions - we ignore this and only save one version of a question
            xmlHelper.WriteElement("question_version");
            WriteQuestionVersionContainer(xmlHelper, question);

            xmlHelper.CloseElement(); // </question_version>
            xmlHelper.CloseElement(); // </question_bank_entry>
        }


        /// <summary>
        /// Writes the `question_version` element of questions.xml
        /// </summary>
        /// <param name="xmlHelper">Service used for writing to the XML file</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionVersionContainer(IXmlService xmlHelper, Question question)
        {
            xmlHelper.WriteElementWithAttributes("question_versions", new Dictionary<string, string>
            {
                { "id", question.QuestionId.ToString() },
            });

            xmlHelper.WriteElements(new Dictionary<string, string>
            {
                { "version", "1" }, // Since we only save one version of a question, this can be hardcoded to 1
                { "status", question.QuestionReady ? "ready" : "draft" }, // TODO: Determine if `draft` is really the correct value
            });

            xmlHelper.WriteElement("questions");
            WriteQuestionElement(xmlHelper, question);

            xmlHelper.CloseElement(); // </questions>
            xmlHelper.CloseElement(); // </question_versions>
        }


        /// <summary>
        /// writes the concrete data for a question to the XML file
        /// </summary>
        /// <param name="xmlHelper">Service used for writing to the XML file</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionElement(IXmlService xmlHelper, Question question)
        {
            // Use the appropriate question strategy based on the question type to write the specific plugin block
            IQuestionStrategy? pluginWriter = _strategyResolver.ResolveStrategy(question);

            // If a strategy was found, use its type. Else Fallback to the Helper.
            string questionTypeString = pluginWriter?.GetQuestionTypeString() ?? _quizHelper.MapQuestionType(question);

            xmlHelper.WriteElementWithAttributes("question", new Dictionary<string, string>
            {
                { "id", question.QuestionId.ToString() },
            });

            var questionBaseElements = new Dictionary<string, string>
            {
                { "parent", "0" },
                { "name", question.QuestionName },
                { "questiontext", question.QuestionText },
                { "questiontextformat", _generalHelper.EnumIntegerToString(question.QuestionTextFormat) },
                { "generalfeedback", question.GeneralFeedback ?? string.Empty },
                { "generalfeedbackformat", _generalHelper.EnumIntegerToString(question.GeneralFeedbackFormat) },
                { "defaultmark", question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture) },
                { "penalty", question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture) },
                { "qtype", questionTypeString},
                { "length", "1" },
                { "stamp", GenerateStamp() },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(DateTime.UtcNow) },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(DateTime.UtcNow) },
                { "createdby", "0" },
                { "modifiedby", "0" },
            };
            xmlHelper.WriteElements(questionBaseElements);

            pluginWriter?.WritePluginData(xmlHelper, question);

            // Comments
            WriteEmptyPlugin(xmlHelper, "qbank_comment_question", "comments");

            // Custom Fields
            WriteEmptyPlugin(xmlHelper, "qbank_customfields_question", "customfields");

            // Hints
            WriteHints(xmlHelper, question);

            // Tags
            _tagsWriter.WriteTags(xmlHelper, question.Tags);

            xmlHelper.CloseElement(); // </question>
        }


        /// <summary>
        /// Writes the `question_hints` element of questions.xml
        /// </summary>
        /// <param name="xmlService">Service used for writing to the XML file</param>
        /// <param name="question">the question to save, containing the relevant hints (can be none)</param>
        private void WriteHints(IXmlService xmlService, Question question)
        {
            // Handle no hints
            if (question.Hints == null || question.Hints.Count == 0)
            {
                xmlService.WriteElement("question_hints");
                xmlService.CloseElement(); // </question_hints>
                return;
            }

            xmlService.WriteElement("question_hints");
            foreach (var hint in question.Hints)
            {
                int hintId = _sequentialIdGenerator.GetNextHintId();
                xmlService.WriteElementWithAttributes("question_hint", new Dictionary<string, string>
                {
                    { "id", hintId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "hint", hint.HintText },
                    { "hintformat", _generalHelper.EnumIntegerToString(hint.HintTextFormat) },
                    { "shownumcorrect", hint.ShowNumberOfCorrectAnswers ? "1" : "0" },
                    { "clearwrong", hint.ClearIncorrectResponses ? "1" : "0" },
                    { "options", "$@NULL@$" },
                });

                xmlService.CloseElement(); // </question_hint>
            }

            xmlService.CloseElement(); // </question_hints>
        }


        /// <summary>
        /// Generates a unique stamp string in a specific format combining IP address, port, current Unix time, and UUID
        /// </summary>
        /// <returns>A unique string in the format "IPAddress:Port+UnixTimestamp+UUID".</returns>
        private static string GenerateStamp()
        {
            // Format seems to be: IPAddress:Port+UnidentifiedNumber+UUID
            // Example from `question.xml`: 172.21.0.3:8080+250328142456+mQIQma | 25.03.2028 14:24:56 ???
            // Another example: 172.21.0.3:8080+250418132115+UdkSCb | 25.04.2018 13:21:15 ???
            // FIXME: Don't hardcode the server ip + port
            return $"{DEFAULT_SERVER_ADDRESS}:{DEFAULT_SERVER_PORT}+{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}+{Guid.NewGuid():N}";
        }


        /// <summary>
        /// Writes an empty plugin element with the specified plugin name and child element container to the XML file
        /// </summary>
        /// <param name="xmlService">Service used for writing to the XML file</param>
        /// <param name="pluginName">The name of the plugin to be written</param>
        /// <param name="childContainer">The name of the child container element within the plugin</param>
        private void WriteEmptyPlugin(IXmlService xmlService, string pluginName, string childContainer)
        {
            xmlService.WriteElement($"plugin_{pluginName}");
            xmlService.WriteElement(childContainer);
            xmlService.CloseElement(); // childContainer
            xmlService.CloseElement(); // plugin
        }


        /// <summary>
        /// Writes the top-level question category to the XML file. Moodle requires this to serve as the parent category for all other question categories
        /// </summary>
        /// <param name="xmlService">Service used for writing to the XML file</param>
        /// <param name="id">The unique identifier of the top-level category</param>
        /// <param name="contextId">The ID representing the context in which the category belongs</param>
        /// <param name="contextLevel">The context level of the category (e.g., course or module)</param>
        /// <param name="contextInstanceId">The unique identifier of the context instance - whatever that is</param>
        private void WriteTopCategory(IXmlService xmlService, int id, int contextId, int contextLevel, int contextInstanceId)
        {
            xmlService.WriteElementWithAttributes("question_category", new Dictionary<string, string>
            {
                { "id", id.ToString() },
            });

            xmlService.WriteElements(new Dictionary<string, string>
            {
                { "name", "top" },
                { "contextid",        contextId.ToString() },
                { "contextlevel",     contextLevel.ToString() },  // 50 = course, 70 = module
                { "contextinstanceid",contextInstanceId.ToString() },
                { "info", "" }, { "infoformat","0" },
                { "stamp", GenerateStamp() },
                { "parent", "0" },
                { "sortorder", "0" },
                { "idnumber", "$@NULL@$" },
            });
            xmlService.WriteElement("question_bank_entries");
            xmlService.CloseElement(); // </question_bank_entries>
            xmlService.CloseElement(); // </question_category>
        }
    }
}
