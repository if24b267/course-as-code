# Analysis of CodeRunner Questions in Moodle XML Backups

!!! warning
    The insights described on this page originate from reverse engineering conducted **before** the activity was implemented in Course as Code. Since small errors often came to light during the implementation process, and these were (unfortunately) not always corrected accordingly in the reverse engineering documentation, it's possible that the following information is in some parts **incomplete** or **incorrect**.

    It is therefore recommended to always cross-reference the information with the implementation in the writer classes.

## General Information

CodeRunner is a Moodle question type plugin that allows instructors to create programming questions where students' code submissions are executed and graded automatically. This document analyzes the XML structure used in Moodle backup files for CodeRunner questions.

### Key Components

- **Question Type**: `coderunner` - A specialized question type for programming exercises
- **Language Options**: Supports multiple programming languages including Java, Python, C/C++, PHP, and more
- **Execution Environment**: Uses a sandbox system (typically Jobe) to safely execute student code
- **Test Cases**: Defines expected outputs for specific inputs to validate student code
- **Template System**: Uses templates to wrap student code for testing

## Analysis of XML Files

### **questions.xml**

The `questions.xml` file contains the main structure and configuration of CodeRunner questions.

#### CodeRunner Question Base Structure

- **Question ID**: Unique identifier for the question (e.g., `16`, `17`)
- **Name**: Name of the question (e.g., `Java Program: Hello World`)
- **Question Text**: The instructions and requirements for the programming task
- **Question Text Format**: Format of the question text (typically `1` for HTML)
- **General Feedback**: Feedback provided to all students after attempting the question
- **General Feedback Format**: Format of the general feedback
- **Default Mark**: Default point value for the question (e.g., `1.0000000`)
- **Penalty**: Penalty for each incorrect attempt (e.g., `0.0000000`)
- **Question Type**: Specified as `coderunner`
- **Length**: Always `1` for CodeRunner questions
- **Stamp**: A unique identifier with format `[server-ip]:[port]+[timestamp]+[uuid]`
- **Time Created**: Unix timestamp when the question was created
- **Time Modified**: Unix timestamp when the question was last modified

#### CodeRunner-Specific Elements

The CodeRunner-specific elements are contained within the `plugin_qtype_coderunner_question` tag, which includes two main sections:

1. **coderunner_options**: Contains all configuration options for the CodeRunner question
2. **coderunner_testcases**: Contains all test cases used to validate student submissions

##### coderunner_options

The `coderunner_options` section contains a single `coderunner_option` element with an `id` attribute. This element includes numerous configuration settings:

- **coderunnertype**: The programming language or question type (e.g., `java_program`, `c_program`, `python3`)
- **prototypetype**: Whether this is based on a prototype question (`0` for standard)
- **allornothing**: Whether grading is all-or-nothing (`1` for true)
- **penaltyregime**: Penalty scheme for multiple attempts (e.g., `10, 20, ...`)
- **precheck**: Precheck configuration (`0` = none, `1` = examples, `2` = selected, `3` = all)
- **hidecheck**: Whether to hide the Check button (`0` for false)
- **showsource**: Whether to show source code to students (`0` for false)
- **answerboxlines**: Number of lines in the answer box (e.g., `18`)
- **answerboxcolumns**: Number of columns in the answer box (e.g., `100`)
- **answerpreload**: Code preloaded in the answer box (can be empty)
- **globalextra**: Extra code included for all test cases (can be empty)
- **useace**: ACE editor settings (typically `$@NULL@$`)
- **resultcolumns**: Columns to display in results (typically `$@NULL@$`)
- **template**: Template code for testing (typically `$@NULL@$`)
- **iscombinatortemplate**: Whether the template is a combinator (typically `$@NULL@$`)
- **allowmultiplestdins**: Allow multiple standard inputs (typically `$@NULL@$`)
- **answer**: The model solution or correct answer
- **validateonsave**: Whether to validate the question on save (`1` for true)
- **testsplitterre**: Regular expression for splitting test cases (typically `$@NULL@$`)
- **language**: Language identifier (typically `$@NULL@$`)
- **acelang**: Language for ACE editor (typically `$@NULL@$`)
- **sandbox**: Sandbox to use (typically `$@NULL@$`)
- **grader**: Grading method to use (typically `$@NULL@$`)
- **cputimelimitsecs**: CPU time limit in seconds (typically `$@NULL@$`)
- **memlimitmb**: Memory limit in MB (typically `$@NULL@$`)
- **sandboxparams**: Parameters for sandbox (typically `$@NULL@$`)
- **templateparams**: Template parameters (can be empty)
- **hoisttemplateparams**: Whether to hoist template parameters (`1` for true)
- **extractcodefromjson**: Whether to extract code from JSON (`1` for true)
- **templateparamslang**: Language for template parameters (e.g., `None`)
- **templateparamsevalpertry**: Whether to evaluate parameters per try (`0` for false)
- **templateparamsevald**: Evaluated template parameters (e.g., `{}`)
- **twigall**: Whether to apply Twig to all fields (`0` for false)
- **uiplugin**: UI plugin to use (typically `$@NULL@$`)
- **uiparameters**: Parameters for UI plugin (can be empty)
- **attachments**: Number of attachments allowed (`0` for none)
- **attachmentsrequired**: Number of required attachments (`0` for none)
- **maxfilesize**: Maximum file size in bytes (e.g., `10240`)
- **filenamesregex**: Regular expression for filenames (can be empty)
- **filenamesexplain**: Explanation of filename requirements (can be empty)
- **displayfeedback**: Whether to display feedback (`1` for true)
- **giveupallowed**: Whether to allow a "give up" option (`0` for false)
- **prototypeextra**: Extra prototype configuration (typically `$@NULL@$`)

