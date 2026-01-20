namespace CourseAsCode.Model.Enums.Forums;

/// <summary>
/// Defines the subscription modes for Moodle forums.
/// Controls how users are subscribed to forum notifications.
/// </summary>
public enum SubscriptionMode
{
    /// <summary>
    /// Optional subscription - users can choose to subscribe or unsubscribe.
    /// This is the default mode.
    /// </summary>
    Optional = 0,       // FORUM_CHOOSESUBSCRIBE

    /// <summary>
    /// Forced subscription - all users are automatically subscribed and cannot unsubscribe.
    /// Useful for announcements or important course forums.
    /// </summary>
    Forced = 1,         // FORUM_FORCESUBSCRIBE

    /// <summary>
    /// Auto subscription - users are initially subscribed but can unsubscribe later.
    /// Good for ensuring users see initial posts but can opt out.
    /// </summary>
    Auto = 2,           // FORUM_INITIALSUBSCRIBE

    /// <summary>
    /// Subscription disabled - users cannot subscribe to the forum at all.
    /// No email notifications will be sent.
    /// </summary>
    Disabled = 3        // FORUM_DISALLOWSUBSCRIBE
}
