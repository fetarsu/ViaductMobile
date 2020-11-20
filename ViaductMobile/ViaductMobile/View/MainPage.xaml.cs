using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using ViaductMobile.View;
using Xamarin.Forms;
using static ViaductMobile.Models._Enums;

namespace ViaductMobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        User loggedUser;
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
            Methods.ReadAllUsers();
        }

        public MainPage(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            //ToolbarItem welcomeItem = new ToolbarItem() { Text = "Witaj " + user.Nickname + "!" };
            //this.ToolbarItems.Add(welcomeItem);
            string userPermission = loggedUser.Permission;
            if ((enPermission)Enum.Parse(typeof(enPermission), userPermission) == enPermission.Admin)
            {
                ToolbarItem deliveryCart = new ToolbarItem() { Text = "Karta dostaw", IconImageSource = "delivery.png"};
                ToolbarItem employeesPanel = new ToolbarItem() { Text = "Pracownicy", IconImageSource = "employees.png" };
                ToolbarItem adressesPanel = new ToolbarItem() { Text = "Adresy", IconImageSource = "house.png" };
                ToolbarItem pizzasPanel = new ToolbarItem() { Text = "Produkty", IconImageSource = "pizza.png" };
                this.ToolbarItems.Add(pizzasPanel);
                this.ToolbarItems.Add(adressesPanel);
                this.ToolbarItems.Add(deliveryCart);
                this.ToolbarItems.Add(employeesPanel);
                employeesPanel.Clicked += MoveToEmployeePanelClicked;
                deliveryCart.Clicked += MoveToDelivererCartClicked;
                adressesPanel.Clicked += MoveToAdressesPanelClicked;
                pizzasPanel.Clicked += MoveToPizzasPanelClicked;
            }
            ToolbarItem userPanel = new ToolbarItem() { Text = "Panel użytkownika", IconImageSource="user.png" };
            userPanel.Clicked += MoveToUserPanelClicked;
            this.ToolbarItems.Add(userPanel);

        }
        private void MoveToAdressesPanelClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AdressesPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        private void MoveToPizzasPanelClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new PizzasPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        private void MoveToDelivererCartClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        private void MoveToUserPanelClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new UserPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
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
        async void MoveToEmployeePanelClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new EmployeePanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
    }
}