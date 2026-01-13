using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Logic.Interfaces;

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
        /// <param name="xmlService">XmlHelper instance initialized on the correct document</param>
        void WriteGradeItems(IXmlService xmlService, Activity activity);
    }
}
