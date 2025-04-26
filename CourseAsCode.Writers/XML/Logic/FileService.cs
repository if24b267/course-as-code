using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Logic
{
    public class FileService : IFileService
    {
        public bool Exists(string path) => File.Exists(path);
        public FileStream OpenRead(string path) => File.OpenRead(path);
        public void Copy(string source, string dest, bool overwrite) => File.Copy(source, dest, overwrite);
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);
    }
}
