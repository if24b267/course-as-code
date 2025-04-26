using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum QuestionBehaviour
{
    [Description("immediate feedback, but allows students to have multiple attempts at the question before moving on to the next question. students get fewer marks on subsequent attempts")]
    Adaptive,
    [Description("immediate feedback, but allows students to have multiple attempts at the question before moving on to the next question. no penalties for subsequent attempts")]
    AdaptiveNoPenalties,
    [Description("students must enter an answer to each question and then submit the entire quiz before anything is graded or they get any feedback")]
    Deferred,
    [Description("certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. the grading is adjusted by the choice of certainty")]
    DeferredCBM,
    [Description("students can submit their response immediately during the quiz attempt and get it graded. however, they can only submit one response and cannot change it later")]
    Immediate,
    [Description("certainty-based marking (CBM): the student does not only answer the question, but they also indicate how sure they are they got the question right. the grading is adjusted by the choice of certainty")]
    ImmediateCBM,
    [Description("allows multiple attempts on a question with possible grade penalties. students submit answers via the 'Check' button and can retry using 'Try again' if incorrect. after each wrong attempt, a hint is displayed. once correct, answers cannot be changed. if attempts run out, the student is graded accordingly and shown feedback. the number of tries equals the number of hints plus one (minimum of three)")]
    InteractiveMultipleTries
}