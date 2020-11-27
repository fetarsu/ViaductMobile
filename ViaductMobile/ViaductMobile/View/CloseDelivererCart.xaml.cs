using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseDelivererCart : ContentPage
    {
        User loggedUser, chosedUser;
        Report report;
        bool closed, reload;
        Deliverer cart;
        public string userr;
        List<Deliverer> cartList = new List<Deliverer>();
        string delivererId, selectedUser, reportId, chosedUserr;
        Decimal cash, bonus;
        Deliverer newDeliverer;
        DateTime deliverDate, chosedDate;
        Employee newEmployee;
        public CloseDelivererCart(User loggedUser, Deliverer newDeliverer, Employee newEmployee, DateTime chosedDate, string chosedUserr)
        {
            InitializeComponent();
            this.chosedDate = chosedDate;
            this.chosedUserr = chosedUserr;
            this.loggedUser = loggedUser;
            chooseDayPicker.Date = chosedDate.Date;
            usersPicker.SelectedItem = chosedUserr;
            this.newDeliverer = newDeliverer;
            this.newEmployee = newEmployee;
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

        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        private void chooseDayPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (reload == true)
            {
                ReloadData();
                reload = false;
            }
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
        private async void RestoreDayClicked(object sender, EventArgs e)
        {
            Supply s = new Supply();
            Report report = new Report();
            var reportList = await report.ReadTodayReport(deliverDate);
            var reportt = reportList.SingleOrDefault();
            if(reportt.Closed == true)
            {
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, aby przywrócić dostawcę należy najpierw przywrócić raport", "OK");
            }
            else
            {
                var listOfSupplys = await s.ReadSupply(newDeliverer.Id);
                newDeliverer.Closed = false;
                await newDeliverer.UpdateDeliverer(newDeliverer);
                var emp = await newEmployee.ReadEmployeeCart(usersPicker.SelectedItem.ToString(), chooseDayPicker.Date);
                var employee = emp.SingleOrDefault();
                var result = employee.DeleteEmployee(employee);
                App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, listOfSupplys))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            
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
        private void usersPicker_Focused(object sender, FocusEventArgs e)
        {
            reload = true;
        }

        private void usersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                userr = usersPicker.SelectedItem.ToString();
                ReloadData();
                reload = false;
            }
        }


        async void ReloadData()
        {
            if (userr == null)
            {
                var x = await loggedUser.ReadAllUsers();
                Methods.userList = x;
                usersPicker.ItemsSource = x;
                foreach (var item in x)
                {
                    if (item.Equals(loggedUser.Nickname))
                    {
                        usersPicker.SelectedItem = userr = item;
                    }
                }
            }
            DateTime datee = chooseDayPicker.Date;
            cartList = await newDeliverer.ReadDelivererCart(datee, userr);
            if (cartList.Count != 0)
            {
                cart = cartList.SingleOrDefault();
                delivererId = cart.Id;
                if (cart.Closed == false)
                {
                    if (delivererId != null)
                    {
                        App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, datee))
                        {
                            BarBackgroundColor = Color.FromHex("#3B3B3B"),
                            BarTextColor = Color.White
                        };
                    }
                }
                else
                {
                    Employee newEmployee = new Employee();
                    var employeeList = await newEmployee.ReadEmployeeCart(userr, datee);
                    newEmployee = employeeList.SingleOrDefault();
                    App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, cart, newEmployee, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, datee))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }

    }
}