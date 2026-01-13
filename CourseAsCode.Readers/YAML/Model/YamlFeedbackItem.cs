namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlFeedbackItem
    {
        public int Template { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string? Label { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Presentation { get; set; } = string.Empty;
        public int HasValue { get; set; } = 0;
        public int Position { get; set; } = 0;
        public int Required { get; set; } = 0;
        public int DependItem { get; set; } = 0;
        public string DependValue { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;
    }
}
