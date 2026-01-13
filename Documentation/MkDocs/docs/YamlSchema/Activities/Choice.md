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