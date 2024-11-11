# Analysis of files created by Moodle for an empty course with course format weekly and other additional options

## Information provided to Moodle in the course creation process (WEB-UI)

### General
- Course full name: Empty Weekly Course (used other options)
- Course short name: EWC
- Course Category: Category1
- Course Visibility: Show
- Course Start Date: 12 November 2024 00:00
- Course End Date: 24 December 2024 00:00
- Course ID number: 22

### Description
- Course Summary: Test Summary of Weekly Course + Visible groups + 2 Tags
- Course Image: COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png

### Course Format
- Format: Weekly Sections
- Hidden sections: Hidden sections are completely invisible
- Course layout: Show all sections on one page

### Appearance
- Force language: English (en)
- Number of announcements: 4
- Show gradebook to students: Yes
- Show activity reports: Yes
- Show activity dates: Yes

### Files and uploads
- Maximum upload size: 10 MB

### Completion tracking
- Enable completion tracking: Yes
- Show activity completion conditions: Yes

### Groups
- Group mode: Visible groups
- Force group mode: Yes
- Default grouping: None

### Tags
- Tags: EWC, Groups

## Analysis of the backup files created by Moodle for an empty course with course format weekly and other additional options

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
    - seems to contain information about the roles assigned to users in the course
- `files`
  - `12`
    - `120d1648f202004f48b17274c52f33e968d20a6d`
      - this is the file for the COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png (Logo) that was uploaded to the course
- `sections`
    - `section_13`
      - `inforef.xml`
        - empty XML file
      - `section.xml`
        - XML file that indicates, the section is empty
    - `section_14`
      - All other sections are the same as section_13
    - `section_15`
    - `section_16`
    - `section_17`
    - `section_18`
    - `section_19`
    - Corresponds to the "General" section at the top + the 6 sections created in the course creation process
- `badges.xml`
  - empty XML file
- `completion.xml`
  - empty XML file
- `files.xml`
  - contains information about the files uploaded to the course
  - one of the entries correspons to the  COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png file
  - one unknown entry
- `grade_history.xml`
  - empty XML file
- `gradebook.xml`
  - TBD
- `groups.xml`
  - empty XML file
- `moodle_backup.xml`
  - seems to contain metadata about the backup
- `outcomes.xml`
  - empty XML file
- `questions.xml`
  - empty XML file
- `roles.xml`
  - TBD
- `scales.xml`
  - empty XML file
- `users.xml`
  - empty XML file

### Course.xml
Contains general information about the course, mostly the information provided in the course creation process

- `course id="4" contextid="18"`
  - wrapper for the course information
  - `id` is the course id as can be seen in the URL
    - http://localhost/course/view.php?id=4
    - TODO: Determine how the course id is generated
  - `contextid` is used by other files to identify this course
    - e.g. files.xml has a field `contextid="18"` stating that the file is related to this course
- `shortname`
  - The short name of the course as provided in the course creation process
  - `EWC`
- `fullname`
  - The full name of the course as provided in the course creation process
  - `Empty Weekly Course (used other options)`
- `idnumber`
  - The id number of the course as provided in the course creation process
  - `22`
- `summary`
  - The summary of the course as provided in the course creation process
  - `<p>Test Summary of Weekly Course + Visible groups + 2 Tags</p>`
- `summaryformat`
  - The format of the summary - 1 probably means HTML
  - `1`
- `format`
  - The format of the course as provided in the course creation process
  - `weeks`
  - `weeks` seemingly corresponds to "Format: Weekly Sections" in the course creation process
  - TODO: Determine the possible values for this field
- `showgrades`
  - Whether to show grades to students as provided in the course creation process
  - `0` means No
  - `1` means Yes
- `newsitems`
  - The number of announcements to show as provided in the course creation process
  - `4`
- `startdate`
  - The start date of the course as provided in the course creation process
  - saved as a Unix timestamp
  - `1731369600` corresponds to `12 November 2024 00:00`
