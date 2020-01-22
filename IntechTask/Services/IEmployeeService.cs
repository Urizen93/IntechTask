using IntechTask.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace IntechTask.Services
{
    public interface IEmployeeService
    {
        ReadOnlyObservableCollection<Employee> Employees { get; }

        ReadOnlyObservableCollection<Gender> Genders { get; }

        Task Refresh();

        Task SearchEmployees(string fullNameSearchTerm);

        [Pure]
        Task Save(Employee employee);

        [Pure]
        Task Delete(Employee employee);
    }
}