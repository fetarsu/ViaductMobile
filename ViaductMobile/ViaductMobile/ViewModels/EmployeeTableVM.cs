using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class EmployeeTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Employee> employees;
        private Employee selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged(nameof(Employees)); }
        }
        public Employee SelectedEmpolyee
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }
        public ICommand RefreshCommand { get; set; }
        #endregion

        public EmployeeTableVM()
        {
            Employee employee = new Employee();
            Employees = Task.Run(() => employee.ReadEmployee()).Result;
            RefreshCommand = new Command(CmdRefresh);
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(3000);
            IsRefreshing = false;
        }
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
