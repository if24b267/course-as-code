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

- `course`
  - `calendar.xml`
    - empty XML file
  - `comments.xml`
    - empty XML file
  - `competencies.xml`
    - empty XML file
  - `completionDefaults.xml`
    - empty XML file
  - `contentbank.xml`
    - empty XML file
  - `course.xml`
    - contains general information about the course, most of the information provided in the course creation process
  - `enrolments.xml`
    - contains information about the enrolments in the course
    - See [Enrolments](https://docs.moodle.org/405/en/Enrolments)
  - `filters.xml`
    - empty XML file
  - `inforef.xml`
    - contains references to users, roles, and files associated with the course
  - `roles.xml`
    - seems to contain the assignment of roles to users in the course
- `files`
  - `4e`
    - `4e3d7ade69bbaeaacd2e664327cc7e9952b0fbf0`
      - this is the file logo.png that was uploaded to the course
- `sections`
    - `section_1`
      - `inforef.xml`
        - empty XML file
      - `section.xml`
        - empty XML file
    - `section_2`
      - All other sections are the same as section_1
    - `section_3`
    - `section_4`
    - `section_5`
    - Corresponds to the "General" section at the top + the 4 sections created in the course creation process
- `badges.xml`
  - empty XML file
- `completion.xml`
  - empty XML file
- `files.xml`
  - seems to contain information about the files uploaded to the course
  - one of the entries correspons to the logo.png file
  - one unknown entry
- `grade_history.xml`
  - empty XML file
- `gradebook.xml`
  - Contains information about the gradebook setup for the course
- `groups.xml`
  - empty XML file
- `moodle_backup.xml`
  - seems to contain metadata about the backup
- `outcomes.xml`
  - empty XML file
- `questions.xml`
  - empty XML file
- `roles.xml`
  - seems to contain information about the roles available in the course
- `scales.xml`
  - empty XML file
- `users.xml`
  - empty XML file

### Course.xml
Contains general information about the course, mostly the information provided in the course creation process

- `course id="2" contextid="14"`
  - wrapper for the course information
  - `id` is the course id as can be seen in the URL
    - http://localhost/course/view.php?id=2
    - TODO: Determine how the course id is generated
  - `contextid` is used by other files to identify this course
    - e.g. files.xml has a field `contextid="14"` stating that the file is related to this course
- `shortname`
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
  - [Not helpful](https://moodle.org/mod/forum/discuss.php?d=388688)
- `maxbytes`
  - The maximum upload size as provided in the course creation process
  - `0`
  - TODO: Determine the possible values for this field
    - `0` means Site upload limit(40 MB)?
- `legacyfiles`
  - Moodle uses a new file system since version 2.0; this setting probably tells moodle if this backup uses the new or old file system
  - See [moodle docs](https://docs.moodle.org/405/en/Legacy_course_files)
  - `0` means new file system (no legacy files)
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
  - if the course uses a custom theme, it would probably be specified here
  - the value probably must match the name of a theme in the Moodle installation, see [moodle docs](https://moodledev.io/docs/4.4/apis/plugintypes/theme#getting-your-theme-to-appear-correctly-in-theme-selector)
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
  - This might have something to do with automatic backups, see [here](https://moodle.org/mod/forum/discuss.php?d=442987) and [here](https://docs.moodle.org/405/en/Automated_course_backup)
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
    - probably refers to the name of the font used for PDF exports
    - if empty, the default font is used?
- `enableCompletion`
  - Whether to enable completion tracking as provided in the course creation process
  - refers either to [this](https://docs.moodle.org/405/en/Using_Activity_completion) or [that](https://docs.moodle.org/405/en/Course_completion)
  - `1` means Yes
  - `0` means No
- `completionnotify`
  - `0`
  - whether to send a notification when a course/activity is completed?
  - maybe see [here](https://docs.moodle.org/405/en/Notifications#List_of_notification_types)
- `category id="1"`
  - The category of the course as provided in the course creation process
  - TODO: Determine how the id is determined
- `tags`
  - The tags of the course as provided in the course creation process
  - `testTag`
- `customfields`
  - `""`
  - probably refers to [this](https://moodledev.io/docs/4.4/apis/plugintypes/customfield)
- `courseformatoptions`
  - `courseformatoption`
    - TBD
  - `courseformatoption`
    - Probably corresponds to the "Hidden sections" setting in the course creation process

### enrolments.xml
`enrol` seems to define the different enrolment methods available for the course.
`user_enrolments` seems to define the individual users enrolled in the course.

#### Top-Level Structure
- `<enrolments>`: The root element containing all enrolment information.
- `<enrols>`: A container for individual enrolment methods.

#### Inside each `<enrol>` tag
- `id`: Unique identifier, TBD
  - `1`
  - TODO: Determine how the id is generated
- `enrol`: Type of enrolment method (e.g. manual, self, guest, etc.)
  - `manual`
  - See [Enrolment methods](https://docs.moodle.org/405/en/Enrolment_methods)
- `status`: Indicates if the enrolment method is active
  - `0` for active
- `name`: Name of the enrolment method
  - `$@NULL@$` signifies no specific name?
- `enrolperiod`: Duration for which the enrolment is valid
  - `0` means unlimited
- `enrolstartdate` and `enrolenddate`: Start and end dates for the enrolment period
  - represented as Unix timestamps
  - `0` indicates no date set?
- `expirynotify`: Whether expiration notifications are enabled?
  - `0`
- `expirythreshold`: TBD
  - `86400`
- `notifyall`: TBD
  - `0`
- `password`: Password required for enrolment?
  - `$@NULL@$` means no password?
- `cost` and `currency`: Used for paid enrolments?
  - `$@NULL@$` means not applicable?
- `roleid`: Default role assigned upon enrolment?
  - `5` corresponds to the role of a student?
  - TODO: Determine what the role id corresponds to
- `customintX`, `customcharX`, `customdecX`, `customtextX`: Custom fields for additional configurations?
  - `$@NULL@$` indicates unused?
- `timecreated` and `timemodified`: Timestamps for when the enrolment was created and last modified?
  - saved as Unix timestamps

#### `<user_enrolments>` Section
Contains the individual enrolled users for this method.
The one entry in this section corresponds to the Admin User who created the course.
- `enrolment id`: Unique ID for the enrolment instance?
  - `1`
  - TODO: Determine how the id is generated
- `status`: Active (0) or suspended (1).
  - Corresponds to: EmptyCourse -> Participants -> Status -> *Pencil Symbol* of a participant -> Status
- `userid`: ID of the user enrolled?
- `timestart` and `timeend`: Start and end of the enrolment period for this user.
  - Corresponds to: EmptyCourse -> Participants -> Status -> *Pencil Symbol* of a participant -> Enrolment starts and Enrolment ends
- `modifierid`: ID of the user who last modified the enrolment?
  - `2` corresponds to the Admin User who created the course?
- `timemodified`: Timestamp of the last modification.
  - saved as Unix timestamps 

### inforef.xml
Contains references to users, roles, and files associated with the course.\
Probably ensures that the correct users, roles, and files are associated with the course during the restore process.

- `<inforef>`: root element of the file
- `<userref>`: contains references to users who are part of the Moodle course?
  - `<user>`: Each user has an `<id>` tag.
    - `<id>2</id>`: Refers to a user with the internal ID `2` in Moodle
    - This ID likely corresponds to the Admin User who created the course.
- `<roleref>`: lists roles associated with the course?
  - `<role>`: Each role has an `<id>` tag.
    - `<id>3</id>` and `<id>5</id>`: could correspond to specific roles like student, teacher, or manager within the Moodle course.
    - `5` corresponds to the role of a student?
    - TODO: Determine the role corresponding to each ID.
- `<fileref>`: references files linked to the course?
  - `<file>`: Each file has an `<id>` tag.
    - `<id>11</id>` and `<id>12</id>`: These IDs correspond to the id of the two files that appear in files.xml
      - `11` corresponds to the logo.png file uploaded to the course.
      - `12` corresponds to an unknown file.

### roles.xml (inside the course directory)
seems to contain the assignment of roles to users in the course

- `<roles>`: root element that encapsulates all role-related data.
- `<role_overrides>`: used to store any custom permissions or overrides for specific roles?
- `<role_assignments>`: contains role assignments for users?

#### Within `<role_assignments>`:
- `<assignment id="1">`:
  - represents a single role assignment?
  - `id="1"` indicates this is the first assignment entry in the system?
- `<roleid>`: specifies the role assigned to the user
  - `3` likely corresponds to a specific role in Moodle
- `<userid>`: identifies the user to whom the role is assigned
  - `2` likely corresponds to the Admin User who created the course.
- `<timemodified>`: indicates when the role assignment was last modified?
  - stored as a Unix timestamp
- `<modifierid>`: refers to the user who last modified this role assignment?
  - `2` likely corresponds to the Admin User who created the course.
- `<component>` and `<itemid>`: TBD
- `<sortorder>`: TBD

### moodle_backup.xml
contains various metadata about the backup process and the backup itself

- `<moodle_backup>`: root element for the backup metadata

#### Information Section
- `<name>`: The name of the backup file `backup-moodle2-course-2-empco-20241107-1749.mbz`
- `<moodle_version>`: The version of Moodle used to create the backup
  - `2024100700`
  - Corresponds to Moodle version 4.5, see [Moodle versions](https://moodledev.io/general/releases)
- `<moodle_release>`: The release of Moodle used to create the backup
  - `4.5 (Build: 20241007)`
- `<backup_version>` & `<backup_release>`
  - Correspond to `<moodle_version>` and `<moodle_release>`
  - TODO: Determine the difference between these fields
- `<backup_date>`: timestamp when the backup was created
  - `1731001753`
  - saved as a Unix timestamp
  - Corresponds to `Thu Nov 07 2024 18:49:13 GMT+0100`
- `<mnet_remoteusers>`: TBD
  - `0` means disabled?
  - Seems to be [deprecated](https://docs.moodle.org/405/en/MNet)
- `<include_files>`: Indicates whether files are included in the backup
  - `1` means files are included
  - Determined in the backup settings when creating the backup
- `<include_file_references_to_external_content>`: TBD
  - `0` means disabled?
- `<original_wwwroot>`: URL of the Moodle instance that was backed up?
  - `http://127.0.0.1:8080`
- `<original_course_id>`: ID of the ?original? course in Moodle
  - `2`
  - Original refers to the name the course had when the backup was created?
  - **These values seem to be the same as in the course.xml file**
- `<original_course_format>`: The format of the original course
  - `topics`
- `<original_course_fullname>`: Full name of the original course
  - `EmptyCourse`
- `<original_course_shortname>`: Short name of the original course
  - `EmpCo`
- `<original_course_startdate>`: Start date of the original course as unix timestamp
  - `1731024000`
- `<original_course_enddate>`: End date of the original course as unix timestamp
  - `1762560000`
- `<original_course_contextid>`: Context ID of the original course
  - `14`
- `<original_system_contextid>`: TBD
  - `1`
  - This value doesn't appear in the course.xml file

#### Details Section
- `<detail>`: Wrapper for the course details
- `<detail backup_id="c11e1f6cbc2ae39039d726d36c3e6a4a">`: Unique ID for the backup
  - TODO: Determine how the backup ID is generated
  - Only appears in the moodle_backup.xml file
- `<type>`: Type of the backup?
  - `course`
  - TODO: Determine the possible values for this field
- `<format>`: Format of the backup?
  - `moodle2`
  - Seems to refer to the ["new" backup format](https://docs.moodle.org/dev/Backup_2.0)
- `<interactive>`: TBD
  - `1`
- `<mode>`: TBD
  - `70`
- `<execution>`: TBD
  - `2`
- `<executiontime>`: TBD
  - `0`

#### Contents Section
- `<sections>`: wrapper for the course sections
  - contains multiple `<section>` elements 
    - corresponding to the 5 sections in the course
  - Seemingly used to determine the order of the sections in the course
- `<sectionid>`: Unique ID of the section.
  - `1`, `2`, `3`, `4`, `5`
  - TODO: Determine how the section ID is generated
    - simply ascending numbers?
- `<title>`: Section title?
  - `0`, `1`, `2`, `3`, `4`
  - Why are these values numbers?
  - TODO: Try giving the sections names in the course creation process and see if they appear here
- `<directory>`: Path in the backup directory for the section.
  - `sections/section_1`
- `<parentcmid/>`: TBD
  - empty
- `<modname/>`: TBD
  - empty

##### Course Section
- `<course>`: wrapper for the course information
- `<courseid>`: Course ID
  - `2`
  - Corresponds to the course ID in the URL and in the course.xml file
- `<title>`: Course title
  - `EmpCo`
  - Corresponds to the short name of the course in the course.xml file
- `<directory>`: Path in the backup directory for the course
  - `course`

#### Settings Section
Contains the settings that were selected when the backup was created.
Corresponds to the UI settings when creating the backup.

- `<settings>`: wrapper for various backup settings
- `<setting>`: contains individual settings
  - `<level>`: indicates whether the setting applies to the root, section, or other components
    - `root` refers to the course level?
    - `section` refers to the individual sections?
  - `<section>`: The section to which the setting applies?
    - `section_1`, `section_2`, `section_3`, `section_4`, `section_5`
    - only appears for settings that apply to sections
  - `<name>`: name of the setting
    - Seemingly a key-value pair in the form of `<name>` and `<value>`
  - `<value>`: value of the setting
    - `1` or `0` for boolean settings

States which elements are included in the backup, e.g.:
- `users=1`: User data is included
- `logs=0`: Logs are excluded

### gradebook.xml
contains information about the gradebook setup for the course.\
It's probably necessary to understand how grading in moodle works to fully understand this file.

- `<attributes>`: probably a placeholder for additional metadata
- `<grade_categories>`: groups grade items into categories
  - probably refers to [this](https://docs.moodle.org/405/en/Grade_categories)
  - `<grade_category>`: contains information about a single grade category
    - `id`: unique identifier for the grade category
  - `<parent>`: `"$@NULL@$"`
    - ID of the parent category; `$@NULL@$` indicates no parent.
    - probably an integer value if a parent category exists
  - `<depth>`: `"1"`
    - probably indicates the category's depth in the hierarchy
  - `<path>`: `"/1/"`
    - probably path to the category's position in the hierarchy
    - based on IDs
  - `<fullname>`: `"?"`
    - name of the category?
  - `<aggregation>`: `"13"`
    - Maybe the aggregation method used for the category?
    - See [Grade aggregation](https://docs.moodle.org/405/en/Grade_aggregation)
  - `<keephigh>`: `"0"`
    - only retain the X highest grades, X being the selected value for this option
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_categories#Keep_the_highest)
  - `<droplow>`: `"0"`
    - drop the X lowest grades, X being the selected value for this option
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_categories#Drop_the_lowest)
  - `<aggregateonlygraded>`: `"1"`
    - whether to treat non-graded items in the category as 0 zero points reached (0) or ignore them (1)
    - boolean value (1 or 0)
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_categories#Aggregate_only_non-empty_grades)
  - `<aggregateoutcomes>`: `"0"`
    - whether to include outcomes in aggregation (whatever that means)
    - boolean value (1 or 0)
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_categories#Include_outcomes_in_aggregation)
  - `<timecreated>`: `"1731001607"`
    - timestamp for when the category was created
    - Unix timestamp
  - `<timemodified>`: `"1731001607"`
    - timestamp for when the category was last modified
    - Unix timestamp
  - `<hidden>`: `"0"`
    - whether the category is hidden; `0` probably means visible
    - boolean value (1 or 0)
- `<grade_items>`
  - defines individual gradeable items.
  - probably refers to [this](https://docs.moodle.org/405/en/Grade_items)
  - `<grade_item>`: `id="1"`
    - contains information about a single grade item
    - unique identifier for the grade item.
  - `<categoryid>`: `"$@NULL@$"`
    - probably ID of the category to which the item belongs.
    - `$@NULL@$` indicates no category.
  - `<itemname>`: `"$@NULL@$"`
    - name of the grade item, e.g. the assignment name.
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<itemtype>`: `"course"`
    - type of the grade item
    - possible values: assigment, workshop, lesson, quiz?
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<itemmodule>`: `"$@NULL@$"`
  - `<iteminstance>`: `1`
  - `<itemnumber>`: `"$@NULL@$"`
  - `<iteminfo>`: `"$@NULL@$"`
    - general information about the grade item; optional
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<idnumber>`: `"$@NULL@$"`
  - `<calculation>`: `"$@NULL@$"`
    - probably has something to do with [this](https://docs.moodle.org/405/en/Grade_calculations)
  - `<gradetype>`: `"1"` 
    - type of grading
    - There are 4 grade types:
      - None - No grading possible 
      - Value - A numerical value with a maximum and minimum 
      - Scale - An item in a list - see Scales 
      - Text - Feedback only
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<grademax>`: `"0.00000"`
    - The maximum grade that can be given
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Manual_grade_items)
  - `<grademin>`: `"0.00000"`
    - The minimum grade that can be given
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Manual_grade_items)
  - `<scaleid>`: `"$@NULL@$"`
  - `<outcomeid>`: `"$@NULL@$"`
  - `<gradepass>`: `"0.00000"`
    - minimum grade a student must achieve to be considered to have passed this item
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<multfactor>`: `1.00000`
    - factor by which all grade for this grade item will be multiplied
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<plusfactor>`: `0.00000`
    - number that will be added to every grade for this grade item, after the Multiplicator is applied
    - can be negative
    - maybe refers to **offset** in [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)?
  - `<aggregationcoef>`: `0.00000`
  - `<aggregationcoef2>`: `"0.00000"`
  - `<weightoverride>`: `"0"`
    - probably somehow refers to the **weight** as seen on the screenshots in the [moodle docs](https://docs.moodle.org/405/en/Grade_items#/media/File:editingradeitem.png)
  - `<sortorder>`: `"1"`
    - position in the list of grade items?
  - `<display>`: `"0"`
  - `<decimals>`: `$@NULL@$`
    - the number of decimal points to display for each grade?
    - refers to **overall decimal points** on [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)?
  - `<hidden>`: `"0"`
    - whether the grade item is hidden from students
  - `<locked>`: `"0"`
    - whether to lock this grade item
    - A locked grade item doesn't accept changes to its settings or grades from its related module.
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<locktime>`: `"0"`
    - a date after which the locking will be effective, usually after the grades release date
    - probably a Unix timestamp
    - See [moodle docs](https://docs.moodle.org/405/en/Grade_items#Activity-based_grade_items)
  - `<needsupdate>`: `"0"`
    - whether the grade item needs updating???
  - `<timecreated>`: `"1731001607"`
    - timestamp for when the grade item was created.
    - Unix timestamp
  - `<timemodified>`: `"1731001607"`
    - timestamp for when the grade item was last modified.
    - Unix timestamp
  - `<grade_grades>`:
- `<grade_letters>`: 
  - probably refers to [this](https://docs.moodle.org/405/en/Grade_letters)
- `<grade_settings>`: stores global gradebook settings?
  - it seems that each setting is stored as a `<grade_setting>` element
  - `<grade_setting>`:
    - `id`: `"1"`
    - `<name>`: `"minmaxtouse"`
    - `<value>`: `"1"`
  
### roles.xml
seems to contain information about the roles available in the course

- `<roles_definition>`
  - Root element containing all role definitions
  - encapsulates multiple `<role>` elements
- `<role>`: `id="3"`
  - defines a single role
  - each `<role>` element seems to represent a specific role in Moodle
  - `<name>`: ` ` (empty)
    - full name of the role
    - only needed for custom roles, see [moodle docs](https://docs.moodle.org/2x/ca/Creating_custom_roles#Creating_a_new_role)
  - `<shortname>`: `editingteacher`
    - short name of the role
  - `<nameincourse>`: `$@NULL@$`
    - custom name for the role specific to the course?
    - `$@NULL@$` if no custom name is set
  - `<description>`: ` ` (empty)
    - optional description of the role, see [moodle docs](https://docs.moodle.org/2x/ca/Creating_custom_roles#Creating_a_new_role)
  - `<sortorder>`:  `3`
    - position in the list of roles?
  - `<archetype>`: `editingteacher`
    - A role archetype is a template for a role, see [moodle docs](https://moodledev.io/docs/4.4/apis/subsystems/roles#programming-interface)
    - "editing teacher" is the default role assigned to individuals who create course content and then use this content to teach students, see [moodle docs](https://docs.moodle.org/2x/pl/Hosting_for_moodle_teachers)

### files.xml
seems to contain information about the files uploaded to the course

- `<files>`: root element containing all file information
- `<file>`: represents a single file's metadata
  - each file is identified by an `<id>` tag, other files in the backup reference this ID
  - the two entries in this file correspond to the two files mentioned in inforef.xml
  - `<contenthash>`: `4e3d7ade69bbaeaacd2e664327cc7e9952b0fbf0`
    - the file content hash is calculated by taking a **SHA1 hash** of the content of the file, see [moodle docs](https://moodledev.io/docs/4.3/apis/subsystems/files/internals#file-system)
    - used to uniquely identify the file
  - `<contextid>`: `14`
    - probably has something to do with [this](https://docs.moodle.org/405/en/Context) and [this](https://docs.moodle.org/405/en/Override_permissions)
  - `<component>`: `course`
    - moodle component associated with the file?
  - `<filearea>`: `overviewfiles`
  - `<itemid>`: `0`
  - `<filepath>`: ` ` (empty)
    - Why is this field empty? Shouldn't it contain the path to the file?
    - the `logo.png` file is stored in the `4e` directory inside the `files` directory
  - `<filename>`: `logo.png`
    - the original name of the file
  - `<userid>`: `3`
    - identifies the user who uploaded the file
    - seems to correspond to the Admin User who created the course
  - `<filesize>`: `27519`
    - size of the file in bytes
  - `<mimetype>`: `image/png`
    - describes the file's MIME type
  - `<status>`: `0`
  - `<timecreated>`: `1731136235`
    - timestamp for when the file was created
    - Unix timestamp
  - `<timemodified>`: `1731136235`
    - timestamp for when the file was last modified
    - Unix timestamp
  - `<source>`: `logo.png`
    - How does this differ from `<filename>`?
  - `<author>`: `Admin User`
    - the name of the user who created/uploaded the file
  - `<license>`: `unknown`
    - seems to specify the file's license
  - `<sortorder>`: `0`
    - determines the order of files when displayed?
  - `<repositorytype>`: `$@NULL@$`
  - `<repositoryid>`: `$@NULL@$`
  - `<reference>`: `$@NULL@$`

The second file entry in the file.xml file seems to correspond to a file marker of the UNIX file system (`.`)