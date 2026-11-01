using System.Threading;
using System.Threading.Tasks;

namespace CourseAsCode.Uploader;

public interface IMoodleCourseRestorer
{
    Task<MoodleRestoreResult> RestoreCourseAsync(
        string mbzFilePath,
        int? categoryId = null,
        CancellationToken cancellationToken = default);
}
