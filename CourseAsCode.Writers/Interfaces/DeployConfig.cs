namespace CourseAsCode.Writers.Interfaces
{
    public class DeployConfig
    {
        /// <summary>Docker-Containername oder SSH-Host der Moodle-Instanz.</summary>
        public string Target { get; set; } = string.Empty;

        /// <summary>ID der Kurskategorie, in die der Kurs eingespielt wird.</summary>
        public int CategoryId { get; set; } = 1;
    }
}
