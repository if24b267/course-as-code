## ✨ What is Course as Code?

**Course as Code** is a tool that enables the creation of Moodle courses through text formats such as YAML or Markdown (hence the name **Course as Code**). These text files can then be converted into a Moodle backup file (`.mbz`), which may be imported directly into Moodle as a complete course. This approach allows fully functional Moodle courses to be created and edited using any text editor, while changes and collaboration can be managed efficiently with version control systems like Git.

``` yaml
CourseName: Introduction to Course as Code
ShortName: CaCIntro
Summary: Explore Course as Code through this sample course, which covers its fundamental concepts and practical activities.

Visible: true
Category: Showcases
StartDate: 01-01-2025 00:00
EndDate: 31-12-2030 23:59

ForcedLanguage: en
...
```