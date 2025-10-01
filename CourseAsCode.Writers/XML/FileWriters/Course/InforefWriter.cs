using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class InforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "course/inforef.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="InforefWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="InforefWriter"/></param>
        public InforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger<InforefWriter> logger)
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

                    //TODO: Remove hardcoded values
                    /*
                    // <userref>
                    writer.WriteStartElement("userref");
                    foreach (var user in course.EnrolledUsers)
                    {
                        writer.WriteStartElement("user");
                        writer.WriteElementString("id", user.SystemDefaults.Id.ToString());
                        writer.WriteEndElement(); // </user>
                    }
                    writer.WriteEndElement(); // </userref>

                    // <roleref>
                    writer.WriteStartElement("roleref");
                    foreach (var role in course.Roles)
                    {
                        writer.WriteStartElement("role");
                        writer.WriteElementString("id", role.SystemDefaults.RoleId.ToString());
                        writer.WriteEndElement(); // </role>
                    }
                    writer.WriteEndElement(); // </roleref>
                    */
                    writer.WriteStartElement("userref");
                    writer.WriteStartElement("user");
                    writer.WriteElementString("id", "2");
                    writer.WriteEndElement(); // </user>
                    writer.WriteEndElement(); // </userref>

                    // <roleref>
                    writer.WriteStartElement("roleref");
                    writer.WriteStartElement("role");
                    writer.WriteElementString("id", "3");
                    writer.WriteEndElement(); // </role>
                    writer.WriteStartElement("role");
                    writer.WriteElementString("id", "5");
                    writer.WriteEndElement(); // </role>
                    writer.WriteEndElement(); // </roleref>

                    // <fileref>
                    writer.WriteStartElement("fileref");
                    foreach (var section in course.Sections)
                    {
                        foreach (var activity in section.Activities)
                        {
                            if (activity is MoodleFile file)
                            {
                                writer.WriteStartElement("file");
                                writer.WriteElementString("id", file.ActivityId.ToString());
                                writer.WriteEndElement(); // </file>
                            }
                        }
                    }
                    writer.WriteEndElement(); // </fileref>

                    // <question_categoryref>
                    writer.WriteStartElement("question_categoryref");
                    var writtenCategoryIds = new HashSet<int>();
                    foreach (var question in course.Questions)
                    {
                        if (writtenCategoryIds.Add(question.CategoryId))
                        {
                            writer.WriteStartElement("question_category");
                            writer.WriteElementString("id", question.CategoryId.ToString());
                            writer.WriteEndElement(); // </question_category>
                        }
                    }
                    writer.WriteEndElement(); // </question_categoryref>

                    writer.WriteEndElement(); // </inforef>
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

            // Write the ID directly
            writer.WriteElementString("id", id.ToString());

            writer.WriteEndElement(); // Close </type>
        }
    }
}
