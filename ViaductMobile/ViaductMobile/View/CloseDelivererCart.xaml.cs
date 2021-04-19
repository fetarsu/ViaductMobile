using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Globals;
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
        Deliverer delivererCart = new Deliverer();
        Report report;
        bool closed, reload = false, changeDay;
        Deliverer cart;
        public string userr;
        List<Deliverer> cartList = new List<Deliverer>();
        List<string> userNicknameList = new List<string>();
        string delivererId, selectedUser, reportId, chosedUserr, changedUser;
        Decimal cash, bonus;
        Deliverer newDeliverer;
        DateTime deliverDate, chosedDate, changedDate;
        Employee newEmployee;
        public CloseDelivererCart(User loggedUser, Deliverer newDeliverer, DateTime chosedDate, string chosedUserr)
        {
            changeDay = false;
            InitializeComponent();
            changedDate = this.chosedDate = chosedDate;
            changedUser = this.chosedUserr = chosedUserr;
            this.loggedUser = loggedUser;
            this.newDeliverer = newDeliverer;
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
            voltLabel.Text = newDeliverer.Volt.ToString();
            delivererNumberLabel.Text = newDeliverer.DeliveriesNumber.ToString();
            AmountToCashLabel.Text = newDeliverer.AmountToCash.ToString();
            deliverDate = chooseDayPicker.Date;
            ReloadData();
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
        private async void RestoreDayClicked(object sender, EventArgs e)
        {
            Supply s = new Supply();
            Report report = new Report();
            var reportt = await Report.ReadTodayReport(deliverDate);
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
                if (employee != null)
                {
                    var result = employee.DeleteEmployee(employee);
                }
                App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, listOfSupplys))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }     
        }

        private void usersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                changedUser = usersPicker.SelectedItem.ToString();
                SetCorrectUserPicker();
                SetCorrectClosedDay();
            }
        }
        private void chooseDay_PropertyChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                changedDate = chooseDayPicker.Date;
                SetCorrectDatePicker();
                SetCorrectClosedDay();
            }
        }

        async void ReloadData()
        {
            User u = new User();
            Deliverer d = new Deliverer();
            userNicknameList = await u.ReadAllUsers();
            SetCorrectUserPicker();
            SetCorrectDatePicker();
            SetCorrectClosedDay();
        }
        void SetCorrectUserPicker()
        {
            reload = false;
            if (loggedUser.Permission.Equals("Admin") || loggedUser.Permission.Equals("Manager"))
            {
                usersPicker.ItemsSource = userNicknameList;
            }
            else
            {
                usersPicker.ItemsSource.Add(loggedUser.Nickname);
            }
            if (usersPicker.SelectedItem is null)
            {
                if (changedUser is null)
                {
                    foreach (var item in userNicknameList)
                    {
                        if (item.Equals(loggedUser.Nickname))
                        {
                            usersPicker.SelectedItem = item;
                        }
                    }
                }
                else
                {
                    usersPicker.SelectedItem = changedUser;
                }
            }
            reload = true;
        }
        void SetCorrectDatePicker()
        {
            reload = false;
            if (changedDate != null)
            {
                chooseDayPicker.Date = changedDate.Date;
            }
            reload = true;
        }

        async void SetCorrectClosedDay()
        {
            if (changeDay)
            {
                delivererCart = await Deliverer.ReadDelivererCartt(chooseDayPicker.Date, usersPicker.SelectedItem.ToString());
                if (delivererCart != null)
                {
                    if (delivererCart.Closed == false)
                    {
                        App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                        {
                            BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                            BarTextColor = Color.White
                        };
                    }
                    else
                    {
                        App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, delivererCart, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                        {
                            BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                            BarTextColor = Color.White
                        };
                    }
                }
                App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                {
                    BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
                    BarTextColor = Color.White
                };
            }
            else
            {
                changeDay = true;
            }
        }
    }
}