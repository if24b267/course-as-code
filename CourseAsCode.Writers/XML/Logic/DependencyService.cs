using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.GeneralWriter;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins;
using CourseAsCode.Writers.XML.Writer.Sections;
using Microsoft.Extensions.DependencyInjection;

namespace CourseAsCode.Writers.XML.Logic
{
    public class DependencyService
    {
        /// <summary>
        /// Registers all XML related services and helpers required for the application into the provided service collection
        /// </summary>
        /// <param name="services">
        /// The service collection to which the XML services and dependencies will be added.
        /// </param>
        public static void ConfigureXmlServices(IServiceCollection services)
        {
            // General XML services and helpers
            services.AddSingleton<IXmlWriterFactory, DefaultXmlWriterFactory>();
            services.AddSingleton<IGeneralHelper, GeneralHelper>();
            services.AddSingleton<IMoodleVersionHelper, MoodleVersionHelper>();
            services.AddSingleton<ICourseHelper, CourseHelper>();
            services.AddSingleton<ISequentialIdGenerator, SequentialIdGenerator>();
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IRolesHelper, RolesHelper>();
            services.AddSingleton<IUserHelper, UserHelper>();
            services.AddSingleton<IQuizHelper, QuizHelper>();
            services.AddSingleton<ITagsWriter, TagsWriter>();

            // Plugins for the QuestionWriter
            services.AddSingleton<IQuestionPluginWriter, DragDropTextPluginWriter>();
            services.AddSingleton<IQuestionPluginWriter, DropdownPluginWriter>();
            services.AddSingleton<IQuestionPluginWriter, MultiChoicePluginWriter>();
            services.AddSingleton<IQuestionPluginWriter, CodeRunnerPluginWriter>();

            // Register QuestionsWriter (depends on the plugins registered above)
            services.AddSingleton<QuestionsWriter>();

            // Register all other writers
            // -- General Writers --
            services.AddSingleton<CalendarWriter>();
            services.AddSingleton<CommentsWriter>();
            services.AddSingleton<CompetenciesWriter>();
            services.AddSingleton<CompletiondefaultsWriter>();
            services.AddSingleton<ContentbankWriter>();
            services.AddSingleton<CourseWriter>();
            services.AddSingleton<FilesWriter>();
            services.AddSingleton<GradehistoryWriter>();
            services.AddSingleton<GradebookWriter>();
            services.AddSingleton<GroupsWriter>();
            services.AddSingleton<MoodleBackupWriter>();
            services.AddSingleton<OutcomesWriter>();
            services.AddSingleton<QuestionsWriter>();
            services.AddSingleton<RolesWriter>();
            services.AddSingleton<ScalesWriter>();
            services.AddSingleton<UsersWriter>();
            services.AddSingleton<EnrolmentsWriter>();
            services.AddSingleton<FiltersWriter>();
            services.AddSingleton<InforefWriter>();
            services.AddSingleton<RoleAssignmentsWriter>();
            services.AddSingleton<BadgesWriter>();
            services.AddSingleton<CompletionWriter>();
            services.AddSingleton<SectionWriter>();
            services.AddSingleton<InforefSectionsWriter>();

            // -- Activity Writers --
            // Common Activity Writers
            services.AddSingleton<ActivityGradesWriter>();
            services.AddSingleton<ActivityGradehistoryWriter>();
            services.AddSingleton<ActivityInforefWriter>();
            services.AddSingleton<ActivityRolesWriter>();
            services.AddSingleton<ActivityXapistateWriter>();
            services.AddSingleton<ActivityCalendarWriter>();
            services.AddSingleton<ActivityCommentsWriter>();
            services.AddSingleton<ActivityCompetenciesWriter>();
            services.AddSingleton<ActivityCompletionWriter>();
            services.AddSingleton<ActivityFiltersWriter>();

            // Specific Activity Writers
            services.AddSingleton<LabelWriter>();
            services.AddSingleton<ModuleWriterLabel>();
            services.AddSingleton<AssignmentWriter>();
            services.AddSingleton<ModuleWriterAssignment>();
            services.AddSingleton<UrlWriter>();
            services.AddSingleton<ModuleWriterUrl>();
            services.AddSingleton<FileWriter>();
            services.AddSingleton<ModuleWriterFile>();
            services.AddSingleton<ModuleWriterQuiz>();
            services.AddSingleton<QuizWriter>();

            // Register the main writer
            services.AddSingleton<IActivitiesWriterService, ActivitiesWriterService>();
            services.AddSingleton<ISectionFilesWriterService, SectionFilesWriterService>();
            services.AddSingleton<IWriter, Writers.Writer>();
        }
    }
}
