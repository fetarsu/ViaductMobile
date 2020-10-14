using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DelivererCart : ContentPage
    {
        User loggedUser;
        Report report;
        public DelivererCart(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            BindingContext = new ViewModels.DelivererCartVM();
            chooseDayPicker.Date = DateTime.Now;
        }

    private void chooseDay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("Xd");
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
        private async void AddSupply(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AddSupply(delivererCartDataGrid));
        }
        [Obsolete]
        private async void EditSupply(object sender, EventArgs e)
        {
            Supply clickedRow = (Supply)delivererCartDataGrid.SelectedItem;
            if (clickedRow != null)
            {
                await PopupNavigation.PushAsync(new AddSupply(clickedRow, delivererCartDataGrid));
            }
        }

        async void DeleteSupply(object sender, EventArgs e)
        {
            Supply x = (Supply)delivererCartDataGrid.SelectedItem;
            await x.DeleteSupply(x);
            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM().Supplies;
        }
    }
}