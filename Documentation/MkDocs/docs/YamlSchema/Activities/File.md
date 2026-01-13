# File

Defines a file resource to be provided to students.

## Properties

| Property | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `FilePath` | `string` | **Yes** | The local path to the file that should be included in the course. |

## Example
```YAML
Type: FILE
Name: Moodle Backup File (.mbz) of this sample course
FilePath: ./files/backup-moodle2-course-0-CaCIntro-20250523-1012.mbz
```