using System.Xml;
using NLog;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityRolesWriter : XmlWriterBase
    {
        private const string DEFAULT_ROLES_RELATIVE_PATH = "roles.xml";
        
        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ActivityRolesWriter() 
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }
        
        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public ActivityRolesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }
        
        /// <summary>
        /// Writes the roles.xml file required for an activity
        /// </summary>
        /// <param name="outputPath">The output directory path</param>
        public void WriteRoles(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLES_RELATIVE_PATH));
                
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("roles");
                    
                    writer.WriteStartElement("role_overrides");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <role_overrides>
                    
                    writer.WriteStartElement("role_assignments");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <role_assignments>
                    
                    writer.WriteEndElement(); // Close <roles>
                    writer.WriteEndDocument();
                }
                
                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
