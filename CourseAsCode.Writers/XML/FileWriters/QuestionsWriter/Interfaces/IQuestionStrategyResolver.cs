using CourseAsCode.Model.Questions;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces
{
    public interface IQuestionStrategyResolver
    {
        IQuestionStrategy ResolveStrategy(Question question);
    }
}
