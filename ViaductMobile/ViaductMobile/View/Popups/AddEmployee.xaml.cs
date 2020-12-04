using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEmployee : PopupPage
    {
        User clickedRow;
        bool edit, add;
        string notification;
        int bar, kitchen, deliverer;
        Xamarin.Forms.DataGrid.DataGrid employeesDataGrid;
        public AddEmployee(Xamarin.Forms.DataGrid.DataGrid employeesDataGrid)
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            passwordEntry.Text = Methods.RandomString(8);
            permissionPicker.ItemsSource = Methods.permissionList;
            Button resetPasswordButton = new Button();
            this.employeesDataGrid = employeesDataGrid;
            edit = false;
        }
        public AddEmployee(User clickedRow, Xamarin.Forms.DataGrid.DataGrid employeesDataGrid)
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            this.clickedRow = clickedRow;
            this.employeesDataGrid = employeesDataGrid;
            nicknameEntry.Text = clickedRow.Nickname;
            barRateEntry.Text = clickedRow.BarRate.ToString();
            kitchenRateEntry.Text = clickedRow.KitchenRate.ToString();
            deliverRateEntry.Text = clickedRow.DeliverRate.ToString();
            passwordEntry.Text = "********";
            passwordEntry.IsReadOnly = true;
            permissionPicker.ItemsSource = Methods.permissionList;
            permissionPicker.SelectedItem = clickedRow.Permission;
            edit = true;
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            BindingContext = new ViewModels.EmployeePanelVM();
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            if (nicknameEntry.Text == null)
            {
                add = false;
                notification += "pseudonim ";
            }
            if (passwordEntry.Text == null)
            {
                add = false;
                notification += "hasło ";
            }
            else
            {
                if (passwordEntry.Text.Length < 6)
                {
                    add = false;
                    notification += "hasło ";
                }
            }
            if (permissionPicker.SelectedItem == null)
            {
                add = false;
                notification += "uprawnienia ";
            }
            try { bar = int.Parse(barRateEntry.Text); }
            catch
            {
                if (barRateEntry.Text == null || barRateEntry.Text == "")
                    bar = 0;
                else { add = false; notification += "stawka bar "; }
            }
            try { bar = int.Parse(barRateEntry.Text); }
            catch
            {
                if (kitchenRateEntry.Text == null || kitchenRateEntry.Text == "")
                    kitchen = 0;
                else { add = false; notification += "stawka kuchnia "; }
            }
            try { deliverer = int.Parse(deliverRateEntry.Text); }
            catch
            {
                if (deliverRateEntry.Text == null || deliverRateEntry.Text == "")
                    deliverer = 0;
                else { add = false; notification += "stawka dostawy "; }
            }
            if (edit == false)
            {
                var hash = SecurePasswordHasher.Hash(passwordEntry.Text);
                User newUser = new User()
                {
                    Nickname = nicknameEntry.Text,
                    Password = hash,
                    Permission = permissionPicker.SelectedItem.ToString(),
                    BarRate = bar,
                    KitchenRate = kitchen,
                    DeliverRate = deliverer
                };
                bool result = await newUser.SaveUser();
            }
            else
            {
                clickedRow.Nickname = nicknameEntry.Text;
                clickedRow.Permission = permissionPicker.SelectedItem.ToString();
                clickedRow.BarRate = bar;
                clickedRow.KitchenRate = kitchen;
                clickedRow.DeliverRate = deliverer;
                bool result = await clickedRow.UpdateUser(clickedRow);
            }
            employeesDataGrid.ItemsSource = new ViewModels.EmployeePanelVM().Users;
        }
    }
}