# Question Bank (`questions.yaml`)

Question bank files allow you to define a list of questions that can be imported into the Moodle Question Bank and subsequently used in Quiz activities. These files are referenced in the `config.yaml` under `QuestionListPaths`.

The root structure is a list of questions.

## Question List

| Property | Type | Description |
| :--- | :--- | :--- |
| `Questions` | `List<YamlQuestion>` | A list of question definitions. |

---

## Question Definition (`YamlQuestion`)

All question types share a single YAML structure. Some properties are common to all questions, while others are specific to certain question types (e.g., CodeRunner, Essay).

### Common Properties

| Property | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `QuestionName` | `string` | **Required** | The name of the question as shown in the question bank. |
| `QuestionText` | `string` | **Required** | The main body of the question presented to the learner. |
| `QuestionType` | `QuestionType` | **Required** | The type of question (e.g., `MULTIPLECHOICE`, `ESSAY`). |
| `Category` | `string` | - | The category the question belongs to (e.g., "Module 1/Easy"). |
| `DefaultMark` | `float` | `1.0` | The default mark for the question. |
| `QuestionReady` | `bool` | `true` | Whether the question is ready for use (true) or a draft (false). |
| `Tags` | `List<string>` | - | Optional tags for organizing questions. |
| `ShuffleAnswers` | `bool` | `false` | Whether to shuffle the answer choices (if applicable). |
| `QuestionTextFormat` | `FormattingFormat` | `PlainText` | The format of the question text (`HTML`, `Markdown`, `PlainText`). |

### Feedback & Hints

| Property | Type | Description |
| :--- | :--- | :--- |
| `CorrectResponseFeedback` | `string` | Feedback shown when the response is correct. |
| `PartiallyCorrectResponseFeedback` | `string` | Feedback shown when the response is partially correct. |
| `IncorrectResponseFeedback` | `string` | Feedback shown when the response is incorrect. |
| `Hints` | `List<YamlHint>` | A list of hints available to the student. |
| `HintPenalty` | `float` | Penalty (0.0 - 1.0) applied for each hint used. |

### Standard Answer Options
*Used by Single/Multiple Choice, True/False, and Short Answer.*

| Property | Type | Description |
| :--- | :--- | :--- |
| `Answers` | `List<YamlQuestionAnswer>` | List of possible answers and their grades. |
| `UseCase` | `bool` | **ShortAnswer only**: Whether case sensitivity applies. |

### CodeRunner Specific Properties
*Used when `QuestionType` is `CODERUNNER`.*

| Property | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `CodeRunnerType` | `string` | - | The specific CodeRunner question type (e.g., `c_function`, `python3`). |
| `CodeRunnerTestCases` | `List<YamlCodeRunnerTestCase>` | - | A list of test cases to validate the student's code. |
| `Answer` | `string` | - | The sample correct answer/code. |
| `AnswerPreload` | `string` | - | Code preloaded into the answer box. |
| `AnswerBoxLines` | `int` | `18` | Height of the answer box in lines. |
| `AnswerBoxColumns` | `int` | `100` | Width of the answer box. |
| `Precheck` | `int` | `0` | Precheck setting (e.g., 0 = Disabled). |
| `AllOrNothing` | `bool` | `true` | If true, all test cases must pass to get marks. |
| `ValidateOnSave` | `bool` | `true` | Whether to validate code when saved. |

### Essay Specific Properties
*Used when `QuestionType` is `ESSAY`.*

| Property | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `ResponseFormat` | `string` | `"editor"` | Format of the response field. |
| `ResponseRequired` | `bool` | `true` | Whether a response is required. |
| `ResponseFieldLines` | `int` | `15` | Size of the input box. |
| `MinWordLimit` | `int?` | - | Minimum word count. |
| `MaxWordLimit` | `int?` | - | Maximum word count. |
| `AttachmentsAllowed` | `int` | `0` | Number of allowed attachments. |
| `FileTypesList` | `string` | - | Allowed file extensions (e.g., `.pdf,.docx`). |
| `ResponseTemplate` | `string` | - | Template text for the student's response. |
| `GraderInfo` | `string` | - | Information for the grader (grading guide). |

### Calculated / Numerical Specific Properties
*Used when `QuestionType` is `CALCULATED`, `SIMPLECALCULATED`, etc.*

| Property | Type | Description |
| :--- | :--- | :--- |
| `NumericalOptions` | `YamlNumericalOption` | Settings for unit display and grading. |
| `NumericalUnits` | `List<YamlNumericalUnit>` | Definitions of units and their multipliers. |
| `DatasetDefinitions` | `List<YamlDatasetDefinition>` | Variables/Wildcards used in the calculation. |
| `CalculatedRecords` | `List<YamlCalculatedRecord>` | Specific calculated answer records. |
| `CalculatedOptions` | `YamlCalculatedOption` | General options for calculated questions. |

### Drag and Drop / Sub-Questions
*Used by Matching, Dropdown, and Drag & Drop Text.*

| Property | Type | Description |
| :--- | :--- | :--- |
| `SubQuestions` | `List<YamlSubQuestion>` | Sub-questions for matching/dropdown types. |
| `DragDropAnswers` | `List<YamlDragDropAnswerText>`| The draggable text items. |

---

## Helper Objects

### YamlQuestionAnswer
Used for standard choice-based questions.

| Property | Type | Description |
| :--- | :--- | :--- |
| `AnswerText` | `string` | The text of the answer. |
| `AnswerGrade` | `float` | The grade percentage (e.g., `100` for correct, `0` for incorrect). |
| `Feedback` | `string` | Specific feedback for this answer choice. |

### YamlHint
Used for hints in interactive mode.

| Property | Type | Description |
| :--- | :--- | :--- |
| `HintText` | `string` | The hint text. |
| `ClearIncorrectResponses`| `bool` | Whether to clear incorrect responses when the hint is shown. |
| `ShowNumberOfCorrectAnswers`| `bool` | Whether to show how many correct answers were found. |

### YamlCodeRunnerTestCase
Used for CodeRunner questions.

| Property | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `TestCode` | `string` | - | The code to run for the test. |
| `Expected` | `string` | - | The expected output. |
| `Stdin` | `string` | - | Standard input for the test case. |
| `Mark` | `decimal` | `1.0` | Marks awarded for passing this test. |
| `UseAsExample` | `bool` | `false` | Whether this is an example test case shown to students. |
| `Display` | `string` | `"SHOW"` | Visibility setting (e.g., `SHOW`, `HIDE`). |

### YamlSubQuestion
Used for Matching/Dropdown questions.

| Property | Type | Description |
| :--- | :--- | :--- |
| `SubQuestionText` | `string` | The question text or stem. |
| `CorrectAnswer` | `string` | The correct matching answer. |
| `WrongAnswers` | `List<string>` | Distractor answers (if supported). |