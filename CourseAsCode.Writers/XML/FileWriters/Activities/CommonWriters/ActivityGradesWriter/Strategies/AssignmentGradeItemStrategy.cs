using System.Globalization;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    /// <summary>
    /// Strategy for writing grade items for Assignment activities
    /// </summary>
    public class AssignmentGradeItemStrategy : IGradeItemStrategy
    {
        private readonly IGeneralHelper _generalHelper;

        public AssignmentGradeItemStrategy(IGeneralHelper generalHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }


        /// <summary>
        /// Writes the grade_item element for an Assignment activity
        /// </summary>
        /// <param name="xmlService">The XmlHelper instance to use for writing XML elements</param>
        /// <param name="activity">The activity to write the grade item for</param>
        public void WriteGradeItems(IXmlService xmlService, Activity activity)
        {
            // This strategy is only applicable to Assignment activities
            if (activity is not Assignment assignment)
            {
                return;
            }

            xmlService.WriteElementWithAttributes("grade_item", new Dictionary<string, string>
            {
                { "id", activity.ActivityId.ToString() },
            });

            var gradeItemElements = new Dictionary<string, string>
            {
                { "categoryid", assignment.GradeCategory.GradeCategoryId.ToString() },
                { "itemname", assignment.GradeName ?? assignment.Name },
                { "itemtype", "mod" },
                { "itemmodule", assignment.ModuleName },
                { "iteminstance", "1" },
                { "itemnumber", "0" },
                { "iteminfo", assignment.GradeInfo ?? "$@NULL@$" },
                { "idnumber", assignment.CustomId != null ? assignment.CustomId.ToString() : "$@NULL@$" },
                { "calculation", "$@NULL@$" },
                { "gradetype", _generalHelper.EnumIntegerToString(assignment.GradeType) },
                { "grademax", assignment.MaxGrade.ToString("F5", CultureInfo.InvariantCulture) },
                { "grademin", (assignment.MinGrade ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "scaleid", "$@NULL@$" },
                { "outcomeid", "$@NULL@$" },
                { "gradepass", (assignment.GradeToPass ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "multfactor", (assignment.GradeMultiplicator ?? 1).ToString("F5", CultureInfo.InvariantCulture) },
                { "plusfactor", (assignment.GradeOffset ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "aggregationcoef", 0.ToString("F5", CultureInfo.InvariantCulture) },
                { "aggregationcoef2", ((assignment.GradingWeight ?? 0) / 100.0m).ToString("F5", CultureInfo.InvariantCulture) },
                { "weightoverride", assignment.GradingWeight.HasValue ? "1" : "0" },
                { "sortorder", "6" },
                { "display", "0" },
                { "decimals", "$@NULL@$" },
                { "hidden", assignment.GradeHiddenUntil != null ? _generalHelper.DateTimeToUnixTimestamp(assignment.GradeHiddenUntil.Value) : assignment.GradeHidden ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locked", assignment.GradingLocked ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locktime", assignment.GradingLockedAfter != null ? _generalHelper.DateTimeToUnixTimestamp(assignment.GradingLockedAfter.Value) : "0" },
                { "needsupdate", "0" },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(assignment.TimeCreated) },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(assignment.TimeModified) },
            };

            xmlService.WriteElements(gradeItemElements);
            xmlService.CloseElement(); // Close <grade_item>
        }
    }
}
