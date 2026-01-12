# Assignment Activity - User Guide

The **Assignment Activity** allows students to submit their work for grading and feedback. This guide shows you the most important settings for creating assignments in your YAML files.

## Table of Contents

- [Quick Start](#quick-start)
- [Important Settings](#important-settings)
- [Examples](#examples)
- [Property Reference](#property-reference)

---

## Quick Start

Create a simple assignment by adding a YAML file to your `Activities` folder:

```yaml
Type: ASSIGNMENT
Name: Week 1 Homework
Description: Submit your homework assignment here
Intro: Complete the exercises from Chapter 1 and upload your solutions.
DueDate: 2026-05-15 23:59
Grade: 100
```

**What this does:**
- Creates a basic assignment with essential properties
- Sets a due date for May 15, 2026 at 23:59
- Maximum grade of 100 points
- Students see the description and intro text explaining the task
- Simple configuration - perfect for getting started quickly

---

## Important Settings

### Required Properties

```yaml
Type: ASSIGNMENT           # Must be ASSIGNMENT
Name: Assignment Title     # Shown to students
Description: Brief info    # Summary text
```

### Dates and Deadlines

**Four important dates:**

- **`AllowSubmissionsFromDate`** - When students can start submitting
- **`DueDate`** - Expected deadline
- **`CutoffDate`** - Hard deadline (no submissions after this)
- **`GradingDueDate`** - When grading should be complete

```yaml
# Simple assignment with just a due date
DueDate: 2026-05-15 23:59

# Assignment with grace period
DueDate: 2026-05-15 23:59
CutoffDate: 2026-05-17 23:59  # 2 extra days, marked as late

# Timed exam
AllowSubmissionsFromDate: 2026-05-10 09:00
DueDate: 2026-05-10 12:00  # 3-hour window
```

**Date Format:** `YYYY-MM-DD HH:MM` or `DD-MM-YYYY HH:MM`

### Grading

```yaml
Grade: 100                    # Maximum points
GradeCategory: Homework       # Links to gradebook category
GradeToPass: 60              # Minimum to pass
CompletionSubmit: true       # Auto-complete on submission
```

### Notifications

```yaml
SendNotifications: true              # Notify teacher on submission
SendLateNotifications: true          # Notify on late submission
SendStudentNotifications: true       # Confirm to student
```

### Submission Options

```yaml
AllowSubmissionDrafts: true          # Students can save drafts
RequireSubmissionStatement: true     # Academic integrity pledge
MaxAttempts: 3                       # Allow multiple submissions
AttemptReopenMethod: manual          # How to allow resubmissions
```

### Team Work

```yaml
TeamSubmission: true                 # Enable group submission
RequireAllTeamMembersSubmit: true   # All members must confirm
TeamSubmissionGroupingId: 1         # Link to group setup
```

### Display

```yaml
Visible: true                        # Show to students
AlwaysShowDescription: false         # Hide intro until open date
```

### Advanced

```yaml
BlindMarking: true                   # Hide student names while grading
HideGrader: true                     # Hide grader name from students
MarkingWorkflow: true                # Multi-stage grading
```

---

## Examples

### Example 1: Simple Homework

**Use case:** Quick weekly homework assignments with basic settings. Perfect for regular coursework that doesn't need drafts or complex grading.

```yaml
Type: ASSIGNMENT
Name: Week 1 Homework
Description: Chapter 1 exercises
Intro: Complete exercises 1-10. Show all work.
DueDate: 2026-02-08 23:59         # Simple deadline, no grace period
Grade: 20                          # Typical points for weekly assignments
SendNotifications: true            # Get notified when students submit
```

**What this does:**
- Simple deadline without grace period - students know exactly when to submit
- 20 points maximum (appropriate for weekly homework)
- Notifications enabled so you're informed about submissions
- No drafts needed for smaller assignments - keeps it simple

### Example 2: Essay with Drafts

**Use case:** Long-term writing assignments where students need time to work and revise. Includes draft saving, grace period, academic integrity pledge, and gradebook integration.

```yaml
Type: ASSIGNMENT
Name: Research Essay
Description: 2000-word research paper
Intro: |
  Requirements:
  - 2000-2500 words
  - Minimum 5 sources
  - APA format
  
AllowSubmissionDrafts: true        # Students can save drafts and edit multiple times
DueDate: 2026-04-15 23:59          # Expected deadline
CutoffDate: 2026-04-17 23:59       # 2-day grace period (marked as late)
Grade: 100                         # Higher points for major assignments
GradeCategory: Essays              # Links to gradebook category for organization
RequireSubmissionStatement: true   # Academic integrity pledge required
SendNotifications: true            # Get notified about submissions
```

**What this does:**
- Students can save work-in-progress drafts before final submission
- 2-day grace period after due date - submissions accepted but marked as late
- Integrated with gradebook category for better grade organization
- Requires students to confirm academic integrity statement
- 100 points appropriate for substantial writing assignments

### Example 3: Group Project

**Use case:** Team assignments where all members must acknowledge submission. Ensures everyone is aware of what's being submitted and prevents single-person submissions.

```yaml
Type: ASSIGNMENT
Name: Team Project
Description: Develop a web application
TeamSubmission: true               # Enable group submission mode
RequireAllTeamMembersSubmit: true  # All team members must confirm
TeamSubmissionGroupingId: 1        # Links to pre-configured group structure
DueDate: 2026-05-31 23:59
Grade: 200                         # Higher points for comprehensive projects
GradeCategory: Projects
```

**What this does:**
- Enables team submission mode for collaborative work
- All team members must confirm the submission - prevents single person from submitting without others knowing
- Links to existing group configuration (groups must be set up beforehand)
- 200 points reflect the substantial effort required for team projects
- One submission per group, but all members receive the same grade

### Example 4: Online Exam

**Use case:** Time-limited examinations with specific start and end times. Opens at a precise time and closes automatically after the exam duration.

```yaml
Type: ASSIGNMENT
Name: Midterm Exam
Description: 2-hour timed exam
AllowSubmissionsFromDate: 2026-03-15 10:00  # Exam opens at exactly 10:00 AM
DueDate: 2026-03-15 12:00                   # Closes automatically at 12:00 PM (2h window)
Grade: 100
RequireSubmissionStatement: true            # Confirm independent work
MaxAttempts: 1                              # Only one attempt allowed
```

**What this does:**
- Exam opens exactly at 10:00 AM - students cannot submit before this time
- Closes automatically at 12:00 PM - creates a 2-hour time window for all students
- Only one submission attempt (typical for examinations)
- Requires confirmation of independent work (academic integrity)
- No CutoffDate means no grace period - hard deadline enforced

---

## Property Reference

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Type` | String | Required | Must be `ASSIGNMENT` |
| `Name` | String | Required | Assignment title |
| `Description` | String | Required | Brief description |
| `Intro` | String | `""` | Detailed instructions |
| `DueDate` | DateTime | None | Deadline |
| `CutoffDate` | DateTime | None | Hard deadline |
| `AllowSubmissionsFromDate` | DateTime | None | Open date |
| `GradingDueDate` | DateTime | None | Grading deadline |
| `Grade` | Integer | `100` | Maximum points |
| `GradeCategory` | String | None | Gradebook category |
| `GradeToPass` | Float | `0` | Passing score |
| `CompletionSubmit` | Boolean | `true` | Complete on submit |
| `AllowSubmissionDrafts` | Boolean | `false` | Enable drafts |
| `SendNotifications` | Boolean | `false` | Notify teachers |
| `SendLateNotifications` | Boolean | `false` | Notify on late |
| `SendStudentNotifications` | Boolean | `false` | Confirm to student |
| `RequireSubmissionStatement` | Boolean | `false` | Integrity pledge |
| `MaxAttempts` | Integer | `1` | Submission attempts |
| `AttemptReopenMethod` | String | `none` | `none`, `manual`, `untilpass` |
| `TeamSubmission` | Boolean | `false` | Group work |
| `RequireAllTeamMembersSubmit` | Boolean | `false` | All confirm |
| `BlindMarking` | Boolean | `false` | Anonymous grading |
| `Visible` | Boolean | `true` | Show to students |

---

## Quick Tips

* **Use clear instructions** in the `Intro` field  
* **Set realistic deadlines** - avoid midnight if possible  
* **Add grace periods** with `CutoffDate` for major work  
* **Group assignments** with `GradeCategory`  
* **Enable notifications** to track submissions  
* **Test your assignments** in a sandbox course first
