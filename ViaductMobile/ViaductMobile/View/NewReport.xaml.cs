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
using ViaductMobile.Globals;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewReport : ContentPage
    {
        Report readReport;
        User loggedUser;
        bool employeetable;
        decimal shouldBe;
        string nickname;
        List<Employee> listEmployee = new List<Employee>();
        List<Operation> listOperation = new List<Operation>();
        ViewModels.ReportTableVM bindingReport;
        ViewModels.EmployeeTableVM bindingEmployee;
        ViewModels.OperationTableVM bindingOperation;
        ViewModels.DelivererTableVM bindingDeliverer;
        Xamarin.Forms.DataGrid.DataGrid employeeDataGridd, operationDataGridd;
        public NewReport(Report readReport)
        {
            nickname = "viaduct";
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
            nickname = loggedUser.Nickname;
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
        public NewReport(Report readReport, List<Employee> listEmployeee, List<Operation> listOperationn, bool employeetable, User loggedUser)
        {
            this.loggedUser = loggedUser;
            nickname = loggedUser.Nickname;
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
                decimal x = item.AmountToShouldBe -item.Courses;
                shouldBe = shouldBe + x;
            }
            decimal q = readReport.Start;
            decimal w = readReport.ReportAmount;
            decimal e = readReport.Terminal;
            shouldBe = shouldBe + q + w - e;
            return shouldBe;
        }
        public void BlockButtons()
        {
            DateTime currentDate = DateTime.Now;
            DateTime nextDay = currentDate.AddDays(1);
            DateTime prevDay = currentDate.AddDays(-1);
            if ((readReport.Date.Day != prevDay.Day && readReport.Date.Month != prevDay.Month && readReport.Date.Year != prevDay.Year) ||
                (readReport.Date.Day != nextDay.Day && readReport.Date.Month != nextDay.Month && readReport.Date.Year != nextDay.Year) ||
                (readReport.Date.Day != currentDate.Day && readReport.Date.Month != currentDate.Month && readReport.Date.Year != currentDate.Year))
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
            await PopupNavigation.PushAsync(new AddReportOperation(listEmployee, listOperation, readReport, loggedUser));
        }
        [Obsolete]
        private async void Edit_Operation_Clicked(object sender, EventArgs e)
        {
            Operation selectedRow = (Operation)operationDataGrid.SelectedItem;
            if (selectedRow == null)
                await DisplayAlert("Uwaga", "Żaden wiersz nie jest zaznaczony", "OK");
            else
                await PopupNavigation.PushAsync(new AddReportOperation(listEmployee, listOperation, selectedRow, readReport, loggedUser));
        }
        private async void Delete_Operation_Clicked(object sender, EventArgs e)
        {
            Operation x = (Operation)operationDataGrid.SelectedItem;
            if(x == null)
                await DisplayAlert("Uwaga", "Żaden wiersz nie jest zaznaczony", "OK");
            else
            {
                listOperation.Remove(x);
                await x.DeleteOperations(x);
                Logs newLog = new Logs()
                {
                    UserId = nickname,
                    DeletedTable = "Operation",
                    Date = DateTime.Now
                };
                bool r = await newLog.SaveLogs();
                App.Current.MainPage = new NavigationPage(new NewReport(readReport, listEmployee, listOperation, employeetable, loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }

        private async void Delete_Employee_Clicked(object sender, EventArgs e)
        {
            Employee x = (Employee)employeesDataGrid.SelectedItem;
            if (x == null)
                await DisplayAlert("Uwaga", "Żaden wiersz nie jest zaznaczony", "OK");
            else
            {
                await x.DeleteEmployee(x);
                listEmployee.Remove(x);
                Logs newLog = new Logs()
                {
                    UserId = nickname,
                    DeletedTable = "Employee",
                    Date = DateTime.Now
                };
                bool r = await newLog.SaveLogs();
                App.Current.MainPage = new NavigationPage(new NewReport(readReport, listEmployee, listOperation, employeetable, loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }   
        }
        [Obsolete]
        private async void Add_Employee_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AddReportEmployee(listEmployee, listOperation, readReport, loggedUser));
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
            if (selectedRow == null)
                await DisplayAlert("Uwaga", "Żaden wiersz nie jest zaznaczony", "OK");
            else
                await PopupNavigation.PushAsync(new AddReportEmployee(listEmployee, listOperation, selectedRow, readReport, loggedUser));
        }
        private async void SendReportClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            if(readReport.Pizzas <= 0 || readReport.EmployeePizzas <= 0)
            {
                await DisplayAlert("Uwaga", "Pizze lub pizze pracownicze nie są wypełnione", "OK");
            }
            else
            {
                if (readReport.Closed == false)
                {
                    readReport.Closed = true;
                    var result = await readReport.UpdateReport();
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("viaduct.email.sender@gmail.com");
                    mail.IsBodyHtml = true;
                    mail.To.Add(Texts.emailToSendReportAddress);
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
                        "Pracownicze pizze: " + readReport.EmployeePizzas.ToString() + "<br><hr>" +
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
                        if (item.DocumentNumber != null)
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
}