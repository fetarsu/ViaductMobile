using Acr.UserDialogs;
using System;
using System.Linq;
using ViaductMobile.Algorithms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        User user = new User();
        public LoginPage()
        {
            InitializeComponent();
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            if(!(loginEntry.Text == null || loginEntry.Text.Length == 0) && !(passwordEntry.Text == null || passwordEntry.Text.Length == 0))
            {
                var userList = await user.ReadUser(loginEntry.Text);
                user = userList.FirstOrDefault();
            }
            if(user.Nickname != null)
            {
                bool result = SecurePasswordHasher.Verify(passwordEntry.Text, user.Password);
                if (result)
                {
                    UserDialogs.Instance.HideLoading();
                    App.Current.MainPage = new NavigationPage(new MainPage(user))
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
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }
    }
}