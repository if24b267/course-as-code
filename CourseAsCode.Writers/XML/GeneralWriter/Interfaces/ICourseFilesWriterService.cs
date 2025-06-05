using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.GeneralWriter.Interfaces
{
    public interface ICourseFilesWriterService
    {
        void WriteCourseFiles(Course course, string outputPath);
    }
}
