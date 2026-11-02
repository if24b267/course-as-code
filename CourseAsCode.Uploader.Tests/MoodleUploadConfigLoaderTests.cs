namespace CourseAsCode.Uploader.Tests;

[TestFixture]
public class MoodleUploadConfigLoaderTests
{
    private string _dir = null!;

    [SetUp]
    public void SetUp()
    {
        _dir = Path.Combine(Path.GetTempPath(), "CacConfigTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_dir))
        {
            Directory.Delete(_dir, true);
        }
    }

    private static Func<string, string?> Env(params (string Key, string Value)[] values) =>
        name => values.FirstOrDefault(v => MoodleUploadConfigLoader.EnvPrefix + v.Key == name).Value;

    [Test]
    public void Load_WithoutFileAndEnv_ReturnsValidDefaults()
    {
        var result = MoodleUploadConfigLoader.Load(null, Env(), _dir);

        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Options.ContainerName, Is.EqualTo("moodle"));
        Assert.That(result.Options.DefaultCategoryId, Is.EqualTo(1));
    }

    [Test]
    public void Load_EnvironmentVariables_OverrideDefaults()
    {
        var result = MoodleUploadConfigLoader.Load(null, Env(("CONTAINER", "course-as-code-moodle-1"), ("CATEGORY_ID", "3")), _dir);

        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Options.ContainerName, Is.EqualTo("course-as-code-moodle-1"));
        Assert.That(result.Options.DefaultCategoryId, Is.EqualTo(3));
    }

    [Test]
    public void Load_FileValues_AreUsed_AndEnvironmentWinsOverFile()
    {
        File.WriteAllText(
            Path.Combine(_dir, MoodleUploadConfigLoader.DefaultConfigFileName),
            "{ \"containerName\": \"from-file\", \"defaultCategoryId\": 5 }");

        var fileOnly = MoodleUploadConfigLoader.Load(null, Env(), _dir);
        var withEnv = MoodleUploadConfigLoader.Load(null, Env(("CONTAINER", "from-env")), _dir);

        Assert.That(fileOnly.Options.ContainerName, Is.EqualTo("from-file"));
        Assert.That(fileOnly.Options.DefaultCategoryId, Is.EqualTo(5));
        Assert.That(withEnv.Options.ContainerName, Is.EqualTo("from-env"));
        Assert.That(withEnv.Options.DefaultCategoryId, Is.EqualTo(5));
    }

    [Test]
    public void Load_NonNumericCategory_ReturnsError()
    {
        var result = MoodleUploadConfigLoader.Load(null, Env(("CATEGORY_ID", "abc")), _dir);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.Contains("CATEGORY_ID")), Is.True);
    }

    [Test]
    public void Load_CategoryZero_ReturnsError()
    {
        var result = MoodleUploadConfigLoader.Load(null, Env(("CATEGORY_ID", "0")), _dir);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void Load_ExplicitFileMissing_ReturnsError()
    {
        var result = MoodleUploadConfigLoader.Load(Path.Combine(_dir, "gibt-es-nicht.json"), Env(), _dir);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.Contains("not found")), Is.True);
    }

    [Test]
    public void Load_BrokenJson_ReturnsError()
    {
        File.WriteAllText(Path.Combine(_dir, MoodleUploadConfigLoader.DefaultConfigFileName), "{ kaputt");

        var result = MoodleUploadConfigLoader.Load(null, Env(), _dir);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.Contains("not valid JSON")), Is.True);
    }

    [Test]
    public void Load_EmptyContainer_IsErrorInDockerModeButFineInLocalMode()
    {
        File.WriteAllText(
            Path.Combine(_dir, MoodleUploadConfigLoader.DefaultConfigFileName),
            "{ \"containerName\": \"\" }");

        var docker = MoodleUploadConfigLoader.Load(null, Env(), _dir);
        var local = MoodleUploadConfigLoader.Load(null, Env(("MODE", "LocalProcess")), _dir);

        Assert.That(docker.IsValid, Is.False);
        Assert.That(local.IsValid, Is.True);
    }
}
