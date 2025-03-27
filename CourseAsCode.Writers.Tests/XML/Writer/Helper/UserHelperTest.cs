using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.Tests.XML.Writer.Helper
{
    [TestFixture]
    public class UserHelperTests
    {
        private UserHelper _userHelper;

        [SetUp]
        public void SetUp()
        {
            _userHelper = new UserHelper();
        }

        [TestCase(Country.Austria, "AT")]
        [TestCase(Country.Germany, "DE")]
        [TestCase(Country.Switzerland, "CH")]
        [TestCase((Country)999, "AT")] // default case: unknown value returns "AT"
        public void GetCountryFromEnum_ReturnsExpectedValue(Country country, string expected)
        {
            // Act
            var result = _userHelper.GetCountryFromEnum(country);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(AuthenticationMethod.ManualCreation, "manual")]
        [TestCase(AuthenticationMethod.NoLogin, "nologin")]
        [TestCase(AuthenticationMethod.EMailSelfRegistration, "email")]
        [TestCase(AuthenticationMethod.CAS, "cas")]
        [TestCase(AuthenticationMethod.ExternalDatabase, "db")]
        [TestCase(AuthenticationMethod.LDAP, "ldap")]
        [TestCase(AuthenticationMethod.LTI, "lti")]
        [TestCase(AuthenticationMethod.MNet, "mnet")]
        [TestCase(AuthenticationMethod.OAuth2, "oauth2")]
        [TestCase(AuthenticationMethod.Shibboleth, "shibboleth")]
        [TestCase(AuthenticationMethod.Webservice, "webservice")]
        [TestCase(AuthenticationMethod.NoAuthentication, "none")]
        [TestCase((AuthenticationMethod)999, "manual")] // default case: unknown value returns "manual"
        public void GetAuthMethodFromEnum_ReturnsExpectedValue(AuthenticationMethod method, string expected)
        {
            // Act
            var result = _userHelper.GetAuthMethodFromEnum(method);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}