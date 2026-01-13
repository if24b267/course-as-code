# Feedback

Defines a Feedback activity for gathering data.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Anonymous` | `int` | Whether feedback is anonymous (1) or not (0). |
| `MultipleSubmit` | `int` | Whether multiple submissions are allowed. |
| `EmailNotification` | `int` | Whether to send email notifications for submissions. |
| `Items` | `List<YamlFeedbackItem>`| The questions/items in the feedback form. |