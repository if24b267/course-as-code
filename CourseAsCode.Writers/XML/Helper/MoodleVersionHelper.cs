using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// <para>Helper class to handle Moodle version-related operations such as mapping enums to version strings.</para>
    /// <para>Implements the <see cref="IMoodleVersionHelper"/> interface.</para>
    /// </summary>
    public class MoodleVersionHelper : IMoodleVersionHelper
    {
        private readonly ILogger<MoodleVersionHelper> _logger;


        /// <summary>
        /// Creates a new instance of the MoodleVersionHelper class
        /// </summary>
        /// <param name="logger">Logger instance specific to MoodleVersionHelper</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public MoodleVersionHelper(ILogger<MoodleVersionHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public string GetMoodleVersionStringFromEnum(MoodleVersion version)
        {
            return version switch
            {
                // Moodle 4.4.x
                MoodleVersion.Moodle440 => "2024042200",
                MoodleVersion.Moodle441 => "2024042201",
                MoodleVersion.Moodle442 => "2024042202",
                MoodleVersion.Moodle443 => "2024042203",
                MoodleVersion.Moodle444 => "2024042204",
                MoodleVersion.Moodle445 => "2024042205",
                MoodleVersion.Moodle446 => "2024042206",
                MoodleVersion.Moodle447 => "2024042207",
                MoodleVersion.Moodle448 => "2024042208",
                MoodleVersion.Moodle449 => "2024042209",
                MoodleVersion.Moodle4410 => "2024042210",

                // Moodle 4.5.x
                MoodleVersion.Moodle450 => "2024100700",
                MoodleVersion.Moodle451 => "2024100701",
                MoodleVersion.Moodle452 => "2024100702",
                MoodleVersion.Moodle453 => "2024100703",
                MoodleVersion.Moodle454 => "2024100704",
                MoodleVersion.Moodle455 => "2024100705",
                MoodleVersion.Moodle456 => "2024100706",

                // Moodle 4.6.x
                MoodleVersion.Moodle460 => "2024102100",

                // Moodle 5.0.x
                MoodleVersion.Moodle500 => "2025041400",
                MoodleVersion.Moodle501 => "2025041401",

                _ => "2024100700",
            };
        }


        public string GetMoodleReleaseStringFromEnum(MoodleVersion version)
        {
            return version switch
            {
                // Moodle 4.4.x
                MoodleVersion.Moodle440 => $"4.4 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle441 => $"4.4.1 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle442 => $"4.4.2 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle443 => $"4.4.3 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle444 => $"4.4.4 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle445 => $"4.4.5 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle446 => $"4.4.6 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle447 => $"4.4.7 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle448 => $"4.4.8 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle449 => $"4.4.9 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle4410 => $"4.4.10 (Build: {GetMoodleVersionStringFromEnum(version)})",

                // Moodle 4.5.x
                MoodleVersion.Moodle450 => $"4.5 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle451 => $"4.5.1 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle452 => $"4.5.2 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle453 => $"4.5.3 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle454 => $"4.5.4 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle455 => $"4.5.5 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle456 => $"4.5.6 (Build: {GetMoodleVersionStringFromEnum(version)})",

                // Moodle 4.6.x
                MoodleVersion.Moodle460 => $"4.6 (Build: {GetMoodleVersionStringFromEnum(version)})",

                // Moodle 5.0.x
                MoodleVersion.Moodle500 => $"5.0 (Build: {GetMoodleVersionStringFromEnum(version)})",
                MoodleVersion.Moodle501 => $"5.0.1 (Build: {GetMoodleVersionStringFromEnum(version)})",

                _ => $"4.5 (Build: {GetMoodleVersionStringFromEnum(MoodleVersion.Moodle450)})",
            };
        }
    }
}
