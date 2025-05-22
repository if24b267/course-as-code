# Analysis of Assignment Activity Backup Files Created by Moodle

!!! warning
    The insights described on this page originate from reverse engineering conducted **before** the activity was implemented in Course as Code. Since small errors often came to light during the implementation process, and these were (unfortunately) not always corrected accordingly in the reverse engineering documentation, it's possible that the following information is in some parts **incomplete** or **incorrect**.

    It is therefore recommended to always cross-reference the information with the implementation in the writer classes.

## General Information

### Assignment Information

-   **Name**: `TestAssignment` - The name of the assignment activity as displayed to users
    
-   **Module Name**: `assign` - Specifies the type of module (assignment activity)
    
-   **ID**: `2` - Unique ID for the assignment activity
    
-   **Module ID**: `13` - ID of the module instance within the course
    
-   **Context ID**: `68` - ID for the context in which this assignment exists
    
-   **Added Timestamp**: `1734429980` (Unix Timestamp) - Time when the assignment was added
    
-   **Visible**: `1` (Yes) - Indicates whether the assignment is visible to users
    

## Analysis of XML Files

### **assign.xml**

The `assign.xml` file contains the main configuration and structure of the assignment activity

#### Assignment Details

-   **Name**: `TestAssignment` - Name of the assignment
    
-   **Intro**: Empty - The introductory text for the assignment
    
-   **Intro Format**: `1` (HTML) - Specifies the format of the introductory text
    
-   **Always Show Description**: `1` - Description is always visible
    
-   **Submission Drafts**: `1` - Allows submission drafts before final submission
    
-   **Send Notifications**: `1` - Sends notifications to graders
    
-   **Send Late Notifications**: `0` - Does not send notifications for late submissions
    
-   **Send Student Notifications**: `1` - Sends notifications to students
    
-   **Due Date**: `1734998400` - Timestamp for the assignment's due date
    
-   **Cutoff Date**: `0` - No cutoff date is set
    
-   **Grading Due Date**: `1735603200` - Timestamp for the grading due date
    
-   **Allow Submissions From Date**: `1734393600` - Date when students can start submitting assignments
    
-   **Grade**: `100` - Maximum grade for the assignment
    
-   **Completion Submit**: `1` - Marks the activity complete when a submission is made
    
-   **Require Submission Statement**: `1` - Requires students to accept a submission statement
    
-   **Team Submission**: `0` - Team submissions are not enabled
    
-   **Require All Team Members Submit**: `0` - Does not require all team members to submit
    
-   **Team Submission Grouping ID**: `0` - No grouping ID specified
    
-   **Blind Marking**: `0`(Graders can see student identities while grading) - Blind marking is disabled
    
-   **Hide Grader**: `0` - Grader's identity is visible
    
-   **Reveal Identities**: `0` - Does not reveal student identities
    
-   **Attempt Reopen Method**: `untilpass` - Students can reopen attempts until they pass
    
-   **Max Attempts**: `1` - Allows only one submission attempt
    
-   **Marking Workflow**: `0` - Marking workflow is disabled
    
-   **Marking Allocation**: `0` - Marking allocation is disabled
    
-   **Marking Anonymous**: `0` - Marking is not anonymous
    
-   **Prevent Submission Not In Group**: `0` - Does not restrict submissions to groups
    
-   **Time Modified**: `1734430025` - Timestamp when the assignment was last modified
    
-   **Activity**: `<p>Introduction</p>` - Introductory content in HTML
    
-   **Activity Format**: `1` (HTML) - Format of the activity description
    
-   **Time Limit**: `0` - No time limit is set
    
-   **Submission Attachments**: `0` - File attachments are not required for submissions
    

#### **Submissions**

The assignment contains **2 submissions**:

-   **Submission ID**: `1`:
    
    -   **User ID**: `0` (Anonymous)
        
    -   **Time Created**: `1734429980`
        
    -   **Time Modified**: `1734429980`
        
    -   **Status**: `new` - Submission status is new
        
    -   **Attempt Number**: `0` - First attempt
        
    -   **Latest**: `1` - Indicates this is the latest submission
        
-   **Submission ID**: `2`:
    
    -   **User ID**: `2` - Submission by user with ID `2`
        
    -   **Time Created**: `1734430025`
        
    -   **Time Modified**: `1734430025`
        
    -   **Status**: `new` - Submission status is new
        
    -   **Attempt Number**: `0` - First attempt
        
    -   **Latest**: `1` - Indicates this is the latest submission
        

#### **Plugin Configurations**

The assignment activity includes the following configurations:

-   **Online Text Submission**:
    
    -   Enabled: `0` - Online text submission is disabled
        
-   **File Submission**:
    
    -   Enabled: `1` - File submission is enabled
        
    -   **Max File Submissions**: `20` - Allows up to 20 files
        
    -   **Max Submission Size**: `0` - No size restriction.
        
    -   **File Types List**: `document` - Restricts submissions to document file types
        
-   **Comments**:
    
    -   Enabled (Submission): `1` - Comments are enabled for submissions
        
    -   Enabled (Feedback): `1` - Comments are enabled for feedback
        
    -   Comment Inline: `0` - Inline comments are disabled
        
