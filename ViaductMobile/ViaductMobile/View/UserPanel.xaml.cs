﻿using Acr.UserDialogs;
using Microcharts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using SkiaSharp;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPanel : ContentPage
    {
        User loggedUser;
        decimal firstMonthSalary, secondMonthSalary;
        Employee emp = new Employee();
        OverdueCash selectedRow;
        DateTime currentDate;
        public UserPanel(User loggedUser, decimal firstMonthSalary, decimal secondMonthSalary)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            this.firstMonthSalary = firstMonthSalary;
            this.secondMonthSalary = secondMonthSalary;
            welcomeLabel.Text = "Witaj " + loggedUser.Nickname + "!";
            permissionLabel.Text = "Uprawnienia: " + loggedUser.Permission;
            barRateLabel.Text = "Stawka bar: " + loggedUser.BarRate;
            kitchenRateLabel.Text = "Stawka kuchnia: " + loggedUser.KitchenRate;
            delivererRateLabel.Text = "Stawka dostawy: " + loggedUser.DeliverRate;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            BindingContext = new ViewModels.OverdueEmployeeVM(loggedUser);
            ChartEntry[] entries = new[]
            {
                new ChartEntry((float)firstMonthSalary)
                {
                    Label = "Poprzedni",
                    ValueLabel = firstMonthSalary.ToString(),
                    ValueLabelColor = SKColor.Parse("#FFFFFF"),
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry((float)secondMonthSalary)
                {
                    Label = "Ten miesiac",
                    ValueLabel = secondMonthSalary.ToString(),
                    ValueLabelColor = SKColor.Parse("#FFFFFF"),
                    Color = SKColor.Parse("#77d065")
                },
            };
            chartView.Chart = new BarChart() { Entries = entries, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 40, LabelColor = SKColor.Parse("#FFFFFF"), BackgroundColor = SKColor.Parse("#272727"), Margin = 40 };
        }
        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }
        [Obsolete]
        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new ChangePassword(loggedUser));
        }
        [Obsolete]

        public async void GetDailyWage_Clicked(object sender, EventArgs e)
        {
            selectedRow = (OverdueCash)overdueDataGrid.SelectedItem;
            currentDate = DateTime.Now;
            if (currentDate.Hour < 7)
            {
                currentDate = currentDate.AddDays(-1);
            }
            Report report = new Report();
            var reportList = await Report.ReadTodayReport(currentDate);
            if (reportList.Closed == false)
            {
                Operation operation = new Operation()
                {
                    Name = selectedRow.Reason,
                    Authorizing = loggedUser.Nickname,
                    DocumentNumber = null,
                    Amount = selectedRow.Amount,
                    Date = currentDate,
                    Type = "brak",
                    ReportId = reportList.Id
                };
                bool result = await operation.SaveOperations();
                bool result2 = await selectedRow.DeleteOverdueCash(selectedRow);
                Logs newLog = new Logs()
                {
                    UserId = loggedUser.Nickname,
                    DeletedTable = "OverdueCash",
                    Date = DateTime.Now
                };
                bool r = await newLog.SaveLogs();
            }
            else
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, odbierz dniówkę następnego dnia", "OK");
            overdueDataGrid.ItemsSource = new ViewModels.OverdueEmployeeVM(loggedUser).Overdues;

        }

    }
}