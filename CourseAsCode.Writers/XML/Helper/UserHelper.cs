using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides helper methods for handling user-related data
    /// </summary>
    public class UserHelper : IUserHelper
    {
        public string GetCountryFromEnum(Country country)
        {
            return country switch
            {
                Country.Austria => "AT",
                Country.Germany => "DE",
                Country.Switzerland => "CH",

                _ => "AT",
            };
        }


        public string GetAuthMethodFromEnum(AuthenticationMethod authenticationMethod)
        {
            return authenticationMethod switch
            {
                AuthenticationMethod.ManualCreation => "manual",
                AuthenticationMethod.NoLogin => "nologin",
                AuthenticationMethod.EMailSelfRegistration => "email",
                AuthenticationMethod.CAS => "cas",
                AuthenticationMethod.ExternalDatabase => "db",
                AuthenticationMethod.LDAP => "ldap",
                AuthenticationMethod.LTI => "lti",
                AuthenticationMethod.MNet => "mnet",
                AuthenticationMethod.OAuth2 => "oauth2",
                AuthenticationMethod.Shibboleth => "shibboleth",
                AuthenticationMethod.Webservice => "webservice",
                AuthenticationMethod.NoAuthentication => "none",

                _ => "manual",
            };
        }
    }
}
