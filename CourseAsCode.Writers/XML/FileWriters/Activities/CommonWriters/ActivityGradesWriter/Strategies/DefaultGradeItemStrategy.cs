using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    /// <summary>
    /// Default strategy for activities that don't require grade items
    /// </summary>
    public class DefaultGradeItemStrategy : IGradeItemStrategy
    {
        /// <summary>
        /// Writes the grade_item element for the given activity
        /// </summary>
        /// <param name="xmlHelper">The XML helper to use for writing</param>
        /// <param name="activity">The activity to write the grade item for</param>
        public void WriteGradeItems(IXmlHelper xmlHelper, Activity activity)
        {
            // No grade items to write for this activity type
        }
    }
}
