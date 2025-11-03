# Analysis of Wiki Activity Backup Files Created by Moodle

!!! warning
    The insights described on this page originate from reverse engineering conducted **before** the activity was implemented in Course as Code. Since small errors often came to light during the implementation process, and these were (unfortunately) not always corrected accordingly in the reverse engineering documentation, it's possible that the following information is in some parts **incomplete** or **incorrect**.

    It is therefore recommended to always cross-reference the information with the implementation in the writer classes.

## General Information

### Wiki Information

-   **Name**: `Test_Wiki` - The name of the wiki activity as displayed to users

-   **Module Name**: `wiki` - Specifies the type of module (wiki activity)

-   **ID**: `1` - Unique ID for the wiki activity

-   **Module ID**: `10` - ID of the module instance within the course

-   **Context ID**: `26` - ID for the context in which this wiki exists

-   **Added Timestamp**: `1762210519` (Unix Timestamp) - Time when the wiki was added

-   **Visible**: `1` (Yes) - Indicates whether the wiki is visible to users


## Analysis of XML Files

### **wiki.xml**

The `wiki.xml` file contains the main configuration and structure of the wiki activity

#### Wiki Details

-   **Name**: `Test_Wiki` - Name of the wiki

-   **Intro**: Empty - The introductory text for the wiki

-   **Intro Format**: `1` (HTML) - Specifies the format of the introductory text

-   **Time Created**: `0` - Timestamp when the wiki was created (0 indicates default/unspecified)

-   **Time Modified**: `1762210581` - Timestamp when the wiki was last modified

-   **First Page Title**: `Test_First_Page_Wiki` - The title of the wiki's initial page

-   **Wiki Mode**: `collaborative` - The collaboration mode of the wiki

-   **Default Format**: `html` - The default format for wiki pages

-   **Force Format**: `0` - Whether to enforce the default format (0 = No, users can choose)

-   **Edit Begin**: `0` - Start time for editing period (0 = no restriction)

-   **Edit End**: `0` - End time for editing period (0 = no restriction)


#### Wiki Mode

The wiki mode determines how users can collaborate on the wiki:

| Value in Moodle XML | Description                                                                                     |
|---------------------|-------------------------------------------------------------------------------------------------|
| collaborative       | All students can edit all pages in the wiki                                                     |
| individual          | Each student has their own wiki, which only they can edit                                       |

#### Default Format

The default format determines how wiki content is authored and displayed:

| Value in Moodle XML | Description                                           |
|---------------------|-------------------------------------------------------|
| html                | HTML editor (rich text)                               |
| creole              | Creole wiki markup language                           |
| nwiki               | NWiki wiki markup language                            |

#### **Subwikis**

A wiki contains one or more subwikis. Subwikis are separate wiki instances that can be associated with groups or individual users.

-   **Subwiki ID**: `1` - Unique identifier for the subwiki

-   **Group ID**: `0` - ID of the group this subwiki belongs to (0 = no group)

-   **User ID**: `0` - ID of the user this subwiki belongs to (0 = not user-specific)


#### **Pages**

Each subwiki contains one or more pages:

-   **Page ID**: `1` - Unique identifier for the page

-   **Title**: `Test_First_Page_Wiki` - The title of the page

-   **Cached Content**: `<p>First Edit</p>` - The rendered HTML content of the page

-   **Time Created**: `1762210538` - Timestamp when the page was created

-   **Time Modified**: `1762210606` - Timestamp when the page was last modified

-   **Time Rendered**: `1762210606` - Timestamp when the content was last rendered

-   **User ID**: `2` - ID of the user who last modified the page

-   **Page Views**: `5` - Number of times the page has been viewed

-   **Read Only**: `0` - Whether the page is read-only (0 = No, 1 = Yes)


#### **Versions**

Each page maintains a version history of all edits:

-   **Version ID**: `1` / `2` - Unique identifier for each version

-   **Content**: Empty / `<p>First Edit</p>` - The content of this version

-   **Content Format**: `html` - The format of the content

-   **Version Number**: `0` / `1` - Sequential version number (starts at 0 for initial creation)

-   **Time Created**: `1762210538` / `1762210606` - Timestamp when this version was created

-   **User ID**: `2` - ID of the user who created this version


#### **Synonyms**

-   Empty - No synonyms (page title aliases) have been defined


#### **Links**

-   Empty - No internal wiki links have been created


----------

### **module.xml**

The `module.xml` file defines the wiki module and its placement within the course

#### Module Details

-   **Module ID**: `10` - ID for the module instance

-   **Version**: `2024100700` - Moodle version in which the module was created

-   **Module Name**: `wiki` - Type of module

-   **Section ID**: `7` - ID for the course section

-   **Section Number**: `5` - Position of the section within the course

-   **ID Number**: Empty - Custom ID for the module

-   **Added Timestamp**: `1762210519` - Time when the module was added

-   **Score**: `0` - Default score value

-   **Indent**: `0` - Level of indentation in the course layout

-   **Visible**: `1` (Yes) - Indicates module visibility

-   **Visible on Course Page**: `1` - Whether the module is displayed on the course page

-   **Visible Old**: `1` - Previous visibility state (used during visibility changes)

