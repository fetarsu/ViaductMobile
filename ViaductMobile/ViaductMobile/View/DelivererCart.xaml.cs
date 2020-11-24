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
    public partial class DelivererCart : ContentPage
    {
        User loggedUser;
        Report report;
        bool closed, emptyDevliererId, reload = false;
        Deliverer newDeliverer = new Deliverer();
        Deliverer cart;
        public string userr;
        List<Deliverer> cartList = new List<Deliverer>();
        string delivererId, selectedUser, reportId;
        DateTime deliverDate, chosedDate;
        public DelivererCart(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            usersPicker.ItemsSource = Methods.userList;
            usersPicker.SelectedItem = loggedUser.Nickname;
        }
        public DelivererCart(User loggedUser, List<Supply> listOfSupply)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            usersPicker.ItemsSource = Methods.userList;
            usersPicker.SelectedItem = selectedUser;
            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(listOfSupply).Supplies;
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

        private void AddSupply(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AddSupply(delivererCartDataGrid, loggedUser, cartList.Count(), delivererId, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
        [Obsolete]
        private void EditSupply(object sender, EventArgs e)
        {
            Supply clickedRow = (Supply)delivererCartDataGrid.SelectedItem;
            if (clickedRow != null)
            {
                App.Current.MainPage = new NavigationPage(new AddSupply(clickedRow, delivererCartDataGrid, loggedUser, delivererId))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }
        [Obsolete]
        private async void CloseDayClicked(object sender, EventArgs e)
        {
            Report report = new Report();
            var idList = await report.ReadTodayReport(chooseDayPicker.Date);
            if (!idList.Any())
            {
                Report readReport = new Report();
                readReport.Start = 0;
                readReport.ReportAmount = 0;
                readReport.Terminal = 0;
                readReport.Date = chooseDayPicker.Date;
                readReport.ShouldBe = 0;
                readReport.AmountIn = 0;
                readReport.Difference = 0;
                readReport.Pizzas = 0;
                await readReport.SaveReport();
                reportId = readReport.Id;
            }
            else
            {
                reportId = idList.SingleOrDefault().Id;
            }
            var listOfSupplys = new ViewModels.DelivererCartVM(delivererId).Supplies;
            await PopupNavigation.PushAsync(new CloseDeliverDay(loggedUser, listOfSupplys, chooseDayPicker.Date, reportId, cart));
        }

        async void DeleteSupply(object sender, EventArgs e)
        {
            Supply x = (Supply)delivererCartDataGrid.SelectedItem;
            await x.DeleteSupply(x);
            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(delivererId).Supplies;
        }

        private void usersPicker_Focused(object sender, FocusEventArgs e)
        {
            reload = true;
        }

        private void usersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(reload == true)
            {
                userr = usersPicker.SelectedItem.ToString();
                ReloadData();
                reload = false;
            }
        }
        private void chooseDay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (reload == true)
            {
                ReloadData();
                reload = false;
            }
        }

        async void ReloadData()
        {
            if(userr == null)
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
                        delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(delivererId).Supplies;
                    }
                }
                else
                {
                    Employee newEmployee = new Employee();
                    var employeeList = await newEmployee.ReadEmployeeCart(userr, datee);
                    newEmployee = employeeList.SingleOrDefault();
                    App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, cart, newEmployee))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
            }
            else
            {
                delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM().Supplies;
            }
        }
    }
}