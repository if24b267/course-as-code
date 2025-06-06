namespace CourseAsCode.Writers.XML.Logic.Interfaces
{
    public interface IFileService
    {
        bool Exists(string path);
        FileStream OpenRead(string path);
        bool Copy(string source, string dest, bool overwrite);
        bool CreateDirectory(string path);
    }
}
