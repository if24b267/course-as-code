using Microsoft.Playwright;

namespace CourseAsCode.Uploader;

public interface IMoodleUploader : IAsyncDisposable
{
    IPage? CurrentPage { get; }

    Task InitializeAsync(CancellationToken cancellationToken = default);

    Task NavigateToLoginPageAsync(CancellationToken cancellationToken = default);
}
