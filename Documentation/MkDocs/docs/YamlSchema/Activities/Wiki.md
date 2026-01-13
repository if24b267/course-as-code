# Wiki

Defines a Wiki activity for collaborative editing.

## Properties

| Property | Type | Description |
| :--- | :--- | :--- |
| `FirstPageTitle` | `string` | Title of the wiki's first page. |
| `WikiMode` | `WikiMode` | Collaboration mode: `Collaborative` or `Individual`. |
| `DefaultFormat` | `WikiFormat` | Default format for content (`Html`, `Creole`, `Nwiki`). |
| `ForceFormat` | `bool` | Whether to force the default format. |
| `Subwikis` | `List<YamlWikiSubwiki>` | Content of the wiki (subwikis and pages). |

## Example
```YAML
Name: Cool Wiki
Type: WIKI
Description: This is a wiki activity.
Intro: This is a cool intro
IntroFormat: HTML
FirstPageTitle: This is the first page title
WikiMode: Collaborative
WikiFormat: HTML
ForceFormat: false
Subwikis:
  - PagePaths:
      - ./wikiPages/wikiPage1.yaml
      - ./wikiPages/wikiPage2.yaml
  - PagePaths:
      - ./wikiPages/wikiPage3.yaml
```

```YAML
Title: Wiki Page 1 Title
CachedContent: This is the cached content for Wiki Page 1.
ReadOnly: false
```