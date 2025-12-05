using CourseAsCode.Model;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.FileWriters.Questions;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer;

namespace CourseAsCode.Writers.XML.GeneralWriter
{
    public class RootFilesWriterService : IRootFilesWriterService
    {
        private readonly BadgesWriter _badgesWriter;
        private readonly CompletionWriter _completionWriter;
        private readonly FilesWriter _filesWriter;
        private readonly GradebookWriter _gradebookWriter;
        private readonly GradehistoryWriter _gradehistoryWriter;
        private readonly GroupsWriter _groupsWriter;
        private readonly MoodleBackupWriter _moodleBackupWriter;
        private readonly OutcomesWriter _outcomesWriter;
        private readonly QuestionsWriter _questionsWriter;
        private readonly RolesWriter _rolesWriter;
        private readonly ScalesWriter _scalesWriter;
        private readonly UsersWriter _usersWriter;


        public RootFilesWriterService(
            BadgesWriter badgesWriter,
            CompletionWriter completionWriter,
            FilesWriter filesWriter,
            GradebookWriter gradebookWriter,
            GradehistoryWriter gradehistoryWriter,
            GroupsWriter groupsWriter,
            MoodleBackupWriter moodleBackupWriter,
            OutcomesWriter outcomesWriter,
            QuestionsWriter questionsWriter,
            RolesWriter rolesWriter,
            ScalesWriter scalesWriter,
            UsersWriter usersWriter)
        {
            _badgesWriter = badgesWriter ?? throw new ArgumentNullException(nameof(badgesWriter));
            _completionWriter = completionWriter ?? throw new ArgumentNullException(nameof(completionWriter));
            _filesWriter = filesWriter ?? throw new ArgumentNullException(nameof(filesWriter));
            _gradebookWriter = gradebookWriter ?? throw new ArgumentNullException(nameof(gradebookWriter));
            _gradehistoryWriter = gradehistoryWriter ?? throw new ArgumentNullException(nameof(gradehistoryWriter));
            _groupsWriter = groupsWriter ?? throw new ArgumentNullException(nameof(groupsWriter));
            _moodleBackupWriter = moodleBackupWriter ?? throw new ArgumentNullException(nameof(moodleBackupWriter));
            _outcomesWriter = outcomesWriter ?? throw new ArgumentNullException(nameof(outcomesWriter));
            _questionsWriter = questionsWriter ?? throw new ArgumentNullException(nameof(questionsWriter));
            _rolesWriter = rolesWriter ?? throw new ArgumentNullException(nameof(rolesWriter));
            _scalesWriter = scalesWriter ?? throw new ArgumentNullException(nameof(scalesWriter));
            _usersWriter = usersWriter ?? throw new ArgumentNullException(nameof(usersWriter));
        }


        /// <summary>
        /// Writes the XML files belonging in the root directory of the backup file of the given course to the specified output directory.
        /// </summary>
        /// <param name="course">The course object containing the data required for writing root files.</param>
        /// <param name="outputPath">The directory path where the generated files will be written.</param>
        public void WriteRootFiles(Course course, string outputPath)
        {
            _badgesWriter.WriteBadges(outputPath);
            _completionWriter.WriteCompletion(course, outputPath);
            _filesWriter.WriteFiles(course, outputPath);
            _gradebookWriter.WriteGradebook(course, outputPath);
            _gradehistoryWriter.WriteGradehistory(course, outputPath);
            _groupsWriter.WriteGroups(course, outputPath);
            _moodleBackupWriter.WriteMoodleBackup(course, outputPath);
            _outcomesWriter.WriteOutcomes(course, outputPath);
            _questionsWriter.WriteQuestions(course, outputPath);
            _rolesWriter.WriteRoles(course, outputPath);
            _scalesWriter.WriteScales(course, outputPath);
            _usersWriter.WriteUsers(course, outputPath);
        }
    }
}
