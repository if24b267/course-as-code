using CourseAsCode.Readers.Interfaces;

namespace CourseAsCode.Readers.AbstractClasses;

public abstract class SectionReader<T> : IModelObjectReader<T>
{
	public abstract T? Read(string modelObjectPath);
}