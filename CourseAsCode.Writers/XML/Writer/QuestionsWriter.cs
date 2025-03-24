using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    /// <summary>
    /// Handles the creation and writing of the questions.xml file for a course.
    /// </summary>
    public class QuestionsWriter : XmlWriterBase
    {
        private const string DEFAULT_QUESTIONS_RELATIVE_PATH = "questions.xml";
        private const string DEFAULT_SERVER_ADDRESS = "172.21.0.3";
        private const int DEFAULT_SERVER_PORT = 8080;
        
        private readonly IGeneralHelper _generalHelper;

        // Counters for IDs | TODO: Refactor to use a more robust ID generation strategy
        private int _nextCategoryId = DEFAULT_TOP_CATEGORY_ID + 1;
        private int _nextBankEntryId = 1;
        private int _nextQuestionId = 1; 
        private int _nextAnswerId = 1;
        private int _nextHintId = 1;
        
        private const int DEFAULT_TOP_CATEGORY_ID = 1;
        private const int DEFAULT_CONTEXT_ID = 383; // Every value >> 100 seems to be working fine | Needs to be the same as in `QuizWriter.cs`
        private const int DEFAULT_CONTEXT_LEVEL = 50; // 50 = course, 70 = module
        private const int DEFAULT_CONTEXT_INSTANCE_ID = 135; // Honestly, no idea what this is for
        
        
        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public QuestionsWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
            _generalHelper = GeneralHelper.Instance;
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public QuestionsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger, IGeneralHelper generalHelper) : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper;
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

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("question_categories");
                    writer.WriteEndElement(); // Close <question_categories>
                    
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_QUESTIONS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_QUESTIONS_RELATIVE_PATH}");
                throw;
            }
        } 
        
  
        /// <summary>
        /// Writes the `question_category` element of questions.xml
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="categoryId">the category ID</param>
        /// <param name="categoryName">the category name</param>
        /// <param name="questions">the list of questions belonging to that category</param>
        private void WriteQuestionCategory(XmlWriter writer, int categoryId, string categoryName, List<Question> questions)
        {
            writer.WriteStartElement("question_category");
            writer.WriteAttributeString("id", categoryId.ToString());
            
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
                { "idnumber", "$@NULL@$" }
            };
            WriteElements(writer, categoryElements);

            writer.WriteStartElement("question_bank_entries");
            foreach (var question in questions)
            {
                // Write all questions belonging to this category
                WriteQuestionBankEntry(writer, categoryId, question);
                
                // Store the ID of the category the question was saved to, so that other Writers (specifically InfoRefWriter) can reference it later
                question.CategoryId = categoryId;
            }
            writer.WriteEndElement(); // </question_bank_entries>

            writer.WriteEndElement(); // </question_category>
        }


        /// <summary>
        /// Writes the `question_bank_entry` element of questions.xml
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="categoryId">the category ID of the category to which the question / questionbank belongs</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionBankEntry(XmlWriter writer, int categoryId, Question question)
        {
            int bankEntryId = _nextBankEntryId++;
            question.BankEntryId = bankEntryId; // Store the ID of the question bank entry the question was saved to, so that QuizWriter can reference it later
            
            writer.WriteStartElement("question_bank_entry");
            writer.WriteAttributeString("id", bankEntryId.ToString());

            WriteElements(writer, new Dictionary<string, string>
            {
                { "questioncategoryid", categoryId.ToString() },
                { "idnumber", "$@NULL@$" },
                { "ownerid", "0" }
            });

            // Moodle uses an internal version control system for questions - we ignore this and only save one version of a question
            writer.WriteStartElement("question_version");
            WriteQuestionVersionContainer(writer, question);
            writer.WriteEndElement(); // </question_version>

            writer.WriteEndElement(); // </question_bank_entry>
        }

        
        /// <summary>
        /// Writes the `question_version` element of questions.xml
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionVersionContainer(XmlWriter writer, Question question)
        {
            // Store the ID that was assigned to the question, so that QuizWriter can reference it later
            question.QuestionId = _nextQuestionId++;
            
            writer.WriteStartElement("question_versions");
            writer.WriteAttributeString("id", question.QuestionId.ToString());

            WriteElements(writer, new Dictionary<string, string>
            {
                { "version", "1" }, // Since we only save one version of a question, this can be hardcoded to 1
                { "status", question.QuestionReady ? "ready" : "draft" } // TODO: Determine if `draft` is really the correct value
            });

            writer.WriteStartElement("questions");
            WriteQuestionElement(writer, question);
            writer.WriteEndElement(); // </questions>

            writer.WriteEndElement(); // </question_versions>
        }
        
        
        /// <summary>
        /// writes the concrete data for a question to the XML file; currently supports Single- and MultipleChoice questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        private void WriteQuestionElement(XmlWriter writer, Question question)
        {
            writer.WriteStartElement("question");
            writer.WriteAttributeString("id", question.QuestionId.ToString());

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
                { "qtype", EnumMappers.MapQuestionType(question) },
                { "length", "1" },
                { "stamp", GenerateStamp() },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(DateTime.UtcNow) },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(DateTime.UtcNow) },
                { "createdby", "0" },
                { "modifiedby", "0" }
            };
            WriteElements(writer, questionBaseElements);

            switch (question)
            {
                case MultipleChoiceQuestion multichoiceQuestion:
                    WriteMultiChoicePlugin(writer, multichoiceQuestion);
                    break;
                // Implement additional question types here
                default:
                    LogWarn($"Unsupported question type {question.GetType().Name} � skipping plugin block");
                    break;
            }

            // Comments
            WriteEmptyPlugin(writer, "qbank_comment_question", "comments");
            
            // Custom Fields
            WriteEmptyPlugin(writer, "qbank_customfields_question", "customfields");

            // Hints
            WriteHints(writer, question);
            
            // Tags
            TagsWriter.WriteTags(writer, question.Tags, "tags", Logger);

            writer.WriteEndElement(); // </question>
        }

        
        /// <summary>
        /// Writes the `plugin_qtype_multichoice_question` element of questions.xml that is used for multiple choice questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="multiChoiceQuestion">the question to save</param>
        private void WriteMultiChoicePlugin(XmlWriter writer, MultipleChoiceQuestion multiChoiceQuestion)
        {
            writer.WriteStartElement("plugin_qtype_multichoice_question");

            // Answers ------------------------------------------------------------
            writer.WriteStartElement("answers");
            foreach (var answer in multiChoiceQuestion.Answers)
            {
                int answerId = _nextAnswerId++;
                writer.WriteStartElement("answer");
                writer.WriteAttributeString("id", answerId.ToString());
                
                WriteElements(writer, new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", _generalHelper.EnumIntegerToString(answer.AnswerTextFormat) },
                    { "fraction", answer.AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.AnswerFeedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(answer.AnswerFeedbackFormat) }
                });
                
                writer.WriteEndElement(); // </answer>
            }
            writer.WriteEndElement(); // </answers>

            // Multichoice settings -------------------------------------------------
            writer.WriteStartElement("multichoice");
            int multichoiceId = _nextAnswerId++; // reuse counter for simplicity (avoids creating a new counter for just this element)
            writer.WriteAttributeString("id", multichoiceId.ToString());

            var multichoiceElements = new Dictionary<string, string>
            {
                { "layout", "0" },
                { "single", multiChoiceQuestion.SingleChoice ? "1" : "0" },
                { "shuffleanswers", multiChoiceQuestion.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", multiChoiceQuestion.CorrectResponseFeedback ?? string.Empty },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", multiChoiceQuestion.PartiallyCorrectResponseFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", multiChoiceQuestion.IncorrectResponseFeedback ?? string.Empty },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.IncorrectResponseFeedbackFormat) },
                { "answernumbering", EnumMappers.MapNumberingFormat(multiChoiceQuestion.ChoiceNumbering) },
                { "shownumcorrect", multiChoiceQuestion.Hints != null && multiChoiceQuestion.Hints.Any(h => h.ShowNumberOfCorrectAnswers) ? "1" : "0" },
                { "showstandardinstruction", multiChoiceQuestion.ShowStandardInstructions ? "1" : "0" }
            };
            WriteElements(writer, multichoiceElements);

            writer.WriteEndElement(); // </multichoice>
            writer.WriteEndElement(); // </plugin_qtype_multichoice_question>
        }


        /// <summary>
        /// Writes the `question_hints` element of questions.xml
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save, containing the relevant hints (can be none)</param>
        private void WriteHints(XmlWriter writer, Question question)
        {
            if (question is not MultipleChoiceQuestion multichoiceQuestion || multichoiceQuestion.Hints == null || multichoiceQuestion.Hints.Count == 0)
            {
                writer.WriteStartElement("question_hints");
                writer.WriteEndElement();
                return;
            }

            writer.WriteStartElement("question_hints");
            foreach (var hint in multichoiceQuestion.Hints)
            {
                int hintId = _nextHintId++;
                writer.WriteStartElement("question_hint");
                writer.WriteAttributeString("id", hintId.ToString());

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "hint", hint.HintText },
                    { "hintformat", _generalHelper.EnumIntegerToString(hint.HintTextFormat) },
                    { "shownumcorrect", hint.ShowNumberOfCorrectAnswers ? "1" : "0" },
                    { "clearwrong", hint.ClearIncorrectResponses ? "1" : "0" },
                    { "options", "$@NULL@$" }
                });

                writer.WriteEndElement(); // </question_hint>
            }
            writer.WriteEndElement(); // </question_hints>
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
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="pluginName">The name of the plugin to be written</param>
        /// <param name="childContainer">The name of the child container element within the plugin</param>
        private void WriteEmptyPlugin(XmlWriter writer, string pluginName, string childContainer)
        {
            writer.WriteStartElement($"plugin_{pluginName}");
            writer.WriteStartElement(childContainer);
            writer.WriteEndElement(); // childContainer
            writer.WriteEndElement(); // plugin
        }


        /// <summary>
        /// Writes the top-level question category to the XML file. Moodle requires this to serve as the parent category for all other question categories
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="id">The unique identifier of the top-level category</param>
        /// <param name="contextId">The ID representing the context in which the category belongs</param>
        /// <param name="contextLevel">The context level of the category (e.g., course or module)</param>
        /// <param name="contextInstanceId">The unique identifier of the context instance - whatever that is</param>
        private void WriteTopCategory(XmlWriter writer, int id, int contextId, int contextLevel, int contextInstanceId)
        {
            writer.WriteStartElement("question_category");
            writer.WriteAttributeString("id", id.ToString());
            WriteElements(writer, new Dictionary<string,string>{
                { "name", "top" },
                { "contextid",        contextId.ToString() },
                { "contextlevel",     contextLevel.ToString() },  // 50 = course, 70 = module
                { "contextinstanceid",contextInstanceId.ToString() },
                { "info", "" }, { "infoformat","0" },
                { "stamp", GenerateStamp() },
                { "parent","0" }, { "sortorder","0" }, { "idnumber","$@NULL@$" }
            });
            writer.WriteStartElement("question_bank_entries"); writer.WriteEndElement();
            writer.WriteEndElement(); // </question_category>
        }
    }
}
