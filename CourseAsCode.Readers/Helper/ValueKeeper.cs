namespace CourseAsCode.Readers.Helper;

// TODO: change this so values are being dependency injected
public static class ValueKeeper
{
	public static string? CoursePath { get; set; }
	public static int CurrentActivityId { get; set; }
}