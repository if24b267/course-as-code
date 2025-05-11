# Analysis of Dropdown (Matching) Questions in Moodle XML Backups

## General Information

In Moodle, dropdown questions are implemented as a type of "matching" question. These questions allow instructors to create prompts (sub-questions) where students must select the correct answer from a dropdown menu of options. This document analyzes the XML structure used in Moodle backup files for dropdown/matching questions.

### Key Components

- **Question Type**: `match` - The question type identifier in Moodle
- **UI Presentation**: Presents as a list of sub-questions, each with its own dropdown menu containing possible answers
- **Grading**: Each sub-question is typically worth an equal portion of the total question mark
- **Answer Pool**: All correct answers and distractors are pooled together to create the dropdown options

## Analysis of XML Files

### **questions.xml**

The `questions.xml` file contains the main structure and configuration of dropdown/matching questions.

#### Dropdown/Matching Question Base Structure

- **Question ID**: Unique identifier for the question (e.g., `12`, `13`)
- **Name**: Name of the question (e.g., `Democracies around the world`, `Programming Paradigms`)
- **Question Text**: The instructions for the matching task
- **Question Text Format**: Format of the question text (typically `1` for HTML)
- **General Feedback**: Feedback provided to all students after attempting the question
- **General Feedback Format**: Format of the general feedback
- **Default Mark**: Default point value for the question (e.g., `1.0000000`)
- **Penalty**: Penalty for each incorrect attempt (e.g., `0.3333333`)
- **Question Type**: Specified as `match`
- **Length**: Always `1` for matching questions
- **Stamp**: A unique identifier with format `[server-ip]:[port]+[timestamp]+[uuid]`
- **Time Created**: Unix timestamp when the question was created
- **Time Modified**: Unix timestamp when the question was last modified

#### Dropdown/Matching-Specific Elements

The dropdown/matching-specific elements are contained within the `plugin_qtype_match_question` tag, which includes two main sections:

1. **matchoptions**: Contains configuration options for the matching question
2. **matches**: Contains all sub-questions and their corresponding correct answers, as well as distractor options

##### matchoptions

The `matchoptions` section contains an element with an `id` attribute and includes settings for the matching question:

- **shuffleanswers**: Whether to shuffle the answer options (`1` for yes, which is typical for dropdown questions)
- **correctfeedback**: Feedback shown for a completely correct answer
- **correctfeedbackformat**: Format of the correct feedback
- **partiallycorrectfeedback**: Feedback shown for partially correct answers
- **partiallycorrectfeedbackformat**: Format of the partially correct feedback
- **incorrectfeedback**: Feedback shown for completely incorrect answers
- **incorrectfeedbackformat**: Format of the incorrect feedback
- **shownumcorrect**: Whether to show the number of correct answers (`0` for no, `1` for yes)

##### matches

The `matches` section contains multiple `match` elements, each with an `id` attribute. There are two types of matches:

1. **Sub-question matches**: These have both `questiontext` and `answertext` elements, where:
   - `questiontext`: The prompt or stem for the sub-question
   - `questiontextformat`: Format of the sub-question text
   - `answertext`: The correct answer for this sub-question

2. **Distractor-only matches**: These have an empty `questiontext` and only contain an `answertext` element:
   - `questiontext`: Empty (represented as `""`)
   - `questiontextformat`: Format (still present but irrelevant)
   - `answertext`: A distractor option that will appear in dropdown menus but isn't the correct answer for any sub-question

### **Example Dropdown/Matching Question**

Here's a simplified representation of a dropdown/matching question:

```xml
<question id="13">
  <parent>0</parent>
  <name>Programming Paradigms</name>
  <questiontext><p>Select the primary programming paradigm associated with each language.</p></questiontext>
  <questiontextformat>1</questiontextformat>
  <generalfeedback></generalfeedback>
  <generalfeedbackformat>2</generalfeedbackformat>
  <defaultmark>1.0000000</defaultmark>
  <penalty>0.2500000</penalty>
  <qtype>match</qtype>
  <length>1</length>
  <stamp>172.21.0.3:8080+1746965782+d76ee80668b945dd9fff588a80830952</stamp>
  <timecreated>1746958582</timecreated>
  <timemodified>1746958582</timemodified>
  <createdby>0</createdby>
  <modifiedby>0</modifiedby>
  <plugin_qtype_match_question>
    <matchoptions id="23">
      <shuffleanswers>1</shuffleanswers>
      <correctfeedback></correctfeedback>
      <correctfeedbackformat>2</correctfeedbackformat>
      <partiallycorrectfeedback></partiallycorrectfeedback>
      <partiallycorrectfeedbackformat>2</partiallycorrectfeedbackformat>
      <incorrectfeedback></incorrectfeedback>
      <incorrectfeedbackformat>2</incorrectfeedbackformat>
      <shownumcorrect>0</shownumcorrect>
    </matchoptions>
    <matches>
      <!-- Sub-question matches (prompt + correct answer) -->
      <match id="24">
        <questiontext><p>Haskell</p></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Functional</answertext>
      </match>
      <match id="25">
        <questiontext><p>Java</p></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Object-Oriented</answertext>
      </match>
      <match id="26">
        <questiontext><p>C</p></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Procedural</answertext>
      </match>
      <match id="27">
        <questiontext><p>Prolog</p></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Logic Programming</answertext>
      </match>
      
      <!-- Distractor-only matches (empty questiontext) -->
      <match id="28">
        <questiontext></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Functional</answertext>
      </match>
      <match id="29">
        <questiontext></questiontext>
        <questiontextformat>1</questiontextformat>
        <answertext>Markdown Language</answertext>
      </match>
    </matches>
  </plugin_qtype_match_question>
  <plugin_qbank_comment_question>
    <comments />
  </plugin_qbank_comment_question>
  <plugin_qbank_customfields_question>
    <customfields />
  </plugin_qbank_customfields_question>
  <question_hints />
  <tags />
</question>
```

