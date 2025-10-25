# Grade Categories
Grade Categories are used to manage activities that can be graded within the gradebook.

```
- Course Overall
    - Theoretical Part   (60%)
        - Theory Test #1
        - Theory Test #2
        - Final Exam
    - Practical Part     (40%)
        - Exercise #1
        - Exercise #2
```

## Nested Grade Categories
In the example above, `Course Overall`, `Theoretical Part`, and `Practical Part` are all Grade Categories. As you can see, Grade Categories can be arranged hierarchically. `Theoretical Part` and `Practical Part` are children of `Course Overall`. The depth of these hierarchies is theoretically unlimited.

## Weighting
Each Grade Category can be given a weight, which is used to calculate how much its score contributes to the grade calculation of the parent category. In this example, the `Theoretical Part` counts for more than the `Practical Part`. So, if a user were to score `50/100 points` in the `Theoretical Part` and `80/100 points` in the `Practical Part`, this would result in an overall score of - depending on the calculation method, we'll assume the default one for this example -

```
50/100 = 50% | 0.5 (grade) * 0.6 (weight) = 0.3
80/100 = 80% | 0.8 (grade) * 0.4 (weight) = 0.32
0.3 + 0.32 = 0.62 | 62% of 200 = 124
```

`124/200 points` or `62%`.

!!! tip
    The sum of the weights of all child Grade Categories must always add up to exactly 100%.

    !!! example "Valid Distribution"
        ```
        - Course Overall
            - Theoretical Part       (50%)
                - Theory Test #1
                - Theory Test #2
                - Final Exam
            - Practical Part         (40%)
                - Exercise #1
                - Exercise #2
            - Collaboration in Class (10%)
        ```

        This would be a valid distribution, as 50 + 40 + 10 = 100%.

    !!! example "Invalid Distribution"
        ```
        - Course Overall
            - Theoretical Part       (60%)
                - Theory Test #1
                - Theory Test #2
                - Final Exam
            - Practical Part         (40%)
                - Exercise #1
                - Exercise #2
            - Collaboration in Class (10%)
        ```

        This would be an invalid distribution, as 60 + 40 + 10 = 110%.

## Grade Aggregation
In addition to weighting, you can also define how the individual Grade Items (activities) within a Grade Category are calculated together. The following aggregation methods are available:

-   **Mean of grades**
    - The sum of all grades divided by the total number of grades.
-   **Weighted mean**
    - Same as `Mean of grades`, but each grade item can be given a weight to change its importance in the overall mean.
-   **Simple weighted mean**
    - Same as `Weighted mean`, but the weight of each item is its Maximum grade.
-   **Mean of grades (with extra credits)**
    - Same as `Mean of grades`, but allows some Grade Items to be weighted more heavily. (Note: This is an older aggregation method used for backward compatibility).
-   **Median of grades**
    - The middle grade when grades are arranged in order of size.
-   **Lowest grade**
    - The lowest grade of any activity in the category.
-   **Highest grade**
    - The highest grade of any activity in the category.
-   **Mode of grades**
    - The grade that occurs most frequently.
-   **Natural (Default)**
    - The sum of all grade values scaled by weight.

A more detailed description is available in the [Moodle Documentation](https://docs.moodle.org/500/en/Gradebook_uses).

## Knockout Criteria (Must-Pass)
For each Grade Category, you can set a `GradeToPass`, which is a minimum score that must be achieved through the activities in that category for it to be considered passed. This allows you to specify that certain sub-sections of a course must be passed individually. A student would therefore fail if they do not pass a specific Grade Category, even if they have achieved enough points overall.

!!! example
    ```
    - Course Overall
        - Theoretical Part       (25%)  | Grade To Pass: 12/25 points
            - Theory Test #1
            - Theory Test #2
            - Final Exam
        - Practical Part         (25%)  | Grade To Pass: 12/25 points
            - Exercise #1
            - Exercise #2
        - Collaboration in Class (25%)  | Grade To Pass: 12/25 points
        - Oral Exam              (25%)  | Grade To Pass: 12/25 points
    ```

    If a student achieves 20 points each in `Theoretical Part`, `Practical Part`, and `Collaboration in Class`, but only 10 points in the `Oral Exam`, they have a total of 70 points and would normally have passed (70/100 -> 70% -> Satisfactory). However, because the threshold of 12 points for the `Oral Exam` was not met, the student in this example would fail the course regardless.