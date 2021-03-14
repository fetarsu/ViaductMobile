using Acr.UserDialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Globals;
using ViaductMobile.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, IBackToPreviousWindow
    {
        User user = new User();
        public LoginPage()
        {
            InitializeComponent();
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            var result = await TryToLogin(loginEntry.Text, passwordEntry.Text);
            if (result)
            {
                App.Current.MainPage = new NavigationPage(new MainPage(user))
                {
                    BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                    BarTextColor = Color.White
                };
            }
            UserDialogs.Instance.HideLoading();
        }
        public async Task<bool> TryToLogin(string login, string password)
        {   
            if (!(login == null || login.Length == 0) && !(password == null || password.Length == 0))
            {
                var userList = await user.ReadUser(login);
                user = userList.FirstOrDefault();
            }
            if (user.Nickname != null)
            {
                bool result = SecurePasswordHasher.Verify(password, user.Password);
                if (result)
                {         
                    return true;
                }
                else
                {
                    await DisplayAlert(Texts.errorDisplayAlertHeader, Texts.wrongLoginDetailsDisplayAlertMessage, Texts.okDisplayAlertMessage);
                    return false;
                }
            }
            else
            {
                await DisplayAlert(Texts.errorDisplayAlertHeader, Texts.wrongLoginDetailsDisplayAlertMessage, Texts.okDisplayAlertMessage);
                return false;
            }
        }
        public void BackViaSystemButton()
        {
            OnBackButtonPressed();
        }
        public void BackViaAppButton(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
            return true;
        }
    }
}