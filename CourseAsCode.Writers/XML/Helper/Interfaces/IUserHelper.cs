using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface IUserHelper
{
    string GetCountryFromEnum(Country country);
    string GetAuthMethodFromEnum(AuthenticationMethod authenticationMethod);
}
