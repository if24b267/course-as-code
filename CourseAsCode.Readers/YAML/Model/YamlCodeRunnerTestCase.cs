namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlCodeRunnerTestCase
    {
        public int Id { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public int TestType { get; set; } = 0;
        public string Expected { get; set; } = string.Empty;
        public bool UseAsExample { get; set; } = false;
        public string Display { get; set; } = "SHOW";
        public bool HideRestIfFail { get; set; } = false;
        public decimal Mark { get; set; } = 1.000m;
        public string StdIn { get; set; } = string.Empty;
        public string Extra { get; set; } = string.Empty;
    }
}