##### coderunner_testcases

The `coderunner_testcases` section contains multiple `coderunner_testcase` elements, each with an `id` attribute. Each test case includes:

- **testcode**: The code used to test the student's submission
- **testtype**: The type of test (`0` for standard)
- **expected**: The expected output for successful completion
- **useasexample**: Whether to use this test case as an example (`0` for false, `1` for true)
- **display**: How to display the test case (`SHOW`, `HIDE`, `HIDE_IF_FAIL`, `HIDE_IF_SUCCEED`)
- **hiderestiffail**: Whether to hide remaining tests if this one fails (`0` for false, `1` for true)
- **mark**: The mark value for this test case (e.g., `1.000`)
- **stdin**: Standard input for the test (can be empty)
- **extra**: Extra data for the test case (can be empty)

#### Common Plugins

CodeRunner questions also include standard Moodle question plugins:

- **plugin_qbank_comment_question**: For comments on the question
- **plugin_qbank_customfields_question**: For custom fields
- **question_hints**: For hints offered to students

### **Example CodeRunner Question (Java)**

Here's a simplified representation of a Java CodeRunner question:

```xml
<question id="16">
  <name>Java Program: Hello World</name>
  <questiontext>
    <text><![CDATA[<p>Write a complete Java program that prints exactly:</p>
<p>Hello, World!</p>
<p>Your program must define a public class named Main and include a main method. No input is needed.</p>]]></text>
    <format>1</format>
  </questiontext>
  <generalfeedback>
    <text><![CDATA[<p>Make sure you understand how to create a main class in Java.</p>]]></text>
    <format>1</format>
  </generalfeedback>
  <defaultmark>1.0000000</defaultmark>
  <penalty>0.0000000</penalty>
  <qtype>coderunner</qtype>
  <plugin_qtype_coderunner_question>
    <coderunner_options>
      <coderunner_option id="1">
        <coderunnertype>java_program</coderunnertype>
        <prototypetype>0</prototypetype>
        <allornothing>1</allornothing>
        <penaltyregime>10, 20, ...</penaltyregime>
        <precheck>0</precheck>
        <hidecheck>0</hidecheck>
        <showsource>0</showsource>
        <answerboxlines>18</answerboxlines>
        <answerboxcolumns>100</answerboxcolumns>
        <answerpreload></answerpreload>
        <globalextra></globalextra>
        <answer>public class Main {
    public static void main(String[] args) {
        System.out.println("Hello, World!");
    }
}</answer>
        <validateonsave>1</validateonsave>
        <templateparams></templateparams>
        <hoisttemplateparams>1</hoisttemplateparams>
        <extractcodefromjson>1</extractcodefromjson>
        <templateparamslang>None</templateparamslang>
        <templateparamsevalpertry>0</templateparamsevalpertry>
        <templateparamsevald>{}</templateparamsevald>
        <twigall>0</twigall>
        <displayfeedback>1</displayfeedback>
        <giveupallowed>0</giveupallowed>
      </coderunner_option>
    </coderunner_options>
    <coderunner_testcases>
      <coderunner_testcase id="1">
        <testcode>Hello World Output</testcode>
        <testtype>0</testtype>
        <expected>Hello, World!</expected>
        <useasexample>0</useasexample>
        <display>SHOW</display>
        <hiderestiffail>0</hiderestiffail>
        <mark>1.000</mark>
        <stdin></stdin>
        <extra></extra>
      </coderunner_testcase>
    </coderunner_testcases>
  </plugin_qtype_coderunner_question>
  <question_hints />
</question>
```

