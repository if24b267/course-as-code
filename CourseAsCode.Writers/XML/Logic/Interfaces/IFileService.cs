namespace CourseAsCode.Writers.XML.Logic.Interfaces;

public interface IFileService
{
    bool Exists(string path);
    FileStream OpenRead(string path);
    void Copy(string source, string dest, bool overwrite);
    void CreateDirectory(string path);
}
