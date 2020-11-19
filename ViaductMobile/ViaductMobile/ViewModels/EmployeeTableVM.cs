using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class EmployeeTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Employee> employees;
        Report readReport;
        List<Employee> empList = new List<Employee>();
        Employee employeee;
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

        public EmployeeTableVM(Report readReport)
        {
            this.readReport = readReport;
            Employee employee = new Employee();
            Employees = Task.Run(() => employee.ReadEmployeeReport(readReport)).Result;
            Methods.reportEmployeeList = Employees;
            RefreshCommand = new Command(CmdRefresh);
        }
        public EmployeeTableVM(List<Employee> list)
        {
            this.empList = list;
            Employees = empList;
            Methods.reportEmployeeList = Employees;
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
