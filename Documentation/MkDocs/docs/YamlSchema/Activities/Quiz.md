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