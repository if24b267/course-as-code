# Activities Overview

Activities are the interactive elements of a course. Each activity is defined in its own YAML file. All activities share a common set of base properties.

## Common Base Properties

These properties are available for all activity types.

| Property | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `Type` | `ActivityType` | **Yes** | The type of the activity (e.g., `QUIZ`, `ASSIGNMENT`, `URL`). |
| `Name` | `string` | **Yes** | The name of the activity as displayed to the user. |
| `Description` | `string` | No | The description of the activity. |
| `DescriptionFormat` | `FormattingFormat` | No | The format of the description (e.g., `HTML`, `Markdown`, `MoodleAutoFormat`). Default is `MoodleAutoFormat`. |

## Available Activities

* [Assignment](Assignment.md)
* [Choice](Choice.md)
* [Feedback](Feedback.md)
* [File](File.md)
* [Folder](Folder.md)
* [Page](Page.md)
* [Quiz](Quiz.md)
* [Text and Media Area (Label)](TextMediaArea.md)
* [URL](Url.md)
* [Wiki](Wiki.md)