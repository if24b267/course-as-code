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
Explanation of the fields in the course.xml file
- `course id="2" contextid="14"`
  - wrapper for the course information
  - `id` is the course id as can be seen in the URL
    - http://localhost/course/view.php?id=2
    - TODO: Determine how the course id is generated
  - `contextid` is used by other files to identify this course
    - e.g. files.xml has a field `contextid="14"` stating that the file is related to this course
- `shortname
  - The short name of the course as provided in the course creation process
  - `EmpCo`
- `fullname`
  - The full name of the course as provided in the course creation process
  - `EmptyCourse`
- `idnumber`
  - The id number of the course as provided in the course creation process
  - `empty`
- `summary`
  - The summary of the course as provided in the course creation process
  - `<p>Test Summary</p>`
- `summaryformat`
  - The format of the summary - 1 probably means HTML
  - `1`
- `format`
  - The format of the course as provided in the course creation process
  - `topics`
  - `topics` seemingly corresponds to "Format: Custom Sections" in the course creation process
  - TODO: Determine the possible values for this field
- `showgrades`
  - Whether to show grades to students as provided in the course creation process
  - `0` means No
  - `1` means Yes
- `newsitems`
  - The number of announcements to show as provided in the course creation process
  - `0`
- `startdate`
  - The start date of the course as provided in the course creation process
  - saved as a Unix timestamp
  - `1731024000` corresponds to `08 November 2024 00:00`
- `enddate`
  - The end date of the course as provided in the course creation process
  - saved as a Unix timestamp
  - `1762560000` corresponds to `08 November 2025 00:00`
- `marker`
  - `0`
- `maxbytes`
  - The maximum upload size as provided in the course creation process
  - `0`
  - TODO: Determine the possible values for this field
    - `0` means Site upload limit(40 MB)?
- `legacyfiles`
  - `0`
- `showreports`
  - Whether to show activity reports as provided in the course creation process
  - `0` means No
  - `1` means Yes
- `visible`
  - The visibility of the course as provided in the course creation process
  - `1` means Show
  - `0` means Hide
- `groupmode`
  - The group mode of the course as provided in the course creation process
  - `0` means No groups
  - `1` means Separate groups?
  - `2` means Visible groups?
- `groupmodeforce`
  - Whether to force the group mode as provided in the course creation process
  - `0` means No
  - `1` means Yes
- `defaultgroupingid`
  - The default grouping of the course as provided in the course creation process
  - `0` means None
- `lang`
  - The force language setting as provided in the course creation process
  - `""` means Do not force
- `theme`
  - `""`
- `timecreated`
  - The time the course was created
  - saved as a Unix timestamp
  - `1731001588` corresponds to `Thu Nov 07 2024 18:46:28`
- `timemodified`
  - The time the course was last modified
  - saved as a Unix timestamp
  - `1731001588` corresponds to `Thu Nov 07 2024 18:46:28`
- `requested`
  - `0`
- `showActivityDates`
  - Whether to show activity dates as provided in the course creation process
  - `1` means Yes
  - `0` means No
- `showCompletionConditions`
  - Whether to show activity completion conditions as provided in the course creation process
  - `1` means Yes
  - `0` means No
- `pdfexportfont`
    - `"$@NULL@$"`
- `enableCompletion`
  - Whether to enable completion tracking as provided in the course creation process
  - `1` means Yes
  - `0` means No
- `completionnotify`
  - `0`
- `category id="1"`
  - The category of the course as provided in the course creation process
- `Category1`
  - TODO: Determine how the id is determined
- `tags`
  - The tags of the course as provided in the course creation process
  - `testTag`
- `customfields`
  - `""`
- `courseformatoptions`
  - `courseformatoption`
    - TBD
  - `courseformatoption`
    - Probably corresponds to the "Hidden sections" setting in the course creation process

### enrolments.xml

### inforef.xml

### roles.xml

### moodle_backup.xml