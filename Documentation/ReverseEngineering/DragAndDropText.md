# Analysis of Drag-and-Drop Text (Lückentext) Questions in Moodle XML Backups

## General Information

Drag-and-drop text questions (often referred to as "Lückentext" in German) are a question type in Moodle that allows students to fill in gaps in text by dragging and dropping words or phrases. These questions are represented as "ddwtos" (drag-and-drop words into text) in Moodle's question system. This document analyzes the XML structure used in Moodle backup files for drag-and-drop text questions.

### Key Components

- **Question Type**: `ddwtos` - The question type identifier in Moodle
- **UI Presentation**: Text with gaps marked by placeholders (`[[1]]`, `[[2]]`, etc.) that students fill by dragging words/phrases from a selection area
- **Drag Groups**: Words/phrases can be organized into different groups (often color-coded in the UI)
- **Grading**: Usually all-or-nothing or partial credit per correct placement

## Analysis of XML Files

### **questions.xml**

The `questions.xml` file contains the main structure and configuration of drag-and-drop text questions.

#### Drag-and-Drop Text Question Base Structure

- **Question ID**: Unique identifier for the question (e.g., `14`, `15`)
- **Name**: Name of the question (e.g., `Computer Science Basics Q1`, `Linux File Permissions Q1`)
- **Question Text**: The text containing placeholders where words will be dragged (e.g., `The [[1]] is a type of system software that manages [[5]] and software resources...`)
- **Question Text Format**: Format of the question text (typically `1` for HTML or `4` for Markdown)
- **General Feedback**: Feedback provided to all students after attempting the question
- **General Feedback Format**: Format of the general feedback
- **Default Mark**: Default point value for the question (e.g., `1.0000000`)
- **Penalty**: Penalty for each incorrect attempt (e.g., `0.2500000`)
- **Question Type**: Specified as `ddwtos`
- **Length**: Always `1` for drag-and-drop text questions
- **Stamp**: A unique identifier with format `[server-ip]:[port]+[timestamp]+[uuid]`
- **Time Created**: Unix timestamp when the question was created
- **Time Modified**: Unix timestamp when the question was last modified

#### Drag-and-Drop Text Specific Elements

The drag-and-drop text specific elements are contained within the `plugin_qtype_ddwtos_question` tag, which includes two main sections:

1. **answers**: Contains the draggable text items (choices)
2. **ddwtos**: Contains configuration options for the question

##### answers

The `answers` section contains multiple `answer` elements, each with an `id` attribute. Each `answer` element includes:

- **answertext**: The text of the draggable item (e.g., `Operating System`, `Hardware`)
- **answerformat**: Format of the answer text (typically `1` for HTML)
- **fraction**: Always `0.0000000` for drag-and-drop questions (not used for grading)
- **feedback**: Contains serialized PHP object with drag group information:
  - Format: `O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}`
  - `draggroup`: The group to which this draggable item belongs (e.g., `"1"`, `"2"`)
  - `infinite`: Whether the item can be used multiple times (`b:0` for false, `b:1` for true)
- **feedbackformat**: Always `0` for drag-and-drop questions

##### ddwtos

The `ddwtos` section contains an element with an `id` attribute and includes settings for the drag-and-drop question:

- **shuffleanswers**: Whether to shuffle the draggable items (`1` for yes, `0` for no)
- **correctfeedback**: Feedback shown for a completely correct answer
- **correctfeedbackformat**: Format of the correct feedback
- **partiallycorrectfeedback**: Feedback shown for partially correct answers
- **partiallycorrectfeedbackformat**: Format of the partially correct feedback
- **incorrectfeedback**: Feedback shown for completely incorrect answers
- **incorrectfeedbackformat**: Format of the incorrect feedback
- **shownumcorrect**: Whether to show the number of correct answers (`0` for no, `1` for yes)

### **Example Drag-and-Drop Text Question**

Here's a simplified representation of a drag-and-drop text question:

