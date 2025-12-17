# Course as Code - Developer Wiki
Course as Code is a tool that enables the creation of Moodle courses through text formats such as YAML or Markdown (hence the name _Course as Code_). These text files can then be converted into a Moodle backup file (.mbz), which may be imported directly into Moodle as a complete course. This approach allows fully functional Moodle courses to be created and edited using any text editor, while changes and collaboration can be managed efficiently with version control systems like Git.

This documentation contains parts aimed at both end-users, who want to use **Course as Code** to create their courses and import them into Moodle, as well as developers who want to contribute to *Course as Code* and/or adapt it to their personal needs.

*   For a step-by-step guide on how to create your first Moodle course with *Course as Code* and import it into a locally hosted Moodle environment, see [Usage](./Usage/index.md).
*   For a detailed overview of how *Course as Code* is structured internally, which components it consists of, and how they interact with each other, see [Architecture](./Architecture/index.md).
*   For information on the individual activities supported by *Course as Code* (features, implementation in the application, and specific characteristics), see [Activities](./Activities/index.md).
*   If you would like to learn how the grading of activities works in Moodle or in *Course as Code*, please read [Grading](./Grading/index.md).
*   The findings from reverse engineering the Moodle backup files (`.mbz`) are available in the correspondingly named section: [Moodle Backup File (.mbz)](./MoodleBackupFile/index.md).