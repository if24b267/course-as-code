# Importing a Course into Moodle

## Automatic Deployment

To automatically restore your course into Moodle, use the `--deploy <container>` flag when running CourseAsCode:

```bash
# Build and automatically restore into the given Moodle Docker container:
CourseAsCode -i "/path/to/course" -o "/path/to/output" --deploy course-as-code-moodle-1

# When running from source:
dotnet run --project CourseAsCode/CourseAsCode.csproj -i "Documentation/exampleCourse" -o "output" --deploy course-as-code-moodle-1
```

`<container>` is the name or ID of the Docker container running Moodle (see `docker ps`). If you don't know it, use `--upload` (or `-u`) instead: it does the same, but detects a running container whose name contains `moodle` automatically.

This compiles your course into a `.mbz` file and immediately restores it as a new course into category `1` (Miscellaneous).

### How it works

Deployment is abstracted by the `IDeployer` interface (`CourseAsCode.Uploader`), implemented by `MoodleDeployer`. It uses Moodle's own CLI restore script:

1. `docker cp <file>.mbz <container>:/tmp/...` copies the backup into the container.
2. `docker exec <container> php /opt/bitnami/moodle/admin/cli/restore_backup.php --file=... --categoryid=1` restores it.
3. The temporary file is removed from the container again.

We chose this approach over automating the web UI (Playwright, see `MoodleUploader`) because:

- `restore_backup.php` is Moodle's official, stable restore entry point; UI automation breaks whenever the Moodle UI changes.
- No login credentials and no browser installation are required.
- The restore runs synchronously and reports success via its exit code, independent of the *asynchronous backup/restore* setting.

The trade-off: the Moodle instance must run in a Docker container reachable via `docker exec` (e.g. the local setup from [Moodle Environment Setup](Moodle_Environment_Setup.md)). Remote Moodle servers are not supported.

---

## Manual Import via Web UI

If you build without `--deploy`/`--upload`, you can manually upload the `.mbz` file through the Moodle browser interface:

1. Log in to Moodle as an administrator or teacher.
2. Navigate to **Site administration** > **Courses** > **Restore course**.
3. Drag and drop your `.mbz` file into the upload box and click **Restore this course**.
4. Confirm the settings and select the destination category to finish the restore.
