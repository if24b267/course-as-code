using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies
{
    public class GradeItemStrategyResolver : IGradeItemStrategyResolver
    {
        private readonly Dictionary<Type, IGradeItemStrategy> _strategies;
        private readonly IGradeItemStrategy _defaultStrategy;

        /// <summary>
        /// Initializes a new instance of the GradeItemStrategyResolver class
        /// </summary>
        /// <param name="quizStrategy">The strategy to use for Quiz activities</param>
        /// <param name="assignmentStrategy">The strategy to use for Assignment activities</param>
        /// <param name="defaultStrategy">The default strategy to use for other activity types</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public GradeItemStrategyResolver(QuizGradeItemStrategy quizStrategy, AssignmentGradeItemStrategy assignmentStrategy, DefaultGradeItemStrategy defaultStrategy)
        {
            _defaultStrategy = defaultStrategy ?? throw new ArgumentNullException(nameof(defaultStrategy));

            _strategies = new Dictionary<Type, IGradeItemStrategy>
            {
                { typeof(Quiz), quizStrategy ?? throw new ArgumentNullException(nameof(quizStrategy)) },
                { typeof(Assignment), assignmentStrategy ?? throw new ArgumentNullException(nameof(assignmentStrategy)) },
                // Add more activity type mappings here as needed
            };
        }

        /// <summary>
        /// Returns the strategy to use for the given activity type.
        /// </summary>
        /// <param name="activity">The activity for which to get the strategy</param>
        /// <returns>The appropriate <see cref="IGradeItemStrategy"/> for the activity type</returns>
        /// <exception cref="ArgumentNullException">Thrown if the activity is null</exception>
        public IGradeItemStrategy GetStrategy(Activity activity)
        {
            ArgumentNullException.ThrowIfNull(activity);
            return _strategies.GetValueOrDefault(activity.GetType(), _defaultStrategy);
        }
    }
}
