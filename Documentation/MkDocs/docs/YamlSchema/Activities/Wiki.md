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