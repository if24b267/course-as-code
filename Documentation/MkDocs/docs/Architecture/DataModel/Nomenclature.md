# Nomenclature
When working with Moodle or **Course as Code**, you will likely encounter some technical terms like *Context*, *GroupMode*, or *Archetype*. All these terms describe specific features or configuration options of Moodle, and have been adopted into our project with the same nomenclature for reasons of traceability.

The definitions and meanings of the terms have been explained as best as possible in the codebase using Data Annotations. However, understanding the concepts is often non-trivial, which is why this page explains some of these abstract terms in more detail:

## GroupMode
Groups are used to divide students into groups within a course. The group mode defines how these groups are set up.  
Generally, the group mode can be set at the course level (`Course.cs`) and overridden at the activity level (`Activities/Activity.cs`).  

The group mode can be forced at the course level (see property `ForceGroupMode` in `Course.cs`), which means that the GroupMode can no longer be changed at the activity level.

| Value in Data Model | Value in Moodle XML | Description                                                                       |
|---------------------|---------------------|-----------------------------------------------------------------------------------|
| NoGroups            | 0                   | no groups are used in the course                                                  |
| SeparateGroups      | 1                   | students are divided into separate groups and can only see their own groups' work |
| VisibleGroups       | 2                   | students are divided into separate groups but can see other groups' work          |

