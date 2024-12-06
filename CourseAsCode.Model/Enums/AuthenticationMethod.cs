namespace CourseAsCode.Model.Enums;

public enum AuthenticationMethod
{
    // WARNING: If an authentication method is selected for which the corresponding plugin is not installed, the user will not be able to log in
    Email,
    Manual,
    NoLogin, // blocks the user from logging in
    CAS,
    ExternalDatabase,
    LDAP,
    LTI,
    MNet,
    OAuth2,
    Shibboleth,
    Webservice,
    NoAuthentication // no authentication required
}