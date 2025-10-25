using System.Xml;
using CourseAsCode.Model.Activities;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces
{
    /// <summary>
    /// Defines the strategy for writing the `grade_items` section for different activity types
    /// </summary>
    public interface IGradeItemStrategy
    {
        /// <summary>
        /// Writes the grade items section for a specific activity type
        /// </summary>
        void WriteGradeItems(XmlWriter writer, Activity activity);
    }
}
