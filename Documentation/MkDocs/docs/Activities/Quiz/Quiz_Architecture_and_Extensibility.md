### Quiz Architecture & Developer Guide

The Quiz Activity is unique. Unlike a simple URL or a File, a Quiz acts like a "container" that can hold many different types of questions (like Essay, Multiple Choice, etc.).

Because every question type looks different in the code, we built the system to be very flexible. This guide explains how it works and how you can add new question types.

<hr>
#### How data flows (The Lifecycle)

To handle the different question types, the system follows a strict pipeline:

Reader: Reads the generic YAML file and figures out exactly which type of question it is (e.g., "This is a ShortAnswer").

Model: Creates the specific C# object for that question in memory.

Writer: When saving the file, the system looks for the specific "Writer Strategy" that knows how to turn this exact question into Moodle XML.

```
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
```

<hr>
#### Guide: How to add a new Question Type
##### Step 0: Find out what the XML looks like

Before you write code, you need to know what Moodle expects. The easiest way is to copy an existing question from Moodle:

Create: Go to Moodle, create a test course, and create a question of the type you want (e.g., "Numerical").

Export: Make a backup of this course (.mbz file).

Look: Unzip the backup file and open questions.xml.

Find: Search for your question. You will see a tag like `<plugin_qtype_numerical_question>`. This is the part that your code needs to generate later.
<hr>
##### Step 1: Add the Name (Enum)

First, we need to give the new type a name in our system.

File: CourseAsCode.Readers.Enums.QuestionType.cs

```
public enum QuestionType
{
    // ... other types ...
    NUMERICAL // <--- Add this
}
```
<hr>
##### Step 2: Define the Data Fields (Model)

Create a class that defines what data this question has. It must inherit from Question.

File: CourseAsCode.Model.Questions.Types.Numerical.NumericalQuestion.cs

Tip: Use [Description] to explain what the fields do.

```
public class NumericalQuestion : Question
{
    [Description("How much error is allowed (e.g. 0.1)")]
    public double Tolerance { get; set; }
    
    public List<NumericalAnswer> Answers { get; set; } = new();
}
```
<hr>
##### Step 3: Update the Reader

Now, teach the Reader how to load your new class from the YAML file.

File: CourseAsCode.Readers.YAML.Readers.YamlQuestionReader.cs

Action: Add a line to the switch statement to handle your new type.

```
case QuestionType.NUMERICAL:
    return ConvertYamlQuestionToNumericalQuestion(yamlQuestion);
```
<hr>
##### Step 4: Create the XML Writer (Strategy)

This is the most important part. Create a class that knows how to write the XML for this specific question. We call this a "Strategy".

File: CourseAsCode.Writers.XML.FileWriters.Questions.Strategies.NumericalStrategy.cs

```
public class NumericalStrategy : IQuestionStrategy
{
    private readonly ISequentialIdGenerator _idGenerator;

    // We need the generator to create unique IDs for Moodle
    public NumericalStrategy(ISequentialIdGenerator idGenerator) 
    {
        _idGenerator = idGenerator;
    }

    public string GetQuestionTypeString() => "numerical"; // The internal Moodle name

    public void WritePluginData(IXmlService xmlService, Question question)
    {
        var model = (NumericalQuestion)question;
        
        xmlService.WriteElement("plugin_qtype_numerical_question");
        
        // Example: Writing the tolerance value to XML
        xmlService.WriteElement("tolerance", model.Tolerance.ToString());
        
        xmlService.CloseElement();
    }
}
```
<hr>
##### Step 5: Connect everything (Registration)

Finally, tell the application that your new Strategy exists.

Register the Service: In CourseAsCode.Writers.XML.Logic.DependencyService.cs, add:
    
```
services.AddTransient<NumericalStrategy>();
```

Update the Resolver: In QuestionStrategyResolver.cs, tell it when to use your new strategy:

```
    case NumericalQuestion:
        return _serviceProvider.GetRequiredService<NumericalStrategy>();
```

<hr>
#### Testing

It is highly recommended to write a test for your new Strategy. If the XML is even slightly wrong, Moodle will not be able to restore the backup.

Where: CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies

How: We mock the XML Writer to check if your Strategy writes the correct tags and attributes.

```
[Test]
public void WritePluginData_WritesToleranceCorrectly()
{
    // 1. Create a dummy question
    var question = new NumericalQuestion { Tolerance = 5.0 };
    
    // 2. Run the writer
    _strategy.WritePluginData(_mockXmlService, question);

    // 3. Check if "tolerance" was written with value "5"
    _mockXmlService.Received().WriteElement("tolerance", "5");
}
```
