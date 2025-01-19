namespace CourseAsCode.Readers.Interfaces;

public interface IModelObjectReader<T>
{
	public T? Read(string modelObjectPath);
}