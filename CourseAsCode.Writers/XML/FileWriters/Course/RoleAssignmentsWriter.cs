using System.Xml;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Static;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class RoleAssignmentsWriter : XmlWriterBase
    {
        private const string DEFAULT_ROLE_ASSIGNMENTS_RELATIVE_PATH = "course/roles.xml";
        private readonly IGeneralHelper _helper;


        /// <summary>
        /// Creates a new instance of the <see cref="RoleAssignmentsWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="RoleAssignmentsWriter"/></param>
        /// <param name="helper">General Helper providing various helper methods</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public RoleAssignmentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<RoleAssignmentsWriter> logger, IGeneralHelper helper)
            : base(xmlWriterFactory, logger)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }


        /// <summary>
        /// writes the information on which role is assigned to which user | course/roles.xml.
        /// </summary>
        /// <param name="course">initialized Moodle Course.</param>
        /// <param name="outputPath"></param>
        public void WriteRoleAssignments(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLE_ASSIGNMENTS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("roles");

                    // Role overrides
                    writer.WriteStartElement("role_overrides");
                    writer.WriteEndElement(); // Close </role_overrides>

                    // Role assignments
                    writer.WriteStartElement("role_assignments");

                    for (int i = 0; i < course.EnrolledUsers.Count; i++)
                    {
                        WriteSingleRoleAssignment(writer, course.EnrolledUsers[i], i);
                        LogDebug($"Assigned role {course.EnrolledUsers[i].Role.SystemDefaults.RoleId} to user {course.EnrolledUsers[i].SystemDefaults.Id}: '{course.EnrolledUsers[i].Username}'");
                    }

                    writer.WriteEndElement(); // Close </role_assignments>

                    writer.WriteEndElement(); // Close </roles>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_ROLE_ASSIGNMENTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_ROLE_ASSIGNMENTS_RELATIVE_PATH}");
                throw;
            }
        }


        /// <summary>
        /// writes the assignment of one specific user to one specific role; to be used inside the `roles` tag within course/roles.xml.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="user">User for whom the entry is to be created.</param>
        /// <param name="assignmentId">Unique ID for this specific assignment.</param>
        private void WriteSingleRoleAssignment(XmlWriter writer, User user, int assignmentId)
        {
            WriteStartElementWithAttributes(writer, "assignment", new Dictionary<string, string>
            {
                { "id", assignmentId.ToString() },
            });

            var elements = new Dictionary<string, string>
            {
                { "roleid", user.Role.SystemDefaults.RoleId.ToString() },
                { "userid", user.SystemDefaults.Id.ToString() },
                { "timemodified", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.RoleChange) },
                { "modifierid", GeneralDefaults.DEFAULTADMINUSERID.ToString() },
                { "component", "" }, // TODO: Determine function of this field
                { "itemid", "0" },   // TODO: Determine function of this field
                { "sortorder", "0" }, // TODO: Determine function of this field
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close </assignment>
        }
    }
}
