using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ViaductMobile.Models;
using ViaductMobile.Algorithms;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReportEmployee : PopupPage
    {
        Employee employee;
        Report readReport;
        bool edit;
        User selectedUser = new User();
        List<Employee> employeeList = new List<Employee>();
        List<Operation> operationList = new List<Operation>();
        decimal cash, partOfCash;
        int rate;
        string nickname, position;
        Xamarin.Forms.DataGrid.DataGrid employeesDataGrid, operationDataGrid;
        public AddReportEmployee(List<Employee> employeeListt, List<Operation> operationListt, Report readReport)
        {
            InitializeComponent();
            nicknamePicker.ItemsSource = Methods.userList;
            positionPicker.ItemsSource = Methods.positionList;
            this.readReport = readReport;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.employeeList = employeeListt;
            this.operationList = operationListt;
            edit = false;
        }
        public AddReportEmployee(Employee employee, Xamarin.Forms.DataGrid.DataGrid employeesDataGrid, Report readReport)
        {
            InitializeComponent();
            nicknamePicker.ItemsSource = Methods.userList;
            positionPicker.ItemsSource = Methods.positionList;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.readReport = readReport;
            this.employeesDataGrid = employeesDataGrid;
            this.employee = employee;
            nicknamePicker.SelectedItem = employee.Nickname;
            positionPicker.SelectedItem = employee.Position;
            workFromTimePicker.Time = employee.WorkFrom.TimeOfDay;
            workToTimePicker.Time = employee.WorkTo.TimeOfDay;
            bonusEntry.Text = employee.Bonus.ToString();
            edit = true;
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            BindingContext = new ViewModels.EmployeeTableVM(readReport);
            await PopupNavigation.PopAsync(true);
        }
        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            List<User> selectedUserList = await selectedUser.FindSinleUser(nicknamePicker.SelectedItem.ToString());
            selectedUser = selectedUserList.SingleOrDefault();
            TimeSpan godzina24 = new TimeSpan(24, 00, 00);
            double start_liczba = workFromTimePicker.Time.TotalHours;
            double koniec_liczba = workToTimePicker.Time.TotalHours;
            double roznica, minusRozpoczecie;

            if (koniec_liczba >= 0 && koniec_liczba < 8)
            {
                minusRozpoczecie = (godzina24 - workFromTimePicker.Time).TotalHours;
                roznica = minusRozpoczecie + koniec_liczba;
            }
            else
            {
                roznica = (workToTimePicker.Time - workFromTimePicker.Time).TotalHours;
            }
            decimal roznica2 = (decimal)(roznica);
            if (positionPicker.SelectedItem.Equals("Bar"))
            {
                rate = selectedUser.BarRate;
                cash = rate * roznica2;
            }
            else if (positionPicker.SelectedItem.Equals("Kuchnia"))
            {
                rate = selectedUser.KitchenRate;
                cash = rate * roznica2;
            }
            else if (positionPicker.SelectedItem.Equals("Kierownictwo"))
            {
                rate = 0;
                cash = 0;
            }
            PickUpDailyWage();
        }
        [Obsolete]
        async void PickUpDailyWage()
        {
            bool answer = await DisplayAlert("Pytanie", "Czy odebrało Ci się całą dniówkę " + cash +"zł?", "Tak", "Nie");
            if(answer == false)
            {
                string result = await DisplayPromptAsync("Nieodebrana dniówka", "Wpisz część dniówki jaką udało Ci się odebrać (sama liczba, 0 = nic)");
                partOfCash = decimal.Parse(result);
            }
            if (answer == true && edit == false)
            {
                Employee newEmployee = new Employee()
                {
                    Nickname = nicknamePicker.SelectedItem.ToString(),
                    Rate = rate.ToString(),
                    Position = positionPicker.SelectedItem.ToString(),
                    WorkFrom = Convert.ToDateTime(workFromTimePicker.Time.ToString()),
                    WorkTo = Convert.ToDateTime(workToTimePicker.Time.ToString()),
                    DayWage = cash,
                    Bonus = decimal.Parse(bonusEntry.Text),
                    ReportId = readReport.Id,
                    Date = readReport.Date
                };
                bool result = await newEmployee.SaveEmployee();
                employeeList.Add(newEmployee);
            }
            else if (answer == true && edit == true)
            {
                employee.Nickname = nicknamePicker.SelectedItem.ToString();
                employee.Rate = rate.ToString();
                employee.Position = positionPicker.SelectedItem.ToString();
                employee.WorkFrom = Convert.ToDateTime(workFromTimePicker.Time.ToString());
                employee.WorkTo = Convert.ToDateTime(workToTimePicker.Time.ToString());
                employee.DayWage = cash;
                employee.Bonus = decimal.Parse(bonusEntry.Text);
                employee.ReportId = readReport.Id;
                employee.Date = readReport.Date;
                bool result = await employee.UpdateEmployee(employee);
            }
            else if (answer == false && edit == false)
            {
                Employee newEmployee = new Employee()
                {
                    Nickname = nicknamePicker.SelectedItem.ToString(),
                    Rate = rate.ToString(),
                    Position = positionPicker.SelectedItem.ToString(),
                    WorkFrom = Convert.ToDateTime(workFromTimePicker.Time.ToString()),
                    WorkTo = Convert.ToDateTime(workToTimePicker.Time.ToString()),
                    DayWage = partOfCash,
                    Bonus = decimal.Parse(bonusEntry.Text),
                    ReportId = readReport.Id,
                    Date = readReport.Date
                };
                OverdueCash overdue = new OverdueCash()
                {
                    UserId = selectedUser.Id,
                    Amount = cash - partOfCash,
                    Date = readReport.Date,
                    Reason = "Zaległa dniówka"
                };
                bool result = await newEmployee.SaveEmployee();
                bool result2 = await overdue.SaveOverdueCash();
                employeeList.Add(newEmployee);
            }
            else if (answer == false && edit == true)
            {
                await DisplayAlert("Uwaga", "Wybrałeś edycję dniówki, która w teorii została odebrana i zmieniłeś ją na nieodebraną, lepiej ją usuń niż tak zmieniać bo robisz zamieszanie typie", "OK");
            }
            await DisplayAlert("", "Pomyślnie dodano pracownika", "OK");
            Methods.reportEmployeeList = employeeList;
            await PopupNavigation.PopAsync(true);
            App.Current.MainPage = new NavigationPage(new NewReport(readReport, employeeList, operationList))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
    }
}