See [moodle docs](https://docs.moodle.org/405/en/Groups#Group_modes) for more information

## Context
In Moodle, users can have different permissions in different places. For example, a user might be a Teacher in one course, and a Student in another, and so have different permissions in each place. These places are called contexts. 
Contexts are hierarchical, meaning they form a tree-like structure, with permissions inheriting downwards unless overridden.

![Context Tree](../../assets/Moodle-contexts.jpg)

| Value in Data Model | Value in Moodle XML | Description                                                                                      |
|---------------------|---------------------|--------------------------------------------------------------------------------------------------|
| System              | 10                  | highest hierarchy level, applies site-wide                                                       |
| User                | 30                  | the standard permission of a user are defined in their own context (uncertain information)       |
| CourseCategory      | 40                  |                                                                                                  |
| Course              | 50                  |                                                                                                  |
| Module              | 70                  | also called "Activity module", appears within courses or within the 'site' course (= front page) |
| Block               | 80                  | appear within courses or within the 'site' course (= front page), lowest hierarchy level         |

See [moodle docs](https://docs.moodle.org/405/en/Context) for more information

## Archetypes
Archetypes are blueprints for roles. They define a standard set of permissions that serve as a starting point for creating custom roles.  
There is no archetype for the administrator, as it is a special role that is not assigned to a user in the same way as other roles.

| Value in Data Model          | Value in Moodle XML | Description                                    |
|------------------------------|---------------------|------------------------------------------------|
| Manager                      | manager             | lesser administrator role                      |
| CourseCreator                | coursecreator       | can create courses                             |
| EditingTeacher               | editingteacher      | can manage and add content to courses          |
| NonEditingTeacher            | teacher             | can grade in courses but not edit them         |
| Student                      | student             | can access and participate in courses          |
| Guest                        | guest               | can view courses but not participate           |
| AuthenticatedUser            | user                | the role all logged-in users have              |
| AuthenticatedUserOnFrontpage | frontpage           | a logged-in user role for the front page only  |

See [moodle docs #1](https://docs.moodle.org/405/en/Creating_custom_roles#Role_archetypes) and [moodle docs #2](https://docs.moodle.org/405/en/Standard_roles) for more information

## Enrolment
The process of adding students to courses is called Enrolment. There are various methods of enrolling students into courses, once they have logged in to Moodle (= Enrolment methods):

| Subclass in Data Model | Value in Moodle XML | Description                                                |
|------------------------|---------------------|------------------------------------------------------------|
| GuestEnrolment         | guest               | users can view course materials but not participate        |
| ManualEnrolment        | manual              | the administrator or course teacher adds users manually    |
| SelfEnrolment          | self                | a user can choose to enrol him/herself into a course       |
| CohortSync             | TBD                 | users are part of a Cohort which is added to the course    |
| CourseMetaLink         | TBD                 | users enrolled in other courses are given automatic access |
| CategoryEnrolment      | TBD                 | users are enrolled in all courses in a category            |
| ExternalDatabase       | TBD                 | users are enrolled from a database such as Access, MySQL   |
| FlatFile               | TBD                 | users are enrolled with a csv file                         |
| IMSEnterprise          | TBD                 | users are enrolled with this standard XML file format      |
| LDAPEnrolment          | TBD                 | users are authenticated and then enrolled through LDAP     |
| MNetEnrolment          | TBD                 | users are enrolled via a linked Moodle site                |
| PayPalEnrolment        | TBD                 | users purchase enrolment with Paypal                       |

Only Guest Enrolment, Manual Enrolment and Self Enrolment are provided by Moodle core, the other methods are provided by plugins.  
See [moodle docs #1](https://docs.moodle.org/405/en/Course_enrolment), [moodle docs #2](https://docs.moodle.org/405/en/Enrolment_methods) and [moodle docs #3](https://docs.moodle.org/2x/pl/Enrolments) for more information

## FormattingFormat
Moodle provides text fields in various places where text can be saved.  
For many of these texts, the format in which they are stored must be specified in the backup.

| Value in Data Model | Value in Moodle XML | Description                                                                          |
|---------------------|---------------------|--------------------------------------------------------------------------------------|
| MoodleAutoFormat    | 0                   | see [moodle docs](https://docs.moodle.org/405/en/Formatting_text#Moodle_auto-format) |
| HTML                | 1                   | text saved as HTML                                                                   |
| PlainText           | 2                   | text saved as plain text                                                             |
| Markdown            | 4                   | text saved as Markdown                                                               |

See [moodle docs](https://docs.moodle.org/405/en/Formatting_text) for more information

## Component
The component refers to the subsystem or plugin responsible for the file, typically following Moodle's "Frankenstyle" naming convention. For example, `mod_forum` represents the forum module, and `block_html` represents the HTML block. Each subsystem is permitted to access only its own file areas, ensuring proper encapsulation and security.

| Value in Data Model | Value in Moodle XML | Description                                                                                                         |
|-------------|---------------------|---------------------------------------------------------------------------------------------------------------------|
| Course      | course              | files managed by the course itself, e.g. the course logo                                                            |
| Resource    | mod_resource        | files managed by a file resource within the course, see [moodle docs](https://docs.moodle.org/405/en/File_resource) |
| Forum       | mod_forum           | files managed by a forum within the course                                                                          |
| Glossary    | mod_glossary        | files managed by a glossary within the course                                                                       |
| HtmlBlock   | block_html          | files managed by a HTML Block within the course                                                                     |

See [moodle docs](https://moodledev.io/docs/5.0/apis/subsystems/files#file-areas) for more information

## FileArea
Within a component, files are further categorized into file areas, which are uniquely identified by a

- a `contextid` (see [Context](#Context))
- a full `component` name (see [Component](#Component))
- a file area type, e.g. `intro` or `post`
- a unique `itemid`

| Value in Data Model | Value in Moodle XML | Description                                                                                                   |
|---------------------|---------------------|---------------------------------------------------------------------------------------------------------------|
| Overview            | overviewfiles       | files managed by the course overview block, see [moodle docs](https://docs.moodle.org/405/en/Course_overview) |
| CourseContent       | content             | Main content of the course                                                                                    |

See [moodle docs](https://moodledev.io/docs/5.0/apis/subsystems/files#file-areas) for more information

## License
The licence given to a file when uploaded to Moodle does not in any way affect its visibility on a course.

| Value in Data Model                       | Value in Moodle XML | Description                                                                                                                                                 |
|-------------------------------------------|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| None                                      | unknown             | no license specified                                                                                                                                        |
| AllRightsReserved                         | allrightsreserved   | indicates that the copyright holder reserves all the rights  provided by copyright law, such as distribution, performance, and creation of derivative works |
| PublicDomain                              | public              | creative work to which no exclusive intellectual property rights apply                                                                                      |
| CreativeCommons                           | cc-4.0              | [Creative Commons - 4.0 International](https://creativecommons.org/licenses/by/4.0/)                                                                        |
| CreativeCommonsNonCommercial              | cc-nc-4.0           | [Creative Commons - NonCommercial 4.0 International](https://creativecommons.org/licenses/by-nc/4.0/)                                                       |
| CreativeCommonsNoDerivatives              | cc-nd-4.0           | [Creative Commons - NoDerivatives 4.0 International](https://creativecommons.org/licenses/by-nd/4.0/)                                                       |
| CreativeCommonsNonCommercialNoDerivatives | cc-nc-nd-4.0        | [Creative Commons - NonCommercial-NoDerivatives 4.0 International](https://creativecommons.org/licenses/by-nc-nd/4.0/)                                      |
| CreativeCommonsNonCommercialShareAlike    | cc-nc-sa-4.0        | [Creative Commons - NonCommercial-ShareAlike 4.0 International](https://creativecommons.org/licenses/by-nc-sa/4.0/)                                         |
| CreativeCommonsShareAlike                 | cc-sa-4.0           | [Creative Commons - ShareAlike 4.0 International](https://creativecommons.org/licenses/by-sa/4.0/)                                                          |

See [moodle docs](https://docs.moodle.org/405/en/Licences) for more information