## Overview
Reader unit tests verify that the YAML parsers in `CourseAsCode.Readers` correctly deserialize text configurations, transform them into domain model entities (e.g. `Forum`, `URL`, `Quiz`), and handle invalid input gracefully without crashing.

All reader tests are located in:
```
CourseAsCode.Readers.Tests/YAML/Readers/
```

!!! info "Purpose of Reader Tests"
    A Reader test suite ensures that:
    
    1. Valid YAML files map correctly to all domain model properties.
    2. Non-existent files return `null` instead of throwing an unhandled exception.
    3. Missing required fields or malformed syntax are caught safely.
    4. Contextual identifiers from `ValueKeeper` (such as `CurrentActivityId`) are assigned.

## Test Class Architecture
Each reader test class should inherit from `TestBase`. This base class automatically manages an isolated temporary directory for test fixtures and resets global state between test runs.

!!! tip "Inheriting from TestBase"
    By inheriting from `TestBase`, you get:
    
    * `TempDirectory`: A dedicated temporary folder created for each individual test.
    * `CreateTestYamlFile(filename, content)`: A helper method that writes test YAML content to disk and returns the absolute path.
    * Automatic cleanup: Deletes temporary files in `[TearDown]`.

```csharp
[TestFixture]
public class YamlForumReaderTests : TestBase
{
    private YamlForumReader _reader;
    private IYamlFileReaderHelper _mockHelper;
```

## Setup & Mocking
To keep tests isolated and fast, mock the dependency `IYamlFileReaderHelper` using **NSubstitute** inside the `[SetUp]` method.

The mock should inspect the written file content and simulate two outcomes:
* **Valid YAML:** Returns a populated YAML DTO (e.g. `YamlForum`).
* **Missing Required Fields:** Throws a `YamlException` (simulating parser failure).

```csharp
[SetUp]
public void Setup()
{
    _mockHelper = Substitute.For<IYamlFileReaderHelper>();

    _mockHelper.DeserializeFromFile<YamlForum>(Arg.Any<string>())
        .Returns(callInfo =>
        {
            var path = callInfo.Arg<string>();
            if (!File.Exists(path)) return null;

            var content = File.ReadAllText(path);
            if (content.Contains("Name:"))
            {
                return new YamlForum
                {
                    Name = "Test Forum",
                    ForumType = ForumType.General,
                    Intro = "Forum Testing"
                };
            }

            // Simulate missing required field
            throw new YamlDotNet.Core.YamlException("Required property missing");
        });

    _reader = new YamlForumReader(_mockHelper);
    ValueKeeper.CurrentActivityId = 1;
}
```

## Standard Test Cases
A complete reader test suite should contain at least the following four core scenarios:

### 1. Non-Existent File
Verifies that passing a non-existent file path returns `null` without throwing an exception.

```csharp
[Test]
public void Read_FileDoesNotExist_ReturnsNull()
{
    var nonExistentPath = Path.Combine(TempDirectory, "nonexistent.yaml");

    var result = _reader.Read(nonExistentPath);

    Assert.That(result, Is.Null);
}
```

### 2. Valid File (Happy Path)
Verifies that a well-formed YAML file produces a fully mapped domain model instance.

```csharp
[Test]
public void Read_ValidFile_ReturnsForum()
{
    var path = CreateTestYamlFile("valid_forum.yaml", "Name: Test Forum\n");

    var result = _reader.Read(path);

    Assert.Multiple(() =>
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Forum"));
        Assert.That(result.ForumType, Is.EqualTo(ForumType.General));
        Assert.That(result.Intro, Is.EqualTo("Forum Testing"));
    });
}
```

### 3. Missing Required Fields
Verifies that invalid or incomplete YAML (e.g. missing `Name:`) is caught by the reader's `try-catch` block and returns `null`.

```csharp
[Test]
public void Read_MissingNameField_ReturnsNull()
{
    // Content lacks the required "Name:" field
    var path = CreateTestYamlFile("invalid_forum.yaml", "Intro: Without Name\n");

    var result = _reader.Read(path);

    Assert.That(result, Is.Null);
}
```

### 4. Context ID Assignment
Verifies that global context values managed by `ValueKeeper` are correctly applied to the generated activity.

```csharp
[Test]
public void Read_UsesCurrentActivityId()
{
    var path = CreateTestYamlFile("valid_forum.yaml", "Name: Test Forum\n");
    ValueKeeper.CurrentActivityId = 42;

    var result = _reader.Read(path);

    Assert.That(result.ActivityId, Is.EqualTo(42));
}
```

## Running the Tests
To execute reader tests from the command line, use the `dotnet test` CLI:

```bash
# Run all tests in the Readers.Tests project
dotnet test CourseAsCode.Readers.Tests/CourseAsCode.Readers.Tests.csproj

# Run only the specific reader test fixture
dotnet test --filter "FullyQualifiedName~YamlForumReaderTests"
```

!!! success "Review Checklist"
    Before submitting a pull request, ensure:

    - [x] Test class inherits from `TestBase`.
    - [x] `IYamlFileReaderHelper` is mocked with `NSubstitute`.
    - [x] Temporary files are created via `CreateTestYamlFile(...)`.
    - [x] All 4 standard scenarios are covered (Non-existent, Valid, Invalid, ActivityId).
    - [x] All tests run green locally via `dotnet test`.
