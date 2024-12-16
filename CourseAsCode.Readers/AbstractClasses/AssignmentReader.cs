using CourseAsCode.Readers.Interfaces;

namespace CourseAsCode.Readers.AbstractClasses;

public abstract class AssignmentReader<T> : IModelObjectReader<T>
{
	public abstract T Read(string filePath);
}