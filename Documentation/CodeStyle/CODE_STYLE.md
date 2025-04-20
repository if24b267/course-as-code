# Code Style Guidelines

This project uses several code analysis and style enforcement tools to maintain consistent code quality across the codebase.

## Tools Used

### 1. .NET Analyzers
Built-in .NET analyzers provide code quality rules for design, performance, security, and other categories.

### 2. StyleCop Analyzers
StyleCop enforces a set of style and consistency rules for C# code. It helps maintain consistent coding styles across the codebase.

### 3. Roslynator
A collection of analyzers, refactorings, and fixes for C# that enhances the .NET Compiler Platform ("Roslyn").

### 4. EditorConfig
Defines coding styles directly in the repository to be shared across different editors and IDEs.

## Configuration Files

- `.editorconfig`: Contains code style settings for all files
- `Directory.Build.props`: Adds analyzer packages to all projects
- `stylecop.json`: StyleCop configuration
- `stylecop.ruleset`: Custom StyleCop rule settings
- `roslynator.config`: Roslynator-specific configuration

## Naming Conventions

- **Interfaces**: Start with 'I' (e.g., `IMyInterface`)
- **Classes, Structs, Enums**: PascalCase (e.g., `MyClass`)
- **Methods & Properties**: PascalCase (e.g., `MyMethod()`)
- **Private Fields**: Begin with underscore, then camelCase (e.g., `_myField`)
- **Parameters**: camelCase, with or without underscore prefix (both allowed)
- **Public Fields**: PascalCase (e.g., `MyField`)
- **Local Variables**: camelCase (e.g., `myVariable`)

## Coding Preferences

- **this qualifier**: Optional (using "this." is not enforced either way)
- **var**: Preferred when the type is apparent
- **Braces**: Required for all control structures (if, else, for, do, while)
- **Object creation**: Both styles allowed (`new MyObject()` and `new()`)
- **Line length**: No strict limit, but keep reasonable (consider 120 characters)
- **Formatting**: Flexible formatting is allowed (IDE0055 is disabled)
- **Blank lines**: Optional between adjacent elements (SA1516 is disabled)
- **Comments**: XML documentation for public members

## IDE Integration

### Visual Studio

These analyzers are automatically integrated with Visual Studio when you open the solution.

### Visual Studio Code

Install the following extensions:
- C# Dev Kit
- EditorConfig for VS Code

### JetBrains Rider

Rider has built-in support for .NET analyzers, StyleCop, and EditorConfig.

## CI Integration

Code style violations are checked during the CI pipeline. The build will fail if there are code style violations that are configured as errors.

## Suppressing Warnings

If you need to suppress a specific analyzer warning, use one of these approaches:

```csharp
// For a single line
#pragma warning disable CAXXXX
var x = SomeMethod();
#pragma warning restore CAXXXX

// For a code block
[SuppressMessage("Category", "CAXXXX:Description", Justification = "Reason for suppressing")]
public void SomeMethod() 
{
    // Method implementation
}
```

Only suppress warnings when you have a good reason to do so and always add a justification comment.