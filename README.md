# CourseAsCode

[![Publish & Test CourseAsCode](https://github.com/Davimalu/course-as-code/actions/workflows/test-and-publish.yml/badge.svg?branch=develop)](https://github.com/Davimalu/course-as-code/actions/workflows/test-and-publish.yml)

CourseAsCode is a powerful tool that enables you to define Moodle courses in code (YAML format) and generate Moodle backup (.mbz) files that can be imported into any Moodle installation. This approach brings software development principles to course creation, allowing for version control, collaboration, and automation.

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
  - [Moodle Development Environment Setup](#moodle-development-environment-setup)
- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Detailed Usage](#detailed-usage)
  - [Command Line Arguments](#command-line-arguments)
  - [Input Directory Structure](#input-directory-structure)
  - [YAML Format](#yaml-format)
- [Supported Activity Types](#supported-activity-types)
- [Supported Question Types](#supported-question-types)
- [Examples](#examples)
- [Development](#development)
  - [Building from Source](#building-from-source)
  - [Running Tests](#running-tests)
- [Troubleshooting](#troubleshooting)
- [Documentation](#documentation)

## Features

- Define Moodle courses in YAML, a human-readable data format
- Version control your course content with Git or other version control systems
- Generate complete Moodle backup files (.mbz) ready for import
- Support for multiple activity types:
  - Text and Media Areas (Labels)
  - URLs
  - Files
  - Quizzes with various question types
  - Assignments
- Support for rich question types:
  - Multiple Choice (single and multiple answers)
  - Dropdown (Matching)
  - Drag and Drop Text (Lückentext)
  - CodeRunner (programming exercises)
- Modular architecture allowing for future extensions

## Requirements

- .NET 8.0 SDK or later
- Windows, macOS, or Linux operating system

For deployment:
- Moodle 4.0 or later

### Moodle Development Environment Setup
To set up a local installation of Moodle for development, follow these instructions:

1. Create a new file: `docker-compose.yml`
2. Add the following content to the file:

```yaml
# Copyright Broadcom, Inc. All Rights Reserved.
# SPDX-License-Identifier: APACHE-2.0

services:
  mariadb:
    image: docker.io/bitnamilegacy/mariadb:latest
    environment:
      # ALLOW_EMPTY_PASSWORD is recommended only for development.
      - ALLOW_EMPTY_PASSWORD=yes
      - MARIADB_USER=bn_moodle
      - MARIADB_DATABASE=bitnami_moodle
      - MARIADB_CHARACTER_SET=utf8mb4
      - MARIADB_COLLATE=utf8mb4_unicode_ci
    volumes:
      - 'mariadb_data:/bitnami/mariadb'
  moodle:
    image: docker.io/bitnamilegacy/moodle:5.0
    ports:
      - '80:8080'
      - '443:8443'
    environment:
      - MOODLE_DATABASE_HOST=mariadb
      - MOODLE_DATABASE_PORT_NUMBER=3306
      - MOODLE_DATABASE_USER=bn_moodle
      - MOODLE_DATABASE_NAME=bitnami_moodle
      # ALLOW_EMPTY_PASSWORD is recommended only for development.
      - ALLOW_EMPTY_PASSWORD=yes
    volumes:
      - 'moodle_data:/bitnami/moodle'
      - 'moodledata_data:/bitnami/moodledata'
    depends_on:
      - mariadb

volumes:
  mariadb_data:
    driver: local
  moodle_data:
    driver: local
  moodledata_data:
    driver: local
```

3. Start the containers:

```bash
(sudo) docker compose up -d
```

4. Moodle is now reachable at `http://localhost`
5. Log in with the default credentials:

```txt
user: user
password: bitnami
```

## Installation

### Option 1: Download pre-built binaries

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

## Project Structure

CourseAsCode consists of several projects with distinct responsibilities:

- **CourseAsCode**: Main CLI application and entry point
- **CourseAsCode.Model**: Internal data model representing Moodle courses and activities
- **CourseAsCode.Readers**: Components for reading course definitions from various formats
- **CourseAsCode.Writers**: Components for writing Moodle backup files in the correct format
- **CourseAsCode.*.Tests**: Test projects for ensuring functionality

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
│   ├── questions1.yaml
│   └── questions2.yaml
└── Sections/            # Individual section content
    ├── Section1/
    │   ├── section.yaml    # Section metadata
    │   └── Activities/     # Section activities
    │       ├── label1.yaml
    │       ├── url1.yaml
    │       ├── file1.yaml
    │       ├── quiz1.yaml
    │       └── files/      # Files to be included in activities
    └── Section2/
        ├── section.yaml
        └── Activities/
            └── ...
```

### YAML Format

#### config.yaml

Contains the main course configuration:

```yaml
fullName: "My Course Title"
shortName: "MYCOURSE"
format: "topics"  # topics, weekly, social, etc.
language: "en"
startDate: "2023-09-01"
endDate: "2024-01-31"
```

#### sections.yaml

Defines the order and structure of course sections:

```yaml
sections:
  - path: "Sections/Introduction"
    number: 1
  - path: "Sections/Topic1"
    number: 2
  - path: "Sections/Topic2"
    number: 3
```

#### section.yaml

Contains metadata for an individual section:

```yaml
name: "Introduction to the Course"
summary: "Welcome to the course. This section introduces the main topics."
visible: true
```

#### Activity YAML files

Each activity has its own YAML file with specific properties. For example, a URL activity:

```yaml
type: "url"
name: "Course Website"
intro: "Link to our course website"
externalUrl: "https://example.com/course"
display: 0  # 0=automatic, 1=embed, 2=open, 3=popup
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

See the [examples](#examples) section for YAML samples of each activity type.

## Supported Question Types

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

See the [examples](#examples) section for YAML samples of each question type.

## Examples

The project includes complete examples in the `Documentation/exampleCourse` directory. You can use these as templates for your own courses:

```bash
# Copy the example course as a starting point
cp -r Documentation/exampleCourse my-new-course

# Customize it to fit your needs, then run CourseAsCode
CourseAsCode -i my-new-course -o my-output
```

## Development

### Building from Source

Prerequisites:
- .NET 8.0 SDK
- Git

Steps:
```bash
# Clone the repository
git clone https://github.com/Davimalu/course-as-code.git
cd course-as-code

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project CourseAsCode/CourseAsCode.csproj -- -i "/path/to/course" -o "/path/to/output"
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test CourseAsCode.Readers.Tests

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Troubleshooting

### Common Issues

1. **Missing files in the generated backup**
   - Ensure all referenced files exist in the correct locations
   - Check file paths are correctly specified in the YAML files

2. **Import errors in Moodle**
   - Verify the Moodle version compatibility
   - Check for syntax errors in YAML files
   - Examine the application logs for detailed error information

3. **CodeRunner questions not working**
   - Ensure the target Moodle installation has the CodeRunner plugin installed
   - Verify the question type (e.g., `java_program`, `c_program`) is supported in the target Moodle instance

### Logs
Logging is performed using **Nlog**. The logs are written to:

- Console (basic information)
- `logs/app.log` (detailed)

## Documentation
Additional documentation is available at [davimalu.github.io/course-as-code/](https://davimalu.github.io/course-as-code/)
