using DynamicData;
using IntechTask.DataAccess.DataMappers;
using IntechTask.Models;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;

namespace IntechTask.Services
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();
        private readonly ObservableCollection<Gender> _genders = new ObservableCollection<Gender>();

        private readonly ILogger<EmployeeService> _logger;
        private readonly IReadOnlyDataMapper<Gender> _genderMapper;
        private readonly IEmployeeDataMapper _employeeMapper;

        public EmployeeService(IReadOnlyDataMapper<Gender> genderMapper, IEmployeeDataMapper employeeMapper, ILogger<EmployeeService> logger)
        {
            _genderMapper = genderMapper;
            _employeeMapper = employeeMapper;
            _logger = logger;

            Employees = new ReadOnlyObservableCollection<Employee>(_employees);
            Genders = new ReadOnlyObservableCollection<Gender>(_genders);
        }

        public ReadOnlyObservableCollection<Employee> Employees { get; }

        public ReadOnlyObservableCollection<Gender> Genders { get; }

        public async Task Refresh()
        {
            _employees.Clear();
            _genders.Clear();

            try
            {
                _genders.AddRange(await _genderMapper.GetAll());
                _employees.AddRange(await _employeeMapper.GetAll());
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error during loading of the list of employees and genders");
                throw;
            }
        }

        public async Task SearchEmployees(string fullNameSearchTerm)
        {
            _employees.Clear();

            try
            {
                _employees.AddRange(await _employeeMapper.FindByFullName(fullNameSearchTerm));
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error during the search of specific employees");
                throw;
            }
        }

        public async Task Save(Employee employee)
        {
            try
            {
                if (employee.ID is null)
                {
                    await _employeeMapper.Insert(employee);
                    _employees.Add(employee);
                }
                else
                {
                    await _employeeMapper.Update(employee);
                }
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "-------------");
                throw;
            }
        }

        public async Task Delete(Employee employee)
        {
            try
            {
                await _employeeMapper.Delete(employee);
                _employees.Remove(employee);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "-------------");
                throw;
            }
        }
    }
}