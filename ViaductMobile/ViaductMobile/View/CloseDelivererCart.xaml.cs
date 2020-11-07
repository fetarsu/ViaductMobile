using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseDelivererCart : ContentPage
    {
        User loggedUser;
        Report report;
        bool closed;
        Decimal cash, bonus;
        Deliverer newDeliverer;
        DateTime deliverDate;
        public CloseDelivererCart(User loggedUser, Deliverer newDeliverer, Decimal cash, Decimal bonus)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            this.newDeliverer = newDeliverer;
            this.cash = cash;
            this.bonus = bonus;
            coursesLabel.Text = newDeliverer.Courses.ToString();
            VkLabel.Text = newDeliverer.V_k.ToString();
            VgLabel.Text = newDeliverer.V_g.ToString();
            PoLabel.Text = newDeliverer.P_o.ToString();
            PgLabel.Text = newDeliverer.P_g.ToString();
            GoLabel.Text = newDeliverer.G_o.ToString();
            GgLabel.Text = newDeliverer.G_g.ToString();
            UberGLabel.Text = newDeliverer.Uber_g.ToString();
            UberOLabel.Text = newDeliverer.Uber_o.ToString();
            SoLabel.Text = newDeliverer.S_o.ToString();
            SgLabel.Text = newDeliverer.S_g.ToString();
            KikLabel.Text = newDeliverer.Kik.ToString();
            delivererNumberLabel.Text = newDeliverer.DeliveriesNumber.ToString();
            bonusLabel.Text = bonus.ToString();
            cashForDayLabel.Text = cash.ToString();
            AmountToCashLabel.Text = newDeliverer.AmountToCash.ToString();
            deliverDate = chooseDayPicker.Date = DateTime.Now;
            ReadAllUsers();
        }
        public CloseDelivererCart(User loggedUser, Deliverer newDeliverer)
        {
            this.newDeliverer = newDeliverer;
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            BindingContext = new ViewModels.DelivererCartVM();
            deliverDate = chooseDayPicker.Date = DateTime.Now;
        }

        private void chooseDay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("Xd");
        }

        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }

        [Obsolete]
        private async void CloseDayClicked(object sender, EventArgs e)
        {
            Report report = new Report();
            var id = report.ReadTodayReport(deliverDate);
            var listOfSupplys = new ViewModels.DelivererCartVM().Supplies;
            await PopupNavigation.PushAsync(new CloseDeliverDay(loggedUser, listOfSupplys, deliverDate));
        }
        async void ReadAllUsers()
        {
            var x = await loggedUser.ReadAllUsers();
            usersPicker.ItemsSource = x;
            foreach (var item in x)
            {
                if (item.Equals(loggedUser.Nickname))
                {
                    usersPicker.SelectedItem = item;
                }
            }

        }
    }
}