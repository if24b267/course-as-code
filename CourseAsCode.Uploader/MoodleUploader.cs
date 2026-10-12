using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace CourseAsCode.Uploader;

public class MoodleUploader : IMoodleUploader
{
    private readonly MoodleUploaderOptions _options;
    private readonly ILogger<MoodleUploader>? _logger;

    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    public MoodleUploader(MoodleUploaderOptions options, ILogger<MoodleUploader>? logger = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public IPage? CurrentPage => _page;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger?.LogInformation("Initializing Playwright and launching browser (Headless: {Headless})...", _options.Headless);

        _playwright = await Playwright.CreateAsync().ConfigureAwait(false);

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _options.Headless
        }).ConfigureAwait(false);

        _context = await _browser.NewContextAsync().ConfigureAwait(false);
        _context.SetDefaultTimeout(_options.TimeoutMs);

        _page = await _context.NewPageAsync().ConfigureAwait(false);
        _logger?.LogInformation("Browser session initialized successfully.");
    }

    public async Task NavigateToLoginPageAsync(CancellationToken cancellationToken = default)
    {
        if (_page == null)
        {
            throw new InvalidOperationException("Uploader has not been initialized. Call InitializeAsync first.");
        }

        var loginUri = new Uri(_options.BaseUrl, "login/index.php");
        _logger?.LogInformation("Navigating to Moodle login page: {Url}", loginUri);

        await _page.GotoAsync(loginUri.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).ConfigureAwait(false);

        _logger?.LogInformation("Awaiting credential input fields on login page...");

        // Ensure username and password input fields are visible and ready for user credentials
        await _page.WaitForSelectorAsync("#username", new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);

        await _page.WaitForSelectorAsync("#password", new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);

        _logger?.LogInformation("Login page reached successfully. Ready for credentials input.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync().ConfigureAwait(false);
            _page = null;
        }

        if (_context != null)
        {
            await _context.CloseAsync().ConfigureAwait(false);
            _context = null;
        }

        if (_browser != null)
        {
            await _browser.CloseAsync().ConfigureAwait(false);
            _browser = null;
        }

        _playwright?.Dispose();
        _playwright = null;

        GC.SuppressFinalize(this);
    }
}
