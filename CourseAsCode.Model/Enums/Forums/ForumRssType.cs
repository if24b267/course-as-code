namespace CourseAsCode.Model.Enums.Forums;

/// <summary>
/// Defines the RSS feed types for Moodle forums.
/// Controls what content is included in the forum's RSS feed.
/// </summary>
public enum ForumRssType
{
    /// <summary>
    /// RSS feed is disabled for this forum.
    /// </summary>
    None = 0,

    /// <summary>
    /// RSS feed contains discussions - each discussion thread is an RSS item.
    /// Shows the first post of each discussion.
    /// </summary>
    Discussions = 1,

    /// <summary>
    /// RSS feed contains posts - each individual post is an RSS item.
    /// Shows all posts including replies.
    /// </summary>
    Posts = 2
}
