using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PizzasPanel : ContentPage
    {
        User loggedUser;
        public PizzasPanel(User loggedUser)
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.PizzasPanelVM();
            this.loggedUser = loggedUser;
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
        private async void AddClicked(object sender, EventArgs e)
        {
            //await PopupNavigation.PushAsync(new AddEmployee(employeesDataGrid));
        }
        [Obsolete]
        private async void EditClicked(object sender, EventArgs e)
        {
            User clickedRow = (User)employeesDataGrid.SelectedItem;
            if (clickedRow != null)
            {
                //await PopupNavigation.PushAsync(new AddEmployee(clickedRow, employeesDataGrid));
            }
        }

        async void DeleteClicked(object sender, EventArgs e)
        {
            User x = (User)employeesDataGrid.SelectedItem;
            await x.DeleteUser(x);
            employeesDataGrid.ItemsSource = new ViewModels.EmployeePanelVM().Users;
        }
    }
}