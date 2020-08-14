using Acr.UserDialogs;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //async void SaveButton_Clicked(object sender, EventArgs e)
        //{
        //    Book book = new Book()
        //    {
        //        Name = loginEntry.Text,
        //        Author = passwordEntry.Text
        //    };

        //    bool result = await book.SaveBook();
        //}
        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            User user = new User();
            var listOfUsers = await user.ReadBooks();
            if (listOfUsers.Where(x => x.Login.Equals(loginEntry.Text) && x.Password.Equals(passwordEntry.Text)).Any())
            {
                user = listOfUsers.Where(x => x.Login.Equals(loginEntry.Text) && x.Password.Equals(passwordEntry.Text)).FirstOrDefault();
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