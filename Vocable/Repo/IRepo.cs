namespace Vocable
{
    public interface IRepo<T> where T : IContainId
    {
        T Create(T item);
        T Delete(int id);
        List<T> ReadAll();
        T ReadById(int id);
        T Update(int id, T updatedItem);
    }
}