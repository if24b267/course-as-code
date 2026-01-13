# Page

Defines a Page resource (HTML content).

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Intro` | `string` | Intro text for the page. |
| `IntroFormat` | `FormattingFormat` | Format of the intro (e.g., HTML, Markdown). |
| `Content` | `string` | The main HTML/Markdown content of the page. |
| `ContentFormat` | `FormattingFormat` | Format of the content (e.g., HTML, Markdown). |

## Example
```YAML
Name: "Example Page: Welcome"
Type: PAGE
Intro: "<p>This is a short intro shown on the course page.</p>"
IntroFormat: HTML
Content: "<h2>Welcome to the Example Page</h2><p>This is the main content of the page.</p>"
ContentFormat: HTML
```