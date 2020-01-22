using DynamicData.Binding;
using IntechTask.DesktopClient.Misc;
using IntechTask.Infrastructure;
using IntechTask.Models;
using IntechTask.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace IntechTask.DesktopClient.ViewModels
{
    public sealed class EmployeeViewModel : ObservableObject, IEditableObject
    {
        private readonly IEmployeeService _service;

        private Gender _gender;
        private bool _isEditing;

        public EmployeeViewModel(Employee employee, IEmployeeService service)
        {
            _service = service;
            Employee = employee;
            DeleteCommand = new AsyncCommand(Delete);

            Employee.WhenValueChanged(x => x.GenderID)
                .Subscribe(genderID => Gender = _service.Genders.First(gender => gender.ID == genderID));

            this.WhenValueChanged(x => x.Gender, false)
                .Subscribe(gender => Employee.GenderID = gender.ID);
        }

        public ReadOnlyObservableCollection<Gender> AvailableGenders => _service.Genders;

        public Employee Employee { get; }

        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public IAsyncCommand DeleteCommand { get; }

        public async Task Delete() => await _service.Delete(Employee);

        public void BeginEdit()
        {
            Employee.BeginEdit();
            _isEditing = true;
        }

        public void CancelEdit()
        {
            Employee.CancelEdit();
            _isEditing = false;
        }

        public void EndEdit()
        {
            if (!_isEditing) return;

            Observable.FromAsync(async () => await _service.Save(Employee))
                .Subscribe();
        }
    }
}