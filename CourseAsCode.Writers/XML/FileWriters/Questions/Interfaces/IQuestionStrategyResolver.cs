using CourseAsCode.Model.Questions;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces
{
    /// <summary>
    /// Resolves the strategy for writing a question based on its type
    /// </summary>
    public interface IQuestionStrategyResolver
    {
        /// <summary>
        /// Resolves the strategy for writing a question based on its type
        /// </summary>
        /// <param name="question">The question to resolve the strategy for</param>
        /// <returns>The <see cref="IQuestionStrategy"/> instance to use for writing the question</returns>
        IQuestionStrategy? ResolveStrategy(Question question);
    }
}
