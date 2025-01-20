namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class URLDefaults
{
    private static Random _rnd = new Random();
    
    public static readonly int DEFAULT_MODULE_ID = _rnd.Next(1, 1000);
    public const string DEFAULT_MODULE_NAME = "url";
}