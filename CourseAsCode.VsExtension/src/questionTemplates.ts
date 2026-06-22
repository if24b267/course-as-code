// Question scaffolding templates for the "Create Question" command.
//
// Each questionpool file is deserialized by the C# reader as a SINGLE YamlQuestion
// (YamlQuestionReader.Read → DeserializeFromFile<YamlQuestion>), NOT a `Questions:` list.
// Quizzes reference a question by `Ref:` = relative path (without .yaml) into questionpool/.
//
// Templates for the well-understood types are complete and generate as-is once names/texts
// are filled in. The calculated/drag-and-drop-image families are inherently complex
// (datasets, coordinates, image files) — their templates are valid skeletons that need
// completion per the Moodle question semantics.

export type QuestionType =
  | 'SINGLECHOICE'
  | 'MULTIPLECHOICE'
  | 'TRUEFALSE'
  | 'SHORTANSWER'
  | 'ESSAY'
  | 'DESCRIPTION'
  | 'DROPDOWN'
  | 'DRAGANDDROPTEXT'
  | 'DRAGANDDROPTEXTANDIMAGE'
  | 'CODERUNNER'
  | 'SIMPLECALCULATED'
  | 'CALCULATED'
  | 'CALCULATEDMULTICHOICE';

export const QUESTION_TYPES: QuestionType[] = [
  'SINGLECHOICE',
  'MULTIPLECHOICE',
  'TRUEFALSE',
  'SHORTANSWER',
  'ESSAY',
  'DESCRIPTION',
  'DROPDOWN',
  'DRAGANDDROPTEXT',
  'DRAGANDDROPTEXTANDIMAGE',
  'CODERUNNER',
  'SIMPLECALCULATED',
  'CALCULATED',
  'CALCULATEDMULTICHOICE',
];

const SINGLECHOICE_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Your question?</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: SINGLECHOICE',
  'DefaultMark: 1',
  'ShuffleAnswers: true',
  'Answers:',
  '  - AnswerText: "Correct answer"',
  '    AnswerTextFormat: HTML',
  '    AnswerGrade: 1     # 1 = fully correct',
  '    Feedback: "Correct!"',
  '  - AnswerText: "Wrong answer"',
  '    AnswerTextFormat: HTML',
  '    AnswerGrade: 0',
  '    Feedback: ""',
  '',
].join('\n');

const MULTIPLECHOICE_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Select all that apply.</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: MULTIPLECHOICE',
  'DefaultMark: 1',
  'ShuffleAnswers: true',
  'Answers:',
  '  # Positive grades sum to 1 across all correct options.',
  '  - AnswerText: "Correct option A"',
  '    AnswerGrade: 0.5',
  '    Feedback: ""',
  '  - AnswerText: "Correct option B"',
  '    AnswerGrade: 0.5',
  '    Feedback: ""',
  '  - AnswerText: "Wrong option"',
  '    AnswerGrade: 0',
  '    Feedback: ""',
  '',
].join('\n');

const TRUEFALSE_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Is this statement correct?</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: TRUEFALSE',
  'DefaultMark: 1',
  'CorrectAnswer: true       # the correct response: true or false',
  'ShowStandardInstructions: true',
  'Answers:',
  '  - AnswerText: "true"',
  '    AnswerGrade: 1',
  '    Feedback: "Correct."',
  '  - AnswerText: "false"',
  '    AnswerGrade: 0',
  '    Feedback: "Incorrect."',
  '',
].join('\n');

const SHORTANSWER_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Type the answer.</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: SHORTANSWER',
  'DefaultMark: 1',
  'UseCase: false            # true = answers are case-sensitive',
  'Answers:',
  '  # Each AnswerText is an accepted response; "*" matches anything.',
  '  - AnswerText: "expected answer"',
  '    AnswerGrade: 1',
  '    Feedback: "Correct."',
  '  - AnswerText: "*"',
  '    AnswerGrade: 0',
  '    Feedback: "Not quite."',
  '',
].join('\n');

