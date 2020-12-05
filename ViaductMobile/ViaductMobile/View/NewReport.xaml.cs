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
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Acr.UserDialogs;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewReport : ContentPage
    {
        Report readReport;
        User loggedUser;
        bool employeetable;
        decimal shouldBe;
        List<Employee> listEmployee = new List<Employee>();
        List<Operation> listOperation = new List<Operation>();
        ViewModels.ReportTableVM bindingReport;
        ViewModels.EmployeeTableVM bindingEmployee;
        ViewModels.OperationTableVM bindingOperation;
        ViewModels.DelivererTableVM bindingDeliverer;
        Xamarin.Forms.DataGrid.DataGrid employeeDataGridd, operationDataGridd;
        public NewReport(Report readReport)
        {
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            bindingEmployee = new ViewModels.EmployeeTableVM(readReport);
            BindingContext = bindingEmployee;
            bindingOperation = new ViewModels.OperationTableVM(readReport);
            BindingContext = bindingOperation;
            bindingDeliverer = new ViewModels.DelivererTableVM(readReport);
            BindingContext = bindingDeliverer;
            shouldBe = CalculateShouldBe(bindingEmployee.Employees, bindingOperation.Operations, bindingDeliverer.Deliverers, readReport);
            bindingReport = new ViewModels.ReportTableVM(readReport);
            bindingReport.Reports[0].ShouldBe = shouldBe;
            bindingReport.Reports[0].Difference = bindingReport.Reports[0].AmountIn - shouldBe;
            BindingContext = bindingReport;
            listEmployee = Methods.reportEmployeeList;
            listOperation = Methods.reportOperationList;
            BlockButtons();
            if (readReport.Closed == true)
                ClosedDay();
        }
        public NewReport(Report readReport, User loggedUser)
        {
            this.loggedUser = loggedUser;
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            bindingEmployee = new ViewModels.EmployeeTableVM(readReport);
            BindingContext = bindingEmployee;
            bindingOperation = new ViewModels.OperationTableVM(readReport);
            BindingContext = bindingOperation;
            bindingDeliverer = new ViewModels.DelivererTableVM(readReport);
            BindingContext = bindingDeliverer;
            shouldBe = CalculateShouldBe(bindingEmployee.Employees, bindingOperation.Operations, bindingDeliverer.Deliverers, readReport);
            bindingReport = new ViewModels.ReportTableVM(readReport);
            bindingReport.Reports[0].ShouldBe = shouldBe;
            bindingReport.Reports[0].Difference = bindingReport.Reports[0].AmountIn - shouldBe;
            BindingContext = bindingReport;
            listEmployee = Methods.reportEmployeeList;
            listOperation = Methods.reportOperationList;
            BlockButtons();
            if (readReport.Closed == true)
                ClosedDay();
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
            shouldBe = CalculateShouldBe(bindingEmployee.Employees, bindingOperation.Operations, bindingDeliverer.Deliverers, readReport);
            bindingReport = new ViewModels.ReportTableVM(readReport);
            bindingReport.Reports[0].ShouldBe = shouldBe;
            bindingReport.Reports[0].Difference = bindingReport.Reports[0].AmountIn - shouldBe;
            BindingContext = bindingReport;
            if (employeetable == true)
                employeeExpander.IsExpanded = true;
            else
                operationExpander.IsExpanded = true;
            if (readReport.Closed == true)
                ClosedDay();
        }
        public void ClosedDay()
        {
            addEmployeeButton.IsVisible = editEmployeeButton.IsVisible = deleteEmployeeButton.IsVisible =
                addOperationButton.IsVisible = editOperationButton.IsVisible = deleteOperationButton.IsVisible =
                editReportButton.IsVisible = false;
            sendReportButton.Text = "Przywróć dzień";
            sendReportButton.BackgroundColor = Color.Green;
        }

        public decimal CalculateShouldBe(List<Employee> employeeList, List<Operation> operationList, List<Deliverer> delivererList, Report readReport)
        {
            decimal shouldBe = 0;
            foreach (var item in employeeList)
            {
                shouldBe = shouldBe - item.Bonus - item.DayWage;
            }
            foreach (var item in operationList)
            {
                shouldBe = shouldBe + item.Amount;
            }
            foreach (var item in delivererList)
            {
                shouldBe = shouldBe + item.AmountToCash;
            }
            shouldBe = shouldBe + readReport.Start + readReport.ReportAmount - readReport.Terminal;
            return shouldBe;
        }
        public void BlockButtons()
        {
            DateTime currentDate = DateTime.Now;
            DateTime nextDay = currentDate.AddDays(1);
            DateTime prevDay = currentDate.AddDays(-1);
            if (readReport.Date != prevDay || readReport.Date != nextDay || readReport.Date != currentDate)
            {
                if(loggedUser == null)
                {
                    addEmployeeButton.IsVisible = editEmployeeButton.IsVisible = deleteEmployeeButton.IsVisible =
                    addOperationButton.IsVisible = editOperationButton.IsVisible = deleteOperationButton.IsVisible =
                    editReportButton.IsVisible = sendReportButton.IsVisible = false;
                }
                else
                {
                    if (loggedUser.Permission.Equals("Admin"))
                    {
                        addEmployeeButton.IsVisible = editEmployeeButton.IsVisible = deleteEmployeeButton.IsVisible =
                        addOperationButton.IsVisible = editOperationButton.IsVisible = deleteOperationButton.IsVisible =
                        editReportButton.IsVisible = sendReportButton.IsVisible = true;
                    }
                    else
                    {
                        addEmployeeButton.IsVisible = editEmployeeButton.IsVisible = deleteEmployeeButton.IsVisible =
                        addOperationButton.IsVisible = editOperationButton.IsVisible = deleteOperationButton.IsVisible =
                       editReportButton.IsVisible = sendReportButton.IsVisible = false;
                    }
                }
                
            } 
        }

        protected override bool OnBackButtonPressed()
        {
            if(loggedUser != null)
            {
                App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            return true;
        }
        private void BackClicked(object sender, EventArgs e)
        {
            if (loggedUser != null)
            {
                App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
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
            Operation x = (Operation)operationDataGrid.SelectedItem;
            listOperation.Remove(x);
            await x.DeleteOperations(x);
            App.Current.MainPage = new NavigationPage(new NewReport(readReport, listEmployee, listOperation, employeetable))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        private async void Delete_Employee_Clicked(object sender, EventArgs e)
        {
            Employee x = (Employee)employeesDataGrid.SelectedItem;
            await x.DeleteEmployee(x);
            listEmployee.Remove(x);
            App.Current.MainPage = new NavigationPage(new NewReport(readReport, listEmployee, listOperation, employeetable))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
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
        private async void SendReportClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            if (readReport.Closed == false)
            {
                readReport.Closed = true;
                var result = await readReport.UpdateReport();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("viaduct.email.sender@gmail.com");
                mail.IsBodyHtml = true;
                mail.To.Add("r.switucha@gmail.com");
                mail.Subject = "Raport z dnia " + readReport.Date.Day + "." + readReport.Date.Month;
                mail.Body =
                    "<h3>Rozliczenie</h3>" +
                    "Start: " + readReport.Start.ToString() + "<br>" +
                    "Raport: " + readReport.ReportAmount.ToString() + "<br>" +
                    "Terminal: " + readReport.Terminal.ToString() + "<br>" +
                    "Powinno być: " + readReport.ShouldBe.ToString() + "<br>" +
                    "Jest: " + readReport.AmountIn.ToString() + "<br>" +
                    "Różnica: " + readReport.Difference.ToString() + "<br>" +
                    "Ilość pizz: " + readReport.Pizzas.ToString() + "<br><hr>" +
                    "<h3>Dostawy</h3>";
                foreach (var item in bindingDeliverer.Deliverers)
                {
                    mail.Body += "<h4>" + item.Nickname + "</h4>" +
                    "Kwota z kursów: " + item.Courses.ToString() + "<br>" +
                    "Liczba dostaw: " + item.DeliveriesNumber.ToString() + "<br>" +
                    "Viaduct Karta: " + item.V_k.ToString() + "<br>" +
                    "Viaduct Gotówka: " + item.V_g.ToString() + "<br>" +
                    "Pyszne online: " + item.P_o.ToString() + "<br>" +
                    "Pyszne gotówka: " + item.P_g.ToString() + "<br>" +
                    "Uber online: " + item.Uber_o.ToString() + "<br>" +
                    "Uber gotówka: " + item.Uber_g.ToString() + "<br>" +
                    "Strona online: " + item.S_o.ToString() + "<br>" +
                    "Strona gotówka: " + item.S_g.ToString() + "<br>" +
                    "Glovo online: " + item.G_o.ToString() + "<br>" +
                    "Glovo gotówka: " + item.G_g.ToString() + "<br>" +
                    "KiK: " + item.Kik.ToString() + "<br>" +
                    "Kwota do kasy: " + item.AmountToCash.ToString() + "<br><hr>";
                }
                mail.Body += "<h3>Pracownicy</h3>";
                foreach (var item in bindingEmployee.Employees)
                {
                    mail.Body += "<h4>" + item.Nickname + "</h4>" +
                    "Stanowisko: " + item.Position.ToString() + "<br>" +
                    "Praca od: " + item.WorkFrom.ToString() + "<br>" +
                    "Praca do: " + item.WorkTo.ToString() + "<br>" +
                    "Dniówka: " + item.DayWage.ToString() + "<br>" +
                    "Premia: " + item.Bonus.ToString() + "<br><hr>";
                }
                mail.Body += "<h3>Operacje</h3>";
                foreach (var item in bindingOperation.Operations)
                {
                    mail.Body +=
                    "Nazwa: " + item.Name.ToString() + "<br>" +
                    "Autoryzujący: " + item.Authorizing.ToString() + "<br>";
                    if(item.DocumentNumber != null)
                    {
                        mail.Body += "Nr dokumentu: " + item.DocumentNumber.ToString() + "<br>";
                    }
                    mail.Body += "Kwota: " + item.Amount.ToString() + "<br><hr>";
                }
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("viaduct.email.sender@gmail.com", "Viadukcik4221");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                ClosedDay();
            }
            else
            {
                readReport.Closed = false;
                var result = await readReport.UpdateReport();
                sendReportButton.Text = "Zakończ dzień";
                sendReportButton.BackgroundColor = Color.DarkRed;
                addEmployeeButton.IsVisible = editEmployeeButton.IsVisible = deleteEmployeeButton.IsVisible =
                addOperationButton.IsVisible = editOperationButton.IsVisible = deleteOperationButton.IsVisible =
                editReportButton.IsVisible = true;
            }
            UserDialogs.Instance.HideLoading();
        }
    }
}