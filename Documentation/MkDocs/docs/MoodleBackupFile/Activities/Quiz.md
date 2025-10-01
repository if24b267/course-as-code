# Basic parameters that can be set for a quiz

!!! warning
    The insights described on this page originate from reverse engineering conducted **before** the activity was implemented in Course as Code. Since small errors often came to light during the implementation process, and these were (unfortunately) not always corrected accordingly in the reverse engineering documentation, it's possible that the following information is in some parts **incomplete** or **incorrect**.

    It is therefore recommended to always cross-reference the information with the implementation in the writer classes.

## quiz.xml
all quizzes seem to have `modulename=quiz` and `moduleid=227`

## Name

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                               |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------|
| Name                        | string    | quiz.xml | name               | string          | name of the activity (present in the activity base class) |

## Description

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                      |
|-----------------------------|-----------|----------|--------------------|-----------------|------------------------------------------------------------------|
| Description                 | string    | quiz.xml | intro              | string          | description of the activity (present in the activity base class) |

## Description Format

| Name in Internal Data Model | Data Type                | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|--------------------------|----------|--------------------|-----------------|----------------------------------------------------------------------------------|
| DescriptionFormat           | FormattingFormat (Enum)  | quiz.xml | introformat        | int             | the format in which the description is provided, e.g. plain text, HTML, Markdown |

## Display Description on course page

| Name in Internal Data Model    | Data Type | XML File   | Name in Moodle XML | Data Type (XML) | Description                                                                                      |
|--------------------------------|-----------|------------|--------------------|-----------------|--------------------------------------------------------------------------------------------------|
| DisplayDescriptionOnCoursePage | Boolean   | module.xml | showdescription    | int (1/0)       | if enabled, the description will be displayed on the course page just below the link to the quiz |

## Quiz Opening Time

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                      |
|-----------------------------|-----------|----------|--------------------|-----------------|------------------------------------------------------------------|
| OpenTime                    | DateTime  | quiz.xml | timeopen           | unix timestamp  | Students can only start the quiz after the open time has passed  |

## Quiz Closing Time

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                    |
|-----------------------------|-----------|----------|--------------------|-----------------|----------------------------------------------------------------|
| CloseTime                   | DateTime  | quiz.xml | timeclose          | unix timestamp  | Students must end the quiz before the closing time has passed  |

## Time Limit

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                   |
|----------------------------|-----------|----------|--------------------|-----------------|-------------------------------------------------------------------------------------------------------------------------------|
| TimeLimit                  | int       | quiz.xml | timelimit          | int (seconds)   | if enabled, the time limit is stated on the initial quiz page and a countdown timer is displayed in the quiz navigation block |

## Time Expiry Handling
this variable controls what happens if a student fails to submit their quiz attempt before the time expires.  
If the student is actively working on the quiz at the time, then the countdown timer will always automatically submit the attempt for them but if they have logged out, then this setting controls what happens

| Name in Internal Data Model | Data Type               | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                      |
|-----------------------------|-------------------------|----------|--------------------|-----------------|--------------------------------------------------------------------------------------------------|
| TimeExpiryHandling          | TimeExpiryHandle (Enum) | quiz.xml | overduehandling    | string          | controls what happens if a student fails to submit their quiz attempt before the time expires    |
| GracePeriod                 | int                     | quiz.xml | graceperiod        | int (seconds)   | if TimeExpiryHandling is set to GracePeriod, this variable specifies the grace period in seconds |

### TimeExpiryHandle

| Value in Internal Data Model | Value in Moodle XML | Description                                                                                 |
|------------------------------|---------------------|---------------------------------------------------------------------------------------------|
| AutoSubmit                   | autosubmit          | Open attempts are submitted automatically                                                   |
| GracePeriod                  | graceperiod         | There is a grace period when open attempts can be submitted, but no more questions answered |
| DoNotCount                   | autoabandon         | Attempts must be submitted before time expires, or they are not counted                     |


## GradeToPass

| Name in Internal Data Model | Data Type    | XML File   | Name in Moodle XML | Data Type (XML) | Description                                    |
|-----------------------------|--------------|------------|--------------------|-----------------|------------------------------------------------|
| GradeToPass                 | float (0-10) | grades.xml | gradepass          | float           | determines the minimum grade required to pass  |

The value is used in activity and course completion and in the gradebook, where pass grades are highlighted in green and fail grades in red

