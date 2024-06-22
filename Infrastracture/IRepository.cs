namespace Infrastracture;

public interface IRepository<T>
{
    List<T> GetAll();
}