# Feedback

Defines a Feedback activity for gathering data.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Anonymous` | `int` | Whether feedback is anonymous (1) or not (0). |
| `MultipleSubmit` | `int` | Whether multiple submissions are allowed. |
| `EmailNotification` | `int` | Whether to send email notifications for submissions. |
| `Items` | `List<YamlFeedbackItem>`| The questions/items in the feedback form. |

## Example
```YAML
Name: Example Feedback
Type: FEEDBACK
Description: "<p>Please fill out this self-assessment.</p>"
DescriptionFormat: HTML
Anonymous: 2
EmailNotification: 0
MultipleSubmit: 0
Autonumbering: 0
PublishStats: 1
CompletionSubmit: false
Items:
  - Template: 0
    Name: label
    Type: label
    Presentation: "<p>Legend: (1) low … (4) high</p>"
    HasValue: 0
    Position: 1
    Required: 0
    DependItem: 0
  - Template: 0
    Name: I can describe the basics
    Type: multichoice
    Presentation: "r>>>>>1|2|3|4<<<<<1"
    HasValue: 1
    Position: 2
    Required: 0
    DependItem: 0
    Options: h
```