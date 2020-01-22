using IntechTask.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace IntechTask.DataAccess.DataMappers
{
    public interface IEmployeeDataMapper : IDataMapper<Employee>
    {
        [Pure]
        Task<IReadOnlyList<Employee>> FindByFullName(string searchTerm);
    }
}