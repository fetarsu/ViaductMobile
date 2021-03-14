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

        async void TryToLogin(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
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
                        BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    await DisplayAlert(Texts.errorDisplayAlertHeader, Texts.wrongLoginDetailsDisplayAlertMessage, Texts.okDisplayAlertMessage);
                    UserDialogs.Instance.HideLoading();
                }
            }
            else
            {
                await DisplayAlert(Texts.errorDisplayAlertHeader, Texts.wrongLoginDetailsDisplayAlertMessage, Texts.okDisplayAlertMessage);
                UserDialogs.Instance.HideLoading();
            }
            
        }   

        public async Task BackViaSystemButton()
        {
            try
            {
                OnBackButtonPressed();
            }
            catch (Exception ex)
            {
                await DisplayAlert(Texts.fatalErrorDisplayAlertHeader, "Szczegóły: "+ex.ToString(), Texts.okDisplayAlertMessage);
            }
        }

        public async Task BackViaAppButton()
        {
            try
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                    BarTextColor = Color.White
                };
            }
            catch (Exception ex)
            {
                await DisplayAlert(Texts.fatalErrorDisplayAlertHeader, "Szczegóły: " + ex.ToString(), Texts.okDisplayAlertMessage);
            }
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