const ESSAY_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Discuss the following...</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: ESSAY',
  'DefaultMark: 1',
  'ResponseFormat: editor    # editor | editorfilepicker | plain | monospaced | noinline',
  'ResponseRequired: true',
  'ResponseFieldLines: 15',
  '# MinWordLimit: 50',
  '# MaxWordLimit: 500',
  '# AttachmentsAllowed: 0',
  '# AttachmentsRequired: 0',
  'GraderInfo: "<p>Marking guidance shown to graders.</p>"',
  '',
].join('\n');

const DESCRIPTION_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Informational text shown to students. No answer is required.</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: DESCRIPTION',
  'DefaultMark: 0',
  '',
].join('\n');

const DROPDOWN_TEMPLATE = [
  'QuestionName: ""',
  '# Use [[1]], [[2]], ... placeholders in the text; each maps to a SubQuestion by order.',
  'QuestionText: "<p>The capital of France is [[1]] and of Italy is [[2]].</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: DROPDOWN',
  'DefaultMark: 1',
  'SubQuestions:',
  '  - SubQuestionText: "1"',
  '    CorrectAnswer: "Paris"',
  '    WrongAnswers:',
  '      - "London"',
  '      - "Berlin"',
  '  - SubQuestionText: "2"',
  '    CorrectAnswer: "Rome"',
  '    WrongAnswers:',
  '      - "Madrid"',
  '      - "Vienna"',
  '',
].join('\n');

const DRAGANDDROPTEXT_TEMPLATE = [
  'QuestionName: ""',
  '# Use [[1]], [[2]], ... placeholders; each maps to a DragDropAnswer by order.',
  'QuestionText: "<p>The [[1]] sat on the [[2]].</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: DRAGANDDROPTEXT',
  'DefaultMark: 1',
  'DragDropAnswers:',
  '  - Text: "cat"',
  '  - Text: "mat"',
  '',
].join('\n');

const DRAGANDDROPTEXTANDIMAGE_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Drag each label onto the correct spot on the image.</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: DRAGANDDROPTEXTANDIMAGE',
  'DefaultMark: 1',
  '# Background image is optional; path is relative to this file.',
  '# BackgroundImagePath: ./images/diagram.png',
  '# BackgroundImageName: diagram.png',
  'Drags:',
  '  - Number: 1',
  '    DragGroup: 1',
  '    Infinite: false',
  '    Label: "Label A"',
  '  - Number: 2',
  '    DragGroup: 1',
  '    Infinite: false',
  '    Label: "Label B"',
  'Drops:',
  '  - Number: 1',
  '    XLeft: 50',
  '    YTop: 50',
  '    Choice: 1     # which Drag (by Number) belongs here',
  '    Label: "Spot 1"',
  '  - Number: 2',
  '    XLeft: 150',
  '    YTop: 80',
  '    Choice: 2',
  '    Label: "Spot 2"',
  '',
].join('\n');

const CODERUNNER_TEMPLATE = [
  'QuestionName: ""',
  'QuestionText: "<p>Write a function that returns ...</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: CODERUNNER',
  'CodeRunnerType: "python3"  # e.g. python3 | c | cpp | java | ...',
  'DefaultMark: 1',
  'AllOrNothing: true',
  'AnswerBoxLines: 18',
  '# Reference solution (a file path here is inlined automatically).',
  'Answer: |',
  '  def solution():',
  '      return 42',
  'CodeRunnerTestCases:',
  '  - TestCode: "print(solution())"',
  '    Expected: "42"',
  '    UseAsExample: true',
  '    Display: "SHOW"',
  '    Mark: 1.0',
  '',
].join('\n');

