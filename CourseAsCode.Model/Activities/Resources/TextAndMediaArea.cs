namespace CourseAsCode.Model.Activities.Resources;

public class TextAndMediaArea : Activity
{
	public string IndexTitle { get; set; }
	// TODO: how exactly is this saved in moodle
	public string Content { get; set; }

	public SensibleDefaultsTextAndMediaArea SensibleDefaults { get; set; } = new SensibleDefaultsTextAndMediaArea();
	public SystemDefaultsTextAndMediaArea SystemDefaults { get; set; } = new SystemDefaultsTextAndMediaArea();

	public class SensibleDefaultsTextAndMediaArea
	{
		// TODO
	}


	public class SystemDefaultsTextAndMediaArea
	{
		public int ModuleId { get; set; } = 1;
        public string ModuleName { get; set; } = "label";
        public string MoodleVersion { get; set; } = "2024100700";
        // TODO
    }
}