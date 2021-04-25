using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReportOperation : PopupPage
    {
        string type, notification, operationName, number;
        decimal amount;
        Operation operation;
        User loggedUser;
        Report readReport;
        List<Employee> employeeList = new List<Employee>();
        List<Operation> operationList = new List<Operation>();
        bool edit, employeetable = false, add;
        public AddReportOperation(List<Employee> employeeListt, List<Operation> operationListt, Report readReport, User loggedUser)
        {
            edit = false;
            this.loggedUser = loggedUser;
            InitializeComponent();
            ReadUsers();
            typePicker.ItemsSource = Methods.operationTypeList;
            this.readReport = readReport;
            this.employeeList = employeeListt;
            this.operationList = operationListt;    
        }
        public AddReportOperation(List<Employee> employeeListt, List<Operation> operationListt, Operation operation, Report readReport, User loggedUser)
        {
            edit = true;
            this.loggedUser = loggedUser;
            InitializeComponent();
            ReadUsers();
            typePicker.ItemsSource = Methods.operationTypeList;
            this.operation = operation;
            this.readReport = readReport;
            operationNameEntry.Text = operation.Name;
            numberEntry.Text = operation.DocumentNumber;
            amountEntry.Text = operation.Amount.ToString();
            this.employeeList = employeeListt;
            this.operationList = operationListt;
            typePicker.SelectedItem = operation.Type;     
        }
        public async void ReadUsers()
        {
            nicknamePicker.ItemsSource = await loggedUser.ReadAllUsers();
            if (edit)
            {
                nicknamePicker.SelectedItem = operation.Authorizing;
            }
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            if (operationNameEntry.Text == "" || operationNameEntry.Text == null)
            {
                add = false;
                notification += "nazwa operacji ";
            }
            if (nicknamePicker.SelectedItem == null)
            {
                add = false;
                notification += "autoryzujący ";
            }
            if (numberEntry.Text == null || numberEntry.Text == "")
                number = "";
            try { amount = decimal.Parse(amountEntry.Text); }
            catch
            {
                if (amountEntry.Text == null || amountEntry.Text == "")
                    amount = 0;
                else { add = false; notification += "kwota "; }
            }
            if (typePicker.SelectedItem == null)
            {
                add = false; notification += "typ ";
            }
            if (add == false)
            {
                await DisplayAlert("Uwaga", "Pole " + notification + " zostało źle wypełnione", "OK");
            }
            else
            {
                if (edit == false && !addAndPayCheckBox.IsChecked)
                {

                    Operation newOperation = new Operation()
                    {
                        Name = operationNameEntry.Text,
                        Authorizing = nicknamePicker.SelectedItem.ToString(),
                        DocumentNumber = number,
                        Amount = amount,
                        Date = readReport.Date,
                        Type = typePicker.SelectedItem.ToString(),
                        ReportId = readReport.Id
                    };
                    bool result = await newOperation.SaveOperations();
                    await PopupNavigation.PopAsync(true);
                    operationList.Add(newOperation);
                    Methods.reportOperationList = operationList;
                    App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList, employeetable, loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
                else if (edit == false && addAndPayCheckBox.IsChecked && amount < 0)
                {
                    Operation newOperationFirst = new Operation()
                    {
                        Name = "Zasilenie",
                        Authorizing = nicknamePicker.SelectedItem.ToString(),
                        DocumentNumber = number,
                        Amount = Math.Abs(amount),
                        Date = readReport.Date,
                        Type = "Brak faktury",
                        ReportId = readReport.Id
                    };
                    Operation newOperationSecond = new Operation()
                    {
                        Name = operationNameEntry.Text,
                        Authorizing = nicknamePicker.SelectedItem.ToString(),
                        DocumentNumber = number,
                        Amount = amount,
                        Date = readReport.Date,
                        Type = typePicker.SelectedItem.ToString(),
                        ReportId = readReport.Id
                    };
                    bool result = await newOperationFirst.SaveOperations();
                    bool result2 = await newOperationSecond.SaveOperations();
                    await PopupNavigation.PopAsync(true);
                    operationList.Add(newOperationFirst);
                    operationList.Add(newOperationSecond);
                    Methods.reportOperationList = operationList;
                    App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList, employeetable, loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
                else if (edit == true && !addAndPayCheckBox.IsChecked)
                {
                    operationList.Remove(operation);
                    operation.Name = operationNameEntry.Text;
                    operation.Authorizing = nicknamePicker.SelectedItem.ToString();
                    operation.DocumentNumber = number;
                    operation.Amount = amount;
                    operation.Date = readReport.Date;
                    operation.Type = typePicker.SelectedItem.ToString();
                    operation.ReportId = readReport.Id;
                    bool result = await operation.UpdateOpetarions(operation);
                    operationList.Add(operation);
                    await PopupNavigation.PopAsync(true);
                    Methods.reportOperationList = operationList;
                    App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList, employeetable, loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    await DisplayAlert("Uwaga", "aby użyć opcji wpłać i wypłać operacja musi być nowa (nie można edytować) i kwota musi być na minusie", "OK");
                }
            }
        }
    }
}