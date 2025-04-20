namespace CourseAsCode.Model.Enums;

public enum EMailVisibility
{
    Hidden = 0, // Only users with appropriate permissions such as teachers can view the user's e-mail address
    VisibleToEveryone = 1, // All users on the site can view the user's e-mail address
    VisibleToParticipants = 2, // Only users who are enrolled in the same course as the user can view the user's e-mail address
}
