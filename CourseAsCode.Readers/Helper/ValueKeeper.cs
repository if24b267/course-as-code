namespace CourseAsCode.Readers.Helper;

// TODO: change this so values are being dependency injected
public static class ValueKeeper
{
    public static string? CoursePath { get; set; }

    // currently only being counted up from 1
    public static int CurrentActivityId { get; set; }

    // currently only being counted up from 1
    public static int CurrentSectionId { get; set; }
}
