# Grade Items
Individual elements that appear in the gradebook under a specific [Grade Category](./Grade_Categories.md) are called **Grade Items**. These are usually activities such as quizzes or assignments, but you can also add custom entries that can then be graded manually.

## Grade Categories
Each Grade Item must be assigned to a specific [Grade Category](./Grade_Categories.md) under which it appears.

## Weighting
Like [Grade Categories](./Grade_Categories.md), the weighting of Grade Items can also be changed. For example, you can configure a specific activity to have a greater impact on the total grade within a particular [Grade Category](./Grade_Categories.md) than another.

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

In this example, the *Grade Item* `Final Exam` in the *Grade Category* `Theoretical Part` has a weight of 80% and therefore contributes much more to the overall assessment of the `Theoretical Part` than `Theory Test #1` and `#2`.

!!! note
    In principle, such a weighting can also be achieved simply by configuring the activities so that `Theory Test #1` and `Theory Test #2` are worth 10 points each, and the `Final Exam` is worth 80 points. However, for various reasons, it may be necessary to configure the activities so that the number of points students can achieve in them differs from the desired weighting in the final calculation. In these cases, adjusting the weighting of the Grade Items is useful.

## Settings
A number of settings can be configured for each Grade Item. These can also be set for [Grade Categories](Grade_Categories.md) — in which case, all Grade Items within that [Grade category](./Grade_Categories.md) will inherit its settings unless they are overridden on the Grade Item level.

The following settings are available:
- **Item name**
    - Allows you to set a custom name for the Grade Item in the gradebook. If not set, the name of the activity is displayed instead.
- **Item info**
    - Can be used to store more detailed information about the Grade Item. This is not displayed in the gradebook and is only visible to course administrators.
- **Grade to pass**
    - The number of points (not a percentage) that must be achieved to pass the Grade Item.
- **Grade type**
    - The type of grade that is awarded: None | Value (Default) | [Scale](https://docs.moodle.org/501/en/Scales) | Text Only
- **Overall decimal points**
    - The number of decimal places to display for a grade. This does not affect grade calculations.
- **Multiplicator**
    - A factor by which all grades for this Grade Item are multiplied.
    - For example, if the multiplicator is 2 and the maximum grade is 100, then all grades less than 50 are doubled, and all grades 50 and above are changed to 100.
- **Offset**
    - A number that is added to or subtracted from (can be negative) every grade.
- **Hidden**
    - Specifies whether the Grade Item should be visible to students.
- **Hidden until**
    - Specifies the date from which the Grade Item will no longer be hidden (e.g., if grades are to be released at a specific time).
- **Locked**
    - If a Grade Item is locked, its associated activities can no longer update its grade.
- **Locked after**
    - A date after which the Grade Item will be locked.

Detailed information about this can be found in the [Moodle Documentation](https://docs.moodle.org/501/en/Grade_items).