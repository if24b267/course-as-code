using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.GeneralWriter.Interfaces
{
    public interface ISectionFilesWriterService
    {
        void WriteSectionFiles(Course course, string outputPath);
    }
}
