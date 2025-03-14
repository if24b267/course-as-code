using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityRolesWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_ROLES_RELATIVE_PATH = "roles.xml";
        
        
        /// <summary>
        /// Writes the roles.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteRoles(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLES_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
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
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}