- `enddate`
  - The end date of the course as provided in the course creation process
  - saved as a Unix timestamp
  - `1734998400` corresponds to `24 December 2024 00:00`
- `marker`
  - `0`
- `maxbytes`
  - The maximum upload size as provided in the course creation process
  - `10485760`
  - TODO: Determine the possible values for this field
    - `10485760` means Site upload limit(10 MB)
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
  - `"en"` means force English 
- `theme`
  - `""`
- `timecreated`
  - The time the course was created
  - saved as a Unix timestamp
  - `1731353564` corresponds to `Mon Nov 11 2024 19:32:44`
- `timemodified`
  - The time the course was last modified
  - saved as a Unix timestamp
  - `1731353564` corresponds to `Mon Nov 11 2024 19:32:44`
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
  - TODO: Determine how the id is determined
- `tags`
  - The tags of the course as provided in the course creation process
  - `groups`
  - `ewc`
- `customfields`
  - `""`
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
  - `7`
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
  - `3`
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
    - `<id>39</id>` and `<id>40</id>`: These IDs correspond to the id of the two files that appear in files.xml
      - `39` corresponds to the COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png file uploaded to the course.
      - `40` corresponds to an unknown file.

### roles.xml
Seems to contain information about the roles assigned to users in the course.

- `<roles>`: root element that encapsulates all role-related data.
- `<role_overrides>`: used to store any custom permissions or overrides for specific roles?
- `<role_assignments>`: contains role assignments for users?

