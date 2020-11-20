﻿using Rg.Plugins.Popup.Pages;
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
        string type;
        Operation operation;
        Report readReport;
        List<Employee> employeeList = new List<Employee>();
        List<Operation> operationList = new List<Operation>();
        bool edit, employeetable = false;
        public AddReportOperation(List<Employee> employeeListt, List<Operation> operationListt, Report readReport)
        {
            InitializeComponent();
            nicknamePicker.ItemsSource = Methods.userList;
            this.readReport = readReport;
            this.employeeList = employeeListt;
            this.operationList = operationListt;
            edit = false;
        }
        public AddReportOperation(List<Employee> employeeListt, List<Operation> operationListt, Operation operation, Report readReport)
        {          
            InitializeComponent();
            nicknamePicker.ItemsSource = Methods.userList;
            this.operation = operation;
            this.readReport = readReport;
            operationNameEntry.Text = operation.Name;
            nicknamePicker.SelectedItem = operation.Authorizing;
            numberEntry.Text = operation.DocumentNumber;
            amountEntry.Text = operation.Amount.ToString();
            this.employeeList = employeeListt;
            this.operationList = operationListt;
            if (operation.Type.Equals("Faktura"))
                yesInvoiceCheckBox.IsChecked = true;
            else
                noInvoiceCheckBox.IsChecked = true;
            edit = true;
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            if (yesInvoiceCheckBox.IsChecked == true && noInvoiceCheckBox.IsChecked == false)
                type = "faktura";
            else if (yesInvoiceCheckBox.IsChecked == false && noInvoiceCheckBox.IsChecked == true)
                type = "brak";
            else
                type = null;
            if (edit == false && type != null)
            {
                Operation newOperation = new Operation()
                {
                    Name = operationNameEntry.Text,
                    Authorizing = nicknamePicker.SelectedItem.ToString(),
                    DocumentNumber = numberEntry.Text,
                    Amount = decimal.Parse(amountEntry.Text),
                    Date = readReport.Date,
                    Type = type,
                    ReportId = readReport.Id
                };
                bool result = await newOperation.SaveOperations();
                await PopupNavigation.PopAsync(true);
                operationList.Add(newOperation);
                Methods.reportOperationList = operationList;
                App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList, employeetable))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else if(type != null)
            {
                operationList.Remove(operation);
                operation.Name = operationNameEntry.Text;
                operation.Authorizing = nicknamePicker.SelectedItem.ToString();
                operation.DocumentNumber = operation.DocumentNumber;
                operation.Amount = decimal.Parse(amountEntry.Text);
                operation.Date = readReport.Date;
                operation.Type = type;
                operation.ReportId = readReport.Id;
                bool result = await operation.UpdateOpetarions(operation);
                operationList.Add(operation);
                await PopupNavigation.PopAsync(true);
                Methods.reportOperationList = operationList;
                App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList, employeetable))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                await DisplayAlert("Błąd", "Zaznaczyłeś jednocześnie że operacja ma i nie ma faktury, popraw to", "OK");
            }
        }
    }
}