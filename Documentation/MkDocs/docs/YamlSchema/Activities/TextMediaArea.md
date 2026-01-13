# Text and Media Area (Label)

Defines a Text and Media Area (formerly known as Label), used to display text or media directly on the course page.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `Content` | `List<YamlTextMediaAreaContent>` | A list of content items to display. These can have the Content inline or a file path. |
| `Format` | `TextMediaFieldFormat` | The format of the content. |

## Example
```YAML
Type: label
Name: What is Course as Code?
Format: markdown
Content:
  - Type: markdown
    ContentFilePath: ./content/what-is-cac.md
```