namespace CourseAsCode.Model.Enums.Forums;

/// <summary>
/// Defines the different forum types available in Moodle.
/// Each type provides a different style of discussion forum.
/// </summary>
public enum ForumType
{
    /// <summary>
    /// Standard forum for general use.
    /// Anyone can start new discussion topics or respond to existing ones.
    /// This is the most common and default forum type.
    /// </summary>
    General,        // Moodle value: "general"

    /// <summary>
    /// Standard forum displayed in a blog-like format.
    /// Same as general, but the first post of each discussion is displayed in full on the forum page.
    /// </summary>
    Blog,           // Moodle value: "blog"

    /// <summary>
    /// A single simple discussion.
    /// Only one discussion thread exists. The teacher posts a topic, students can only reply.
    /// Students cannot start new discussion topics.
    /// </summary>
    Single,         // Moodle value: "single"

    /// <summary>
    /// Each person posts one discussion.
    /// Each participant can start exactly one discussion topic, but can reply to as many others as they wish.
    /// Useful when you want learners to 'take ownership' of a particular discussion.
    /// </summary>
    EachUser,       // Moodle value: "eachuser"

    /// <summary>
    /// Question and Answer forum.
    /// Teacher posts a question, students must post their answer before they can see other students' responses.
    /// Best used when there is a particular question that all students should answer.
    /// </summary>
    QAndA           // Moodle value: "qanda"
}