### **Example CodeRunner Question (C)**

Here's a simplified representation of a C CodeRunner question:

```xml
<question id="17">
  <name>C Program: Hello World</name>
  <questiontext>
    <text><![CDATA[<p>Write a complete C program that prints the following text exactly:</p>
<p>Hello, World!</p>
<p>Your program must include the necessary #include statements and a main() function. No input is needed.</p>]]></text>
    <format>1</format>
  </questiontext>
  <generalfeedback>
    <text><![CDATA[<p>Remember to include standard libraries and proper syntax.</p>]]></text>
    <format>1</format>
  </generalfeedback>
  <defaultmark>1.0000000</defaultmark>
  <penalty>0.0000000</penalty>
  <qtype>coderunner</qtype>
  <plugin_qtype_coderunner_question>
    <coderunner_options>
      <coderunner_option id="2">
        <coderunnertype>c_program</coderunnertype>
        <prototypetype>0</prototypetype>
        <allornothing>1</allornothing>
        <penaltyregime>10, 20, ...</penaltyregime>
        <precheck>0</precheck>
        <hidecheck>0</hidecheck>
        <showsource>0</showsource>
        <answerboxlines>18</answerboxlines>
        <answerboxcolumns>100</answerboxcolumns>
        <answerpreload></answerpreload>
        <globalextra></globalextra>
        <answer>#include <stdio.h>

int main() {
    printf("Hello, World!\n");
    return 0;
}</answer>
        <validateonsave>1</validateonsave>
        <templateparams></templateparams>
        <hoisttemplateparams>1</hoisttemplateparams>
        <extractcodefromjson>1</extractcodefromjson>
        <templateparamslang>None</templateparamslang>
        <templateparamsevalpertry>0</templateparamsevalpertry>
        <templateparamsevald>{}</templateparamsevald>
        <twigall>0</twigall>
        <displayfeedback>1</displayfeedback>
        <giveupallowed>0</giveupallowed>
      </coderunner_option>
    </coderunner_options>
    <coderunner_testcases>
      <coderunner_testcase id="2">
        <testcode>Hello World Output</testcode>
        <testtype>0</testtype>
        <expected>Hello, World!</expected>
        <useasexample>0</useasexample>
        <display>SHOW</display>
        <hiderestiffail>0</hiderestiffail>
        <mark>1.000</mark>
        <stdin></stdin>
        <extra></extra>
      </coderunner_testcase>
    </coderunner_testcases>
  </plugin_qtype_coderunner_question>
  <question_hints />
</question>
```

## Integration with Quiz Activities

CodeRunner questions can be included in quiz activities like any other question type. In the quiz XML:

1. The quiz contains question instances (slots) that reference CodeRunner questions by their Question Bank Entry ID
2. The question behavior settings determine how CodeRunner questions are presented and graded
3. Review settings control what feedback is shown to students

## Important Considerations for Implementation

1. **Language Support**: Ensure the Moodle server has the CodeRunner plugin installed with support for the target programming languages
2. **Sandbox Configuration**: CodeRunner requires a properly configured sandbox environment (typically Jobe) to execute code securely
3. **Test Case Design**: Carefully design test cases to ensure accurate and fair assessment of student code
4. **Resource Limits**: Consider memory and CPU time limits for code execution to prevent resource exhaustion
5. **Error Handling**: Account for various error conditions in student submissions (compilation errors, runtime errors, etc.)
6. **Security**: Ensure that the sandbox environment is properly secured to prevent malicious code execution

## Model-to-XML Mappings

When implementing CodeRunner questions in the internal model, the following mappings apply:

1. **CodeRunnerQuestion** model class maps to the `question` element with `qtype="coderunner"`
2. **CodeRunnerTestCase** model class maps to the `coderunner_testcase` elements
3. CodeRunner options map to corresponding elements in the `coderunner_options` section