const SIMPLECALCULATED_TEMPLATE = [
  'QuestionName: ""',
  '# Use {variable} wildcards in the text and the answer formula.',
  'QuestionText: "<p>What is {a} + {b}?</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: SIMPLECALCULATED',
  'DefaultMark: 1',
  'Answers:',
  '  - AnswerText: "{a}+{b}"   # the formula, using the dataset wildcards',
  '    AnswerGrade: 1',
  '    Feedback: ""',
  'DatasetDefinitions:',
  '  - Name: "a"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 2',
  '      - Number: 2',
  '        Value: 5',
  '  - Name: "b"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 3',
  '      - Number: 2',
  '        Value: 4',
  'CalculatedRecords:',
  '  - Tolerance: 0.01',
  '    ToleranceType: 1',
  '    CorrectAnswerLength: 2',
  '    CorrectAnswerFormat: 1',
  '',
].join('\n');

const CALCULATED_TEMPLATE = [
  'QuestionName: ""',
  '# Like SIMPLECALCULATED but with shared dataset options and unit handling.',
  'QuestionText: "<p>What is {a} * {b}?</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: CALCULATED',
  'DefaultMark: 1',
  'Answers:',
  '  - AnswerText: "{a}*{b}"',
  '    AnswerGrade: 1',
  '    Feedback: ""',
  'DatasetDefinitions:',
  '  - Name: "a"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 2',
  '      - Number: 2',
  '        Value: 5',
  '  - Name: "b"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 3',
  '      - Number: 2',
  '        Value: 4',
  'CalculatedRecords:',
  '  - Tolerance: 0.01',
  '    ToleranceType: 1',
  '    CorrectAnswerLength: 2',
  '    CorrectAnswerFormat: 1',
  '',
].join('\n');

const CALCULATEDMULTICHOICE_TEMPLATE = [
  'QuestionName: ""',
  '# A multiple-choice question whose options are calculated formulas.',
  'QuestionText: "<p>What is {a} + {b}?</p>"',
  'QuestionTextFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'QuestionType: CALCULATEDMULTICHOICE',
  'DefaultMark: 1',
  'Answers:',
  '  - AnswerText: "{={a}+{b}}"   # correct calculated option',
  '    AnswerGrade: 1',
  '    Feedback: "Correct."',
  '  - AnswerText: "{={a}-{b}}"   # distractor',
  '    AnswerGrade: 0',
  '    Feedback: ""',
  'DatasetDefinitions:',
  '  - Name: "a"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 2',
  '      - Number: 2',
  '        Value: 5',
  '  - Name: "b"',
  '    Type: 1            # 1 = private (per-question) dataset',
  '    Items:',
  '      - Number: 1',
  '        Value: 3',
  '      - Number: 2',
  '        Value: 4',
  '',
].join('\n');

const TEMPLATES: Record<QuestionType, string> = {
  SINGLECHOICE: SINGLECHOICE_TEMPLATE,
  MULTIPLECHOICE: MULTIPLECHOICE_TEMPLATE,
  TRUEFALSE: TRUEFALSE_TEMPLATE,
  SHORTANSWER: SHORTANSWER_TEMPLATE,
  ESSAY: ESSAY_TEMPLATE,
  DESCRIPTION: DESCRIPTION_TEMPLATE,
  DROPDOWN: DROPDOWN_TEMPLATE,
  DRAGANDDROPTEXT: DRAGANDDROPTEXT_TEMPLATE,
  DRAGANDDROPTEXTANDIMAGE: DRAGANDDROPTEXTANDIMAGE_TEMPLATE,
  CODERUNNER: CODERUNNER_TEMPLATE,
  SIMPLECALCULATED: SIMPLECALCULATED_TEMPLATE,
  CALCULATED: CALCULATED_TEMPLATE,
  CALCULATEDMULTICHOICE: CALCULATEDMULTICHOICE_TEMPLATE,
};

export function getQuestionTemplate(type: QuestionType): string {
  return TEMPLATES[type];
}
