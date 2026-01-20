namespace CourseAsCode.Model.Enums.Forums;

/// <summary>
/// Defines the read tracking options for Moodle forums.
/// Controls whether unread posts are highlighted for users.
/// </summary>
public enum ForumTrackingType
{
    /// <summary>
    /// Read tracking is disabled - unread posts are not tracked or highlighted.
    /// </summary>
    Off = 0,            // FORUM_TRACKING_OFF

    /// <summary>
    /// Optional tracking - users can choose to enable or disable tracking.
    /// Users can toggle via 'Track / Don't track unread posts' link.
    /// </summary>
    Optional = 1,       // FORUM_TRACKING_OPTIONAL

    /// <summary>
    /// Forced tracking - tracking is always on regardless of user preference.
    /// All unread posts will be highlighted for all users.
    /// </summary>
    Forced = 2          // FORUM_TRACKING_FORCED (formerly FORUM_TRACKING_ON)
}
