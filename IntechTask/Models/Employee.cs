using IntechTask.Infrastructure;
using System;
using System.ComponentModel;

namespace IntechTask.Models
{
    public sealed class Employee : ObservableObject, IEditableObject
    {
        private BackUp _backUp;

        private int _genderID;
        private string _fullName;
        private DateTime _dateOfBirth;

        public int? ID { get; set; }

        public int GenderID
        {
            get => _genderID;
            set => SetProperty(ref _genderID, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        public void BeginEdit() =>
            _backUp = new BackUp(GenderID, FullName, DateOfBirth);

        public void CancelEdit()
        {
            if (_backUp is null) return;

            GenderID = _backUp.GenderID;
            FullName = _backUp.FullName;
            DateOfBirth = _backUp.DateOfBirth;
        }

        public void EndEdit() => _backUp = null;

        private sealed class BackUp
        {
            public BackUp(int genderID, string fullName, DateTime dateOfBirth)
            {
                GenderID = genderID;
                FullName = fullName;
                DateOfBirth = dateOfBirth;
            }

            public int GenderID { get; }
            public string FullName { get; }
            public DateTime DateOfBirth { get; }
        }
    }
}