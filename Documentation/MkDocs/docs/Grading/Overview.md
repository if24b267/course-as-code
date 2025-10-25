# Grading
Some activities support grading (e.g., Quizzes and Assignments). Course As Code allows you to configure grading settings for these activities.

## Grade Categories
Individual activities can be assigned to a specific [Grade Category](./Grade_Categories.md). For each Grade Category, you can define default settings that are then inherited by every activity in that category. Additionally, you can specify how the total grade for the category is calculated from the individual activities within it.

In practice, Grade Categories can be used to represent a structure like the following, for example:

```
- Course Overall
    - Theoretical Part   (60%)
        - Theory Test #1         (10%)
        - Theory Test #2         (10%)
        - Final Exam             (80%)
    - Practical Part     (40%)
        - Exercise #1            (20%)
        - Exercise #2            (80%)
```

Each activity and each Grade Category can be specifically weighted. For each Grade Category, a minimum score can be set that the user must achieve to pass. For example, it can be specified that both the Theoretical Part and the Practical Part must be passed individually to receive a passing overall grade.

## Grade Items
Individual activities are referred to as [Grade Items](./Grade_Items.md) in the gradebook (e.g., Quizzes, Assignments, etc.). Each Grade Item can be assigned to a Grade Category. Grade Items inherit their grading settings from the Grade Category they are assigned to, but can manually overridden them if needed. Furthermore, the weighting can also be changed for each Grade Item within a Grade Category.