## Summary

CodeRunner questions provide a powerful mechanism for assessing programming skills through automated execution and testing of student code. The XML structure for these questions is complex but well-organized, allowing for detailed configuration of the programming environment, test cases, and feedback options.

Key features include:
- Support for multiple programming languages
- Configurable test cases with expected outputs
- Options for displaying or hiding test results
- Customizable answer box dimensions
- Model solutions for reference
- Various options for feedback presentation

For successful implementation, ensure that the Moodle server has the appropriate CodeRunner plugin version installed and configured with the necessary programming language support.

<hr>

# CodeRunner XML Structure Analysis
This document provides a detailed analysis of the XML structure used in the CodeRunner Moodle plugin, focusing on the tags, their purposes, and how they represent Moodle UI elements.

## Core XML Files Overview

CodeRunner's functionality is defined through several interconnected XML files in the Moodle backup:

1. **moodle_backup.xml**: The main descriptor file containing course structure information
2. **questions.xml**: Defines CodeRunner question types and their properties
3. **quiz.xml**: Contains quiz-specific configuration and question instances
4. **module.xml**: Defines how the quiz module integrates with the course
5. **course.xml**: Contains course-level settings and format options

## 1. moodle_backup.xml Key Tags

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<information>` | Container for all backup metadata | Not directly visible in UI |
| `<moodle_version>` | Version of Moodle that created the backup | Displayed in restore information |
| `<original_course_fullname>` | Full name of the course | Text field in course settings |
| `<original_course_shortname>` | Short name of the course | Text field in course settings |
| `<original_course_format>` | Course format | Dropdown in course settings (topics, weekly, social, etc.) |
| `<contents>/<activities>` | List of all activities in the course | Activities listed in course page |
| `<settings>` | Controls what components are included | Checkboxes in backup UI |

## 2. questions.xml Key Tags

### Question Category Structure

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<question_category>` | Container for a question category | Category in question bank |
| `<n>` | Name of the category | Text field when creating/editing category |
| `<info>` | Description of the category | Text editor when creating/editing category |
| `<parent>` | Parent category ID | Dropdown selection when creating category |

