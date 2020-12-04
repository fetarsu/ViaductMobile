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
            Methods.ReadAllUsers();
        }

        public MainPage(User loggedUser)
        {
            InitializeComponent();
            createReportButton.IsVisible = true;
            this.loggedUser = loggedUser;
            string userPermission = loggedUser.Permission;
            if (userPermission.Equals("Admin"))
            {
                ToolbarItem employeesPanel = new ToolbarItem() { Text = "Pracownicy", IconImageSource = "employees.png" };
                employeesPanel.Clicked += MoveToEmployeePanelClicked;
                this.ToolbarItems.Add(employeesPanel);
            }
            if(userPermission.Equals("Admin") || loggedUser.DeliverRate > 0)
            {
                ToolbarItem deliveryCart = new ToolbarItem() { Text = "Karta dostaw", IconImageSource = "delivery.png" };
                ToolbarItem adressesPanel = new ToolbarItem() { Text = "Adresy", IconImageSource = "house.png" };
                ToolbarItem pizzasPanel = new ToolbarItem() { Text = "Produkty", IconImageSource = "pizza.png" };
                this.ToolbarItems.Add(pizzasPanel);
                this.ToolbarItems.Add(adressesPanel);
                this.ToolbarItems.Add(deliveryCart);
                deliveryCart.Clicked += MoveToDelivererCartClicked;
                adressesPanel.Clicked += MoveToAdressesPanelClicked;
                pizzasPanel.Clicked += MoveToPizzasPanelClicked;
            }
            ToolbarItem userPanel = new ToolbarItem() { Text = "Panel użytkownika", IconImageSource = "user.png" };
            userPanel.Clicked += MoveToUserPanelClicked;
            this.ToolbarItems.Add(userPanel);
            ToolbarItem logout = new ToolbarItem() { Text = "Wyloguj", IconImageSource = "logout.png" };
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
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
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