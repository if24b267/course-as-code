# Setting up CourseAsCode locally

This guide describes how to set up the CourseAsCode development environment on Windows and verify that the application works correctly.

## Prerequisites

- Git
- Visual Studio Code
- .NET 8 SDK or later
- Docker Desktop with Docker Compose
- PowerShell

Make sure Docker Desktop is running before starting the Moodle environment.

## 1. Clone the repository

```powershell
git clone https://github.com/if24b267/course-as-code.git
cd course-as-code
code .
```

## 2. Start Moodle

The repository already contains a `docker-compose.yml` file.

```powershell
docker compose up -d
```

Check the running containers:

```powershell
docker compose ps
```

Both the Moodle and MariaDB containers should be running.

Open `http://localhost` in your browser.

For additional Moodle configuration instructions, see [Setting up a local Moodle Environment](Moodle_Environment_Setup.md).

## 3. Build the C# solution

```powershell
dotnet restore
dotnet build CourseAsCode.sln
```

The build should complete without errors. Depending on the current codebase, analyzer or code-style warnings may appear.

## 4. Run automated tests

```powershell
dotnet test CourseAsCode.sln --no-restore
```

If tests fail because the Playwright Chromium executable is missing, install the required browser.

On Windows PowerShell, run:

```powershell
powershell -ExecutionPolicy Bypass -File ".\CourseAsCode.Uploader.Tests\bin\Debug\net8.0\playwright.ps1" install chromium
```

The script path assumes that the solution has already been built for .NET 8.

After installing Chromium, run the tests again.

## 5. Generate a Moodle course

The repository contains an example course in `Documentation/exampleCourse`.

Create an output directory:

```powershell
New-Item -ItemType Directory -Force -Path .\output
```

Generate the Moodle backup:

```powershell
dotnet run --project .\CourseAsCode\CourseAsCode.csproj -- -i ".\Documentation\exampleCourse" -o ".\output"
```

The application reads the YAML course definition and generates a Moodle backup (`.mbz`).

## 6. Import the generated course into Moodle

1. Open `http://localhost` and log in as an administrator.
2. Navigate to Site administration > Courses > Restore course.
3. Upload the generated `.mbz` file.
4. Select "Restore as a new course".
5. Choose a course category.
6. Complete the restore wizard.
7. Verify that the course sections and activities were imported correctly.

## Common issues

### Docker containers are not running

Make sure Docker Desktop is running and check:

```powershell
docker compose ps
docker compose logs moodle --tail=100
```

### Playwright browser executable is missing

Install Chromium using the Playwright installation script described in section 4.

### Moodle plugin dependencies are missing

CodeRunner requires the "Adaptive adapted for CodeRunner" question behaviour plugin. Install the compatible dependency before completing the CodeRunner installation.

### Moodle course import fails

Check that the Moodle instance is running, the required plugins are installed, and the generated `.mbz` file exists.

Do not use `docker compose down -v` unless you intentionally want to delete the persistent Moodle and MariaDB data.
