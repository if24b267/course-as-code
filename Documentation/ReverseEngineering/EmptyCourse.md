# Analysis of files created by Moodle for an empty course

## Information provided to Moodle in the course creation process (WEB-UI)

### General
- Course full name: EmptyCourse
- Course short name: EmpCo
- Course Category: Category1
- Course Visibility: Show
- Course Start Date: 08 November 2024 00:00
- Course End Date: 08 November 2025 00:00
- Course ID number: *empty*

### Description
- Course Summary: Test Summary
- Course Image: logo.png

### Course Format
- Format: Custom Sections
- Number of sections: 4
- Hidden sections: Hidden sections are completely invisible
- Course layout: Show all sections on one page

### Appearance
- Force language: Do not force
- Number of announcements: 0
- Show gradebook to students: Yes
- Show activity reports: No
- Show activity dates: Yes

### Files and uploads
- Maximum upload size: Site upload limit(40 MB)

### Completion tracking
- Enable completion tracking: Yes
- Show activity completion conditions: Yes

### Groups
- Group mode: No groups
- Force group mode: No
- Default grouping: None

### Tags
- Tags: testTag

## Analysis of the backup files created by Moodle for an empty course

### Create the backup file
Course -> More -> Course reuse -> Backup -> Backup course with standard settings

### Extraction of the backup (.mbz) file
See [Moodle FAQ](https://docs.moodle.org/405/en/Backup_and_restore_FAQ#How_can_I_extract_original_files_from_a_Moodle_backup_file?)

### Backup file structure
Everything written as a `command` is a description of the file or directory name.

- course
  - calendar.xml
    - `empty XML file`
  - comments.xml
    - `empty XML file`
  - competencies.xml
    - `empty XML file`
  - completionDefaults.xml
    - `empty XML file`
  - contentbank.xml
    - `empty XML file`
  - course.xml
    - `contains general information about the course, most of the information provided in the course creation process`
  - enrolments.xml
    - `contains information about the enrolments in the course`
    - See [Enrolments](https://docs.moodle.org/405/en/Enrolments)
  - filters.xml
    - `empty XML file`
  - inforef.xml
    - `TBD`
  - roles.xml
    - `TBD`
- files
  - 4e
    - 4e3d7ade69bbaeaacd2e664327cc7e9952b0fbf0
      - `this is the file logo.png that was uploaded to the course`
- sections
    - section_1
      - inforef.xml
        - `empty XML file`
      - section.xml
        - `empty XML file`
    - section_2
      - `All other sections are the same as section_1`
    - section_3
    - section_4
    - section_5
    - `Corresponds to the "General" section at the top + the 4 sections created in the course creation process`
- badges.xml
  - `empty XML file`
- completion.xml
  - `empty XML file`
- files.xml
  - `contains information about the files uploaded to the course`
  - `one of the entries correspons to the logo.png file`
  - `one unknown entry`
- grade_history.xml
  - `empty XML file`
- gradebook.xml
  - `TBD`
- groups.xml
  - `empty XML file`
- moodle_backup.xml
  - `TBD`
- outcomes.xml
  - `empty XML file`
- questions.xml
  - `empty XML file`
- roles.xml
  - `TBD`
- scales.xml
  - `empty XML file`
- users.xml
  - `empty XML file`

### Course.xml

### enrolments.xml

### inforef.xml

### roles.xml

### moodle_backup.xml