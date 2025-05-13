using System.ComponentModel.DataAnnotations;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlUrl
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string URL { get; set; }

        public string? Description { get; set; }
    }
}
