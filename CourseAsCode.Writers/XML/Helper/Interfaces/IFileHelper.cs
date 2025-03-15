using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface IFileHelper
{
    string GetFileAreaFromEnum(FileArea fileArea);
    string GetComponentFromEnum(Component component);
    string GetLicenseFromEnum(License license);
}