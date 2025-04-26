using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlQuestionAnswerReader : IQuestionAnswerReader
    {

        public Answer? Read(string modelObjectPath)
        {
            throw new NotImplementedException();
        }

        private Answer ConvertYamlFileToMoodleFile(YamlQuestionAnswer questionAnswer)
        {
            return new Answer()
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
