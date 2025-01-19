using CourseAsCode.Model;

namespace CourseAsCode.Writers.Interfaces;

public interface IWriter
{
    /// <summary>
    /// creates an output file in the desired format (depending on the implementation) from the Moodle course represented in the internal data model
    /// </summary>
    /// <param name="course">Initialized Moodle course</param>
    /// <param name="outputPath">Path in which the output is to be saved</param>
    /// <returns>
    /// <para>True if the course was written successfully</para>
    /// <para>False on error</para>
    /// </returns>
    public bool WriteCourse(Course course, string outputPath);
}