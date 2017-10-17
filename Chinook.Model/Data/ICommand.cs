namespace Chinook.Model.Data
{
    public interface ICommand<T> where T : new()
    {
        IRunner Runner { get; set; }
        int Add(T entity);
        int Update(int id, T entity);
        int Remove(int id);
    }
}