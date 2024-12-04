## Course

### NumberOfAnnouncementsToDisplay  
corresponds to `<newsitems>` in `/course/course.xml`  

Configuration of the announcements forum at the top of the course page  
0 -> no announcements forum | a value greater than 0 enables the forum and determines how many recent announcements are displayed in the "latest announcements" block  

### GroupMode  
default group mode for all activities in this course  

Possible Values:  
`NoGroups` -> no groups are used in the course  
`SeparateGroups` -> students are divided into separate groups and can only see their own groups' work  
`VisibleGroups` -> students are divided into separate groups but can see other groups' work  

See [moodle docs](https://docs.moodle.org/405/en/Groups#Group_modes)  

corresponds to these values in Moodle XML:  
TODO  

### Format  
the format of the course  

Possible Values:  
`CustomSections` -> course is divided into customisable sections  
`WeeklySections` -> course is divided into sections corresponding to each week  
`SingleActivity` -> course contains only one activity or resource  
`Social` -> course is centred around a main forum on the course page  

corresponds to `<format>` in `/course/course.xml`, possible values:  
`CustomSections` -> `topics`  
`WeeklySections` -> `weeks`  
`SingleActivity` -> `singleactivity`  
`Social` -> `social`  

### CourseIdNumber  
The ID number is an alphanumeric field. It has several potential uses. Generally, it is not displayed to students.  
However, it can be used to match this course against an external system's ID, as your course catalogue ID or can be used in the certificate module as a printed field.  

See [moodle docs](https://docs.moodle.org/405/en/Course_settings#Course_ID_number)  

corresponds to `<idnumber>` in `/course/course.xml`  

### MaximumUploadSize  
the default maximum upload size for student submissions for all activities in this course  
can still be modified manually for each activity  

corresponds to `<maxbytes>` in `/course/course.xml`, possible values:  
0 -> maximum upload size supported by webserver  

### Theme  
if the course uses a custom theme, it should be specified here  
the value must match the name of a theme in the Moodle installation, see [moodle docs](https://moodledev.io/docs/4.4/apis/plugintypes/theme#getting-your-theme-to-appear-correctly-in-theme-selector)  

### ShowActivityCompletionConditions  
if enabled, teacher can set and students will be able to see a list of what they must do to complete an activity  
See [moodle docs](https://docs.moodle.org/405/en/Activity_completion) and [moodle docs](https://docs.moodle.org/405/en/Tracking_progress#Completion_conditions)  

### Enable Completion tracking  
if enabled, the completion status for each activity can be tracked by teachers and students  

See [moodle docs](https://docs.moodle.org/405/en/Activity_completion_settings)  

### SummaryFormat  
the format in which the CourseSummary is stored  

corresponds to `<summaryformat>` in `/course/course.xml`, possible values:  

1 -> (probably) HTML  

### UsesLegacyFiles  
Moodle uses a new file system since version 2.0; this setting probably tells Moodle if this backup uses the new or old file system  
See [moodle docs](https://docs.moodle.org/405/en/Legacy_course_files)  

corresponds to `<summaryformat>` in `/course/course.xml`, possible values:  
0 -> backup uses the new file system (no legacy files)  

### CourseLanguage  
By default, Moodle detects a user's language from their browser setting and displays the course in the corresponding language.  
Using this setting, the teacher can force the course to use a specific language setting  

corresponds to `<lang>` in `/course/course.xml`, possible values:  
-> empty for "Do not force"  

### CourseCategory  
Course categories organize courses for all Moodle site participants. The default course category on a new Moodle site is "Category 1".  
A Course creator, Administrator, or Manager can put all courses in the Category 1 category. However, teachers and students will find it easier to find their classes if they are organized in descriptive categories.  
See [moodle docs](https://docs.moodle.org/405/en/Course_categories)  

corresponds to `<category id="1">` in `/course/course.xml`, possible values:  
1 -> default course category  

### Custom Fields  
Custom fields can be used to create field types to be used for custom fields. Instances of these field types can be added to the respective areas that implement Custom fields API.  
See [moodle docs](https://moodledev.io/docs/4.4/apis/plugintypes/customfield)  

corresponds to `<customfields>` in `/course/course.xml`, possible values:  
-> empty for "No custom fields"  
