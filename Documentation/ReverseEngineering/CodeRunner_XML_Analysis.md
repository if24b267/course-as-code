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
