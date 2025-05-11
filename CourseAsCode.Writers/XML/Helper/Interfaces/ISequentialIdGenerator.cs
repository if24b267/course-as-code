namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    /// <summary>
    /// Defines the contract for generating unique IDs during the XML writing process.
    /// </summary>
    public interface ISequentialIdGenerator
    {
        int GetNextCategoryId();
        int GetNextBankEntryId();
        int GetNextAnswerId();
        int GetNextHintId();
        int GetNextCodeRunnerOptionId();
        int GetNextCodeRunnerTestCaseId();
    }
}