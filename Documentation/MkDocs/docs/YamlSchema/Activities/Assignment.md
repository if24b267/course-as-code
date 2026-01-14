# Assignment

Defines a Moodle Assignment activity where students can submit work.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Intro` | `string` | Introduction text for the assignment. |
| `DueDate` | `DateTime` | The due date for the assignment. |
| `CutoffDate` | `DateTime` | The cutoff date for the assignment (submissions after this date are not allowed without extension). |
| `AllowSubmissionsFromDate`| `DateTime` | The date from which submissions are allowed. |
| `GradingDueDate` | `DateTime` | The date by which grading should be completed. |
| `AlwaysShowDescription` | `bool` | Whether to always show the description, even before the "Allow submissions from" date. |
| `AllowSubmissionDrafts` | `bool` | Whether students can save draft submissions. |
| `CompletionSubmit` | `bool` | Whether students must submit to complete this activity. |
| `AllowTeamSubmissions` | `bool` | Whether students submit in groups (teams). |
| `RequireAllTeamMembersSubmit`| `bool` | Whether all group members must submit (if team submission is enabled). |
| `MaxAttempts` | `int` | The maximum number of attempts allowed. |
| `BlindMarking` | `bool` | Whether student identities are hidden from graders. |
| `HideGrader` | `bool` | Whether the identity of the grader is hidden from students. |
| `MarkingWorkflow` | `bool` | Whether to use marking workflow (grading in stages). |
| `MarkingAllocation` | `bool` | Whether marking allocation (assigning graders to students) is enabled. |
| `SendNotifications` | `bool` | Whether to send notifications to graders when submissions are made. |
| `Grade` | `int` | The maximum grade for the assignment. |

## Example
```YAML
Type: ASSIGNMENT
Name: Document your findings so far!
Description: Write a short summary of all the things you have learned so far in this course. Which things stood out to you especially?

# Assignment Specific Settings
Intro: Write a short summary of all the things you have learned so far in this course. Which things stood out to you especially?
AllowSubmissionDrafts: true
SendNotifications: false
DueDate: 31-08-2025 23:59
AllowSubmissionsFromDate: 31-01-2023 00:00
Grade: 50
CompletionSubmit: true
RequireSubmissionStatement: true
MaxAttempts: 1
```