## Attempts allowed

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                  |
|-----------------------------|-----------|----------|--------------------|-----------------|----------------------------------------------------------------------------------------------|
| AllowedAttempts             | int       | quiz.xml | attempts_number    | int             | the number of quiz attempts allowed. A value of 0 means that unlimited attempts are allowed  |

## Grading method

| Name in Internal Data Model | Data Type            | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                             |
|-----------------------------|----------------------|----------|--------------------|-----------------|---------------------------------------------------------------------------------------------------------|
| GradingMethod               | GradingMethod (Enum) | quiz.xml | grademethod        | int             | when multiple attempts are allowed, multiple methods are available for calculating the final quiz grade |

### GradingMethod

| Value in Internal Data Model | Value in Moodle XML | Description                                         |
|------------------------------|---------------------|-----------------------------------------------------|
| HighestGrade                 | 1                   | the highest grade of all attempts                   |
| AverageGrade                 | 2                   | the average (mean) grade of all attempts            |
| FirstAttempt                 | 3                   | the first attempt (all other attempts are ignored)  |
| LastAttempt                  | 4                   | the last attempt (all other attempts are ignored)   |

## New Page Threshhold

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                                 |
|-----------------------------|-----------|----------|--------------------|-----------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| NewPageThreshold            | int       | quiz.xml | questionsperpage   | int             | page breaks will automatically be inserted after x questions according to this setting, 0 means no page breaks -> all questions on one page |

For longer quizzes, it makes sense to stretch the quiz over several pages by limiting the number of questions per page.  
When adding questions to the quiz, page breaks will automatically be inserted according to this setting. However, page breaks may later be moved manually on the editing page.

## Shuffle within questions

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                     |
|-----------------------------|-----------|----------|--------------------|-------------|-----------------------------------------------------------------------------------------------------------------|
| ShuffleWithinQuestions      | boolean   | quiz.xml | shuffleanswers     | int (1/0)   | if enabled,  the parts making up each questions will be randomly shuffled each time a student attempts the quiz |

Only works if the option is also enabled in the questions settings.  
This setting only applies to questions that have multiple parts, such as multiple choice or matching questions.

## Question Behaviour
Students can interact with the questions in the quiz in various different ways. For example, you may wish the students to enter an answer to each question and then submit the entire quiz, before anything is graded or they get any feedback. That would be "Deferred feedback" mode.  
Alternatively, you may wish for students to submit each questions as they go along to get immediate feedback and if they do not get it right immediately, have another try for fewer marks. That would be "Interactive with multiple tries" mode.

| Name in Internal Data Model | Data Type                | XML File | Name in Moodle XML  | Data Type (XML) | Description                                                       |
|-----------------------------|--------------------------|----------|---------------------|-----------------|-------------------------------------------------------------------|
| QuestionBehaviour           | QuestionBehaviour (Enum) | quiz.xml | preferredbehaviour  | string          | the way in which students interact with the questions in the quiz |

### QuestionBehaviour

| Value in Internal Data Model | Value in Moodle XML | Description                                                                                                                                                                                                                                                                                                                                                                                                      |
|------------------------------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Adaptive                     | adaptive            | Immediate Feedback, but allows students to have multiple attempts at the question before moving on to the next question. Students get fewer marks on subsequent attempts                                                                                                                                                                                                                                         |
| AdaptiveNoPenalties          | adaptivenopenalty   | Immediate Feedback, but allows students to have multiple attempts at the question before moving on to the next question. No penalties for subsequent attempts                                                                                                                                                                                                                                                    |
| Deferred                     | deferredfeedback    | Students must enter an answer to each question and then submit the entire quiz before anything is graded or they get any feedback                                                                                                                                                                                                                                                                                |
| DeferredCBM                  | deferredcbm         | Certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. The grading is adjusted by the choice of certainty                                                                                                                                                                                                           |
| Immediate                    | immediatefeedback   | Students can submit their response immediately during the quiz attempt and get it graded. However, they can only submit one response and cannot change it later                                                                                                                                                                                                                                                  |
| ImmediateCBM                 | immediatecbm        | Certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. The grading is adjusted by the choice of certainty                                                                                                                                                                                                           |
| InteractiveMultipleTries     | interactive         | Allows multiple attempts on a question with possible grade penalties. Students submit answers via the 'Check' button and can retry using 'Try again' if incorrect. After each wrong attempt, a hint is displayed. Once correct, answers cannot be changed. If attempts run out, the student is graded accordingly and shown feedback. The number of tries equals the number of hints plus one (minimum of three) |

