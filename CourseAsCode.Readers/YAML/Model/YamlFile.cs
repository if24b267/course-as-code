using System.ComponentModel.DataAnnotations;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlFile : YamlActivity
    {
        [Required]
        public string FilePath { get; set; }
    }
}
