using System.Globalization;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    /// <summary>
    /// Strategy for writing grade items for Quiz activities
    /// </summary>
    public class QuizGradeItemStrategy : IGradeItemStrategy
    {
        private readonly IGeneralHelper _generalHelper;

        public QuizGradeItemStrategy(IGeneralHelper generalHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }


        /// <summary>
        /// Writes the grade_item element for a Quiz activity
        /// </summary>
        /// <param name="xmlService">The XML helper to use for writing</param>
        /// <param name="activity">The activity to write the grade item for</param>
        public void WriteGradeItems(IXmlService xmlService, Activity activity)
        {
            // This strategy is only applicable to Quiz activities
            if (activity is not Quiz quiz)
            {
                return;
            }

            xmlService.WriteElementWithAttributes("grade_item", new Dictionary<string, string>
            {
                { "id", activity.ActivityId.ToString() },
            });

            var gradeItemElements = new Dictionary<string, string>
            {
                { "categoryid", quiz.GradeCategory.GradeCategoryId.ToString() },
                { "itemname", quiz.GradeName ?? quiz.Name },
                { "itemtype", "mod" },
                { "itemmodule", quiz.ModuleName },
                { "iteminstance", "1" },
                { "itemnumber", "0" },
                { "iteminfo", quiz.GradeInfo ?? "$@NULL@$" },
                { "idnumber", quiz.CustomId != null ? quiz.CustomId.ToString() : "$@NULL@$" },
                { "calculation", "$@NULL@$" },
                { "gradetype", _generalHelper.EnumIntegerToString(quiz.GradeType) },
                { "grademax", quiz.MaxGrade.ToString("F5", CultureInfo.InvariantCulture) },
                { "grademin", (quiz.MinGrade ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "scaleid", "$@NULL@$" },
                { "outcomeid", "$@NULL@$" },
                { "gradepass", (quiz.GradeToPass ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "multfactor", (quiz.GradeMultiplicator ?? 1).ToString("F5", CultureInfo.InvariantCulture) },
                { "plusfactor", (quiz.GradeOffset ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "aggregationcoef", 0.ToString("F5", CultureInfo.InvariantCulture) },
                { "aggregationcoef2", ((quiz.GradingWeight ?? 0) / 100.0m).ToString("F5", CultureInfo.InvariantCulture) },
                { "weightoverride", quiz.GradingWeight.HasValue ? "1" : "0" },
                { "sortorder", "6" },
                { "display", "0" },
                { "decimals", "$@NULL@$" },
                { "hidden", quiz.GradeHiddenUntil != null ? _generalHelper.DateTimeToUnixTimestamp(quiz.GradeHiddenUntil.Value) : quiz.GradeHidden ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locked", quiz.GradingLocked ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locktime", quiz.GradingLockedAfter != null ? _generalHelper.DateTimeToUnixTimestamp(quiz.GradingLockedAfter.Value) : "0" },
                { "needsupdate", "0" },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeCreated) },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeModified) },
            };

            xmlService.WriteElements(gradeItemElements);
            xmlService.CloseElement(); // Close <grade_item>
        }
    }
}
