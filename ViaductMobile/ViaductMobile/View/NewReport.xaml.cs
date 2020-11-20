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
        bool employeetable;
        decimal shouldBe;
        List<Employee> listEmployee = new List<Employee>();
        List<Operation> listOperation = new List<Operation>();
        ViewModels.ReportTableVM bindingReport;
        Xamarin.Forms.DataGrid.DataGrid employeeDataGridd, operationDataGridd;
        public NewReport(Report readReport)
        {
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            var bindingEmployee = new ViewModels.EmployeeTableVM(readReport);
            BindingContext = bindingEmployee;
            var bindingOperation = new ViewModels.OperationTableVM(readReport);
            BindingContext = bindingOperation;
            var bindingDeliverer = new ViewModels.DelivererTableVM(readReport);
            BindingContext = bindingDeliverer;
            shouldBe = CalculateShouldBe(bindingEmployee.Employees, bindingOperation.Operations, bindingDeliverer.Deliverers);
            bindingReport = new ViewModels.ReportTableVM(readReport);
            bindingReport.Reports[0].ShouldBe = shouldBe;
            BindingContext = bindingReport;
            listEmployee = Methods.reportEmployeeList;
            listOperation = Methods.reportOperationList;
        }
        public NewReport(Report readReport, List<Employee> listEmployeee, List<Operation> listOperationn, bool employeetable)
        {
            this.employeetable = employeetable;
            this.listEmployee = listEmployeee;
            this.listOperation = listOperationn;
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            var bindingEmployee = new ViewModels.EmployeeTableVM(listEmployee);
            BindingContext = bindingEmployee;
            var bindingOperation = new ViewModels.OperationTableVM(listOperation);
            BindingContext = bindingOperation;
            var bindingDeliverer = new ViewModels.DelivererTableVM(readReport);
            BindingContext = bindingDeliverer;
            shouldBe = CalculateShouldBe(bindingEmployee.Employees, bindingOperation.Operations, bindingDeliverer.Deliverers);
            bindingReport = new ViewModels.ReportTableVM(readReport);
            bindingReport.Reports[0].ShouldBe = shouldBe;
            BindingContext = bindingReport;
            if (employeetable == true)
                employeeExpander.IsExpanded = true;
            else
                operationExpander.IsExpanded = true;
        }

        public decimal CalculateShouldBe(List<Employee> employeeList, List<Operation> operationList, List<Deliverer> delivererList)
        {
            decimal shouldBe = 0;
            foreach(var item in employeeList)
            {
                shouldBe = shouldBe - item.Bonus - item.DayWage;
            }
            foreach(var item in operationList)
            {
                shouldBe = shouldBe + item.Amount;
            }
            foreach(var item in delivererList)
            {
                shouldBe = shouldBe + item.AmountToCash;
            }
            return shouldBe;
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
            await PopupNavigation.PushAsync(new AddReportOperation(listEmployee, listOperation, readReport));
        }
        [Obsolete]
        private async void Edit_Operation_Clicked(object sender, EventArgs e)
        {
            Operation selectedRow = (Operation)operationDataGrid.SelectedItem;
            await PopupNavigation.PushAsync(new AddReportOperation(listEmployee, listOperation, selectedRow, readReport));
        }
        private async void Delete_Operation_Clicked(object sender, EventArgs e)
        {
            operationExpander.IsExpanded = false;
            Operation x = (Operation)operationDataGrid.SelectedItem;
            await x.DeleteOperations(x);
            listOperation.Remove(x);
            operationDataGrid.ItemsSource = new ViewModels.OperationTableVM(listOperation).Operations;
            operationExpander.IsExpanded = true;
        }
        private async void Delete_Employee_Clicked(object sender, EventArgs e)
        {
            employeeExpander.IsExpanded = false;
            Employee x = (Employee)employeesDataGrid.SelectedItem;
            await x.DeleteEmployee(x);
            listEmployee.Remove(x);
            employeesDataGrid.ItemsSource = new ViewModels.EmployeeTableVM(listEmployee).Employees;
            employeeExpander.IsExpanded = true;
        }
        [Obsolete]
        private async void Add_Employee_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AddReportEmployee(listEmployee, listOperation, readReport));
        }

        [Obsolete]
        private async void Edit_Report_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new EditReport(bindingReport.Reports[0], reportDataGrid));
        }

        [Obsolete]
        private async void Edit_Employee_Clicked(object sender, EventArgs e)
        {
            Employee selectedRow = (Employee)employeesDataGrid.SelectedItem;
            await PopupNavigation.PushAsync(new AddReportEmployee(listEmployee, listOperation, selectedRow, readReport));
        }
    }
}