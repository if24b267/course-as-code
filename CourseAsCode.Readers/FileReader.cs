package reader;

import model.FileResource;

public abstract class FileReader implements IModelObjectReader<FileResource, String> {
    @Override
    public abstract FileResource Read(String fileContent);
}
