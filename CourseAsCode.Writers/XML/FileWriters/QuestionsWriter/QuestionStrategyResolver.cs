using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter
{
    public class QuestionStrategyResolver : IQuestionStrategyResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QuestionStrategyResolver> _logger;

        public QuestionStrategyResolver(IServiceProvider serviceProvider, ILogger<QuestionStrategyResolver> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQuestionStrategy ResolveStrategy(Question question)
        {
            return question switch
            {
                CodeRunnerQuestion => _serviceProvider.GetRequiredService<CodeRunnerStrategy>(),
                DragAndDropTextQuestion => _serviceProvider.GetRequiredService<DragDropTextStrategy>(),
                DropdownQuestion => _serviceProvider.GetRequiredService<DropdownStrategy>(),
                MultipleChoiceQuestion => _serviceProvider.GetRequiredService<MultiChoiceStrategy>(),
                _ => throw new ArgumentException($"Question type {question.GetType().Name} is not supported."),
            };
        }
    }
}