### CodeRunner Question Structure

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<question>` | Container for question data | Question editing form |
| `<n>` | Question name | Text field "Question name" |
| `<questiontext>` | The actual question | HTML editor "Question text" |
| `<qtype>` | Question type | Shows as "CodeRunner" in type selector |
| `<defaultmark>` | Default marks | Numeric field "Default mark" |
| `<penalty>` | Penalty for each incorrect try | Numeric field "Penalty for each incorrect try" |

### CodeRunner Specific Options

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<coderunnertype>` | Programming language/question type | Dropdown with options like: python3, c_program, c_function, java_method, etc. |
| `<prototypetype>` | Whether this is a prototype question | Checkbox "Is prototype" |
| `<allornothing>` | All-or-nothing grading | Checkbox "All-or-nothing grading" (0=disabled, 1=enabled) |
| `<penaltyregime>` | Penalty scheme for multiple attempts | Text field "Penalty regime" (e.g., "10, 20, ...") |
| `<precheck>` | Precheck option | Dropdown with options: 0=None, 1=Examples, 2=Selected, 3=All |
| `<hidecheck>` | Whether to hide Check button | Checkbox (0=show, 1=hide) |
| `<showsource>` | Show source code to students | Checkbox (0=hide, 1=show) |
| `<answerboxlines>` | Height of answer box | Numeric field (typical values: 5-30) |
| `<answerboxcolumns>` | Width of answer box | Numeric field (typical values: 60-120) |
| `<answerpreload>` | Starting code provided to students | Text editor "Answer box preload" |
| `<globalextra>` | Extra code included outside the main function, often used for global declarations or includes | Hidden/internal field |
| `<validateonsave>` | Validate on question save | Checkbox (0=no, 1=yes) |
| `<template>` | Template code for testing | Text editor "Template" |
| `<iscombinatortemplate>` | Whether template uses combinator | Checkbox (0=no, 1=yes) |
| `<sandbox>` | Sandbox environment | Dropdown (options: jobe, ideone, etc.) |
| `<grader>` | Grading method | Dropdown (options: TemplateGrader, NearEquality, RegexGrader, etc.) |
| `<cputimelimitsecs>` | CPU time limit in seconds | Numeric field (default varies by language) |
| `<memlimitmb>` | Memory limit in MB | Numeric field (default varies by language) |
| `<attachments>` | Whether to allow file attachments | Dropdown (0=none, 1=optional, 2=required) |
| `<attachmentsrequired>` | Number of required attachments | Numeric field |
| `<maxfilesize>` | Maximum attachment size in bytes | Numeric field |
| `<displayfeedback>` | Show feedback | Checkbox (0=no, 1=yes) |
| `<giveupallowed>` | Allow "Give up" button | Checkbox (0=no, 1=yes) |
| `<useace>` | use the ACE code editor in the UI | Checkbox (0=no, 1=yes) |
| `<resultcolumns>` | Specifies extra columns in the result table | Comma-separated list in a text field |
| `<allowmultiplestdins>` | Allows multiple standard inputs in test cases | Checkbox (0=no, 1=yes) |
| `<answer>` | The correct answer code to be used for grading | Hidden/internal field |
| `<testsplitterre>` | Regular expression used to split test cases | Text field (regex input for splitting tests) |
| `<language>` | Language label for the answer box | Text input |
| `<acelang>` | Language for ACE syntax | Text input |
| `<sandboxparams>` | Advanced parameters to configure the sandbox environment | Text field(JSON or string) |
| `<templateparams>` | Parameters to be used inside the template for generating test code | Text field |
| `<hoisttemplateparams>` | Whether template parameters should be hoisted to the global scope | Checkbox (0=no, 1=yes) |
| `<extractcodefromjson>` | Whether to extract the student code from a JSON object | Checkbox (0=no, 1=yes) |
| `<templateparamslang>` | The programming language used within the template parameters | Checkbox (0=no, 1=yes) |
| `<templateparamsevalpertry>` | Number of evaluations per submission try | Numeric input |
| `<templateparamsevald>` | Default evaluation values for template parameters | Text field (JSON format) |
| `<twigall>` | Internal flag, likely used for Twig templating logic | Hidden/internal field |
| `<uiplugin>` | UI plugin to customize input or interface behavior | Text input |
| `<uiparameters>` | Parameters used by the UI plugin to customize behavior | Text field (JSON format) |
| `<filenamesregex>` | Regular expression to validate filenames for file attachments | Text field (regex) |
| `<filenamesexplain>` | Explanation shown to students about filename requirements | Text field |
| `<prototypeextra>` | Extra configuration for prototype-type questions | Text field |

### Test Cases

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<testcode>` | Code that tests student's answer | Text field "Test code" |
| `<testtype>` | Test type | Typically 0=normal, 1=pre-check only |
| `<expected>` | Expected output from test | Text field "Expected output" |
| `<useasexample>` | Show as example to students | Checkbox (0=no, 1=yes) |
| `<display>` | Visibility of test case | Dropdown: SHOW, HIDE, HIDE_IF_FAIL, HIDE_IF_SUCCEED |
| `<hiderestiffail>` | Hide remaining tests if this fails | Checkbox (0=no, 1=yes) |
| `<mark>` | Mark value for this test | Numeric field |
| `<stdin>` | Standard input for test | Text field "Standard input" |
| `<extra>` | Extra data for the test | Text field "Extra template data" |

## 3. quiz.xml Key Tags

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<n>` | Quiz name | Text field "Name" |
| `<intro>` | Quiz introduction | HTML editor "Description" |
| `<timeopen>` | Opening time | Date/time selector "Open the quiz" |
| `<timeclose>` | Closing time | Date/time selector "Close the quiz" |
| `<timelimit>` | Time limit in seconds | Numeric field "Time limit" (0=no limit) |
| `<overduehandling>` | How to handle overdue attempts | Dropdown: autosubmit, graceperiod, autoabandon |
| `<graceperiod>` | Grace period in seconds | Numeric field "Submission grace period" |
| `<preferredbehaviour>` | Question behavior | Dropdown with options: deferredfeedback, adaptive, interactive, immediatefeedback, manualgrading, etc. |
| `<canredoquestions>` | Allow redo within attempt | Checkbox (0=no, 1=yes) |
| `<attempts_number>` | Maximum attempts allowed | Dropdown (0=unlimited, 1, 2, 3, etc.) |
| `<attemptonlast>` | Each attempt builds on last | Checkbox (0=no, 1=yes) |
| `<grademethod>` | Grading method | Dropdown: 1=highest, 2=average, 3=first, 4=last |
| `<decimalpoints>` | Decimal places in grades | Dropdown (0-7) |
| `<questionsperpage>` | Questions per page | Dropdown (0=all on one page, 1, 2, 3, etc.) |
| `<navmethod>` | Navigation method | Dropdown: free, sequential |
| `<shuffleanswers>` | Shuffle within questions | Checkbox (0=no, 1=yes) |
| `<sumgrades>` | Sum of question grades | Calculated field (not directly editable) |
| `<grade>` | Maximum grade for the quiz | Numeric field "Grade to pass" |
| `<password>` | Access password | Text field "Require password" |
| `<subnet>` | Network address restrictions | Text field "Require network address" |
| `<showuserpicture>` | Show user's picture | Dropdown: 0=no, 1=yes |
| `<showblocks>` | Show blocks during attempt | Dropdown: 0=no, 1=yes |