### AllowRedosDuringAttempt

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                                                               |
|-----------------------------|-----------|----------|--------------------|-----------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AllowRedosDuringAttempt     | boolean   | quiz.xml | canredoquestions   | int (1/0)       | if enabled, students can redo questions during the attempt. This is only applicable to some question behaviours like Immediate, ImmediateCBM and InteractiveMultipleTries |

If enabled, after finishing attempting a question, a 'Try another question like this one' button is displayed. This allows for a similar questions (selected randomly) to be attempted or the same question to be attempted again without the entire quiz attempt having to be submitted and another attempt started.  
This setting is only applicable to question behaviours where it is possible to finish a questions before the attempt is submitted.

## AttemptBuildsOnPreviousAttempts

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                           |
|-----------------------------|-----------|----------|--------------------|-----------------|---------------------------------------------------------------------------------------------------------------------------------------|
| BuildOnPreviousAttempts     | boolean   | quiz.xml | attemptonlast      | int (1/0)       | if multiple attempts are allowed and this settings is enabled, each new quiz attempt will contain the results of the previous attempt |

## Review Options
These options control what information students can see when they review a quiz attempt or look at the quiz reports.

In the backup XML each of the eight review‑options (parameters) gets its own tag (e.g. <reviewattempt>…</reviewattempt>), and the number inside is a bitmask calculated like this:

```pseudocode
Parameter enabled for QuizClosed: 2^4 -> 16 || 0               +
Parameter enabled for AfterAttempt: 2^8 -> 256 || 0            +
Parameter enabled for ImmediatelyAfter: 2^12 -> 4096 || 0      +
Parameter enabled for DuringAttempt: 2^16 -> 65536 || 0
```

### Time
| Value in Internal Data Model | Value in Moodle XML | Description                                                                                                                          |
|------------------------------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------|
| DuringAttempt                |                     | only applicable to some question behaviours like InteractiveMultipleTries. Displays feedback during the attempt                      |
| ImmediatelyAfter             |                     | student can see their grade immediately after clicking on "Submit all and finish"                                                    |
| AfterAttempt                 |                     | student can see their grade two minutes after they clicked on "Submit all and finish"                                                |
| QuizClosed                   |                     | student can see their grade after the quiz close date has passed. If the quiz doesn't have a close date, this state is never reached |

### Parameters
each of these values is a boolean

| Value in Internal Data Model   | Value in Moodle XML | Description                                                                                                                                                                |
|--------------------------------|---------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Attempt                        |                     | whether the student can review the attempt at all                                                                                                                          |
| WhetherCorrect                 |                     | whether the student can see if he answered the question "correct", "partially correct" or "incorrect". Also displays the colour highlighting that conveys this information |
| MaximumMarks                   |                     | whether the student can see the maximum mark he could have achieved for each question                                                                                      |
| Marks                          |                     | whether the student can see the mark obtained for each question and the overall attempt score - only available if MaximumMarks is true                                     |
| SpecificFeedback               |                     | whether the student can see feedback that depends on what response the student gave                                                                                        |
| GeneralFeedback                |                     | whether the student can see general feedback, displayed to all students regardless of their answer. E.g. a fully worked answer and a link to more information              |
| RightAnswer                    |                     | whether the student can see what would have been the correct answer(s)                                                                                                     |
| OverallFeedback                |                     | whether the student can see the feedback given at the end of the attempt, depending on the student's total mark                                                            |


## ShowPicture

| Name in Internal Data Model | Data Type             | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                                                                       |
|-----------------------------|-----------------------|----------|--------------------|-----------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ShowPicture                 | PictureOptions (Enum) | quiz.xml | showuserpicture    | int             | if enabled, a student's name and picture will be shown on-screen during the attempt and on the review screen, making it easier to check that the student is logged in as himself  |

### PictureOptions

| Value in Internal Data Model | Value in Moodle XML |
|------------------------------|---------------------|
| NoImage                      | 0                   |
| SmallImage                   | 1                   |
| LargeImage                   | 2                   |

## GradeDecimalsToDisplay

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                         |
|-----------------------------|-----------|----------|--------------------|-----------------|-------------------------------------------------------------------------------------|
| DecimalsToDisplayGrades     | int       | quiz.xml | decimalpoints      | int (0 - 5)     | specifies the number of digits shown after the decimal point when displaying grades |

Only affects the display of grades, not the grades stored in the database or the internal calculations

## MarkDecimalsToDisplay

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML     | Data Type (XML) | Description                                                                                                     |
|-----------------------------|-----------|----------|------------------------|-----------------|-----------------------------------------------------------------------------------------------------------------|
| DecimalsToDisplayMarks      | int       | quiz.xml | questiondecimalpoints  | int (-1 - 7)    | specifies the number of digits shown after the decimal point when displaying the marks for individual questions |

