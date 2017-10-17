namespace Chinook.Model.Data
{
    public class Command<T> : ICommand<T> where T : new()
    {
        public IRunner Runner { get; set; }

        public Command(IRunner runner)
        {
            Runner = runner;
        }
        public int Add(T entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(int id, T entity)
        {
            throw new System.NotImplementedException();
        }

        public int Remove(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}