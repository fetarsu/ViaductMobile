using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ViaductMobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        User user;
        public MainPage()
        {
            InitializeComponent();
            var ni = NetworkInterface.GetAllNetworkInterfaces()
               .OrderBy(intf => intf.NetworkInterfaceType)
               .FirstOrDefault(intf => intf.OperationalStatus == OperationalStatus.Up
                     && (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                         || intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet));

            var hw = ni.GetPhysicalAddress();
            var x = string.Join(":", (from ma in hw.GetAddressBytes() select ma.ToString("X2")).ToArray());
            ToolbarItem moveToLogin = new ToolbarItem() { Text = "Zaloguj się", IconImageSource = "login.png"};
            moveToLogin.Clicked += MoveToLoginClicked;
            this.ToolbarItems.Add(moveToLogin);
        }
        public MainPage(User user)
        {
            this.user = user;
            InitializeComponent();
            //ToolbarItem welcomeItem = new ToolbarItem() { Text = "Witaj " + user.Nickname + "!" };
            //this.ToolbarItems.Add(welcomeItem);
            if (user.Permission.Title.Equals("Admin") || user.Permission.Title.Equals("Dostawca"))
            {
                ToolbarItem deliveryCart = new ToolbarItem() { Text = "Karta dostaw", IconImageSource = "delivery.png" };
                this.ToolbarItems.Add(deliveryCart);
            }
            ToolbarItem userPanel = new ToolbarItem() { Text = "Panel użytkownika", IconImageSource="user.png" };
            this.ToolbarItems.Add(userPanel);

        }

        private void MoveToLoginClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        private void MoveToChooseDateClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ChooseDate())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
    }
}