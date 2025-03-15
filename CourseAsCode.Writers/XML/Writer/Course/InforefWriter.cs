using System.Xml;
using NLog;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class InforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "course/inforef.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public InforefWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public InforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

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
                        WriteOneInforefSection(writer, user, "user");
                    }

                    writer.WriteEndElement(); // Close </userref>

                    // Reference all roles
                    writer.WriteStartElement("roleref");

                    foreach (var role in course.Roles)
                    {
                        WriteOneInforefSection(writer, role, "role");
                    }

                    writer.WriteEndElement(); // Close </roleref>

                    // Reference all files
                    writer.WriteStartElement("fileref");

                    // TODO: Implement as soon as the file model is available
                    /*foreach (var file in files)
                    {
                        WriteOneInforefSection(writer, file, "file");
                    }*/

                    writer.WriteEndElement(); // Close </fileref>

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
        /// writes the part of the `inforef` section for the corresponding object type
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="instance">instance of the object to be referenced</param>
        /// <param name="type">type of the object to be referenced</param>
        private void WriteOneInforefSection(XmlWriter writer, dynamic instance, string type)
        {
            writer.WriteStartElement(type);

            writer.WriteElementString("id", instance.SystemDefaults.Id.ToString());

            writer.WriteEndElement(); // Close </type>
        }
    }
}