#### Within `<role_assignments>`:
- `<assignment id="3">`:
  - represents a single role assignment?
  - `id="3"` indicates this is the first assignment entry in the system?
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
- `<name>`: The name of the backup file `backup_test_weekly.mbz`
- `<moodle_version>`: The version of Moodle used to create the backup
  - `2024100700`
  - Corresponds to Moodle version 4.5, see [Moodle versions](https://moodledev.io/general/releases)
- `<moodle_release>`: The release of Moodle used to create the backup
  - `4.5 (Build: 20241007)`
- `<backup_version>` & `<backup_release>`
  - Correspond to `<moodle_version>` and `<moodle_release>`
  - TODO: Determine the difference between these fields
- `<backup_date>`: timestamp when the backup was created
  - `1731353758`
  - saved as a Unix timestamp
  - Corresponds to `Mon Nov 11 2024 19:35:58`
- `<mnet_remoteusers>`: TBD
  - `0` means disabled?
  - Seems to be [deprecated](https://docs.moodle.org/405/en/MNet)
- `<include_files>`: Indicates whether files are included in the backup
  - `1` means files are included
  - Determined in the backup settings when creating the backup
- `<include_file_references_to_external_content>`: TBD
  - `0` means disabled?
- `<original_wwwroot>`: URL of the Moodle instance that was backed up?
  - `https://moodle.sanctus.cx`
- `<original_course_id>`: ID of the ?original? course in Moodle
  - `4`
  - Original refers to the name the course had when the backup was created?
  - **These values seem to be the same as in the course.xml file**
- `<original_course_format>`: The format of the original course
  - `weeks`
- `<original_course_fullname>`: Full name of the original course
  - `Empty Weekly Course (used other options)`
- `<original_course_shortname>`: Short name of the original course
  - `EWC`
- `<original_course_startdate>`: Start date of the original course as unix timestamp
  - `1731369600`
- `<original_course_enddate>`: End date of the original course as unix timestamp
  - `1734998400`
- `<original_course_contextid>`: Context ID of the original course
  - `18`
- `<original_system_contextid>`: TBD
  - `1`
  - This value doesn't appear in the course.xml file

#### Details Section
- `<detail>`: Wrapper for the course details
- `<detail backup_id="6078ead34c57f691993f01c2e97accdd">`: Unique ID for the backup
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
  - `13`, `14`, `15`, `16`, `17`, `18`, `19`
  - TODO: Determine how the section ID is generated
    - simply ascending numbers?
- `<title>`: Section title?
  - `0`, `1`, `2`, `3`, `4`, `5`, `6`
  - Why are these values numbers?
  - TODO: Try giving the sections names in the course creation process and see if they appear here
- `<directory>`: Path in the backup directory for the section.
  - `sections/section_13`
- `<parentcmid/>`: TBD
  - empty
- `<modname/>`: TBD
  - empty

##### Course Section
- `<course>`: wrapper for the course information
- `<courseid>`: Course ID
  - `4`
  - Corresponds to the course ID in the URL and in the course.xml file
- `<title>`: Course title
  - `EWC`
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
    - `section_13`, `section_14`, `section_15`, `section_16`, `section_17`, `section_18`, `section_19`
    - only appears for settings that apply to sections
  - `<name>`: name of the setting
    - Seemingly a key-value pair in the form of `<name>` and `<value>`
  - `<value>`: value of the setting
    - `1` or `0` for boolean settings

States which elements are included in the backup, e.g.:
- `users=1`: User data is included
- `logs=0`: Logs are excluded

### gradebook.xml
TBD

### roles.xml
`<roles_definition>`: wrapper for the role sections
  - contains multiple `<role>` elements 
  - contains every role

#### inside every `<role>` 
- `id`: Unique ID of the roles.
  - `3`, `5`
- `<shortname>`: Short names for the roles?
  - `editingteacher`, `student`
- `<nameincourse>`: Name used in the course?
  - `$@NULL@$`
- `<description>`: Description of the role
  - empty
- `<sortorder>`: Sorting order of the roles
  - `3`, `5`
- `<archetype>`: Type of role
  - `editingteacher`, `student`

### files.xml
`<files>`: wrapper for the file sections
  - contains multiple `<file>` elements 
  - contains every file

#### Inside every `<file>`
- `id`: Unique ID of the files
  - `39`, `40`
- `<contenthash>`: Unique hash of the file content for integrity verification
  - `120d1648f202004f48b17274c52f33e968d20a6d`, `da39a3ee5e6b4b0d3255bfef95601890afd80709`
- `<contextid>`: ID of the context where the file is used
  - `18`
- `<component>`: system component where the file is associated
  - `course`
- `<filearea>`: specific area within the component where the file is stored
  - `overviewfiles`
- `<itemid>`: Item ID, used for categorizing files, often set to `0` when unused
  - `0`
- `<filepath>`: directory path of the file within the storage
  - `/`
- `<filename>`: name of the files (Logo and unknown file)
  - `COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png`, `.`
- `<userid>`: ID of the user who uploaded the file
  - `2`
- `<filesize>`: Size of the file in bytes
  - `425449` (for a PNG image), `0` (empty file)
- `<mimetype>`: MIME type of the file, indicating its format
  - `image/png`, `$@NULL@$`
- `<status>`: Status of the file (e.g., `0` for active)
  - `0`
- `<timecreated>`: Unix timestamp of when the file was created
  - `1731353367`
- `<timemodified>`: Unix timestamp of when the file was last modified
  - `1731353564`
- `<source>`: Original source or name of the file
  - `COM_74CA8C-46036F-BF6BF3-846A72-DFC82A-78975E.png`, `$@NULL@$`
- `<author>`: Name of the person who authored or uploaded the file
  - `Admin User`, `$@NULL@$`
- `<license>`: License type of the file, indicating usage rights
  - `unknown`, `$@NULL@$`
- `<sortorder>`: Order in which the file is displayed
  - `0`
- `<repositorytype>`: Type of the repository from which the file originates
  - `$@NULL@$`
- `<repositoryid>`: ID of the repository, if applicable
  - `$@NULL@$`
- `<reference>`: Additional reference information for the file
  - `$@NULL@$`