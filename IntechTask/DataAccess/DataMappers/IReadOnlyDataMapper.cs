using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace IntechTask.DataAccess.DataMappers
{
    public interface IReadOnlyDataMapper<T>
    {
        [Pure]
        Task<IReadOnlyList<T>> GetAll();
    }
}