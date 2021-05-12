using Acr.UserDialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Globals;
using ViaductMobile.Interfaces;
using ViaductMobile.Models;
using ViaductMobile.View;
using Xamarin.Forms;

namespace ViaductMobile
{
    public partial class MainPage : ContentPage
    {
        User loggedUser;
        decimal firstMonthSalarySum = 0, secondMonthSalarySum = 0;
        public MainPage()
        {
            InitializeComponent();
            ToolbarItem moveToLogin = new ToolbarItem() { Text = "Zaloguj się", IconImageSource = "login.png" };
            moveToLogin.Clicked += MoveToLoginClicked;
            this.ToolbarItems.Add(moveToLogin);
            bool result = Methods.getMacAddress();
            if (result)
            {
                createReportButton.IsVisible = true;
            }
        }
        public MainPage(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            createReportButton.IsVisible = true;
            AddToolbarItemsDependingPermissions(loggedUser);
        }
        public void AddToolbarItemsDependingPermissions(User loggedUser)
        {
            if (loggedUser.Permission.Equals("Admin"))
            {
                ToolbarItem employeesPanel = new ToolbarItem() { Text = "Pracownicy", IconImageSource = ImageSource.FromFile("employees.png"), Order = ToolbarItemOrder.Primary, Priority = 0 };
                ToolbarItems.Add(employeesPanel);
                employeesPanel.Clicked += MoveToEmployeePanelClicked;
            }
            if (loggedUser.Permission.Equals("Admin") || loggedUser.DeliverRate > 0)
            {
                ToolbarItem deliveryCart = new ToolbarItem() { Text = "Karta dostaw", IconImageSource = ImageSource.FromFile("delivery.png"), Order = ToolbarItemOrder.Primary, Priority = 1 };
                ToolbarItem adressesPanel = new ToolbarItem() { Text = "Adresy", IconImageSource = ImageSource.FromFile("house.png"), Order = ToolbarItemOrder.Primary, Priority = 2 };
                ToolbarItem pizzasPanel = new ToolbarItem() { Text = "Produkty", IconImageSource = ImageSource.FromFile("pizza.png"), Order = ToolbarItemOrder.Primary, Priority = 3 };
                ToolbarItems.Add(pizzasPanel);
                ToolbarItems.Add(adressesPanel);
                ToolbarItems.Add(deliveryCart);
                deliveryCart.Clicked += MoveToDelivererCartClicked;
                adressesPanel.Clicked += MoveToAdressesPanelClicked;
                pizzasPanel.Clicked += MoveToPizzasPanelClicked;
            }
            ToolbarItem userPanel = new ToolbarItem() { Text = "Panel użytkownika", IconImageSource = ImageSource.FromFile("user.png"), Order = ToolbarItemOrder.Primary, Priority = 4 };
            ToolbarItem logout = new ToolbarItem() { Text = "Wyloguj", IconImageSource = ImageSource.FromFile("logout.png"), Order = ToolbarItemOrder.Primary, Priority = 5 };
            ToolbarItems.Add(userPanel);
            ToolbarItems.Add(logout);
            userPanel.Clicked += MoveToUserPanelClicked;
            logout.Clicked += MoveToLogout;
        }
        private async void MoveToAdressesPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new AdressesPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
        private void MoveToLogout(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
        }
        private async void MoveToPizzasPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new PizzasPanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
        private async void MoveToDelivererCartClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            bool correctVersion = await Methods.CheckProgramVersion();
            if (!correctVersion)
            {
                await DisplayAlert("Uwaga", "Twoja wersja jest nieaktualna, aby przejść dalej musisz zaktualizować aplikacje", "OK");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                var delivererCartt = await Deliverer.ReadDelivererCartt(DateTime.Now, loggedUser.Nickname);
                if(delivererCartt is null)
                {
                    App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                        BarTextColor = Color.White
                    };
                }
                else if (delivererCartt.Closed)
                {
                    App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, delivererCartt, DateTime.Now, loggedUser.Nickname))
                    {
                        BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                        BarTextColor = Color.White
                    };
                }   
            }
            //UserDialogs.Instance.HideLoading();
        }
        private async void MoveToUserPanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            var result = await GetCurrentAndPreviousMonthSalary(loggedUser);
            App.Current.MainPage = new NavigationPage(new UserPanel(loggedUser, result.previousMonthSalarySum, result.currentMonthSalarySum))
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
        async Task<(decimal previousMonthSalarySum, decimal currentMonthSalarySum)> GetCurrentAndPreviousMonthSalary(User loggedUser)
        {
            Employee emp = new Employee();
            var previousMonthSalary = await emp.GetMonthlySalary(loggedUser.Nickname, DateTime.Now.Month - 1, DateTime.Now.Year);
            var currentMonthSalary = await emp.GetMonthlySalary(loggedUser.Nickname, DateTime.Now.Month, DateTime.Now.Year);
            return (previousMonthSalary.Select(x => x.DayWage + x.Bonus).Sum(), currentMonthSalary.Select(x => x.DayWage + x.Bonus).Sum());
        }
        private void MoveToLoginClicked(object sender, EventArgs e)
        {
            User user = new User();
            App.Current.MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
        }

        private void MoveToChooseDateClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ChooseDate(loggedUser))
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
        }
        private async void MoveToEmployeePanelClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            App.Current.MainPage = new NavigationPage(new EmployeePanel(loggedUser))
            {
                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
    }
}