```xml
<question id="14">
  <parent>0</parent>
  <name>Computer Science Basics Q1</name>
  <questiontext><p>The [[1]] is a type of system software that manages [[5]] and software resources, and provides common services for computer programs</p></questiontext>
  <questiontextformat>1</questiontextformat>
  <generalfeedback></generalfeedback>
  <generalfeedbackformat>2</generalfeedbackformat>
  <defaultmark>1.0000000</defaultmark>
  <penalty>0.2500000</penalty>
  <qtype>ddwtos</qtype>
  <length>1</length>
  <stamp>172.21.0.3:8080+1746965782+c2822db97aaa4fd1bb6e9eca15376160</stamp>
  <timecreated>1746958582</timecreated>
  <timemodified>1746958582</timemodified>
  <createdby>0</createdby>
  <modifiedby>0</modifiedby>
  <plugin_qtype_ddwtos_question>
    <answers>
      <answer id="30">
        <answertext>Operating System</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="31">
        <answertext>Compiler</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="32">
        <answertext>Microsoft Word</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="33">
        <answertext>Printer</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="34">
        <answertext>Hardware</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"2";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="35">
        <answertext>Internet</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"2";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
      <answer id="36">
        <answertext>Printing</answertext>
        <answerformat>1</answerformat>
        <fraction>0.0000000</fraction>
        <feedback>O:8:"stdClass":2:{s:9:"draggroup";s:1:"2";s:8:"infinite";b:0;}</feedback>
        <feedbackformat>0</feedbackformat>
      </answer>
    </answers>
    <ddwtos id="37">
      <shuffleanswers>1</shuffleanswers>
      <correctfeedback></correctfeedback>
      <correctfeedbackformat>2</correctfeedbackformat>
      <partiallycorrectfeedback></partiallycorrectfeedback>
      <partiallycorrectfeedbackformat>2</partiallycorrectfeedback>
      <incorrectfeedback></incorrectfeedback>
      <incorrectfeedbackformat>2</incorrectfeedback>
      <shownumcorrect>0</shownumcorrect>
    </ddwtos>
  </plugin_qtype_ddwtos_question>
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
- The question text contains two placeholders: `[[1]]` and `[[5]]`
- There are 7 draggable items divided into 2 groups:
  - Group 1: "Operating System", "Compiler", "Microsoft Word", "Printer"
  - Group 2: "Hardware", "Internet", "Printing"
- Shuffleanswers is enabled, so the draggable items will be displayed in random order

## Placeholder Syntax

The placeholders in the question text follow a specific format: double square brackets containing the number of the gap, e.g., `[[1]]`, `[[2]]`, etc. These numbers correspond to the positions where students should drag and drop the text items.

For example, in the question text:
```
The [[1]] is a type of system software that manages [[5]] and software resources...
```

Students would need to drag:
- The appropriate term to gap `[[1]]` (likely "Operating System")
- The appropriate term to gap `[[5]]` (likely "Hardware")

The numbers inside the placeholders do not necessarily need to be sequential. They can be any integer values, but they should be unique within the question.

## Drag Groups

Draggable items are organized into groups, which is represented in the `feedback` element as a serialized PHP object. This serialized object contains two pieces of information:

1. `draggroup`: A string value indicating the group number (e.g., `"1"`, `"2"`)
2. `infinite`: A boolean value indicating if the item can be used multiple times (`b:0` for false, `b:1` for true)

The serialized format looks like:
```
O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}
```

In the Moodle UI, different drag groups are typically displayed with different background colors to help students distinguish them.

## Handling Categories in the Model

In the internal model, drag-and-drop text items often include a "Category" property that maps to the drag group in the XML. The mapping is as follows:

- If Category is "A", it maps to draggroup "1"
- If Category is "B", it maps to draggroup "2"
- If Category is "C", it maps to draggroup "3"
- And so on...

If Category is null or empty, it typically defaults to draggroup "1".

## Hints Implementation

The `question_hints` element can contain multiple `question_hint` elements, each with:

- **hint**: The text of the hint
- **hintformat**: Format of the hint text
- **shownumcorrect**: Whether to show the number of correct answers after applying the hint
- **clearwrong**: Whether to clear incorrect responses when the hint is shown
- **options**: Additional options (typically `$@NULL@$`)

When hints are present, they influence the behavior of the drag-and-drop question in interactive or adaptive question behavior modes.

## Model-to-XML Mappings

When implementing drag-and-drop text questions in the internal model, the following mappings apply:

1. **DragAndDropTextQuestion** model class maps to the `question` element with `qtype="ddwtos"`
2. **TextChoice** model class items map to the `answer` elements
3. The `Category` property in TextChoice maps to the `draggroup` value in the serialized PHP object in the `feedback` element
4. Feedback settings map to corresponding elements in the `ddwtos` section
5. **Hint** model class items map to `question_hint` elements

## Common Issues and Solutions

1. **Serialized PHP Object**: The `feedback` element contains a serialized PHP object, which must be properly formatted for Moodle to interpret it correctly.

2. **Multiple Drag Groups**: When implementing multiple drag groups, ensure that:
   - Each draggable item is assigned to the correct group
   - The serialized PHP object in the `feedback` element correctly reflects the group assignment
   - The question text contains placeholders that match the expected patterns

3. **Shuffle Answers**: The `shuffleanswers` option determines whether the draggable items are displayed in random order (`1`) or in the order they appear in the XML (`0`).

4. **Placeholder IDs**: The numbers inside the placeholders (`[[1]]`, `[[2]]`, etc.) should be unique within the question, but they do not necessarily need to correspond to the IDs of the draggable items.

## Summary

Drag-and-drop text (Lückentext) questions in Moodle provide a way for students to fill in missing words or phrases in a text by dragging items from a selection area. The XML structure:

- Uses the `ddwtos` question type
- Defines draggable items with their text and group assignments
- Uses placeholders in the question text to indicate where items should be dragged
- Supports organizing draggable items into different groups
- Allows for feedback at various correctness levels
- Supports hints to guide students toward correct answers

When implementing a drag-and-drop text question writer or reader, special attention should be paid to:
1. The format of the placeholders in the question text
2. The correct generation of the serialized PHP object for drag group assignments
3. The mapping between internal model categories and XML drag groups
4. The handling of shuffleanswers and other configuration options