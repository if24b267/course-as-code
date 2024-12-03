namespace CourseAsCode.Readers

{
    public interface CourseReader
    {
        Course Read(string path, string format);
    }
}