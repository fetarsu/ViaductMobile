using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPanel : ContentPage
    {
        User loggedUser;
        public UserPanel(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            welcomeLabel.Text = "Witaj " + loggedUser.Nickname + "!";
            permissionLabel.Text = "Uprawnienia: " + loggedUser.Permission;
            barRateLabel.Text = "Stawka bar: " + loggedUser.BarRate;
            kitchenRateLabel.Text = "Stawka kuchnia: " + loggedUser.KitchenRate;
            delivererRateLabel.Text = "Stawka dostawy: " + loggedUser.DeliverRate;

        }
        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        [Obsolete]
        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new ChangePassword(loggedUser));
        }
    }
}