In this example:
- There are 4 sub-questions (Haskell, Java, C, and Prolog) with their correct answers
- There are 2 distractors (one is a duplicate "Functional" which may confuse students, and "Markdown Language")
- All these answers will be pooled together and presented in dropdown menus for each sub-question

## Important Implementation Details

### Answer Pooling

All answers (both correct answers and distractors) are pooled together to create the dropdown options for each sub-question. The XML structure represents this by:

1. Including matches with both `questiontext` and `answertext` for sub-questions with their correct answers
2. Including additional matches with empty `questiontext` and only `answertext` for distractors

### Duplicate Answers

Note that in the example, "Functional" appears twice - once as the correct answer for "Haskell" and once as a separate distractor. In the Moodle UI, it will only appear once in the dropdown options, as Moodle automatically de-duplicates the answer pool.

### Wrong Answers List in Internal Model

In the internal model, dropdown sub-questions might include a "WrongAnswers" list property for each sub-question. When converting to XML, these wrong answers (distractors) become separate `match` elements with empty `questiontext`.

For example, a model structure like:
```csharp
new DropdownSubQuestion
{
    QuestionText = "<p>Java</p>",
    QuestionTextFormat = FormattingFormat.HTML,
    CorrectAnswer = "Object-Oriented",
    WrongAnswers = new List<string> { "Functional" }
}
```

Would be converted to:
1. A match with "Java" as the questiontext and "Object-Oriented" as the answertext
2. A distractor match with empty questiontext and "Functional" as the answertext

## Hints Implementation

The `question_hints` element can contain multiple `question_hint` elements, each with:

- **hint**: The text of the hint
- **hintformat**: Format of the hint text
- **shownumcorrect**: Whether to show the number of correct answers after applying the hint
- **clearwrong**: Whether to clear incorrect responses when the hint is shown
- **options**: Additional options (typically `$@NULL@$`)

When hints are present, they influence the behavior of the dropdown question in interactive or adaptive question behavior modes.

## Model-to-XML Mappings

When implementing dropdown/matching questions in the internal model, the following mappings apply:

1. **DropdownQuestion** model class maps to the `question` element with `qtype="match"`
2. **DropdownSubQuestion** model class items map to the `match` elements with non-empty `questiontext`
3. The `WrongAnswers` lists from sub-questions map to `match` elements with empty `questiontext`
4. Feedback settings map to corresponding elements in the `matchoptions` section
5. **Hint** model class items map to `question_hint` elements

## Common Issues and Solutions

1. **Duplicate Answers**: Be aware that Moodle de-duplicates the answer pool. If the same answer appears multiple times (either as correct answers or distractors), it will only appear once in the dropdown options.

2. **Empty Feedback**: When feedback is not provided, the XML elements for feedback will still be present but empty.

3. **Answer Shuffling**: By default, the `shuffleanswers` option is set to `1`, which means the order of answers in dropdown menus will be randomized. This can be turned off by setting it to `0`, but this is unusual for dropdown questions.

4. **Sub-question Formatting**: Each sub-question can have its own text format, allowing for rich formatting using HTML.

## Summary

Dropdown (matching) questions in Moodle provide a way to assess students' ability to match items from one list with items from another. The XML structure:

- Uses the `match` question type
- Creates an answer pool from correct answers and distractors
- Presents sub-questions with dropdown menus containing all possible answers
- Supports rich text formatting for question text and sub-questions
- Allows for feedback at various correctness levels
- Supports hints to guide students toward correct answers

When implementing a dropdown question writer or reader, special attention should be paid to the way the answer pool is constructed, ensuring that all correct answers and distractors are properly included in the resulting XML structure.