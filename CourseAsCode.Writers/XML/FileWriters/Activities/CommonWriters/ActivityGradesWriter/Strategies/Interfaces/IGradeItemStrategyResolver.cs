using CourseAsCode.Model.Activities;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces
{
    /// <summary>
    /// Resolves the appropriate grade item strategy for an activity
    /// </summary>
    public interface IGradeItemStrategyResolver
    {
        IGradeItemStrategy GetStrategy(Activity activity);
    }
}
