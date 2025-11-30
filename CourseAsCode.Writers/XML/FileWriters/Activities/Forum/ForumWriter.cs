using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.Forum // Habe den Namespace angepasst, um ihn an die anderen Writer anzupassen
{
    public class ForumWriter : XmlWriterBase
    {
        private const string DEFAULT_FORUM_RELATIVE_PATH = "forum.xml";
        private readonly IGeneralHelper _generalHelper;

        /// <summary>
        /// Creates a new instance of the <see cref="ForumWriter"/> class
        /// </summary>
        public ForumWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ForumWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        /// <summary>
        /// Writes the forum activity to an XML file.
        /// </summary>
        /// <param name="outputPath">The output path where the forum.xml file will be saved.</param>
        /// <param name="forum">The forum object to write.</param>
        public void WriteForum(string outputPath, MoodleForum forum)
        {
            try
            {
                var filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FORUM_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, forum);
                    WriteForumTag(writer, forum);

                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FORUM_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FORUM_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Writes the activity tag and its attributes.
        /// </summary>
        private void WriteActivityAttributes(XmlWriter writer, MoodleForum forum)
        {
            writer.WriteAttributeString("id", forum.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", forum.ModuleId.ToString());
            writer.WriteAttributeString("modulename", forum.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(forum.Context));
        }

        /// <summary>
        /// Writes the forum element and its child elements based on forum.xml structure.
        /// </summary>
        private void WriteForumTag(XmlWriter writer, MoodleForum forum)
        {
            writer.WriteStartElement("forum");
            writer.WriteAttributeString("id", forum.ActivityId.ToString()); // ID des Forum-Tags ist ActivityId

            // Schreibe die einfachen Forum-spezifischen Elemente
            var forumElements = new Dictionary<string, string>
            {
                { "type", forum.Type },
                { "name", forum.Name }, // Name kommt von Activity, wird aber hier wiederholt
                { "intro", forum.Intro ?? "" },
                { "introformat", _generalHelper.EnumIntegerToString(forum.IntroFormat) },
                { "duedate", forum.DueDate.ToString() }, // Unix-Timestamp als int
                { "cutoffdate", forum.CutoffDate.ToString() }, // Unix-Timestamp als int
                { "assessed", forum.Assessed ? "1" : "0" },
                { "assesstimestart", forum.AssessTimeStart.ToString() },
                { "assesstimefinish", forum.AssessTimeFinish.ToString() },
                { "scale", forum.Scale.ToString() },
                { "maxbytes", forum.MaxBytes.ToString() },
                { "maxattachments", forum.MaxAttachments.ToString() },
                { "forcesubscribe", forum.ForceSubscribe ? "1" : "0" },
                { "trackingtype", forum.TrackingType.ToString() },
                { "rsstype", forum.RssType.ToString() },
                { "rssarticles", forum.RssArticles.ToString() },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(forum.TimeModified) }, // TimeModified sollte vom Activity-Objekt kommen
                { "warnafter", forum.WarnAfter.ToString() },
                { "blockafter", forum.BlockAfter.ToString() },
                { "blockperiod", forum.BlockPeriod.ToString() },
                { "completiondiscussions", forum.CompletionDiscussions.ToString() },
                { "completionreplies", forum.CompletionReplies.ToString() },
                { "completionposts", forum.CompletionPosts.ToString() },
                { "displaywordcount", forum.DisplayWordCount ? "1" : "0" },
                { "lockdiscussionafter", forum.LockDiscussionAfter.ToString() },
                { "grade_forum", forum.GradeForum ? "1" : "0" }
            };

            WriteElements(writer, forumElements);

            // Schreibe die komplexen, geschachtelten Strukturen
            WriteDiscussions(writer, forum);

            // Schreibe die leeren Container-Tags (gemäß der Struktur der forum.xml)
            WriteEmptyContainer(writer, "subscriptions");
            WriteEmptyContainer(writer, "digests");
            WriteEmptyContainer(writer, "readposts");
            WriteEmptyContainer(writer, "trackedprefs");
            WriteEmptyContainer(writer, "poststags");
            WriteEmptyContainer(writer, "grades");

            writer.WriteEndElement(); // </forum>
        }

        /// <summary>
        /// Writes the <discussions> element and all nested discussions, posts, and subscriptions.
        /// </summary>
        private void WriteDiscussions(XmlWriter writer, MoodleForum forum)
        {
            writer.WriteStartElement("discussions");

            foreach (var discussion in forum.Discussions)
            {
                writer.WriteStartElement("discussion");
                writer.WriteAttributeString("id", discussion.Id.ToString());

                var discussionElements = new Dictionary<string, string>
                {
                    { "name", discussion.Name },
                    { "firstpost", discussion.FirstPost.ToString() },
                    { "userid", discussion.UserId.ToString() },
                    { "groupid", discussion.GroupId.ToString() },
                    { "assessed", discussion.Assessed ? "1" : "0" },
                    { "timemodified", discussion.TimeModified.ToString() },
                    { "usermodified", discussion.UserModified.ToString() },
                    { "timestart", discussion.TimeStart.ToString() },
                    { "timeend", discussion.TimeEnd.ToString() },
                    { "pinned", discussion.Pinned ? "1" : "0" },
                    { "timelocked", discussion.TimeLocked ? "1" : "0" }
                };
                WriteElements(writer, discussionElements);

                WritePosts(writer, discussion);
                WriteDiscussionSubscriptions(writer, discussion);

                writer.WriteEndElement(); // </discussion>
            }

            writer.WriteEndElement(); // </discussions>
        }

        /// <summary>
        /// Writes the <posts> element for a discussion.
        /// </summary>
        private void WritePosts(XmlWriter writer, ForumDiscussion discussion)
        {
            writer.WriteStartElement("posts");

            foreach (var post in discussion.Posts)
            {
                writer.WriteStartElement("post");
                writer.WriteAttributeString("id", post.Id.ToString());

                var postElements = new Dictionary<string, string>
                {
                    { "parent", post.Parent.ToString() },
                    { "userid", post.UserId.ToString() },
                    { "created", post.Created.ToString() },
                    { "modified", post.Modified.ToString() },
                    { "mailed", post.Mailed ? "1" : "0" },
                    { "subject", post.Subject },
                    { "message", post.Message ?? "" },
                    { "messageformat", _generalHelper.EnumIntegerToString(post.MessageFormat) },
                    { "messagetrust", post.MessageTrust.ToString() },
                    { "attachment", post.Attachment ?? "" }, // attachment ist im XML-Beispiel leer
                    { "totalscore", post.TotalScore.ToString() },
                    { "mailnow", post.MailNow ? "1" : "0" },
                    { "privatereplyto", post.PrivateReplyTo.ToString() }
                };
                WriteElements(writer, postElements);

                // <ratings> ist im XML-Beispiel leer
                WriteEmptyContainer(writer, "ratings");

                writer.WriteEndElement(); // </post>
            }

            writer.WriteEndElement(); // </posts>
        }

        /// <summary>
        /// Writes the <discussion_subs> element for a discussion.
        /// </summary>
        private void WriteDiscussionSubscriptions(XmlWriter writer, ForumDiscussion discussion)
        {
            writer.WriteStartElement("discussion_subs");

            foreach (var subscription in discussion.DiscussionSubscriptions)
            {
                writer.WriteStartElement("discussion_sub");
                writer.WriteAttributeString("id", subscription.Id.ToString());

                var subscriptionElements = new Dictionary<string, string>
                {
                    { "userid", subscription.UserId.ToString() },
                    { "preference", subscription.Preference.ToString() }
                };
                WriteElements(writer, subscriptionElements);

                writer.WriteEndElement(); // </discussion_sub>
            }

            writer.WriteEndElement(); // </discussion_subs>
        }

        private void WriteEmptyContainer(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteEndElement();
        }
    }
}