`-1` is equal to `Same as for grades`

## SafeExamBrowser
If any of these settings are not set, they are set to `$@NULL@$` in the XML file.

### Require Safe Exam Browser

| Name in Internal Data Model | Data Type      | XML File | Name in Moodle XML                                     | Data Type (XML) | Description                           |
|-----------------------------|----------------|----------|--------------------------------------------------------|-----------------|---------------------------------------|
| RequireSEB                  | SEBMode (Enum) | quiz.xml | `<quizaccess_seb_quizsettings>` tag present if enabled | int (-1 - 7)    | whether SEB is required for this quiz |

#### SEBMode

| Value in Internal Data Model | Value in Moodle XML                            | Description                                                                                     |
|------------------------------|------------------------------------------------|-------------------------------------------------------------------------------------------------|
| Disabled                     | all SEB configs tags missing from the XML file | Safe Exam Browser is not required for this quiz                                                 |
| EnabledManualConfig          | 1                                              | Safe Exam Browser is required for this quiz, the config is done manually in Moodle              |
| EnabledUploadConfig          | ???                                            |                                                                                                 |
| EnabledClientConfig          | 4                                              | No specific configuration of SEB is enforced - the quiz can be attempted with any configuration |

// TODO: Moodle doesn't allow you to click through the Upload config option without providing a .seb config file - we need to get our hands on one of these files.

### Show Safe Exam Browser Download Button

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML  | Data Type (XML) | Description                                                                                                    |
|-----------------------------|-----------|----------|---------------------|-----------------|----------------------------------------------------------------------------------------------------------------|
| ShowSEBDownloadButton       | boolean   | quiz.xml | showsebdownloadlink | int (1/0)       | if enabled, a button is shown on the quiz start page that allows the student to download the Safe Exam Browser |

### Safe Exam Browser Exit Link

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                                                                                                                         |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| SEBQuitLinkButton           | string    | quiz.xml | linkquitseb        | string          | if set, an "Exit Safe Exam Browser" button will be shown on the page that appears after the exam is submitted - clicking the link allows the student to exit SEB without having to enter a password |

For an HTTPS site, the link provided must start with `https://`. If no link is provided, the button will not be shown.  
_Quit Links_: Administrators specify a particular URL as the quit link within SEB's settings. This URL can be any valid web address. When SEB detects navigation to this predefined URL, it initiates the process to close the application.

### Safe Exam Browser Confirm Quitting

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                 |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------------------|
| SEBConfirmQuitLink          | boolean   | quiz.xml | userconfirmquit    | int (1/0)       | if enabled, users have to confirm quitting SEB when a quit link is detected |

### Enable Quitting of Safe Exam Browser

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                |
|-----------------------------|-----------|----------|--------------------|-----------------|----------------------------------------------------------------------------|
| SEBEnableQuit               | boolean   | quiz.xml | allowuserquitseb   | int (1/0)       | if enabled, users can quit SEB by clicking the quit button in the task bar |

Alternatively, users can also exit SEB by pressing `Ctrl + Q` or by clicking the close button in the top right corner of the browser window.

### Quit Password

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML)    | Description                                                      |
|-----------------------------|-----------|----------|--------------------|--------------------|------------------------------------------------------------------|
| SEBQuitPassword             | string    | quiz.xml | quitpassword       | string (plaintext) | if set, a password must be entered to quit the Safe Exam Browser |

If no quit password is set, then SEB just prompts "Are you sure you want to quit SEB?" when the user tries to quit.

### Enabled Reload in Exams

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                        |
|-----------------------------|-----------|----------|--------------------|-----------------|------------------------------------|
| SEBEnableReload             | boolean   | quiz.xml | allowreloadinexam  | int (1/0)       | if enabled, page reload is allowed |

The user can reload the page by pressing the reload button in the task bar, by pressing `F5` or by pressing `Ctrl + R`.

### Safe Exam Browser: Show Task Bar

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                 |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------------------|
| SEBShowTaskBar              | boolean   | quiz.xml | showsebtaskbar     | int (1/0)       | if enabled, the task bar is shown in the Safe Exam Browser window          |

The task bar is required to display items such as Wi-Fi control, reload button, time and keyboard layout.

