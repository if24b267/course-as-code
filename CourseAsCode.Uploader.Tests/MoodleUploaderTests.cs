namespace CourseAsCode.Uploader.Tests
{
    [TestFixture]
    public class MoodleUploaderTests
    {
        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenOptionsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MoodleUploader(null!));
        }

        [Test]
        public void NavigateToLoginPageAsync_ThrowsInvalidOperationException_WhenNotInitialized()
        {
            var options = new MoodleUploaderOptions
            {
                BaseUrl = new Uri("https://example.com")
            };

            var uploader = new MoodleUploader(options);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await uploader.NavigateToLoginPageAsync();
            });
        }

        /// <summary>
        /// Launches the browser, navigates to the Moodle login page, and verifies that the credential input fields are loaded.
        /// Can be run directly from Visual Studio Test Explorer or with:
        ///   dotnet test --filter "FullyQualifiedName~NavigateToLoginPageAsync_NavigatesToLoginAndFindsCredentialFields"
        /// </summary>
        [Test]
        [Category("Integration")]
        public async Task NavigateToLoginPageAsync_NavigatesToLoginAndFindsCredentialFields()
        {
            var moodleUrlString = Environment.GetEnvironmentVariable("MOODLE_URL") ?? "http://localhost/?redirect=0";
            var isHeadless = bool.TryParse(Environment.GetEnvironmentVariable("MOODLE_HEADLESS"), out var parsedHeadless)
                ? parsedHeadless
                : false; // False by default so you can watch the browser in Test Explorer

            var options = new MoodleUploaderOptions
            {
                BaseUrl = new Uri(moodleUrlString),
                Headless = isHeadless,
                TimeoutMs = 15000
            };

            await using var uploader = new MoodleUploader(options);

            // 1. Initialize Playwright and launch Chromium browser
            await uploader.InitializeAsync();

            // 2. Navigate to login page
            await uploader.NavigateToLoginPageAsync();

            // 3. Verify that the login page and credential inputs are present
            Assert.That(uploader.CurrentPage, Is.Not.Null);

            var currentUrl = uploader.CurrentPage!.Url;
            Assert.That(currentUrl, Does.Contain("login/index.php"));

            var isUsernameVisible = await uploader.CurrentPage.Locator("#username").IsVisibleAsync();
            var isPasswordVisible = await uploader.CurrentPage.Locator("#password").IsVisibleAsync();

            Assert.That(isUsernameVisible, Is.True, "Username field (#username) should be visible on the login page.");
            Assert.That(isPasswordVisible, Is.True, "Password field (#password) should be visible on the login page.");
        }
    }
}
