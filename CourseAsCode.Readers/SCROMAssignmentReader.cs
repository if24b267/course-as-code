package reader;

import model.Assignment;

public class SCORMAssignmentReader extends AssignmentReader
{

    @Override
    public Assignment Read(String fileContent)
{
    // Placeholder for SCORM-specific logic
    Assignment assignment = new Assignment();
    assignment.setTitle("SCORM Assignment");
    assignment.setDescription("Parsed from SCORM data");
    return assignment;
}
}