### Safe Exam Browser: Show Reload Button

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                            |
|-----------------------------|-----------|----------|--------------------|-----------------|------------------------------------------------------------------------|
| SEBShowReloadButton         | boolean   | quiz.xml | showreloadbutton   | int (1/0)       | if enabled, a reload button is shown in the Safe Exam Browser task bar |

### Safe Exam browser: Show Time

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                     |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------|
| SEBShowTime                 | boolean   | quiz.xml | showtime           | int (1/0)       | if enabled, the time is shown in the Safe Exam Browser task bar |

### Safe Exam browser: Show Keyboard Layout

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                |
|-----------------------------|-----------|----------|--------------------|-----------------|----------------------------------------------------------------------------|
| SEBShowKeyboardLayout       | boolean   | quiz.xml | showkeyboardlayout | int (1/0)       | if enabled, the keyboard layout is shown in the Safe Exam Browser task bar |

Also allows the user to switch to other keyboard layouts, which have been enabled in the operating system.

### Safe Exam browser: Show Wi-Fi Control

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                   |
|-----------------------------|-----------|----------|--------------------|-----------------|-------------------------------------------------------------------------------|
| SEBShowWifiControl          | boolean   | quiz.xml | showwificontrol    | int (1/0)       | if enabled, a Wi-Fi control button is shown in the Safe Exam Browser task bar |

The button allows users to reconnect to Wi-Fi networks which have previously been connected to.

### Safe Exam browser: Enable audio controls

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                 |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------------------|
| SEBEnableAudioControls      | boolean   | quiz.xml | enableaudiocontrol | int (1/0)       | if enabled, audio controls are shown in the Safe Exam Browser task bar      |

### Safe Exam browser: Enable spell checking

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                 |
|-----------------------------|-----------|----------|--------------------|-----------------|-----------------------------------------------------------------------------|
| SEBEnableSpellCheck         | boolean   | quiz.xml | allowspellchecking | int (1/0)       | if enabled, spell checking is enabled in the Safe Exam Browser             |

### Safe Exam browser: Enable URL filtering

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML   | Data Type (XML) | Description                                              |
|-----------------------------|-----------|----------|----------------------|-----------------|----------------------------------------------------------|
| SEBEnableURLFiltering       | boolean   | quiz.xml | activateurlfiltering | int (1/0)       | if enabled, URLs will be filtered when loading web pages |

Requires a filter to be defined in the following settings:

#### Safe Exam Browser: Filter embedded content

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML    | Data Type (XML) | Description                                                              |
|-----------------------------|-----------|----------|-----------------------|-----------------|--------------------------------------------------------------------------|
| SEBFilterEmbeddedContent    | boolean   | quiz.xml | filterembeddedcontent | int (1/0)       | if enabled, embedded content will also be filtered using the filter set  |

#### Safe Exam Browser: Allowed URLs

| Name in Internal Data Model | Data Type    | XML File | Name in Moodle XML | Data Type (XML) | Description                                                             |
|-----------------------------|--------------|----------|--------------------|-----------------|-------------------------------------------------------------------------|
| SEBAllowedURLs              | List<string> | quiz.xml | expressionsallowed | string          | a list of URLs that are allowed to be accessed in the Safe Exam Browser |

#### Safe Exam Browser: Allowed URLs (Regex)

| Name in Internal Data Model | Data Type    | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                            |
|-----------------------------|--------------|----------|--------------------|-----------------|----------------------------------------------------------------------------------------|
| SEBAllowedURLsRegex         | List<string> | quiz.xml | regexallowed       | string          | a list of regular expressions that are allowed to be accessed in the Safe Exam Browser |

#### Safe Exam Browser: Blocked URLs

| Name in Internal Data Model | Data Type    | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                  |
|-----------------------------|--------------|----------|--------------------|-----------------|------------------------------------------------------------------------------|
| SEBBlockedURLs              | List<string> | quiz.xml | expressionsblocked | string          | a list of URLs that are blocked from being accessed in the Safe Exam Browser | 

#### Safe Exam Browser: Blocked URLs (Regex)

| Name in Internal Data Model | Data Type    | XML File | Name in Moodle XML | Data Type (XML) | Description                                                                                 |
|-----------------------------|--------------|----------|--------------------|-----------------|---------------------------------------------------------------------------------------------|
| SEBBlockedURLsRegex         | List<string> | quiz.xml | regexblocked       | string          | a list of regular expressions that are blocked from being accessed in the Safe Exam Browser |

### Safe Exam Browser: Allowed Exam Keys

