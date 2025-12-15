using System.Collections.Generic;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface IFileHelper
{
    string GetFileAreaFromEnum(FileArea fileArea);
    string GetComponentFromEnum(Component component);
    string GetLicenseFromEnum(License license);

    /// <summary>
    /// Returns all file ids that are referenced/used by the given activity inside the given course.
    /// Implementations should return an empty enumeration when no files are associated.
    /// </summary>
    /// <param name="course">Course context (may be needed to resolve files)</param>
    /// <param name="activity">Activity instance</param>
    /// <returns>Enumerable of file ids</returns>
    IEnumerable<int> GetFileIdsForActivity(Course course, Activity activity);
}
