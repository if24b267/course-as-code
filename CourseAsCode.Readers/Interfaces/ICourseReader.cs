using CourseAsCode.Readers.Enums;
using CourseAsCode.Model;

namespace CourseAsCode.Readers.Interfaces;

public interface ICourseReader
{
    public Course? Read(string path, CourseFormat format);
}