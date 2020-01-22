using DynamicData;
using DynamicData.Binding;
using IntechTask.DesktopClient.Misc;
using IntechTask.Infrastructure;
using IntechTask.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace IntechTask.DesktopClient.ViewModels
{
    public sealed class EmployeesViewModel : ObservableObject
    {
        private readonly IEmployeeService _service;
        private string _searchField;

        public EmployeesViewModel(IEmployeeService service, DispatcherScheduler scheduler)
        {
            _service = service;

            RefreshCommand = new AsyncCommand(Refresh);

            _service.Employees
                .ToObservableChangeSet()
                .Transform(employee => new EmployeeViewModel(employee, _service))
                .ObserveOn(scheduler)
                .Bind(out var employees)
                .Subscribe();
            Employees = employees;

            this.WhenValueChanged(x => x.SearchField, false)
                .Throttle(TimeSpan.FromSeconds(.5))
                .Subscribe(async searchTerm => await _service.SearchEmployees(searchTerm));

            Employees.WhenAnyPropertyChanged()
                .Subscribe(x => Debug.WriteLine(x));
        }

        public ReadOnlyObservableCollection<EmployeeViewModel> Employees { get; }

        public IAsyncCommand RefreshCommand { get; }

        public string SearchField
        {
            get => _searchField;
            set => SetProperty(ref _searchField, value);
        }

        private async Task Refresh() => await _service.Refresh();
    }
}