See [Safe Exam Browser Documentation](https://safeexambrowser.org/developer/documents/SEB-Specification-BrowserExamKey.pdf) for an explanation of exam keys  
Basically, exam keys in Safe Exam Browser are unique identifiers generated for each exam to verify that the correct SEB version and specific exam settings are being used by the student.
They work by adding a SHA256-hashed key (combined with the requested URL) to each HTTP request, allowing the LMS to securely validate the browser’s configuration.

| Name in Internal Data Model | Data Type    | XML File | Name in Moodle XML     | Data Type (XML)  | Description                                                     |
|-----------------------------|--------------|----------|------------------------|------------------|-----------------------------------------------------------------|
| SEBAllowedExamKeys          | List<string> | quiz.xml | allowedbrowserexamkeys | string           | a list of browser exam keys that are allowed to access the quiz |

If no exam keys are provided, no check is performed -> any browser version and/or configuration can be used.

## Password

| Name in Internal Data Model | Data Type | XML File | Name in Moodle XML | Data Type (XML)    | Description                                                                      |
|-----------------------------|-----------|----------|--------------------|--------------------|----------------------------------------------------------------------------------|
| Password                    | string    | quiz.xml | password           | string (plaintext) | if a password is specified, a student must enter it in order to attempt the quiz |

## Feedback
Overall Feedback is text that is shown after a quiz has been attempted (Overall Feedback)  
By specifying additional grade boundaries (as percentage), the text shown can depend on the grade obtained (Specific Feedback)

| Name in Internal Data Model | Data Type      | XML File | Name in Moodle XML   | Data Type (XML) | Description                                                                 |
|-----------------------------|----------------|----------|----------------------|-----------------|-----------------------------------------------------------------------------|
| Feedback                    | List<Feedback> | quiz.xml | feedbacks            | various         | the feedback that is shown to the student after the quiz has been attempted |

### Feedback (Class)

| Name in Internal Data Model | Data Type                | XML File | Name in Moodle XML  | Data Type (XML) | Description                                                                                  |
|-----------------------------|--------------------------|----------|---------------------|-----------------|----------------------------------------------------------------------------------------------|
| GradeBoundary               | float                    | quiz.xml | mingrade & maxgrade | float           | the minimum grade (in percent) required to show this feedback                                |
| FeedbackText                | string                   | quiz.xml | feedbacktext        | string          | the text that is shown to the student if the grade is greater than or equal to GradeBoundary |
| OverallFeedbackFormat       | FormattingFormat (Enum)  | quiz.xml | feedbackformat      | int             | the format in which the feedback text is provided, e.g. plain text, HTML, Markdown           |

# Questions

## All Question Types

### Category
A question can only have one category associated with it. Which categories are available is stored in `questions.xml` in the root folder of the moodle backup.  
Each of these categories has a `question_bank_entries` tag that includes all questions that are part of this category.

Each of these questions is given a unique `id` which is used to reference the question in the quiz activity (`quiz.xml` file).

### Question Name

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                |
|-----------------------------|-----------|---------------|--------------------|-----------------|--------------------------------------------------------------------------------------------|
| QuestionName                | string    | questions.xml | name               | string          | the name of the question - the question will be shown using that name in the question bank |

### Question Text

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-----------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| QuestionText                | string    | questions.xml | questiontext       | string          | the text of the question                                                         |

#### Question Text Format

| Name in Internal Data Model | Data Type               | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-------------------------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| QuestionTextFormat          | FormattingFormat (Enum) | questions.xml | questiontextform   | int             | the format in which the question text is provided, e.g. plain text, HTML, Markdown |

### Question Status

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML)          | Description                                                             |
|-----------------------------|-----------|---------------|--------------------|--------------------------|-------------------------------------------------------------------------|
| QuestionReady               | bool      | questions.xml | status             | string (??? and `ready`) | whether the question is just a draft (false) or ready to be used (true) |

### Default Mark

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                        |
|-----------------------------|-----------|---------------|--------------------|-----------------|------------------------------------------------------------------------------------|
| DefaultMark                 | float     | questions.xml | defaultmark        | float           | the default mark for the question, if not specified otherwise in the quiz activity |

### General Feedback
General feedback is shown to the student after they have completed the question. Unlike specific feedback, which depends on the question type and what response the student gave, the same general feedback is shown to all students.  
This can be used to give students a fully worked answer and perhaps a link to more information they can use if they did not understand the question.

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                  |
|-----------------------------|-----------|---------------|--------------------|-----------------|------------------------------------------------------------------------------|
| GeneralFeedback             | string    | questions.xml | generalfeedback    | string          | the general feedback that is shown to the student after the question is done |

