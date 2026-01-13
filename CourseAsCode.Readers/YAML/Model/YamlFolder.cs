using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlFolder : YamlActivity
    {
        public bool VisibleToStudents { get; set; } = true;
        public bool DisplayDescription { get; set; } = false;
        public List<string> YamlFilePaths { get; set; } = [];
    }
}
