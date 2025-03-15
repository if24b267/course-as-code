using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper;

public interface IRolesHelper
{
    string GetArchetypeFromEnum(Archetype archetype);
}