#### General Feedback Format

| Name in Internal Data Model | Data Type               | XML File      | Name in Moodle XML    | Data Type (XML) | Description                                                                            |
|-----------------------------|-------------------------|---------------|-----------------------|-----------------|----------------------------------------------------------------------------------------|
| GeneralFeedbackFormat       | FormattingFormat (Enum) | questions.xml | generalfeedbackformat | int             | the format in which the general feedback is provided, e.g. plain text, HTML, Markdown  |

### Question Type

| Name in Internal Data Model | Data Type           | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                |
|-----------------------------|---------------------|---------------|--------------------|-----------------|----------------------------------------------------------------------------|
| QuestionType                | QuestionType (Enum) | questions.xml | qtype              | string          | determines the type of the question, e.g. multiple choice, True/False, ... |

#### QuestionType

| Value in Internal Data Model | Value in Moodle XML | Description              |
|------------------------------|---------------------|--------------------------|
| MultipleChoice               | multichoice         | Multiple Choice question |

## Multiple Choice Questions specific

### Shuffle Answers

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| ShuffleAnswers              | bool      | questions.xml | shuffleanswers     | int (1/0)       | if enabled, the order of the answer is randomly shuffled for each attempt                     |

only works if the "Shuffle within questions" option is also enabled in the activity settings

### Choice Numbering

| Name in Internal Data Model | Data Type              | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                     |
|-----------------------------|------------------------|---------------|--------------------|-----------------|-------------------------------------------------------------------------------------------------|
| ChoiceNumbering             | NumberingFormat (Enum) | questions.xml | answernumbering    | string          | what format should be used to number the choices in the question, e.g. 1, 2, 3, 4 or A, B, C, D |

#### NumberingFormat

| Value in Internal Data Model | Value in Moodle XML | Description                                                    |
|------------------------------|---------------------|----------------------------------------------------------------|
| AlphabeticLower              | abc                 | Choices are numbered using letters, e.g. a, b, c, d            |
| AlphabeticUpper              | ABCD                | Choices are numbered using letters, e.g. A, B, C, D            |
| Numeric                      | 123                 | Choices are numbered using numbers, e.g. 1, 2, 3, 4            |
| RomanLower                   | iii                 | Choices are numbered using roman numerals, e.g. i, ii, iii, iv |
| RomanUpper                   | IIII                | Choices are numbered using roman numerals, e.g. I, II, III, IV |
| None                         | none                | Choices are not numbered at all                                |

### Show standard instructions

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML      | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|-------------------------|-----------------|-----------------------------------------------------------------------------------------------|
| ShowStandardInstructions    | bool      | questions.xml | showstandardinstruction | int (1/0)       | if enabled, the standard instructions for the question type are shown to the student          |

e.g. "Select one:" or "Select one or more:" for multiple choice questions

### Answers
Create a class Answer, then a question has a list of Answers

#### Answer

##### AnswerText

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| AnswerText                  | string    | questions.xml | answertext         | string          | the text of the answer                                                                       |

###### AnswerTextFormat

| Name in Internal Data Model | Data Type               | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-------------------------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| AnswerTextFormat            | FormattingFormat (Enum) | questions.xml | answerformat       | int             | the format in which the answer text is provided, e.g. plain text, HTML, Markdown |

##### AnswerGrade

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| AnswerGrade                 | float     | questions.xml | fraction           | float           | the grade that is given to the student if this answer is selected                            |

usually 1.0000000 for correct answers and 0.0000000 for incorrect answers, but can also be anything in between.  
Negative values down to -1.0000000 are also possible, e.g. to subtract points for incorrect answers.

##### AnswerFeedback

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| AnswerFeedback              | string    | questions.xml | feedback           | string          | the feedback that is shown to the student if this answer is selected                          |

###### AnswerFeedbackFormat

| Name in Internal Data Model | Data Type               | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-------------------------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| AnswerFeedbackFormat        | FormattingFormat (Enum) | questions.xml | feedbackformat     | int             | the format in which the feedback text is provided, e.g. plain text, HTML, Markdown |

### Single or Multiple Choice

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| SingleChoiceQuestion        | bool      | questions.xml | single             | int (1/0)       | if enabled, only one answer can be selected, otherwise multiple answers can be selected       |

### Specific Feedback

#### Correct Response Feedback

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| CorrectResponseFeedback     | string    | questions.xml | correctfeedback    | string          | the feedback that is given to the student if the student's response is correct               |

#### Correct Response Feedback Format