-   **Group Mode**: `1` (Separate groups) - Group settings applied to the module

-   **Grouping ID**: `0` - ID of the grouping (0 = no grouping)

-   **Completion**: `0` (No completion tracking) - Completion tracking setting

-   **Completion Grade Item Number**: `$@NULL@$` - Grade item associated with completion

-   **Completion Pass Grade**: `0` - Grade required to mark completion

-   **Completion View**: `0` - Whether viewing the activity counts towards completion

-   **Completion Expected**: `0` - Expected completion date (0 = no date set)

-   **Availability**: `$@NULL@$` - Custom availability restrictions

-   **Show Description**: `0` - Whether the description is shown on the course page

-   **Download Content**: `1` - Allows downloading of content for offline use

-   **Language**: Empty - Language applied to the module


#### Group Mode Values

| Value in Moodle XML | Description                                                              |
|---------------------|--------------------------------------------------------------------------|
| 0                   | No groups                                                                |
| 1                   | Separate groups - each group works in their own wiki, can't see others  |
| 2                   | Visible groups - each group works in their own wiki, but can see others |

----------

### **grades.xml**

The `grades.xml` file contains the grade information for the wiki

-   **Grade Items**: Empty - Wiki activities typically don't have automatic grading

-   **Grade Letters**: Empty - No custom grade letters defined


----------

### **inforef.xml**

The `inforef.xml` file links users to the wiki

#### References

-   **User References**:

    -   User ID: `2` - References a user related to the wiki (likely the page author)


----------

### **comments.xml**

The `comments.xml` file tracks comments made within the wiki

-   **Comments**: Empty - No comments have been recorded


----------

### **competencies.xml**

The `competencies.xml` file tracks competencies linked to the wiki

-   **Competencies**: Empty - No competencies have been associated with this activity


----------

### **grade_history.xml**

The `grade_history.xml` records changes and updates to **grades over time**

-   Empty - No grade history has been recorded


----------

### **completion.xml**

The `completion.xml` file tracks completion settings and statuses

#### Completion Views

-   Empty - No completion settings or statuses recorded

----------

### **calendar.xml**

The `calendar.xml` contains information about **calendar events** related to the activity

-   Empty - No calendar entries have been recorded


----------

### **roles.xml**

The `roles.xml` file defines role assignments for the wiki

#### Role Assignments

-   No role overrides or assignments exist


----------

### **xapistate.xml**

The `xapistate.xml` file tracks xAPI states

#### States

-   Empty - No xAPI states recorded


----------

### **filters.xml**

The `filters.xml` file defines filter settings for the wiki

#### Filter Settings

-   **Filter Actives**: Empty - No active filters applied

-   **Filter Configs**: Empty - No configurations set for filters


----------

## Summary of Required Parameters for Wiki Implementation

Based on the analysis of the backup files, the following parameters are required to implement a Wiki activity in Course as Code:

### Essential Parameters

1.  **Name** (string) - The name of the wiki activity

2.  **Intro** (string) - The introductory/description text for the wiki

3.  **Intro Format** (int/enum) - The format of the intro text (HTML, Plain Text, Markdown)

4.  **First Page Title** (string) - The title of the initial wiki page

5.  **Wiki Mode** (string/enum) - The collaboration mode (`collaborative` or `individual`)

6.  **Default Format** (string/enum) - The default format for wiki content (`html`, `creole`, or `nwiki`)

7.  **Force Format** (boolean) - Whether to enforce the default format

8.  **Edit Begin** (timestamp/datetime) - Optional start time for editing period

9.  **Edit End** (timestamp/datetime) - Optional end time for editing period


### Subwiki Parameters

10. **Group ID** (int) - For group-based wikis (0 for no group)

11. **User ID** (int) - For individual wikis (0 for shared wiki)


### Page Parameters (for initial page)

12. **Page Title** (string) - Title of the page (can be same as First Page Title)

13. **Page Content** (string) - The initial content of the first page

14. **Read Only** (boolean) - Whether the page should be read-only


### Module-Level Parameters

15. **Group Mode** (int/enum) - How groups interact with the wiki (0=None, 1=Separate, 2=Visible)

16. **Visible** (boolean) - Whether the wiki is visible to students

17. **Show Description** (boolean) - Whether to display the description on the course page


### Optional/Advanced Parameters

-   Timestamps (created, modified, rendered) - Can be auto-generated

-   Version history - Managed by Moodle after creation

-   Page views - Tracked by Moodle

-   Synonyms and internal links - Created by users during wiki usage

-   Comments - Added by users after wiki creation


----------

## Notes 

1.  **Initial Structure**: When creating a wiki through Course as Code, it should include at minimum one subwiki with one page (the first page)

2.  **Version History**: The initial page should have at least one version record (version 0) representing its creation

3.  **Content Format**: Support for multiple formats (HTML, Creole, NWiki) should be considered, with HTML being the most common

4.  **Collaboration Modes**: The two main modes (collaborative vs individual) significantly affect the wiki structure and should be clearly configurable

5.  **Time Restrictions**: Edit begin/end times are optional but useful for time-limited collaborative activities

6.  **Group Integration**: The wiki integrates with Moodle's group system through group_mode, group_id, and grouping_id
