package reader;

import model.URLResource;

public abstract class URLReader implements IModelObjectReader<URLResource, String> {
    @Override
    public abstract URLResource Read(String fileContent);
}
