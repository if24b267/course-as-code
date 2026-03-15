# VS Code Extension v2 — Design Spec

**Date:** 2026-03-15
**Status:** Approved

---

## Overview

Redesign and expand the CourseAsCode VS Code extension to provide a structured, guided authoring experience. The extension scaffolds the correct Moodle-compatible file/folder structure and validates it, removing the need for users to know the expected layout by heart.

---

## Commands

Three commands are introduced. All are available via the Command Palette (Ctrl+Shift+P). Only "Init Course" is also available in the Explorer right-click context menu.

| Command ID | Label | Command Palette | Explorer Right-Click |
|---|---|---|---|
| `courseascodetools.initCourse` | Init Course | Yes | Yes |
| `courseascodetools.updateSections` | Update Sections | Yes | No |
| `courseascodetools.createActivity` | Create Activity | Yes | No |

"Create Activity" is only enabled when at least one `Sections/*/` directory exists on disk. The extension sets a context variable (`courseascodetools.hasSections`) by watching the filesystem, which drives the `when` clause in `package.json`.

---

## Command: Init Course

**Purpose:** Scaffold a new course from scratch.

**Trigger:** Command palette or right-click on a folder in the Explorer.

**Flow:**
1. If triggered from right-click — use the selected folder as root. If from command palette — open a folder picker dialog.
2. If any of `config.yaml`, `sections.yaml`, or `Sections/` already exist at the root — show an error notification and abort. Never overwrite an existing course.
3. Generate the following structure:

```
<root>/
├── config.yaml
├── sections.yaml
└── Sections/
    └── Introduction/
        └── section.yaml
```

**`config.yaml`:**
```yaml
CourseName: My Course
ShortName: MC
CourseIdNumber: ""
Summary: ""
```

**`sections.yaml`:**
```yaml
Sections:
  - SectionPath: Sections/Introduction
```

**`Sections/Introduction/section.yaml`:**
```yaml
SectionName: Introduction
```

---

## Command: Update Sections

**Purpose:** Validate that the declared sections in `sections.yaml` exist on disk.

**Trigger:** Command palette only.

**Flow:**
1. Find `sections.yaml` at the workspace root.
2. For each `SectionPath` entry, check:
   - Does the directory exist?
   - Does `section.yaml` exist inside it?
3. Report each missing item as a VS Code warning notification, e.g.:
   - `Missing directory: Sections/Advanced`
   - `Missing section.yaml in: Sections/Introduction`
4. If all items are valid — show a single success notification: `All sections are valid.`

No files are created or modified. This command is a read-only validator.

---

## Command: Create Activity

**Purpose:** Scaffold a new activity YAML file in the correct section, registered in `section.yaml`.

**Trigger:** Command palette only (Ctrl+Shift+P). Enabled only when at least one `Sections/*/` directory exists.

**Flow:**
1. **Section picker** — quick-pick dropdown listing all `Sections/*/` directories found on disk.
2. **Activity type picker** — quick-pick dropdown:

| Label | File prefix |
|---|---|
| Forum | `Forum_` |
| Page | `Page_` |
| Quiz | `Quiz_` |
| Assignment | `Assignment_` |

3. **Name input** — free text input box for the activity name (e.g. `MyForum`).
4. Generate:
   - `Sections/<section>/Activities/` directory (if not present)
   - `Sections/<section>/Activities/<Type>_<name>.yaml` pre-populated with the type's YAML template
   - Add `- ActivityFilePath: Activities/<Type>_<name>.yaml` to `Sections/<section>/section.yaml` under an `Activities:` block (creates the block if not present)

**Resulting `section.yaml` after adding an activity:**
```yaml
SectionName: Introduction
Activities:
  - ActivityFilePath: Activities/Forum_MyForum.yaml
```

### Activity YAML Templates

Each type has a pre-populated template. Templates must reflect the fields expected by the CourseAsCode reader.

**Forum (`Forum_<name>.yaml`):** Fields from `YamlForum` model — ForumType, SubscriptionMode, TrackingType, MaxAttachments, completion fields, etc.

**Page (`Page_<name>.yaml`):** Fields from `YamlPage` model — Intro, Content, format enums.

**Quiz (`Quiz_<name>.yaml`):** Fields from `YamlQuiz` model.

**Assignment (`Assignment_<name>.yaml`):** Fields from `YamlAssignment` model.

---

## IntelliSense: sections.yaml Snippet

A VS Code snippet scoped to `sections.yaml` files. Typing `section` + Tab expands to:

```yaml
  - SectionPath: Sections/
```

Cursor is placed at the end of the line so the user types the section name immediately.

Schema-based autocomplete for activity YAML files continues to work via the existing `redhat.vscode-yaml` integration and the JSON schemas in `schemas/`.

---

## File Naming Convention

Activity files use the full type name as prefix, separated by underscore:

- `Forum_<name>.yaml`
- `Page_<name>.yaml`
- `Quiz_<name>.yaml`
- `Assignment_<name>.yaml`

This replaces the previous abbreviated prefixes (`FRM_`, `PGE_`, etc.) to improve readability for users unfamiliar with the abbreviations.

---

## Error Handling

| Scenario | Behavior |
|---|---|
| Init Course on existing course | Error notification, abort |
| Update Sections — missing directory | Warning notification per missing item |
| Update Sections — missing section.yaml | Warning notification per missing file |
| Create Activity — no sections exist | Command disabled (greyed out in palette) |
| Create Activity — activity file already exists | Error notification, abort |

---

## Out of Scope

- Creating or deleting sections from within the extension (directory creation is manual or via Init Course/Update Sections in a future pass)
- Validating activity YAML content beyond what redhat.vscode-yaml already provides via JSON schema
- Support for activity types beyond Forum, Page, Quiz, Assignment in this pass
