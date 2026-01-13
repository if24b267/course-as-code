using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.Calculated;
using CourseAsCode.Model.Questions.Types.CalculatedMulti;
using CourseAsCode.Model.Questions.Types.CalculatedSimple;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.Description;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Model.Questions.Types.Essay;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Model.Questions.Types.TrueFalse;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions
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

        public IQuestionStrategy? ResolveStrategy(Question question)
        {
            return question switch
            {
                CodeRunnerQuestion => _serviceProvider.GetRequiredService<CodeRunnerStrategy>(),
                DragAndDropTextQuestion => _serviceProvider.GetRequiredService<DragDropTextStrategy>(),
                DragAndDropTextAndImageQuestion => _serviceProvider.GetRequiredService<DragAndDropTextAndImageStrategy>(),
                DropdownQuestion => _serviceProvider.GetRequiredService<DropdownStrategy>(),
                MultipleChoiceQuestion => _serviceProvider.GetRequiredService<MultiChoiceStrategy>(),
                DescriptionQuestion => null, // Description questions do not require a strategy as they have no plugin data
                ShortAnswerQuestion => _serviceProvider.GetRequiredService<ShortAnswerStrategy>(),
                EssayQuestion => _serviceProvider.GetRequiredService<EssayStrategy>(),
                TrueFalseQuestion => _serviceProvider.GetRequiredService<TrueFalseStrategy>(),
                CalculatedQuestion => _serviceProvider.GetRequiredService<CalculatedStrategy>(),
                CalculatedMultiChoiceQuestion => _serviceProvider.GetRequiredService<CalculatedMultiStrategy>(),
                SimpleCalculatedQuestion => _serviceProvider.GetRequiredService<CalculatedSimpleStrategy>(),
                _ => throw new ArgumentException($"Question type {question.GetType().Name} is not supported."),
            };
        }
    }
}
