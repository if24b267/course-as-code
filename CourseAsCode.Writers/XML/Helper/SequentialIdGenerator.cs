using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// generates unique, sequential integer IDs for various entities
    /// </summary>
    /// <remarks>
    /// This class provides methods to generate ID values for categories, bank entries, answers, hints,
    /// code runner options, and code runner test cases. Each method ensures that the ID generated is unique
    /// and sequential, incrementing with each call.
    /// </remarks>
    public class SequentialIdGenerator : ISequentialIdGenerator
    {
        private int _nextCategoryId = 2; // Moodle's top category ID is 1, our categories thus start at 2
        private int _nextBankEntryId = 1;
        private int _nextAnswerId = 1;
        private int _nextHintId = 1;
        private int _nextCodeRunnerOptionId = 1;
        private int _nextCodeRunnerTestCaseId = 1;

        public int GetNextCategoryId()
        {
            return _nextCategoryId++;
        }

        public int GetNextBankEntryId()
        {
            return _nextBankEntryId++;
        }

        public int GetNextAnswerId()
        {
            return _nextAnswerId++;
        }

        public int GetNextHintId()
        {
            return _nextHintId++;
        }

        public int GetNextCodeRunnerOptionId()
        {
            return _nextCodeRunnerOptionId++;
        }

        public int GetNextCodeRunnerTestCaseId()
        {
            return _nextCodeRunnerTestCaseId++;
        }
    }
}
