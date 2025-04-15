using CourseAsCode.Model;
using CourseAsCode.Model.Activities;

namespace CourseAsCode.Writers.XML.Writer.Activities.Interfaces;

public interface IActivityModuleWriter
{
    void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject);
}
