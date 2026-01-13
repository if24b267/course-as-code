# Choice

Defines a Choice activity (polling).

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Publish` | `bool` | Whether to publish results to students. |
| `ShowResults` | `bool` | How/When to show results to students. |
| `AllowUpdate` | `bool` | Allow students to change their answer. |
| `AllowMultiple` | `bool` | Allow students to select more than one option. |
| `LimitAnswers` | `bool` | Limit the number of responses allowed for each option. |
| `TimeOpen` | `DateTime` | Time when the choice opens. |
| `TimeClose` | `DateTime` | Time when the choice closes. |
| `Options` | `List<YamlChoiceOption>` | A list of options students can choose from. |

## Examples
```YAML
Name: Example Choice 1
Type: CHOICE
Description: "<p>This is an example choice activity.</p>"
DescriptionFormat: HTML
Publish: false
ShowResults: false
Display: 0
AllowUpdate: false
AllowMultiple: false
ShowUnanswered: true
LimitAnswers: false
TimeOpen: 01-01-2026 00:00
TimeClose: 31-12-2026 00:00
CompletionSubmit: false
ShowPreview: false
IncludeInactive: false
ShowAvailable: false
Options:
  - Text: "Option 1"
    MaxAnswers: 0
  - Text: "Option 2"
    MaxAnswers: 0
  - Text: "Option 3"
    MaxAnswers: 0
```