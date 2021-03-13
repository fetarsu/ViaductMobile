using Acr.UserDialogs;
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
        Report report = new Report();
        bool closed, emptyDevliererId, reload = false, start, anyDeliverCartSupply;
        Deliverer newDeliverer = new Deliverer();
        Deliverer cart;
        public string userr;
        List<Deliverer> cartList = new List<Deliverer>();
        List<String> oneUserList = new List<string>();
        string delivererId, selectedUser, reportId;
        DateTime deliverDate, chosedDate, dateee;
        public DelivererCart(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            oneUserList.Add(loggedUser.Nickname);
            usersPicker.ItemsSource = oneUserList;
            usersPicker.SelectedItem = loggedUser.Nickname;
            ReloadData();
        }
        public DelivererCart(User loggedUser, DateTime chosedDate)
        {
            this.chosedDate = chosedDate;
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            oneUserList.Add(loggedUser.Nickname);
            usersPicker.ItemsSource = oneUserList;
            usersPicker.SelectedItem = loggedUser.Nickname;
            chooseDayPicker.Date = chosedDate.Date;
            ReloadData();
        }
        public DelivererCart(User loggedUser, List<Supply> listOfSupply)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            oneUserList.Add(loggedUser.Nickname);
            usersPicker.ItemsSource = oneUserList;
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

        private async void AddSupply(object sender, EventArgs e)
        {
            if(report == null)
            {
                App.Current.MainPage = new NavigationPage(new AddSupply(delivererCartDataGrid, loggedUser, cartList.Count(), delivererId, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else if (report.Closed == true)
            {
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, aby przywrócić dostawcę należy najpierw przywrócić raport", "OK");
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new AddSupply(delivererCartDataGrid, loggedUser, cartList.Count(), delivererId, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }
        [Obsolete]
        private async void EditSupply(object sender, EventArgs e)
        {
            if (report.Closed == true)
            {
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, aby przywrócić dostawcę należy najpierw przywrócić raport", "OK");
            }
            else
            {
                Supply clickedRow = (Supply)delivererCartDataGrid.SelectedItem;
                if (clickedRow != null)
                {
                    App.Current.MainPage = new NavigationPage(new AddSupply(clickedRow, delivererCartDataGrid, loggedUser, cartList.Count(), delivererId, chooseDayPicker.Date))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
                else
                    await DisplayAlert("Uwaga", "Proszę zaznaczyć wiersz", "OK");
            }
        }
        [Obsolete]
        private async void CloseDayClicked(object sender, EventArgs e)
        {
            if (report.Closed == true)
            {
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, aby przywrócić dostawcę należy najpierw przywrócić raport", "OK");
            }
            else
            {
                if (report != null)
                {
                    var readedReport = await report.ReadTodayReport(chooseDayPicker.Date);
                    if (readedReport[0] != null)
                    {
                        reportId = readedReport[0].Id;
                    }
                    else
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
                }
                else
                {
                    reportId = report.Id;
                }
                var listOfSupplys = new ViewModels.DelivererCartVM(delivererId).Supplies;
                await PopupNavigation.PushAsync(new CloseDeliverDay(loggedUser, listOfSupplys, chooseDayPicker.Date, usersPicker.SelectedItem.ToString(), reportId, cart));
            }

        }

        async void DeleteSupply(object sender, EventArgs e)
        {
            if (report.Closed == true)
            {
                await DisplayAlert("Uwaga", "Raport tego dnia został zamknięty, aby przywrócić dostawcę należy najpierw przywrócić raport", "OK");
            }
            else
            {
                Supply x = (Supply)delivererCartDataGrid.SelectedItem;
                if (x != null)
                {
                    await x.DeleteSupply(x);
                    delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(delivererId).Supplies;
                    Logs newLog = new Logs()
                    {
                        UserId = loggedUser.Nickname,
                        DeletedTable = "Supply",
                        Date = DateTime.Now
                    };
                    bool r = await newLog.SaveLogs();
                }
                else
                    await DisplayAlert("Uwaga", "Proszę zaznaczyć wiersz", "OK");
            }
        }
        private void usersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                userr = usersPicker.SelectedItem.ToString();
                ReloadData();
            }
        }
        private void chooseDay_PropertyChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                dateee = chooseDayPicker.Date;
                ReloadData();
            }
        }

        async void ReloadData()
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            await Task.Delay(1);
            if (userr == null)
            {
                var uList = Methods.userList;
                foreach (var item in uList)
                {
                    if (item.Equals(loggedUser.Nickname))
                    {
                        usersPicker.SelectedItem = userr = item;
                    }
                }
            }
            if (dateee.Year == 0001)
            {
                dateee = chooseDayPicker.Date;
            }
            DateTime datee = dateee;
            chooseDayPicker.Date = datee;
            cartList = await newDeliverer.ReadDelivererCart(datee, userr);
            await Task.Delay(100);
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
                    App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, cart, newEmployee, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
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
            reload = true;
            Report r = new Report();
            var rList = await r.ReadTodayReport(datee);
            if (rList == null)
                report = null;
            else
                report = rList.SingleOrDefault();

            chooseDayPicker.Unfocus();
            UserDialogs.Instance.HideLoading();
        }
    }
}