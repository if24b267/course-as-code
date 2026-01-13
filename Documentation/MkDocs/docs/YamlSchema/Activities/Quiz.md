# Quiz

Defines a Quiz activity.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `OpeningTime` | `DateTime` | Students can only start the quiz after this timestamp. |
| `ClosingTime` | `DateTime` | Students must finish the quiz before this timestamp. |
| `TimeLimit` | `int` | Overall time limit in seconds (0 = no time limit). |
| `AllowedAttempts` | `int` | Number of allowed attempts (0 = unlimited). |
| `GradeToPass` | `float` | Minimum points required to pass. |
| `Password` | `string` | Password required to attempt the quiz. |
| `ShuffleQuestions` | `bool` | Whether to shuffle the order of questions. |
| `QuestionIds` | `int[]` | A list of IDs of the questions to include in this quiz. |
| `QuestionBehaviour`| `QuestionBehaviour` | Defines how students interact with questions (e.g., `DeferredFeedback`, `ImmediateFeedback`). |

## Example
```YAML
Name: Graded test about quizzes
Type: QUIZ
Description: Test your knowledge about the Quiz Activity!
DescriptionFormat: MARKDOWN
OpeningTime: 31-01-2023 00:00
ClosingTime: 31-12-2025 23:59
TimeLimit: 600
TimeExpiryHandling: AutoSubmit
AllowedAttempts: 1
GradeToPass: 2
BuildOnPreviousAttempt: false
GradingMethod: HighestGrade
QuestionBehaviour: Deferred
ShuffleQuestions: true
AllowRedosDuringAttempt: false
QuestionIds: [1, 2, 3, 4, 5, 6, 7, 8, 9]
```