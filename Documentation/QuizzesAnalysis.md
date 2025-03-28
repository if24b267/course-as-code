# Basic parameters that can be set for a quiz

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

Corresponding tags are `reviewattempt`, `reviewcorrectness`, `reviewmaxmarks`, `reviewmarks`, `reviewspecificfeedback`, `reviewgeneralfeedback`, `reviewrightanswer` and `reviewoverallfeedback`.  
Information seems to be stored in a numeric code, similar to file permissions in Unix.

`4368` -> parameter enabled at all times  
`272` -> parameter enabled for AfterAttempt and QuizClosed  
`4112` -> parameter enabled for ImmediatelyAfter and QuizClosed  
`4096` -> parameter enabled only for ImmediatelyAfter  
`4368` -> parameter enabled for ImmediatelyAfter, AfterAttempt and QuizClosed  
`0` -> parameter disabled for all states  

//I'm not sure if these values are even consistent  
// TODO: Further research needed

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
| OverallFeedback                |                     | whether the sudent can see the feedback given at the end of the attempt, depending on the sutdent's total mark                                                             |


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

| Name in Internal Data Model | Data Type      | XML File | Name in Moodle XML     | Data Type (XML) | Description                                                                                                     |
|-----------------------------|----------------|----------|------------------------|-----------------|-----------------------------------------------------------------------------------------------------------------|
| RequireSEB                  | SEBMode (Enum) | quiz.xml | questiondecimalpoints  | int (-1 - 7)    | specifies the number of digits shown after the decimal point when displaying the marks for individual questions |

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