## 4. module.xml Key Tags

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<modulename>` | Type of module | Module selection in course editing |
| `<sectionid>` | Course section ID | Not directly visible |
| `<sectionnumber>` | Position in course | Topic/week number in course |
| `<added>` | Timestamp when added | Not directly editable |
| `<visible>` | Module visibility | Eye icon toggle in course page |
| `<visibleoncoursepage>` | Shown on course page | Checkbox "Show on course page" |
| `<groupmode>` | Group mode setting | Dropdown: 0=no groups, 1=separate groups, 2=visible groups |
| `<completion>` | Completion tracking | Dropdown: 0=none, 1=manual, 2=automatic |
| `<completionview>` | Complete when viewed | Checkbox "Student must view this activity" |
| `<completionexpected>` | Expected completion date | Date selector |
| `<availability>` | Access restrictions | Complex restriction settings UI |
| `<showdescription>` | Show description on course | Checkbox "Display description on course page" |
| `<downloadcontent>` | Allow download | Checkbox "Download course content" |

## 5. course.xml Key Tags

| XML Tag | Description | UI Representation |
|---------|-------------|-------------------|
| `<shortname>` | Course short name | Text field "Course short name" |
| `<fullname>` | Course full name | Text field "Course full name" |
| `<format>` | Course format | Dropdown: topics, weekly, social, etc. |
| `<showgrades>` | Show gradebook to students | Dropdown: 0=no, 1=yes |
| `<newsitems>` | Number of announcements | Dropdown: 0-10 |
| `<startdate>` | Course start date | Date selector |
| `<enddate>` | Course end date | Date selector |
| `<maxbytes>` | Maximum upload size | Dropdown with byte values |
| `<visible>` | Course visibility | Dropdown: 0=hidden, 1=visible |
| `<groupmode>` | Default group mode | Dropdown: 0=no groups, 1=separate, 2=visible |
| `<groupmodeforce>` | Force group mode | Checkbox "Force" |
| `<showactivitydates>` | Show activity dates | Checkbox "Show activity completion conditions" |
| `<showcompletionconditions>` | Show completion conditions | Checkbox "Show activity completion conditions" |
| `<enablecompletion>` | Enable completion tracking | Checkbox "Enable completion tracking" |

## CodeRunner UI to XML Mapping

The CodeRunner question creation form in Moodle has several sections that map directly to XML elements:

### General Settings
- Question name → `<n>` 
- Question text → `<questiontext>`
- Default mark → `<defaultmark>`
- General feedback → `<generalfeedback>`

### CodeRunner Question Type
- Question type → `<coderunnertype>`
- Customization → `<prototypetype>`
- Answer box size → `<answerboxlines>` and `<answerboxcolumns>`
- Preloaded answer → `<answerpreload>`

### Grading Options
- All-or-nothing grading → `<allornothing>`
- Penalty regime → `<penaltyregime>`
- Pre-check options → `<precheck>`

### Advanced Customization
- Template → `<template>`
- Test case visibility → combination of `<display>` tags in test cases
- Sandbox → `<sandbox>`
- Memory/CPU limits → `<memlimitmb>` and `<cputimelimitsecs>`

### Test Cases Section
Each test case row in the UI maps to a `<coderunner_testcase>` element in XML, with columns mapping to:

- Test code → `<testcode>`
- Expected output → `<expected>` 
- Display → `<display>`
- Use as example → `<useasexample>`
- Hide rest if fail → `<hiderestiffail>`
