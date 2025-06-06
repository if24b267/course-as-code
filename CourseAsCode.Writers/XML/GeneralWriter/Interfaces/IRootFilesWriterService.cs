using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.GeneralWriter.Interfaces
{
    public interface IRootFilesWriterService
    {
        void WriteRootFiles(Course course, string outputPath);
    }
}
