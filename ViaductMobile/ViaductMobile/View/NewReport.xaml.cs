using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewReport : ContentPage
    {
        Report readReport;
        List<Employee> listEmployee = new List<Employee>();
        List<Operation> listOperation = new List<Operation>();
        Xamarin.Forms.DataGrid.DataGrid employeeDataGridd, operationDataGridd;
        public NewReport(Report readReport)
        {
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.ReportTableVM(readReport);
            BindingContext = new ViewModels.EmployeeTableVM(readReport);
            BindingContext = new ViewModels.OperationTableVM(readReport);
            BindingContext = new ViewModels.DelivererTableVM(readReport);
            listEmployee = Methods.reportEmployeeList;
            listOperation = Methods.reportOperationList;
        }
        public NewReport(Report readReport, List<Employee> listEmployeee, List<Operation> listOperationn)
        {
            this.listEmployee = listEmployeee;
            this.listOperation = listOperationn;
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.ReportTableVM(readReport);
            BindingContext = new ViewModels.EmployeeTableVM(listEmployeee);
            BindingContext = new ViewModels.OperationTableVM(readReport);
            BindingContext = new ViewModels.DelivererTableVM(readReport);
        }
        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }

        [Obsolete]
        private async void Add_Operation_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AddReportOperation(operationDataGrid, readReport));
        }
        [Obsolete]
        private async void Edit_Operation_Clicked(object sender, EventArgs e)
        {
            Operation x = (Operation)operationDataGrid.SelectedItem;
            await PopupNavigation.PushAsync(new AddReportOperation(x, operationDataGrid, readReport));
        }
        private async void Delete_Operation_Clicked(object sender, EventArgs e)
        {
            Operation x = (Operation)operationDataGrid.SelectedItem;
            await x.DeleteOperations(x);
            operationDataGrid.ItemsSource = new ViewModels.OperationTableVM(readReport).Operations;
        }
        private async void Delete_Employee_Clicked(object sender, EventArgs e)
        {
            Employee x = (Employee)employeesDataGrid.SelectedItem;
            await x.DeleteEmployee(x);
            listEmployee.Remove(x);
            employeesDataGrid.ItemsSource = new ViewModels.EmployeeTableVM(listEmployee).Employees;
        }
        [Obsolete]
        private async void Add_Employee_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AddReportEmployee(listEmployee, listOperation, readReport));
        }
        [Obsolete]
        private async void Edit_Employee_Clicked(object sender, EventArgs e)
        {
            Employee x = (Employee)employeesDataGrid.SelectedItem;
            await PopupNavigation.PushAsync(new AddReportEmployee(x, employeesDataGrid, readReport));
        }
    }
}