| Name in Internal Data Model    | Data Type               | XML File      | Name in Moodle XML    | Data Type (XML) | Description                                                                        |
|--------------------------------|-------------------------|---------------|-----------------------|-----------------|------------------------------------------------------------------------------------|
| CorrectResponseFeedbackFormat  | FormattingFormat (Enum) | questions.xml | correctfeedbackformat | int             | the format in which the feedback text is provided, e.g. plain text, HTML, Markdown |

#### Partially Correct Response Feedback

| Name in Internal Data Model      | Data Type | XML File      | Name in Moodle XML       | Data Type (XML) | Description                                                                              |
|----------------------------------|-----------|---------------|--------------------------|-----------------|------------------------------------------------------------------------------------------|
| PartiallyCorrectResponseFeedback | string    | questions.xml | partiallycorrectfeedback | string          | the feedback that is given to the student if the student's response is partially correct |

#### Partially Correct Response Feedback Format

| Name in Internal Data Model            | Data Type               | XML File      | Name in Moodle XML             | Data Type (XML) | Description                                                                        |
|----------------------------------------|-------------------------|---------------|--------------------------------|-----------------|------------------------------------------------------------------------------------|
| PartiallyCorrectResponseFeedbackFormat | FormattingFormat (Enum) | questions.xml | partiallycorrectfeedbackformat | int             | the format in which the feedback text is provided, e.g. plain text, HTML, Markdown |

#### Incorrect Response Feedback

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-----------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| IncorrectResponseFeedback   | string    | questions.xml | incorrectfeedback  | string          | the feedback that is given to the student if the student's response is incorrect |

#### Incorrect Response Feedback Format

| Name in Internal Data Model     | Data Type               | XML File      | Name in Moodle XML      | Data Type (XML) | Description                                                                        |
|---------------------------------|-------------------------|---------------|-------------------------|-----------------|------------------------------------------------------------------------------------|
| IncorrectResponseFeedbackFormat | FormattingFormat (Enum) | questions.xml | incorrectfeedbackformat | int             | the format in which the feedback text is provided, e.g. plain text, HTML, Markdown |

### Hints (Multiple Tries)
Create a class hint, then a question has a list of hints

#### Hint

##### HintText

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| HintText                    | string    | questions.xml | hint               | string          | the text of the hint                                                                        |

###### HintTextFormat

| Name in Internal Data Model | Data Type               | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                    |
|-----------------------------|-------------------------|---------------|--------------------|-----------------|--------------------------------------------------------------------------------|
| HintTextFormat              | FormattingFormat (Enum) | questions.xml | hintformat         | int             | the format in which the hint text is provided, e.g. plain text, HTML, Markdown |

##### Clear Incorrect Responses

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                      |
|-----------------------------|-----------|---------------|--------------------|-----------------|----------------------------------------------------------------------------------|
| ClearIncorrectResponses     | bool      | questions.xml | clearwrong         | int (1/0)       | if enabled, the student's incorrect responses are cleared when the hint is shown |

##### Show the number of correct answers

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                              |
|-----------------------------|-----------|---------------|--------------------|-----------------|------------------------------------------------------------------------------------------|
| ShowNoOfCorrectAnswers      | bool      | questions.xml | shownumcorrect     | int (1/0)       | if enabled, the number of correct answers is shown to the student when the hint is shown |

### Hint Penalty
When questions are run using the ‘Interactive with multiple tries’ or ‘Adaptive mode’ behaviour, so that the student will have several tries to get the question right, then this option controls how much they are penalised for each incorrect try.  
The penalty is a proportion of the total question grade, so if the question is worth three marks, and the penalty is 0.333333, then the student will score 3 if they get the question right first time, 2 if they get it right second try, and 1 if they get it right on the third try.  
For some multi-part questions this scoring logic is applied separately to each part of the question. The details depend on the question type and can be complicated, but the principle is to give students credit for the knowledge they have demonstrated as fairly as possible.

| Name in Internal Data Model | Data Type | XML File      | Name in Moodle XML | Data Type (XML) | Description                                                                                   |
|-----------------------------|-----------|---------------|--------------------|-----------------|-----------------------------------------------------------------------------------------------|
| HintPenalty                 | float     | questions.xml | penalty            | float           | the penalty that is applied to the student's grade for each incorrect try                     |

### Tags

## Additional Remarks
Moodle seems to have its own kind of versioning system for questions. Even when a question is edited, the old version is not deleted but stored in the database and also included in the XML file.  
The new question is just placed alongside the old question and the `version` tag is counted up by one.