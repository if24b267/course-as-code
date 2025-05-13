using CourseAsCode.Model;
using CourseAsCode.Readers.Enums;

namespace CourseAsCode.Readers.Interfaces
{
    public interface ICourseReader
    {
        public Course? Read(string path, CourseFormat format);
    }
}
