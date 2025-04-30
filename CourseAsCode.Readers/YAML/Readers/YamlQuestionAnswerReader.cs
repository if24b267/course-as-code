using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlQuestionAnswerReader : IQuestionAnswerReader
    {

        public MultiChoiceAnswer? Read(string modelObjectPath)
        {
            throw new NotImplementedException();
        }

        private MultiChoiceAnswer ConvertYamlFileToMoodleFile(YamlQuestionAnswer questionAnswer)
        {
            return new MultiChoiceAnswer()
            {
                AnswerText = questionAnswer.AnswerText,
                AnswerTextFormat = questionAnswer.AnswerTextFormat,
                AnswerGrade = questionAnswer.AnswerGrade,
                AnswerFeedback = questionAnswer.Feedback,
                AnswerFeedbackFormat = questionAnswer.FeedbackFormat
            };
        }
    }
}
