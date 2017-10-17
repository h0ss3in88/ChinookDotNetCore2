using System.Collections.Generic;

namespace Chinook.Model.Data
{
    public interface IQuery<T> where T : new()
    {
        IRunner Runner { get; set; }
        IEnumerable<T> GetAll();
        T GetById(int id);
    }
}