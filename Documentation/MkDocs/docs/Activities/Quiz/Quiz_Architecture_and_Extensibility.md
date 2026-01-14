### Quiz Architecture & Developer Guide

The **Quiz Activity** is the most complex module in Course as Code. Unlike a simple URL, a Quiz is a container for many different **Question Types** (Short Answer, Multiple Choice, Drag and Drop, etc.). 

Because every question looks different in the final Moodle XML, we use a modular system called the **Strategy Pattern**. This guide explains how it works and how you can add new question types.

<hr>
#### How data flows (The Lifecycle)

To handle different question types, the system follows a strict pipeline:

1.  **Reader:** Reads the generic YAML file and decides which type of question it is (e.g., "This is a SHORTANSWER").
2.  **Model:** Creates the specific C# object (e.g., `ShortAnswerQuestion`) and fills it with data.
3.  **Writer:** The `QuestionsWriter` asks the `StrategyResolver` for the right "Strategy" to turn this specific object into Moodle XML.

```plantuml
@startuml
!theme plain
skinparam componentStyle rectangle

package "1. Input (Reading)" {
    file "questions.yaml" as YAML
    component "YamlQuestionReader" as Reader
    note bottom of Reader:
        1. Reads YAML
        2. Decides Question Type
        3. Creates Object
}

package "2. Memory (Model)" {
    object "ShortAnswerQuestion" as Concrete
}

package "3. Output (Writing)" {
    component "QuestionsWriter" as Writer
    component "QuestionStrategyResolver" as Resolver
    component "ShortAnswerStrategy" as Strategy
    file "backup.mbz" as Output
}

YAML -> Reader : Read
Reader -> Concrete : Create
Concrete -> Writer : Add to list
Writer -> Resolver : "I need a writer for ShortAnswer!"
Resolver -> Strategy : "Here is the ShortAnswerStrategy."
Strategy -> Output : Writes XML
@enduml
```

<hr>
#### Guide: How to add a new Question Type

Follow these steps to implement a new Moodle question type (e.g., Numerical).

##### Step 0: The Prerequisite (Reverse Engineering)

Before writing any code, you must find out what Moodle expects in the XML file.:

1. Create: Go to Moodle, create a test course, and add a question of the type you want to implement.
2. Export: Create a course backup (`.mbz` file).
3. Inspect: Convert the `.mbz` to `.zip`, unzip it, and open the `questions.xml` file.
4. Target: Find your question and look at the `<plugin_qtype_...>` tag. This XML structure is exactly what your Strategy (Step 4) needs to generate.
    
<hr>
##### Step 1: Add the Name (Enum)

Add the new type to the list of supported questions.

File: `CourseAsCode.Readers.Enums.QuestionType.cs`

```
public enum QuestionType
{
    // ... other types ...
    NUMERICAL // <--- Add this
}
```
<hr>
##### Step 2: Create the Data Model

Create a class that defines what data this question needs. It must inherit from `Question`.

File: `CourseAsCode.Model.Questions.Types.Numerical.NumericalQuestion.cs`

Tip: Use `[Description]` attributes to help other developers

```
public class NumericalQuestion : Question
{
    [Description("How many errors are allowed (e.g. 0.1)")]
    public double Tolerance { get; set; }
    
    public List<NumericalAnswer> Answers { get; set; } = new();
}
```
<hr>
##### Step 3: Update the Reader & Mapping

To get your data from the file into the system, you need to update two places in the Reader layer:

1. The YAML Model: Open `CourseAsCode.Readers.YAML.Model.YamlQuestion.cs`. You must add your unique variables here.

2. The Mapping Logic: Open `CourseAsCode.Readers.YAML.Readers.YamlQuestionReader.cs`. Add a case to the `switch` statement and create a mapping method.

!!! warning "Mapping is mandatory" You must manually copy the data from the `YamlQuestion` (the raw input) to your `NumericalQuestion` (the internal model).

```
private NumericalQuestion ConvertYamlQuestionToNumericalQuestion(YamlQuestion yaml)
{
    var question = new NumericalQuestion();
    
    // 1. Map Base Properties (inherited from Question)
    question.QuestionName = yaml.QuestionName;
    question.QuestionText = yaml.QuestionText;
    question.DefaultMark = yaml.DefaultMark;

    // 2. Map your Unique Properties
    question.Tolerance = yaml.Tolerance; 

    return question;
}
```

<hr>
##### Step 4: Create the XML Writer (Strategy)

Create a class that knows how to write the specific XML tags you found in Step 0.

File: `CourseAsCode.Writers.XML.FileWriters.Questions.Strategies.NumericalStrategy.cs`

```
public class NumericalStrategy : IQuestionStrategy {
    private readonly ISequentialIdGenerator _idGenerator;

    public NumericalStrategy(ISequentialIdGenerator idGenerator) {
        _idGenerator = idGenerator;
    }

    public string GetQuestionTypeString() => "numerical";

    public void WritePluginData(IXmlService xmlService, Question question) {
        var model = (NumericalQuestion)question;
        xmlService.WriteElement("plugin_qtype_numerical_question");
        
        // Use _idGenerator.GetNextAnswerId() for any IDs needed in the XML
        xmlService.WriteElement("tolerance", model.Tolerance.ToString());
        
        xmlService.CloseElement();
    }
}
```

<hr>
##### Step 5: Connection & Registration

Finally, tell the application that your new Strategy exists.

1. Register Service: In `DependencyService.cs`, add: `services.AddTransient<NumericalStrategy>();`
2. Update Resolver: In `QuestionStrategyResolver.cs`, add the case: `case NumericalQuestion => _serviceProvider.GetRequiredService<NumericalStrategy>(),`

```
services.AddTransient<NumericalStrategy>();
```

Update the Resolver: In QuestionStrategyResolver.cs, tell it when to use your new strategy:

```
    case NumericalQuestion:
        return _serviceProvider.GetRequiredService<NumericalStrategy>();
```

<hr>

#### Best Practices & Testing
##### Unit Testing

It is Good Practice to write a test for your Strategy. If the XML is wrong, Moodle will fail to import the whole course.

Location: `CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies`
Method: Use `NSubstitute` to mock the `IXmlService`. Check if the correct tags and values are written.

##### Handling IDs

Never hardcode IDs (like `id="1"`). Moodle requires unique, sequential IDs. Always use the `ISequentialIdGenerator` injected via the constructor.

<hr>
#### Example Usage (YAML)

After implementing everything, you can use the new type in your `questions.yaml`:
YAML

```
  - QuestionName: Math Task
    QuestionType: NUMERICAL
    QuestionText: "What is 10 / 3?"
    Tolerance: 0.01
    Answers:
      - AnswerText: "3.33"
        AnswerGrade: 1.0
```
