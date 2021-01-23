using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
            ToolbarItem moveToLogin = new ToolbarItem() { Text = "Zaloguj się", IconImageSource = "login.png"};
            moveToLogin.Clicked += MoveToLoginClicked;
            this.ToolbarItems.Add(moveToLogin);
            var mac =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
            //if (mac.Equals("A89CEDC7483E")) //"D0B128D587E9"
            //{
            //    createReportButton.IsVisible = true;
            //}
            createReportButton.IsVisible = true;
        }

        public MainPage(User loggedUser)
        {
            InitializeComponent();
            createReportButton.IsVisible = true;
            this.loggedUser = loggedUser;
            string userPermission = loggedUser.Permission;
            if (userPermission.Equals("Admin"))
            {
                ToolbarItem employeesPanel = new ToolbarItem() { Text = "Pracownicy", IconImageSource = ImageSource.FromFile("employees.png"), Order = ToolbarItemOrder.Primary, Priority = 0 };
                employeesPanel.Clicked += MoveToEmployeePanelClicked;
                this.ToolbarItems.Add(employeesPanel);
            }
            if(userPermission.Equals("Admin") || loggedUser.DeliverRate > 0)
            {
                ToolbarItem deliveryCart = new ToolbarItem() { Text = "Karta dostaw", IconImageSource = ImageSource.FromFile("delivery.png"), Order = ToolbarItemOrder.Primary, Priority = 0};
                ToolbarItem adressesPanel = new ToolbarItem() { Text = "Adresy", IconImageSource = ImageSource.FromFile("house.png"), Order = ToolbarItemOrder.Primary, Priority = 1 };
                ToolbarItem pizzasPanel = new ToolbarItem() { Text = "Produkty", IconImageSource = ImageSource.FromFile("pizza.png"), Order = ToolbarItemOrder.Primary, Priority = 2 };
                this.ToolbarItems.Add(pizzasPanel);
                this.ToolbarItems.Add(adressesPanel);
                this.ToolbarItems.Add(deliveryCart);
                deliveryCart.Clicked += MoveToDelivererCartClicked;
                adressesPanel.Clicked += MoveToAdressesPanelClicked;
                pizzasPanel.Clicked += MoveToPizzasPanelClicked;
            }
            ToolbarItem userPanel = new ToolbarItem() { Text = "Panel użytkownika", IconImageSource = ImageSource.FromFile("user.png"), Order = ToolbarItemOrder.Primary, Priority = 0 };
            userPanel.Clicked += MoveToUserPanelClicked;
            this.ToolbarItems.Add(userPanel);
            ToolbarItem logout = new ToolbarItem() { Text = "Wyloguj", IconImageSource = ImageSource.FromFile("logout.png"), Order = ToolbarItemOrder.Primary, Priority = 0 };
            logout.Clicked += MoveToLogout;
            this.ToolbarItems.Add(logout);
        }
        private async void MoveToAdressesPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new AdressesPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
        private void MoveToLogout(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        private async void MoveToPizzasPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new PizzasPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
        private async void MoveToDelivererCartClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(100);
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            Configuration c = new Configuration();
            var configList = await c.ReadConfigurationParameter("version");
            var config = configList.SingleOrDefault();
            if (!config.Parameter.Equals(Methods.version))
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Uwaga", "Twoja wersja jest nieaktualna, aby przejść dalej musisz zaktualizować aplikacje", "OK");
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            UserDialogs.Instance.HideLoading();
        }
        private async void MoveToUserPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new UserPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
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
            App.Current.MainPage = new NavigationPage(new ChooseDate(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        private async void MoveToEmployeePanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new EmployeePanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
    }
}