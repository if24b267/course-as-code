using CourseAsCode.Readers.Interfaces;

namespace CourseAsCode.Readers.AbstractClasses;

// TODO: why exactly do we need abstract classes again?
public abstract class AssignmentReader<T> : IModelObjectReader<T>
{
	public abstract T Read(string modelObjectPath);
}