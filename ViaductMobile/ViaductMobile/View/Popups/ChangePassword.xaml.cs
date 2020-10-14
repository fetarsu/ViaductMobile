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
    public partial class ChangePassword : PopupPage
    {
        User loggedUser;
        public ChangePassword(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Change_Clicked(object sender, EventArgs e)
        {
            var verifyPassword = SecurePasswordHasher.Verify(oldPasswordEntry.Text, loggedUser.Password);
            if (newPasswordEntry.Text.Equals(new2PasswordEntry.Text) && verifyPassword == true && newPasswordEntry.Text.Length > 5)
            {
                var hash = SecurePasswordHasher.Hash(newPasswordEntry.Text);
                loggedUser.Password = hash;
                bool result = await loggedUser.UpdateUser(loggedUser);
                await DisplayAlert("Udało się", "Hasło zostało poprawnie zmienione", "OK");
                await PopupNavigation.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Bład", "Stare hasło jest niepoprawne lub nowe haslo nie jest identyczne lub haslo ma mniej niz 6 znakow", "OK");
            }
        }
    }
}