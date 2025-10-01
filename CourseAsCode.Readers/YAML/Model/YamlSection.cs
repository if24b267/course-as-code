using System.ComponentModel.DataAnnotations;

using CourseAsCode.Model.Defaults;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlSection
    {
        [Required]
        public string SectionName { get; set; }

        public bool VisibleToStudents { get; set; } = SectionDefaults.DEFAULT_VISIBLE_TO_STUDENTS;

        public string Summary { get; set; }

        public List<YamlActivityFilePath> Activities { get; set; }
    }
}
