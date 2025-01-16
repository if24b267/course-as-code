using CourseAsCode.Readers.Interfaces;

namespace CourseAsCode.Readers.AbstractClasses;

public abstract class UrlReader<T> : IModelObjectReader<T>
{
	public abstract T? Read(string modelObjectPath);
}