using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IUserHelper
    {
        /// <summary>
        /// Retrieves the country code corresponding to the specified enum value.
        /// </summary>
        /// <param name="country">The country enum for which the country code is to be retrieved.</param>
        /// <returns>A string representing the country code for the provided country enum. Defaults to "AT" if the country enum is not recognized.</returns>
        string GetCountryFromEnum(Country country);

        /// <summary>
        /// Converts the specified authentication method enum to its corresponding string representation as used in Moodle.
        /// </summary>
        /// <param name="authenticationMethod">The authentication method enum to be converted.</param>
        /// <returns>A string corresponding to the provided authentication method. Defaults to "manual" if the enum value is not recognized.</returns>
        string GetAuthMethodFromEnum(AuthenticationMethod authenticationMethod);
    }
}
