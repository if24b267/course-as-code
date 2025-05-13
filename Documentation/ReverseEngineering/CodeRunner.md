# Analysis of CodeRunner Questions in Moodle XML Backups

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