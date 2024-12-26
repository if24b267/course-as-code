using System.Xml;
using CourseAsCode.Writers.Static;
using CourseAsCode.Model;
using CourseAsCode.Writers.Helper;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class RoleAssignmentsWriter : XmlWriterBase
{
    private readonly GeneralHelper _helper = GeneralHelper.Instance;
    
    private new readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = false
    };


    /// <summary>
    /// writes the information on which role is assigned to which user | course/roles.xml
    /// </summary>
    /// <param name="course">initialized Moodle Course</param>
    /// <param name="outputPath"></param>
    public void WriteRoleAssignments(Model.Course course, string outputPath)
    {
        var filePath = Path.GetFullPath(Path.Combine(outputPath, "course/roles.xml"));
        using var writer = XmlWriter.Create(filePath, _settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("roles");

        // Role overrides
        writer.WriteStartElement("role_overrides");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close </role_overrides>

        // Role assignments
        writer.WriteStartElement("role_assignments");

        for (int i = 0; i < course.EnrolledUsers.Count; i++)
        {
            WriteSingleRoleAssignment(writer, course.EnrolledUsers[i], i);
        }

        writer.WriteEndElement(); // Close </role_assignments>

        writer.WriteEndElement(); // Close </roles>
        writer.WriteEndDocument();
    }


    /// <summary>
    /// writes the assignment of one specific user to one specific role; to be used inside the `roles` tag within course/roles.xml
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="user">User for whom the entry is to be created</param>
    /// <param name="assignmentId">Unique ID for this specific assignment</param>
    private void WriteSingleRoleAssignment(XmlWriter writer, User user, int assignmentId)
    {
        WriteStartElementWithAttributes(writer, "assignment", new Dictionary<string, string>
        {
            { "id", assignmentId.ToString() }
        });

        var elements = new Dictionary<string, string>
        {
            { "roleid", user.Role.SystemDefaults.Id.ToString() },
            { "userid", user.SystemDefaults.Id.ToString() },
            { "timemodified", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.RoleChange) },
            { "modifierid", GeneralDefaults.DEFAULT_ADMIN_USER_ID.ToString() },
            { "component", "" }, // TODO: Determine function of this field
            { "itemid", "0" }, // TODO: Determine function of this field
            { "sortorder", "0" } // TODO: Determine function of this field
        };

        WriteElements(writer, elements);

        writer.WriteEndElement(); // Close </assignment>
    }
}