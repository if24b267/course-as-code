# Activities
A course consists of any number of sections, which in turn can contain any number of activities.

```puml
@startuml

package Activities <<Frame>> {
    class Course {
        + FullName: string
        + ShortName: string
        + Summary: string 
        + StartDate: DateTime
        + EndDate: DateTime
        ...
    }

    class Section {
        + Name: string
        + Summary: string
        + VisibleToStudents: bool
        ...
    }

    abstract class Activity {
        + Name: string
        + Description: string
        + VisibleToStudents: bool
        + DisplayDescription: bool
        ...
    }
    
    class Assignment extends Activity {
        + Intro: string
        + DueDate: DateTime
        + CutOffDate: DateTime
        ...
    }
    
    class MoodleFile extends Activity {
        + FileName: string
        + FilePath: string
        ...
    }
    
    class TextAndMediaArea extends Activity {
        + Content: string
        ...
    }
    
    class URL extends Activity {
        + ExternalUrl: string
        ...
    }
    
    class Quiz extends Activity {
        + OpenTime: DateTime
        + CloseTime: DateTime
        + TimeLimit: int
        ...
    }

' A course consists of multiple sections
Course "1" *-- "0..*" Section : contains

' A section consists of multiple activities
Section "1" *-- "0..*" Activity : contains
@enduml
```

!!! info
    There are certain parameters that are required for all activities. These include, for example, the activity's name and its description, or parameters that determine whether the activity should be visible to students, in which language it should be displayed, etc. These parameters (those that are equally required for all activities) are found in the base class `Activity` - the remaining classes that extend the base class then only contain the parameters relevant to the respective activity.

On the following pages, we explain which activities are supported by **Course as Code** and how they can be used.