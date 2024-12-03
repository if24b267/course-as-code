using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Readers.XML
{
    public class XMLCourseReader
    {
        public Course Read(string filePath)
        {
            Course course = new Course();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "fullname":
                                course.FullName = reader.ReadElementContentAsString();
                                break;
                            case "shortname":
                                course.ShortName = reader.ReadElementContentAsString();
                                break;
                            case "summary":
                                course.Description = reader.ReadElementContentAsString();
                                break;
                        }
                    }
                }
            }

            return course;
        }
    }
}
