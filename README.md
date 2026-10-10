# CourseAsCode
[![Deploy MkDocs Material to GitHub Pages](https://github.com/Davimalu/course-as-code/actions/workflows/mkdocs.yml/badge.svg)](https://github.com/Davimalu/course-as-code/actions/workflows/mkdocs.yml)
[![Test CourseAsCode](https://github.com/Davimalu/course-as-code/actions/workflows/test.yml/badge.svg)](https://github.com/Davimalu/course-as-code/actions/workflows/test.yml)
[![Publish CourseAsCode](https://github.com/Davimalu/course-as-code/actions/workflows/publish.yml/badge.svg)](https://github.com/Davimalu/course-as-code/actions/workflows/publish.yml)

Course as Code is a tool that enables the creation of Moodle courses through text formats such as YAML or Markdown (hence the name Course as Code). These text files can then be converted into a Moodle backup file (.mbz), which may be imported directly into Moodle as a complete course. This approach allows fully functional Moodle courses to be created and edited using any text editor, while changes and collaboration can be managed efficiently with version control systems like Git.

## Table of Contents

- [Requirements](#requirements)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Detailed Usage](#detailed-usage)
  - [Command Line Arguments](#command-line-arguments)
  - [Input Directory Structure](#input-directory-structure)
- [Supported Activity Types](#supported-activity-types)
  - [Supported Question Types](#supported-question-types)
- [VS Code Extension](#vs-code-extension)
  - [Features](#features)
  - [Development](#development)
  - [Building the VSIX](#building-the-vsix)
  - [Installing and Testing the VSIX](#installing-and-testing-the-vsix)
- [Troubleshooting](#troubleshooting)
- [Documentation](#documentation)

## Requirements

- .NET 8.0 SDK / Runtime or later
- **Windows** or **Linux** operating system (macOS *may* work, but is not officially supported)

For deployment:
- Moodle 4.5 or later (earlier versions *may* work, but are not officially supported)

See [Setting up a local Moodle Environment](https://davimalu.github.io/course-as-code/Usage/Moodle_Environment_Setup/) for instructions on how you can set up a local Moodle Environment on your machine!

## Installation

### Option 1: Download pre-built binaries (Recommended)

1. Go to the [Releases](https://github.com/Davimalu/course-as-code/releases) page
2. Download the appropriate package for your operating system
3. Extract the archive to a location of your choice
4. Add the directory to your PATH (optional, for convenience)

### Option 2: Build from source

```bash
# Clone the repository
git clone https://github.com/Davimalu/course-as-code.git
cd course-as-code

# Build the project
dotnet build

# Optional: Create a standalone executable
dotnet publish -c Release -r <runtime-identifier> --self-contained
```

Replace `<runtime-identifier>` with the appropriate value for your operating system (e.g., `win-x64`, `linux-x64`, `osx-x64`).

## Quick Start

1. Create a directory structure for your course following the [input directory structure](#input-directory-structure)
2. Write your course definition in YAML (use the examples in `Documentation/exampleCourse` as a reference)
3. Run CourseAsCode:

```bash
# Using the CLI
CourseAsCode -i "/path/to/your/course" -o "/path/to/output/directory"

# Or if running from source
dotnet run --project CourseAsCode/CourseAsCode.csproj -i "/path/to/your/course" -o "/path/to/output/directory"
```

4. Import the generated `.mbz` file into Moodle via the course restore functionality

## Detailed Usage

### Command Line Arguments

```
CourseAsCode -i <input-directory> -o <output-directory> [options]
```

Required arguments:
- `-i, --input <path>`: The input directory containing the YAML course definition
- `-o, --output <path>`: The output directory where the Moodle backup will be saved

Optional arguments:
- `-f, --format <format>`: The input format (currently only YAML is supported)
- `-h, --help`: Display help information
- `-v, --version`: Display version information

### Input Directory Structure

A typical course directory structure looks like this:

```
course-directory/
├── config.yaml          # Main course configuration
├── sections.yaml        # Defines section order and structure
├── Questions/           # Question definitions (optional)
│   ├── quiz-questions1.yaml
│   └── quiz-questions2.yaml
└── Sections/            # Individual section content
    ├── 01-Introduction/
    │   ├── section.yaml    # Section metadata
    │   └── Activities/     # Section activities
    │       ├── QUIZ_quizzes.yaml
    │       ├── TMA_grade-warning.yaml
    │       ├── URL_quiz-documentation.yaml
    │       ├── ASN_course-as-code-summary.yaml
    │       ├── FILE_moodle-backup-file-yaml
    │       ├── content/    # Content of activities like Text and Media Areas, e.g. Markdown or HTML files
    │       └── files/      # Files to be included in activities
    └── 02-Self-Study-1/
        ├── section.yaml
        └── Activities/
            └── ...
```

## Supported Activity Types

CourseAsCode supports the following activity types:

1. **Label (Text and Media Areas)**
   - Rich text content displayed directly on the course page

2. **URL**
   - External links with various display options

3. **File**
   - Uploaded files for students to download

4. **Quiz**
   - Interactive assessments with various question types

5. **Assignment**
   - Tasks for students to complete and submit work

### Supported Question Types (Quiz activity)

CourseAsCode supports the following question types for quizzes:

1. **Multiple Choice**
   - Single answer (radio buttons)
   - Multiple answers (checkboxes)

2. **Dropdown (Matching)**
   - Match items from one list with items from another using dropdown menus

3. **Drag and Drop Text (Lückentext)**
   - Fill in gaps in text by dragging words or phrases

4. **CodeRunner**
   - Programming tasks with automated execution and testing
   - Supports various programming languages (Java, C, Python, etc.)

## Examples

The project includes an example course, featuring all supported features in the `Documentation/exampleCourse` directory. Please use it as a template for your own courses.
```bash
# Copy the example course as a starting point
cp -r Documentation/exampleCourse my-new-course

# Customize it to fit your needs, then run CourseAsCode
CourseAsCode -i my-new-course -o my-output
```

## VS Code Extension

The project includes a VS Code extension (`CourseAsCode.VsExtension`) that provides editor support for CourseAsCode YAML files.

### Features

- **YAML validation** — schema-based validation for `config.yaml`, `section.yaml`, and all activity files
- **Snippets** — quick scaffolding for common YAML structures
- **Course Explorer** — sidebar tree view of your course structure (sections and activities)
- **Commands** (accessible via the Command Palette `Ctrl+Shift+P`):
  - `CourseAsCode: Init Course` — initialize a new course in the current folder
  - `CourseAsCode: Create Section` — add a new section to the course
  - `CourseAsCode: Create Activity` — add a new activity to a section
  - `CourseAsCode: Update Sections` — sync the sections list
  - `CourseAsCode: Refresh` — reload the Course Explorer tree view

### Development

Prerequisites: Node.js and npm installed.

```bash
cd CourseAsCode.VsExtension

# Install dependencies
npm install

# Compile TypeScript once
npm run compile

# Or watch for changes during development
npm run watch
```

To run and debug the extension inside VS Code, open the `CourseAsCode.VsExtension` folder and press `F5`. This launches a new **Extension Development Host** window with the extension loaded.

Run the unit tests with:

```bash
npm test
```

### Building the VSIX

Install the VS Code Extension CLI (`vsce`) if you haven't already:

```bash
npm install -g @vscode/vsce
```

Then package the extension from the extension directory:

```bash
cd CourseAsCode.VsExtension
vsce package
```

This produces a file like `courseascodetools-<version>.vsix` in the same directory.

### Installing and Testing the VSIX

**Via VS Code UI:**

1. Open VS Code
2. Go to the Extensions view (`Ctrl+Shift+X`)
3. Click the `...` menu (top-right of the Extensions panel)
4. Select **Install from VSIX...**
5. Choose the generated `.vsix` file

**Via command line:**

```bash
code --install-extension courseascodetools-0.1.0.vsix
```

After installation, reload VS Code and open a CourseAsCode project folder to verify that YAML validation, snippets, and the Course Explorer sidebar are working correctly.

## Troubleshooting

### Common Issues

1. **Import errors in Moodle**
   - Verify the Moodle version you're using is supported by CourseAsCode
   - Check for syntax errors in your YAML files
   - Check the application logs for any errors

2. **CodeRunner questions not working**
   - Ensure the target Moodle installation has the CodeRunner plugin installed
   - Verify the question type (e.g., `java_program`, `c_program`) is supported in the target Moodle instance

### Logs
Logging is performed using **Nlog**. The logs are written to:

- Console (basic information)
- `logs/app.log` (detailed)

## Documentation
Additional documentation is available at [if24b267.github.io/course-as-code](https://if24b267.github.io/course-as-code/)
