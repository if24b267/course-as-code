# Data Model
This page explains the **internal data model** used to store the data read by the [Reader](Reader.md). It is essentially an alternative way (alternative to [Moodle backup files](../MoodleBackupFile/General.md)) to save a Moodle course, along with all its associated parameters.

For better understanding, we will divide the data model into two sections:

- First, we'll look at the **basic structure of a Moodle course** - the fundamental elements that define a course, including metadata, sections, enrolled users, etc.
- Second, we'll examine the individual **activities that can be part of a course** in more detail.

!!! tip
    Almost every property in the source code of the models has a **Description Attribute** that explains what is stored in the property and how its values affect the Moodle course.

    !!! example

        ```csharp
        [Description("if set, the course will be displayed in this language despite the user's preferences")]
        public Language? CourseLanguage { get; set; }
        ```

## Basic Structure
- First off, there are several data fields that describe and define the Moodle course itself. For instance, it has a **name**, a **short name**, a **start date**, an **end date**, and much more.
- Since a Moodle course doesn't exist independently on the file system but can only be saved and imported as a Moodle Backup file (`.mbz`), some data about this backup file also needs to be stored, such as the **backup file's name**, the **Moodle version** it was created with, and so on.
- Users who participate in a course (whether as a student, lecturer, or administrator) are referred to as **enrolled** in the course. Information about each user participating in the course, such as their **name**, **email address**, etc., must be stored.
    - There are various ways a user on the Moodle learning platform can participate in a specific course. The main idea is that they must be added to the course somehow, and the way this happens is called the **Enrolment Method**.

        !!! example
            - `Manual Enrolment` means an administrator (or another authorized person) manually adds a person to the course.
            - `Self Enrolment` allows individuals to add themselves to the course (possibly by entering a password).
            - `Guest Enrolment` is a special form of participation where a course participant can view course content but cannot participate in activities.

    - Enrolment Methods are associated with both **users** and the **course**. A course supports certain Enrolment Methods (e.g., it allows or disallows self-enrolments), and for each user, the Enrolment Method through which they were added to the course is stored.
    - Additionally, each user is assigned a **role** that determines their permissions.

        !!! example
            Examples of roles include [`Student`](https://docs.moodle.org/500/en/Student_role), [`Teacher`](https://docs.moodle.org/500/en/Teacher_role), [`Administrator`](https://docs.moodle.org/500/en/Site_administrators), etc.

- Aside from this meta-information, the structure of a Moodle course is quite simple:  
It consists of multiple **Sections**, which in turn can contain any number of different **activities** (these will be explored in more detail in the next section).

### UML Diagram

```puml
@startuml

package Basic_Structure <<Frame>> {
    class Course {
        + FullName: string
        + ShortName: string
        + Summary: string 
        + StartDate: DateTime
        + EndDate: DateTime
        ...
    }
    
    class SensibleDefaultsCourse {
        + NumberOfAnnouncementsToDisplay: int
        + Format: enum
        + ShowGrades: bool
        + CourseIdNumber: int
        + MaximumUploadSize: int
        + VisibleToStudents: int
        ...
    }
    
    class SystemDefaultsCourse {
        + Id: int
        + SummaryFormat: int
        + UsesLegacyFiles: bool
        + TimeCreated: DateTime
        + TimeModified: DateTime
        ...
    }
   
    class Section {
        + Name: string
        + Summary: string
        + VisibleToStudents: bool
        ...
    }
    
    class BackupMetadata {
        + Name: string
        + MoodleVersion: enum
        + MoodleUrl: uri
        ...
    }
        
    class User {
        + Username: string
        + Email: MailAdress
        + FirstName: string
        + LastName: string
        + City: string
        + Country: enum
        ...
    }
    
    abstract class EnrolmentMethod {
        + DefaultRole: Role
        + Enabled: bool
    }
    
    class GuestEnrolment extends EnrolmentMethod {
        + Password: string
    }
    
    class ManualEnrolment extends EnrolmentMethod {
        + EnrolmentDuration: int
        + NotifyEnroller: bool
        + NotifyStudent: bool
        + WelcomeMessage: string
        ...
    }
    
    class SelfEnrolment extends EnrolmentMethod {
        + Password: string
        + EnrolmentDuration: int
        + NotifyEnroller: bool
        + NotifyStudent: bool
        + WelcomeMessage: string
        ...
    }
    
    class Role {
        + Name: string
        + ShortName: string
        + Description: string
        + Archetype: enum
        ...
    }
    
    abstract class Activity {
        + Name: string
        + Description: string
        + VisibleToStudents: bool
        + DisplayDescription: bool
        ...
    }
}

' SensibleDefaultsCourse and SystemDefaultsCourse are subclasses of Course (nested class)
Course +-- SensibleDefaultsCourse
Course +-- SystemDefaultsCourse

' The course (or rather the whole backup) is described by the backup metadata
Course "1" *-- "1" BackupMetadata : described by

' A user is assigned exactly one Role
User "0..*" *-- "1" Role : assigned

' A user was enrolled using an enrolment method
User "0..*" *-- "1" EnrolmentMethod : enrolled

' There are multiple roles available in a course
Course "1" *-- "0..*" Role : configures

' A course supports multiple enrolment methods
Course "1" *-- "0..*" EnrolmentMethod : supports

' A course has a number of enrolled users
Course "0..*" *-- "0..*" User : enrolled

' A course consists of multiple sections
Course "1" *-- "0..*" Section : contains

' A section consists of multiple activities
Section "1" *-- "0..*" Activity : contains
@enduml
```

This class diagram visually represents the basic structure of a Moodle course as explained above. To improve readability, the following simplifications have been made for this class diagram:

* **Not all properties are always listed per class:** If a class contains additional properties beyond those listed here, it's indicated by three dots "..." at the bottom of the property list.
* **This class diagram only shows the `SensibleDefaults` and `SystemDefaults` subclasses for the `Course` class.** Moodle stores numerous parameters for many entities: from various internal IDs to extremely specific configuration options that aren't typically needed. To make the model classes leaner, we've moved these properties into subclasses.
    * `SensibleDefaults` contains parameters for which a default value can be used in most cases, but where it's generally conceivable that a user might want to modify them under certain circumstances.
    * `SystemDefaults` contains parameters for which a default value can almost always be used, as it's highly unlikely that a user would want to modify them.

!!! info
    The `SensibleDefaults` and `SystemDefaults` subclasses exist for almost all classes. However, as mentioned above, they are only exemplified for the `Course` class in this class diagram to avoid overcomplicating the diagram.

## Activities

A course consists of any number of sections, which in turn can contain any number of activities.

```puml
@startuml

package Activities <<Frame>> {
    class Course {
        + FullName: string
        + ShortName: string
        + Summary: string 
        + StartDate: DateTime
        + EndDate: DateTime
        ...
    }

    class Section {
        + Name: string
        + Summary: string
        + VisibleToStudents: bool
        ...
    }

    abstract class Activity {
        + Name: string
        + Description: string
        + VisibleToStudents: bool
        + DisplayDescription: bool
        ...
    }
    
    class Assignment extends Activity {
        + Intro: string
        + DueDate: DateTime
        + CutOffDate: DateTime
        ...
    }
    
    class MoodleFile extends Activity {
        + FileName: string
        + FilePath: string
        ...
    }
    
    class TextAndMediaArea extends Activity {
        + Content: string
        ...
    }
    
    class URL extends Activity {
        + ExternalUrl: string
        ...
    }
    
    class Quiz extends Activity {
        + OpenTime: DateTime
        + CloseTime: DateTime
        + TimeLimit: int
        ...
    }

' A course consists of multiple sections
Course "1" *-- "0..*" Section : contains

' A section consists of multiple activities
Section "1" *-- "0..*" Activity : contains
@enduml
```

!!! info
    There are certain parameters that are required for all activities. These include, for example, the activity's name and its description, or parameters that determine whether the activity should be visible to students, in which language it should be displayed, etc. These parameters (those that are equally required for all activities) are found in the base class `Activity` - the remaining classes that extend the base class then only contain the parameters relevant to the respective activity.

 Below, we explain how the activities supported by **Course as Code** are stored in the data model:

### [Assignment](https://docs.moodle.org/500/en/Assignment_activity)
Assignments are a Moodle activity that allows students to submit files of any file type (can be restricted by the teacher) for grading.

!!! warning
    The documentation for Assignments is currently incomplete and/or incorrect and needs to be revised.

### [Moodle File](https://docs.moodle.org/500/en/File_resource)

The File activity refers to a file (of any type) that has been uploaded by the course creator (not the student!) and is available to students as learning material.

!!! info
    This activity is essentially just the shell that displays the file. The truly complex part is integrating the file into the Moodle backup and referencing it in the correct place. See the explicit documentation on File Handling for more on this! **TODO: Hyperlink**

Since this is a pretty simple activity, only a few parameters are necessary to integrate a file into Moodle:

- The file's **name** (`FileName`)
- The **path** to the file (`FilePath`)
- The file's **author** (`Author`) (_optional_)
- The file's **license** (`License`) (_optional_)

### [Text and Media Area](https://docs.moodle.org/500/en/Text_and_media_area)

The **Text and Media Area** (formerly: Label) can be used to embed formatted content (text, photos, multimedia, code, etc.) into a Moodle course, allowing for the design of nice-looking sections. The content can be formatted as `HTML`, `PlainText`, or `Markdown`.

Therefore, to define a Text and Media Area, only the content to be displayed, in the form of a **string** (`Content`), and the **format** in which the content is provided (`FormattingFormat`) are necessary.

!!! info
    `FormattingFormat` is an **enum** that can take the values `HTML`, `PlainText`, `Markdown`, and `MoodleAutoFormat`.

### [URL](https://docs.moodle.org/500/en/URL_resource)

The URL activity references an external resource that is stored outside of the Moodle course. In its case, only the specification of the **external link** (`ExternalUrl`) is necessary.

<hr>

!!! note
    The [Quiz activity](https://docs.moodle.org/500/en/Quiz_activity) is by far the most extensive activity supported by **Course as Code**, as a quiz can, in turn, contain various question types, all of which require their own structure within the data model. Therefore, a separate section in this documentation has been dedicated to it for better overview.

## [Quiz](https://docs.moodle.org/500/en/Quiz_activity)

```puml
@startuml
package Quizzes <<Frame>> {
    abstract class Activity {
        + Name: string
        + Description: string
        + VisibleToStudents: bool
        + DisplayDescription: bool
        ...
    }
    
    class Quiz extends Activity {
        + OpenTime: DateTime
        + CloseTime: DateTime
        + TimeLimit: int
        ...
    }
    
    class SafeExamBrowserSettings extends Quiz {
        + RequireSEB: enum
        + ShowDownloadButton: bool
        + EnableQuit: bool
        + EnableReload: bool
        ...
    }
    
    abstract class Question {
        + QuestionName: string
        + QuestionText: string
        + QuestionReady: bool
        + DefaultMark: float
        ...
    }
    
    class MultipleChoiceQuestion extends Question {
        + ShuffleAnswer: bool
        + ChoiceNumbering: enum
        + ShowStandardInstructions: bool
        + SingleChoice: bool
    }
    
    class SingleChoiceQuestion extends MultipleChoiceQuestion {
        + SingleChoice = true
    }
    
    class MultiChoiceAnswer {
        + AnswerText: string
        + AnswerGrade: string
        + AnswerFeedback: string
        ...
    }
    
    class DropdownQuestion extends Question {
        + SubQuestions: List<DropdownSubQuestion>
    }
    
    class DropdownSubQuestion {
        + QuestionText: string
        + CorrectAnswer: string
        + WrongAnswers: List<string>
    }
    
    class DragAndDropTextQuestion extends Question {
        + ShuffleAnswers: bool
    }
    
    class TextChoice {
        + ID: int
        + Value: string
        + Category: string
    }
    
    class CodeRunnerQuestion extends Question {
        + CodeRunnerType: string
        + AnswerBoxLines: string
        + AnswerBoxColumns: string
        + Answer: string
        ...
    }
    
    class CodeRunnerTestCase {
        + TestCode: string
        + Expected: string
        ...
    }
}

' A quiz contains multiple questions
Question "0..*" --o "0..*" Quiz : contains

MultipleChoiceQuestion "1..*" *-- "1..*" MultiChoiceAnswer : contains
DropdownQuestion "1" *-- "1..*" DropdownSubQuestion : contains
DragAndDropTextQuestion "1" *-- "1..*" TextChoice : contains
CodeRunnerQuestion "1" *-- "1..*" CodeRunnerTestCase : contains

@enduml
```

As with all other activities discussed so far, the Quiz is also an activity that extends the abstract `Activity` class. A quiz broadly consists of two elements:

- The **Quiz activity itself** and parameters that describe it in more detail (e.g., `OpenTime`, `CloseTime`, `GradeToPass`, `AllowedAttempts`, etc.)
- A series of **questions of various types**

The parameters that define the quiz's metadata are the properties of the `Quiz` class (refer to the **Description Attributes** in the Model class for a precise description of all available parameters).

Furthermore, every quiz is associated with `SafeExamBrowserSettings`. The [Safe Exam Browser (SEB)](https://safeexambrowser.org/about_overview_en.html) is a specialized browser that blocks access to many OS functions and other websites to make cheating more difficult for students. For each quiz, it can be configured whether SEB is required to conduct the quiz and how SEB should be configured for that specific quiz - this information is stored in the `SafeExamBrowserSettings` class.

The core of every quiz are the quiz questions that students must answer. A quiz can contain any number of questions of any type, and a specific question can also appear in multiple quizzes.

### Question Types

!!! info
    There are certain parameters that are required for all question types. These include, for example, their **name** (for internal representation in the QuestionBank), their **text** (i.e., the question itself - all questions have a question statement), or general parameters, such as whether the question is a *draft* or a finished question. These parameters are stored in the abstract parent class `Question`, the subclasses then implement only those parameters that are necessary for that particular type of question.

#### [MultipleChoiceQuestion](https://docs.moodle.org/500/en/Multiple_Choice_question_type)
Multiple Choice Questions are the most common and frequently used type of question in Moodle. They consist of a question statement (`QuestionText` in the base class) and a set of possible answer options (`MultiChoiceAnswer`).

For each `MultiChoiceAnswer`, the `AnswerText` can be defined, along with an `AnswerGrade` that determines the value of that answer.

!!! example
    To create a multiple-choice question with only one correct answer, the `AnswerGrade`s must be set as follows:

    - `Answer Text` = Correct Answer | `AnswerGrade` = 1
    - `Answer Text` = Incorrect Answer | `AnswerGrade` = 0
    - `Answer Text` = Incorrect Answer | `AnswerGrade` = 0

    In this case, the student would receive full marks for selecting the correct answer, but also if they blindly check all answers. To prevent this, a **negative** `AnswerGrade` must be assigned to the incorrect answer options (so that their selection leads to deductions).

    - `Answer Text` = Correct Answer | `AnswerGrade` = 1
    - `Answer Text` = Incorrect Answer | `AnswerGrade` = -1/2
    - `Answer Text` = Incorrect Answer | `AnswerGrade` = -1/2

!!! warning annotate
    It is absolutely necessary that the **positive** `AnswerGrade`s sum up to exactly `1` - otherwise, the question will not be recognized correctly by Moodle. For example, if there are 3 correct and 1 incorrect answer option, this must be defined as follows:

    - `Answer Text` = Correct Answer | `AnswerGrade` = 1/3
    - `Answer Text` = Correct Answer | `AnswerGrade` = 1/3
    - `Answer Text` = Correct Answer | `AnswerGrade` = 1/3
    - `Answer Text` = Incorrect Answer | `AnswerGrade` = -1/3 (1)

    Note that for the positive answers, an `AnswerGrade` in the form of a fraction (`⅓`) was chosen instead of a decimal (e.g., `0.33`). This is absolutely necessary for irrational numbers so that Moodle sums them up correctly. (Internally, the fractions are of course stored by C# as floats and thus again as decimals - however, the precision of a float is sufficient to make it clear to Moodle that it is a fraction.)

1. Negative `AnswerGrade`s do not have to sum up to 1; any value can be chosen.  
**On the contrary**: Negative `AnswerGrade`s can even be values greater than 1, so that the user receives overall point deductions if they answer the question incorrectly.

##### [SingleChoiceQuestion](https://docs.moodle.org/500/en/Multiple_Choice_question_type#Single-answer_questions)
The Single Choice Question is a special form of the Multiple Choice Question, where only one of the given answers is correct (in contrast to Multiple Choice Questions where one **or more** answers can be correct). A question is marked as a Single Choice Question by setting the `SingleChoice` Boolean in the `MultipleChoiceQuestion` class to `true` and setting the `AnswerGrade` parameters accordingly.

!!! info
    If the `AnswerGrade`s are set such that only a single answer is correct, but the `SingleChoice` Boolean is left as `false`, only one answer to the question is correct, but the student can still select multiple answer options. If `SingleChoice` is set to `true`, the student can only select **one** answer when taking the quiz and not multiple.

<hr>

#### [DropdownQuestion](https://docs.moodle.org/500/en/Matching_question_type)
The Dropdown question is internally referred to as a `Matching Question` in Moodle and, strictly speaking, consists of multiple sub-questions. The `AnswerText` of the parent `Question` class is displayed at the top as a *heading*, while the actual question consists of several sub-questions, namely the assignment of the correct selectable text to the given text.

![Image of a dropdown question in Moodle to give you a better idea](../assets/DropDownQuestion.png)

The `DropdownQuestion` type therefore consists of several `DropdownSubQuestion`s, which represent the individual interaction possibilities for the user (in this case `Java`, `C`, `Prolog`, and `Haskell`). For each sub-question, the `QuestionText` and the `CorrectAnswer` are defined. To make answering the question more challenging, a series of `WrongAnswers` can also be specified.

!!! bug
    The internal data model does not clearly represent how Dropdown Questions (or rather, Matching Questions) function in Moodle.  
    Fundamentally, the question was designed as an assignment task. This means that every correct answer to a sub-question is also available as an answer option for all other sub-questions, but it is only correct for the one where it was specified as the `CorrectAnswer`.

    !!! example
        SubQuestion1: `CorrectAnswer` = "Miami"  
        SubQuestion2: `CorrectAnswer` = "Florida"  
        SubQuestion3: `CorrectAnswer` = "United States"

        Dropdown Menu for SubQuestion1: "Miami, Florida, United States"  
        Dropdown Menu for SubQuestion2: "Miami, Florida, United States"  
        Dropdown Menu for SubQuestion3: "Miami, Florida, United States"

    If additional incorrect answer options are provided, they are also available as answer options for **all** sub-questions, not just for the sub-question where they were listed in the `WrongAnswer` list.

    !!! example
        SubQuestion1: `CorrectAnswer` = "Miami", `WrongAnswer` = "Florida, Kansas"  
        SubQuestion2: `CorrectAnswer` = "Washington", `WrongAnswer` = "Miami, United States"

        Dropdown Menu for SubQuestion1: "Miami, Florida, Kansas, Washington, United States"  
        Dropdown Menu for SubQuestion2: "Miami, Florida, Kansas, Washington, United States"

<hr>

#### [DragAndDropTextQuestion](https://docs.moodle.org/500/en/Drag_and_drop_into_text_question_type)
The `DragAndDropTextQuestion` format refers to classic fill-in-the-blank questions where the user has to insert the correct word from a set of predefined words into the corresponding gap.

![Image of a cloze text question in Moodle to give you a better idea](../assets/DragAndDropTextQuestion.png)

This question type consists of a `QuestionText` (in the parent class `Question`) that can contain gaps, and multiple `TextChoice` elements, each assigned an `ID` and a `Value` (its content), which can be inserted into the cloze text.

The `QuestionText` should be formulated as continuous text. Gaps can be defined using `[[1]]`, where `1` is a sequential number for each gap and references the `ID` field of the `TextChoice` elements. The `TextChoice` element whose `ID` matches the number in the gap placeholder is considered the correct answer for that text position. `TextChoice` elements with an `ID` that is not referenced anywhere in the cloze text are not considered correct answers anywhere and act as distractors.

!!! example
    The question from the example screenshot would be represented in the data model as follows:

    `QuestionText` = "In Linux, file permissions are controlled by three sets of access rights: [[3]], [[4]], and [[5]]. To view the permissions of a file, you can use the [[1]] command. Permissions are typically displayed as a string like `rwxr-xr--`, where each letter represents a different [[6]]."

    `TextChoice` elements:

    - `ID` = 1, `Value` = "ls -l"
    - `ID` = 2, `Value` = "chmod"
    - `ID` = 3, `Value` = "owner"
    - `ID` = 4, `Value` = "group"
    - `ID` = 5, `Value` = "other"
    - `ID` = 6, `Value` = "permission"
    - `ID` = 7, `Value` = "cd"
    - `ID` = 8, `Value` = "shutdown"
    - `ID` = 9, `Value` = "cat"
    - `ID` = 10, `Value` = "nano"

A `TextChoice` can also be assigned to a specific `Category` to specify distractors individually per gap.

![A different cloze text question in Moodle, making use of categories, to give you a better idea](../assets/DragAndDropTextQuestionWithCategory.png)

!!! example
    The question from the example screenshot would be represented in the data model as follows:

    `QuestionText` = "The [[1]] is a type of system software that manages [[5]] and software resources, and provides common services for computer programs"

    `TextChoice` elements:

    * `ID` = 1, `Value` = "Operating System", `Category` = "A"
    * `ID` = 2, `Value` = "Compiler", `Category` = "A"
    * `ID` = 3, `Value` = "Microsoft Word", `Category` = "A"
    * `ID` = 4, `Value` = "Printer", `Category` = "A"
    * `ID` = 5, `Value` = "Hardware", `Category` = "B"
    * `ID` = 6, `Value` = "Internet", `Category` = "B"
    * `ID` = 7, `Value` = "Printing", `Category` = "B"

<hr>

#### [CodeRunnerQuestion](https://docs.moodle.org/500/en/CodeRunner_question_type)

!!! warning
    The documentation for CodeRunnerQuestions is currently incomplete and/or incorrect and needs to be revised.