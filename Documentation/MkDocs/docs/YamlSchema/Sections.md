# Sections (`sections.yaml` / `section.yaml`)

Sections organize the content of a course. A `sections.yaml` file typically lists the sections and is placed in the root folder of the course.

Individual `section.yaml` files define the content of each section and are placed in the section folder.

## Properties - sections.yaml
| Property | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `Sections` | `List<SectionPaths>` | No | A list of (relative) section folder paths pointing to the section folders. |

### Example
```YAML
Sections:
  - SectionPath: Sections/01-Introduction
  - SectionPath: Sections/02-Supported-Activities
  - SectionPath: Sections/03-Quizzes
  - SectionPath: Sections/04-CS-Basics
```

## Properties - section.yaml

| Property | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `SectionName` | `string` | **Yes** | The name of the section. |
| `Summary` | `string` | No | A summary or description of the section. |
| `VisibleToStudents` | `bool` | No | Whether the section is visible to students (default: `true`). |
| `Activities` | `List<string>` | No | A list of (relative) file paths pointing to the YAML files for the activities contained in this section. |

### Example
```YAML
SectionName: Introduction to Course as Code
Summary: This section covers basics about Course as Code. What is it? What are its core components and the benefits of using it?
VisibleToStudents: true
Activities:
  - ActivityFilePath: Activities/what-is-cac.yaml
  - ActivityFilePath: Activities/cac-components.yaml
```