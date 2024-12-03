package reader;

import model.Assignment;

public abstract class AssignmentReader implements IModelObjectReader<Assignment, String> {
    @Override
    public abstract Assignment Read(String fileContent);
}
