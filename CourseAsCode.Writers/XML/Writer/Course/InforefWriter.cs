using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class InforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "course/inforef.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public InforefWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public InforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the inforef XML file for the specified course to the provided output path
        /// </summary>
        /// <param name="course">The course model containing the data to be written</param>
        /// <param name="outputPath">The file system path where the inforef.xml file will be created</param>
        public void WriteInforef(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_INFOREF_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("inforef");

                    
                    // Reference all enrolled users
                    writer.WriteStartElement("userref");

                    foreach (var user in course.EnrolledUsers)
                    {
                        WriteOneInforefSection(writer, user.SystemDefaults.Id, "user");
                    }

                    writer.WriteEndElement(); // Close </userref>

                    
                    // Reference all roles
                    writer.WriteStartElement("roleref");

                    foreach (var role in course.Roles)
                    {
                        WriteOneInforefSection(writer, role.SystemDefaults.RoleId, "role");
                    }

                    writer.WriteEndElement(); // Close </roleref>

                    
                    // Reference all files
                    writer.WriteStartElement("fileref");
                    
                    foreach (Section section in course.Sections)
                    {
                        foreach (Activity activity in section.Activities)
                        {
                            if (activity is MoodleFile moodleFile)
                            {
                                WriteOneInforefSection(writer, moodleFile.ActivityId, "file");
                            }
                        }
                    }
                    
                    writer.WriteEndElement(); // Close </fileref>
                    
                    
                    // Reference all question categories in the course
                    writer.WriteStartElement("question_categoryref");
                    
                    foreach (var categoryGrouping in course.Questions.GroupBy(q => q.Category))
                    {
                        WriteOneInforefSection(writer, categoryGrouping.First().CategoryId, "question_category");
                    }
                    
                    writer.WriteEndElement(); // Close </question_categoryref>

                    writer.WriteEndElement(); // Close </inforef>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_INFOREF_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_INFOREF_RELATIVE_PATH}");
                throw;
            }
        }

        
        /// <summary>
        /// writes the part of the `inforef` section for the corresponding object type.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="id">id of the object to be referenced</param>
        /// <param name="type">type of the object to be referenced</param>
        private void WriteOneInforefSection(XmlWriter writer, int id, string type)
        {
            writer.WriteStartElement(type);

            writer.WriteElementString("id", id.ToString());

            writer.WriteEndElement(); // Close </type>
        }
    }
}
