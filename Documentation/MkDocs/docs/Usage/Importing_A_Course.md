# Importing a Course into Moodle

## Automatic Upload

To automatically upload and restore your course into Moodle, use the `--upload` (or `-u`) flag when running CourseAsCode:

```bash
# Build and automatically restore into Moodle:
CourseAsCode -i "/path/to/course" -o "/path/to/output" --upload

# When running from source:
dotnet run --project CourseAsCode/CourseAsCode.csproj -i "Documentation/exampleCourse" -o "output" --upload
```

This compiles your course into a `.mbz` file and immediately restores it into the local Moodle container.

---

## Manual Import via Web UI

If you build without the `--upload` flag, you can manually upload the `.mbz` file through the Moodle browser interface:

1. Log in to Moodle as an administrator or teacher.
2. Navigate to **Site administration** > **Courses** > **Restore course**.
3. Drag and drop your `.mbz` file into the upload box and click **Restore this course**.
4. Confirm the settings and select the destination category to finish the restore.
