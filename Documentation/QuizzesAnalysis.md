# Basic parameters that can be set for a quiz

## Name
The activity name

## Description
The activity descriptionh

### Display Description on course page
Boolean
if enabled, the description will be displayed on the course page just below the link to the quiz

## Quiz Opening Time
DateTime
when the quiz opens

Students can only start the quiz after the open time has passed

## Quiz Closing Time
DateTime
when the quiz closes

Students must end the quiz before the closing time has passed

## Time Limit

if enabled, the time limit is stated on the initial quiz page and a countdown timer is displayed in the quiz navigation block

## Time Expiry Handling
Enum

this variable controls what happens if a student fails to submit their quiz attempt before the time expires.
If the student is actively working on the quiz at the time, then the countdown timer will always automatically submit the attempt for them but if they have logged out, 
then this setting controls what happens

Open attempts are submitted automatically
There is a grace period when open attempts can be submitted, but no more questions anwered
    -> amount of extra time allowed needs to be defined ("Submission grace period")
Attempts must be submitted before time expires, or they are not counted

## GradeToPass
int (0-10)

determines the minimum grade required to pass. The value is used in activity and course completion and in the gradebook, where pass grades are highlighted in green and fail grades in red

## Attempts allowed
the number of allowed attempts
0 - 10 or unlimited

## Grading method
Enum

When multiple attempts are allowed, the following methods are available for calculating the final quiz grade:
- Highest Grade of all attempts
- Average (mean) grade of all attempts
- First attempt (all other attempts are ignored)
- Last attempt (all other attempts are ignored)

## New Page Threshhold
int
For longer quizzes it makes sense to stretch the quiz over several pages by limiting the number of questions per page. When adding questions to the quiz, page breaks will automatically be inserted according to this setting. However page breaks may later be moved manually on editing page.

## Shuffle within questions
bool

if enabled, the parts making up each questions will be randomly shuffled each time a student attempts the quiz, provided the option is also enabled in the questions settings. This setting only applies to questions that have multiple parts, such as multiple choice or matching questions.

## Question Behaviour
Enum

Students can interact with the questions in the quiz in various different ways. For example, you may wish the students to enter an answer to each questions and then submit the entire quiz, before anything is graded or they get any feedback. That would be "Deferred feedback" mode.

Alternatively, you may wish for students to submit each questions as they go along to get immediate feedback and if they do not get it right immediately, have another try for fewer marks. That would be "Interactive with multiple tries" mode.

| Value in Internal Data Model  | Value in Moodle XML | Description |
|------------------------------|---------------------|-------------|
| Adaptive                     |                     | Immediate Feedback, but allows students to have multiple attempts at the question before moving on to the next question. Students get fewer marks on subsequent attempts. |
| AdaptiveNoPenalties          |                     | Immediate Feedback, but allows students to have multiple attempts at the question before moving on to the next question. No penalties for subsequent attempts. |
| Deferred                     |                     | Students must enter an answer to each question and then submit the entire quiz before anything is graded or they get any feedback. |
| DeferredCBM                  |                     | Certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. The grading is adjusted by the choice of certainty. |
| Immediate                    |                     | Students can submit their response immediately during the quiz attempt and get it graded. However, they can only submit one response and cannot change it later. |
| ImmediateCBM                 |                     | Certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. The grading is adjusted by the choice of certainty. |
| InteractiveMultipleTries     |                     | Allows multiple attempts on a question with possible grade penalties. Students submit answers via the 'Check' button and can retry using 'Try again' if incorrect. After each wrong attempt, a hint is displayed. Once correct, answers cannot be changed. If attempts run out, the student is graded accordingly and shown feedback. The number of tries equals the number of hints plus one (minimum of three). |

## AttemptBuildsOnPreviousAttempts
boolean

if multiple attempts are allowed and this settings is enabled, each new quiz attempt will contain the results of the previous attempt.
This allows a quiz to be completed over several attempts.

## Review Options
These options control what information students can see when they review a quiz attempt or look at the quiz reports.

### Time
| Value in Internal Data Model  | Value in Moodle XML | Description |
|------------------------------|---------------------|-------------|
| DuringAttempt                     |                     | only applicable to some question behaviours like InteractiveMultipleTries. Displays feedback during the attempt |
| ImmediatelyAfter          |                     | student can see their grade immediately after clicking on "Submit all and finish" |
| AfterAttempt                     |                     | student can see their grade two minutes after they clicked on "Submit all and finish" |
| QuizClosed                  |                     | student can see their grade after the quiz close date has passed. If the quiz doesn't have a close date, this state is never reached |

### Parameters
each of these values is a boolean

| Value in Internal Data Model  | Value in Moodle XML | Description |
|------------------------------|---------------------|-------------|
| Attempt                     |                     | whether the student can review the attempt at all |
| WhetherCorrect          |                     | whether the student can see if he answered the question "correct", "partially correct" or "incorrect". Also displays the colour highlighting that conveys this information |
| MaximumMarks                     |                     | whether the student can see the maximum mark he could have achieved for each question |
| Marks                  |                     | whether the student can see the mark obtained for each question and the overall attempt score - only available if MaximumMarks is true |
| SpecificFeedback                     |                     | whether the student can see feedback that depends on what response the student gave |
| GeneralFeedback          |                     | whether the student can see general feedback, displayed to all students regardless of their answer. E.g. a fully worked answer and a link to more information |
| RightAnswer                     |                     | whether the student can see what would have been the correct answer(s) |
| OverallFeedback                  |                     | whether the sudent can see the feedback given at the end of the attempt, depending on the sutdent's total mark |


## ShowPicture
Enum

if enabled, a student's name and picture will be shown on-screen during the attempt and on the review screen, making it easier to check that the sutdent is logged in as themselves

| Value in Internal Data Model  | Value in Moodle XML |
|------------------------------|---------------------|
| NoImage                     |                     |
| SmallImage          |                     |
| LargeImage                     |                     |

## GradeDecimalsToDisplay
int (0-5)

specifies the number of digits shown after the decimal point when displaying grades.
Only affects the display of grades, not the grades stored in the database or the internal calculations


## MarkDecimalsToDisplay
int (0-7)

specifies the number of digits shown after the decimal point when displaying the marks for individual questions

## SafeExamBrowser
TODO: Seems like A LOT of work

## Password
string

if a password is specified, a student must enter it in order to attempt the quiz

## Feedback
Overall Feedback is text that is shown after a quiz has been attempted (Overall Feedback)
By specifying additional grade boundaries (as percentage), the text shown can depend on the grade obtained (Specific Feedback)

### OverallFeedback
eigene Klasse dafür anlegen?

- FeedbackText
- OverallFeedbackFormat
    - FormattingFormatt Enum

### SpecificFeedback
eigene Klasse dafür anlegen?

- GradeBoundary 
- FeedbackText
- OverallFeedbackFormat
    - FormattingFormatt Enum´

