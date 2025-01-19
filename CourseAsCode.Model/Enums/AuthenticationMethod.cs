using System.ComponentModel;

namespace CourseAsCode.Model.Enums;

public enum AuthenticationMethod
{
	[Description("accounts are created manually by the administrator")]
	ManualCreation,
	[Description("suspends the user's account")]
	NoLogin,
	[Description("users can create their own accounts")]
	EMailSelfRegistration,
	[Description("account details are located on an external CAS server")]
	CAS,
	[Description("account details are located on an external database")]
	ExternalDatabase,
	[Description("account details are located on an external LDAP server")]
	LDAP,
	[Description("works with the Publish as LTI tool enrolment method to connect courses and activities")]
	LTI,
	[Description("Moodle network allows a Moodle administrator to establish a link with another Moodle site and to share some resources with the users of that Moodle")]
	MNet,
	[Description("authenticate with an OAuth 2 service")]
	OAuth2,
	[Description("account details are located on an external Shibboleth server")]
	Shibboleth,
	[Description("for a dedicated account which integrates with your site via web services")]
	Webservice,
	[Description("for testing purposes or if the Moodle site is not available on the Internet. Do NOT use on public servers!")]
	NoAuthentication
}