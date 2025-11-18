# [Moodle File](https://docs.moodle.org/500/en/File_resource)

The File activity refers to a file (of any type) that has been uploaded by the course creator (not the student!) and is available to students as learning material.

!!! info
    This activity is essentially just the shell that displays the file. The truly complex part is integrating the file into the Moodle backup and referencing it in the correct place. See the explicit documentation on File Handling for more on this! **TODO: Hyperlink**

Since this is a pretty simple activity, only a few parameters are necessary to integrate a file into Moodle:

- The file's **name** (`FileName`)
- The **path** to the file (`FilePath`)
- The file's **author** (`Author`) (_optional_)
- The file's **license** (`License`) (_optional_)