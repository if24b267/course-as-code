using CourseAsCode.Model;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.Sections;

namespace CourseAsCode.Writers.XML.GeneralWriter
{
    public class SectionFilesWriterService : ISectionFilesWriterService
    {
        private readonly SectionWriter _sectionWriter;
        private readonly InforefSectionsWriter _inforefSectionsWriter;

        public SectionFilesWriterService(SectionWriter sectionWriter, InforefSectionsWriter inforefSectionsWriter)
        {
            _sectionWriter = sectionWriter ?? throw new ArgumentNullException(nameof(sectionWriter));
            _inforefSectionsWriter = inforefSectionsWriter ?? throw new ArgumentNullException(nameof(inforefSectionsWriter));
        }


        /// <summary>
        /// Writes section files for a specified course to the specified output path.
        /// </summary>
        /// <param name="course">The course object containing the sections to be written.</param>
        /// <param name="outputPath">The directory path where the section files will be written.</param>
        public void WriteSectionFiles(Course course, string outputPath)
        {
            _sectionWriter.WriteSections(outputPath, course.Sections);
            _inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);
        }
    }
}
