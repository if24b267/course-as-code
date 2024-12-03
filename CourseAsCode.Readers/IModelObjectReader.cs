package reader;

public interface IModelObjectReader<T, TFileObject>
{
    T Read(TFileObject fileObject);
}
