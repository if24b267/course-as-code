using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YamlActivityReader : IActivityReader
{
    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly IFileReader _fileReader;
    private readonly ITextAndMediaAreaReader _textMediaAreaReader;
    private readonly IUrlReader _urlReader;
    private readonly IYamlFileReaderHelper _yamlFileReaderHelper;
    private readonly IQuizReader _quizReader;

    public YamlActivityReader(IFileReader fileReader, ITextAndMediaAreaReader textMediaAreaReader, IUrlReader urlReader, IYamlFileReaderHelper yamlFileReaderHelper, IQuizReader quizReader)
    {
        _fileReader = fileReader;
        _textMediaAreaReader = textMediaAreaReader;
        _urlReader = urlReader;
        _yamlFileReaderHelper = yamlFileReaderHelper;
        _quizReader = quizReader;
    }

    public Activity? Read(string modelObjectPath)
    {
        _logger.Debug($"Reading Activity from {modelObjectPath}");
        if (!File.Exists(modelObjectPath))
        {
            _logger.Error($"File {modelObjectPath} does not exist.");
            return null;
        }

        // TODO: this might error
        var yamlActivity = _yamlFileReaderHelper.DeserializeFromFile<YamlActivity>(modelObjectPath);

        if (yamlActivity == null)
        {
            _logger.Error($"Could not deserialize the file {modelObjectPath}.");
            return null;
        }

        switch (yamlActivity.Type)
        {
            // TODO: do we maybe want this in a different reader? -> problem: we dont follow the interface structure since we dont want to read the file twice
            case ActivityType.URL:
                _logger.Debug($"Reading URL activity from {modelObjectPath}");
                return _urlReader.Read(modelObjectPath);
            case ActivityType.FILE:
                _logger.Debug($"Reading file activity from {modelObjectPath}");
                return _fileReader.Read(modelObjectPath);
            case ActivityType.LABEL:
            case ActivityType.TEXTMEDIAAREA:
                _logger.Debug($"Reading text media area activity from {modelObjectPath}");
                return _textMediaAreaReader.Read(modelObjectPath);
            case ActivityType.QUIZ:
                _logger.Debug($"Reading quiz activity from {modelObjectPath}");
                return _quizReader.Read(modelObjectPath);

        }

        return null;
    }

    // TODO: remove this, but first make the defaults work better
    private URL? ConvertYamlActivityToMoodleUrl(YamlActivity yamlActivity)
    {
        // TODO: improve activity reading -> every activity could for example have visible to students set
        // maybe a different class that reads the non specific parts of the activity
        return new URL()
        {
            ActivityId = ValueKeeper.CurrentActivityId,
            Name = yamlActivity.Name,
            Description = yamlActivity.Description,
            ExternalUrl = yamlActivity.URL,
        };
    }
}
