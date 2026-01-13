# Course Configuration (`config.yaml`)

The `config.yaml` file defines the general settings and metadata for the entire course. It corresponds to the `YamlCourse` model.

## Properties

| Property | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `CourseName` | `string` | **Yes** | The full name of the course. |
| `ShortName` | `string` | No | The short name of the course. |
| `Summary` | `string` | No | A summary of the course. |
| `Description` | `string` | No | A description of the course. |
| `Visible` | `bool` | No | Whether the course is visible to students. |
| `Category` | `string` | No | The name of the category the course belongs to. |
| `CategoryId` | `int` | No | The ID of the category the course belongs to. |
| `CategoryDescription` | `string` | No | Description of the course category. |
| `StartDate` | `DateTime` | No | The date the course starts. |
| `EndDate` | `DateTime` | No | The date the course ends. |
| `IdNumber` | `int` | No | An optional ID number for the course (often used for external system integrations). |
| `ShowGrades` | `bool` | No | Whether to allow students to view a list of all their grades in the course. |
| `GroupMode` | `GroupMode` | No | The default group mode for the course (e.g., `None`, `SeparateGroups`, `VisibleGroups`). |
| `ForceGroupMode` | `bool` | No | If true, the group mode is forced for all activities and cannot be changed individually. |
| `EnableCompletionTracking`| `bool` | No | If enabled, students can track their progress through the course. |
| `NumberOfAnnouncements` | `int` | No | How many recent announcements are displayed in the "latest announcements" block. |
| `ForcedLanguage` | `string` | No | If set, the course will be displayed in this language (e.g., 'en', 'de') despite the user's preferences. |
| `GradeCategoriesPath` | `string` | No | Path to the YAML file defining grade categories. |
| `GradingInfosPath` | `string` | No | Path to the YAML file defining grading information. |
| `ShowActivityDates` | `bool` | No | Whether to show activity dates on the course page. |
| `ShowCompletionConditions`| `bool` | No | If enabled, conditions for activity completion are shown on the course page. |
| `QuestionListPaths` | `List<string>`| No | A list of paths to YAML files containing questions for the question bank. |