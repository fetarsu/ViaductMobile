﻿using Acr.UserDialogs;
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
    public partial class DelivererCart : ContentPage
    {
        User loggedUser;
        Deliverer delivererCart = new Deliverer();
        List<string> userNicknameList = new List<string>();
        Report report = new Report();
        bool closed, emptyDevliererId, reload = false, start, anyDeliverCartSupply;
        Deliverer newDeliverer = new Deliverer();
        Deliverer cart;
        public string userr;
        List<Deliverer> cartList = new List<Deliverer>();
        List<String> oneUserList = new List<string>();
        string delivererId, selectedUser, reportId;
        DateTime deliverDate, chosedDate, dateee, changedDate;
        string changedUser;
        public DelivererCart(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            changedDate = DateTime.Now;
            ReloadData();
        }
        public DelivererCart(User loggedUser, DateTime chosedDate)
        {
            InitializeComponent();
            changedDate = this.chosedDate = chosedDate;
            this.loggedUser = loggedUser;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            ReloadData();
        }
        public DelivererCart(User loggedUser, DateTime chosedDate, string chosedUser)
        {
            InitializeComponent();
            changedUser = chosedUser;
            changedDate = this.chosedDate = chosedDate;
            this.loggedUser = loggedUser;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            ReloadData();
        }
        public DelivererCart(User loggedUser, List<Supply> listOfSupply)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(listOfSupply).Supplies;
            chooseDayPicker.Date = DateTime.Now;
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
            if (report == null)
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
            if (report == null)
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
            else if (report.Closed == true)
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
                var listOfSupplys = new ViewModels.DelivererCartVM(delivererCart.Id).Supplies;
                await PopupNavigation.PushAsync(new CloseDeliverDay(loggedUser, listOfSupplys, chooseDayPicker.Date, usersPicker.SelectedItem.ToString(), report.Id, delivererCart));
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
                changedUser = usersPicker.SelectedItem.ToString();
                SetCorrectUserPicker();
                SetCorrectDelivererCart();
                CheckIfButtonsShouldBeVisible();
            }
        }
        private void chooseDay_PropertyChanged(object sender, EventArgs e)
        {
            if (reload == true)
            {
                changedDate = chooseDayPicker.Date;
                SetCorrectDatePicker();
                SetCorrectDelivererCart();
                CheckIfButtonsShouldBeVisible();
            }
        }

        async void ReloadData()
        {
            User u = new User();
            Deliverer d = new Deliverer();
            userNicknameList = await u.ReadAllUsers();
            SetCorrectUserPicker();
            SetCorrectDatePicker();
            SetCorrectDelivererCart();
            CheckIfButtonsShouldBeVisible();
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

        async void SetCorrectDelivererCart()
        {
            delivererCart = await Deliverer.ReadDelivererCartt(chooseDayPicker.Date, usersPicker.SelectedItem.ToString());
            if (delivererCart != null)
            {
                if (delivererCart.Closed == false)
                {
                    delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(delivererCart.Id).Supplies;
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
            else
            {
                string x = null;
                delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(x).Supplies;
            }
            report = await Report.ReadTodayReport(chooseDayPicker.Date);
        }
        void CheckIfButtonsShouldBeVisible()
        {
            if (!loggedUser.Nickname.Equals(usersPicker.SelectedItem))
            {
                closeDayButton.IsVisible = addButton.IsVisible = deleteButton.IsVisible = editButton.IsVisible = false;
            }
            else
            {
                closeDayButton.IsVisible = addButton.IsVisible = deleteButton.IsVisible = editButton.IsVisible = true;
            }
            if (report is null)
            {
                closeDayButton.IsVisible = addButton.IsVisible = deleteButton.IsVisible = editButton.IsVisible = false;
            }
        }
    }
}