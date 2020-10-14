using Acr.UserDialogs;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
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

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            User loggedUser = new User();
            var listOfUsers = await loggedUser.ReadUser();
            loggedUser = listOfUsers.Where(x => x.Nickname.Equals(loginEntry.Text)).FirstOrDefault();
            var result = SecurePasswordHasher.Verify(passwordEntry.Text, loggedUser.Password);
            if (loggedUser != null && result == true)
            {
                UserDialogs.Instance.HideLoading();
                App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                await DisplayAlert("Błąd", "Zły login lub hasło", "OK");
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
    }
}