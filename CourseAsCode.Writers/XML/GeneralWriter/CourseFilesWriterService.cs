using CourseAsCode.Model;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.Course;

namespace CourseAsCode.Writers.XML.GeneralWriter
{
    public class CourseFilesWriterService : ICourseFilesWriterService
    {
        private readonly CalendarWriter _calendarWriter;
        private readonly CommentsWriter _commentsWriter;
        private readonly CompetenciesWriter _competenciesWriter;
        private readonly CompletiondefaultsWriter _completiondefaultsWriter;
        private readonly ContentbankWriter _contentbankWriter;
        private readonly CourseWriter _courseWriter;
        private readonly EnrolmentsWriter _enrolmentsWriter;
        private readonly FiltersWriter _filtersWriter;
        private readonly InforefWriter _inforefWriter;
        private readonly RoleAssignmentsWriter _roleAssignmentsWriter;

        public CourseFilesWriterService(
            CalendarWriter calendarWriter,
            CommentsWriter commentsWriter,
            CompetenciesWriter competenciesWriter,
            CompletiondefaultsWriter completiondefaultsWriter,
            ContentbankWriter contentbankWriter,
            CourseWriter courseWriter,
            EnrolmentsWriter enrolmentsWriter,
            FiltersWriter filtersWriter,
            InforefWriter inforefWriter,
            RoleAssignmentsWriter roleAssignmentsWriter)
        {
            _calendarWriter = calendarWriter ?? throw new ArgumentNullException(nameof(calendarWriter));
            _commentsWriter = commentsWriter ?? throw new ArgumentNullException(nameof(commentsWriter));
            _competenciesWriter = competenciesWriter ?? throw new ArgumentNullException(nameof(competenciesWriter));
            _completiondefaultsWriter = completiondefaultsWriter ?? throw new ArgumentNullException(nameof(completiondefaultsWriter));
            _contentbankWriter = contentbankWriter ?? throw new ArgumentNullException(nameof(contentbankWriter));
            _courseWriter = courseWriter ?? throw new ArgumentNullException(nameof(courseWriter));
            _enrolmentsWriter = enrolmentsWriter ?? throw new ArgumentNullException(nameof(enrolmentsWriter));
            _filtersWriter = filtersWriter ?? throw new ArgumentNullException(nameof(filtersWriter));
            _inforefWriter = inforefWriter ?? throw new ArgumentNullException(nameof(inforefWriter));
            _roleAssignmentsWriter = roleAssignmentsWriter ?? throw new ArgumentNullException(nameof(roleAssignmentsWriter));
        }


        /// <summary>
        /// Writes the files within the course directory of the Moodle Backup
        /// </summary>
        /// <param name="course">The course object containing the data to write to files.</param>
        /// <param name="outputPath">The directory path where the course files will be written (without the course subdirectory).</param>
        public void WriteCourseFiles(Course course, string outputPath)
        {
            _calendarWriter.WriteCalendar(course, outputPath);
            _commentsWriter.WriteComments(course, outputPath);
            _competenciesWriter.WriteCompetencies(course, outputPath);
            _completiondefaultsWriter.WriteCompletionDefaults(course, outputPath);
            _contentbankWriter.WriteContentbank(course, outputPath);
            _courseWriter.WriteCourse(course, outputPath);
            _enrolmentsWriter.WriteEnrolment(course, outputPath);
            _filtersWriter.WriteFilters(course, outputPath);
            _inforefWriter.WriteInforef(course, outputPath);
            _roleAssignmentsWriter.WriteRoleAssignments(course, outputPath);
        }
    }
}
