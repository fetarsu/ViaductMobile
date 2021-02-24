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
    public partial class AdressesPanel : ContentPage
    {
        User loggedUser;
        public AdressesPanel(User loggedUser)
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.AdressesPanelVM();
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
            await PopupNavigation.PushAsync(new AddAdresses(adressesDataGrid));
        }
        [Obsolete]
        private async void EditClicked(object sender, EventArgs e)
        {
            Adress clickedRow = (Adress)adressesDataGrid.SelectedItem;
            if (clickedRow != null)
            {
                await PopupNavigation.PushAsync(new AddAdresses(clickedRow, adressesDataGrid));
            }
        }

        async void DeleteClicked(object sender, EventArgs e)
        {
            Adress x = (Adress)adressesDataGrid.SelectedItem;
            await x.DeleteAdress(x);
            adressesDataGrid.ItemsSource = new ViewModels.AdressesPanelVM().Adresses;
        }
    }
}