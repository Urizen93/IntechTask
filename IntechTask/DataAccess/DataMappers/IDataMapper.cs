using System.Threading.Tasks;

namespace IntechTask.DataAccess.DataMappers
{
    public interface IDataMapper<T> : IReadOnlyDataMapper<T>
    {
        Task Insert(T entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}