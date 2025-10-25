using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public class FakeQuizFactory
    {
        /// <summary>
        /// Creates a basic quiz with predefined parameters and structure
        /// </summary>
        /// <param name="id">The ActivityId for the quiz</param>
        /// <param name="withFeedback">Whether to include feedback entries in the quiz</param>
        /// <returns>A Quiz object with basic settings and questions</returns>
        public static Quiz CreateBasicQuiz(int id, bool withFeedback = false)
        {
            Quiz basicQuiz = new Quiz()
            {
                ActivityId = id,
                Name = "Unit Test Quiz",
                Description = "A quiz for testing",
                DescriptionFormat = FormattingFormat.HTML,
                OpenTime = null,
                CloseTime = null,
                TimeLimit = 15,
                TimeCreated = new DateTime(2025, 4, 20, 9, 0, 0),
                TimeModified = new DateTime(2025, 4, 20, 9, 30, 0),
                ShowPicture = PictureOptions.NoImage,
                Questions = new List<Question>
                {
                    FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA"),
                    FakeQuestionFactory.CreateBasicDragDropTextQuestion(2, "CatA"),
                    FakeQuestionFactory.CreateBasicDropdownQuestion(3, "CatB"),
                },
                ModuleId = 101,
                ModuleName = "quiz",
            };

            if (withFeedback)
            {
                basicQuiz.Feedback = new List<QuizFeedback>()
                {
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>You did it!</p>",
                        GradeBoundary = 100f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>Probably should go back to kindergarten</p>",
                        GradeBoundary = 50f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>Oh, wow that's just sad</p>",
                        GradeBoundary = 0f,
                        OverallFeedbackFormat = FormattingFormat.HTML,
                    },
                };
            }

            return basicQuiz;
        }
    }
}