-   **Edit PDF Feedback**: Disabled (`0`)
    
-   **Offline Feedback**: Disabled (`0`)
    
-   **File Feedback**: Disabled (`0`)
    

----------

### **module.xml**

The `module.xml` file defines the assignment module and its placement within the course

#### Module Details

-   **Module ID**: `13` - ID for the module instance
    
-   **Version**: `2024100700` - Moodle version in which the module was created
    
-   **Module Name**: `assign` - Type of module
    
-   **Section ID**: `20` - ID for the course section
    
-   **Section Number**: `0` - Position of the module within the section
    
-   **ID Number**: `20` - Custom ID for the module
    
-   **Added Timestamp**: `1734429980` - Time when the module was added
    
-   **Score**: `0` - Default score value
    
-   **Indent**: `0` - Level of indentation in the course layout
    
-   **Visible**: `1` (Yes) - Indicates module visibility
    
-   **Visible on Course Page**: `1` - Whether the module is displayed on the course page
    
-   **Group Mode**: `0` (No groups) - Group settings applied to the module
    
-   **Completion**: `2`(Submitting a file) - Marks completion based on conditions
    
-   **Completion Grade Item Number**: `$@NULL@$` - Grade item associated with completion
    
-   **Completion Pass Grade**: `0` - Grade required to mark completion
    
-   **Completion View**: `0` - Whether viewing the activity counts towards completion
    
-   **Completion Expected**: `0` - Expected completion date
    
-   **Availability**: `$@NULL@$` - Custom availability restrictions
    
-   **Show Description**: `0` - Whether the description is shown on the course page
    
-   **Download Content**: `1` - Allows downloading of content for offline use
    
-   **Language**: Empty - Language applied to the module
    

----------

### **grades.xml**

The `grades.xml` file contains the grade information for the assignment

#### Grade Item Details

-   **Grade Item ID**: `36` - ID for the grade item
    
-   **Category ID**: `4` - Category to which this grade item belongs
    
-   **Item Name**: `TestAssignment` - Name of the grade item
    
-   **Item Type**: `mod` - Type of the item (module-related)
    
-   **Item Module**: `assign` - Module to which the grade item belongs
    
-   **Item Instance**: `2` - Instance ID of the module
    
-   **Grade Type**: `1` - Type of grading (value-based)
    
-   **Grade Max**: `100.00000` - Maximum grade achievable
    
-   **Grade Min**: `0.00000` - Minimum grade
    
-   **Grade Pass**: `50.00000` - Minimum grade required to pass
    
-   **Hidden**: `0` - Whether the grade item is visible
    
-   **Locked**: `0` - Whether the grade item is locked
    
-   **Time Created**: `1734429980` - Time when the grade item was created
    
-   **Time Modified**: `1734430025` - Last time the grade item was modified
    

----------

### **inforef.xml**

The `inforef.xml` file links users and grade items to the assignment

#### References

-   **User References**:
    
    -   User ID: `2` - References a user related to the assignment
        
    -   User ID: `4` - References another user
        
-   **Grade Item Reference**:
    
    -   Grade Item ID: `36` - Links to the assignment grade item
        

----------

### **comments.xml**

The `comments.xml` file tracks comments made within the assignment

-   **Comments**: Empty - No comments have been recorded
    

----------

### **competencies.xml**

The `competencies.xml` file tracks competencies linked to the assignment

-   **Competencies**: Empty - No competencies have been associated with this activity
    

----------
### **grade_history.xml**

The `grade_history.xml` records changes and updates to **grades over time**

Empty - No grade_history has been recorded
    

----------

### **completion.xml**

The `completion.xml` file tracks completion settings and statuses

#### Completion Views

-   Empty - No completion settings or statuses recorded


----------

### **calendar.xml**

The `calendar.xml` contains information about **calendar events** related to the activities

- Empty - No calendar entries have been recorded
    

----------


### **roles.xml**

The `roles.xml` file defines role assignments for the assignment

#### Role Assignments

-   No role overrides or assignments exist
    

----------
### **xapistate.xml**

The `xapistate.xml` file tracks xAPI states

#### States

-   Empty - No xAPI states recorded
    

----------



### **filters.xml**

The `filters.xml` file defines filter settings for the assignment

#### Filter Settings

-   **Filter Actives**: Empty - No active filters applied
    
-   **Filter Configs**: Empty - No configurations set for filters
    

----------

### **grading.xml**

The `grading.xml` file defines grading areas for the assignment

#### Grading Area Details

-   **Area ID**: `2` - ID for the grading area
    
-   **Area Name**: `submissions` - Grading area for assignment submissions
    
-   **Active Method**: `$@NULL@$` - No active grading method specified
    

----------

## Summary

The **TestAssignment** activity includes the following key points:

-   A maximum grade of **100** with a passing grade of **50**
    
-   File submissions are enabled, with a limit of **20 files**
    
-   Submission drafts and notifications are enabled
    
-   Two submissions have been recorded, with the latest submission by User ID `2`
    
-   The module is visible on the course page with default settings
    
-   Comments, filters, and competencies are not configured
