using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.GeneralWriter.Interfaces
{
    public interface IActivitiesWriterService
    {
        void WriteActivities(Course course, string outputPath);
    }
}
