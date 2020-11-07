﻿using Rg.Plugins.Popup.Services;
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
    public partial class DelivererCart : ContentPage
    {
        User loggedUser;
        Report report;
        bool closed;
        Deliverer newDeliverer;
        DateTime deliverDate;
        public DelivererCart(User loggedUser)
        {
            this.loggedUser = loggedUser;
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            BindingContext = new ViewModels.DelivererCartVM();
            deliverDate = chooseDayPicker.Date = DateTime.Now;
            ReadAllUsers();
        }
        public DelivererCart(User loggedUser, Deliverer newDeliverer)
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

        private void AddSupply(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AddSupply(delivererCartDataGrid, loggedUser))
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
                App.Current.MainPage = new NavigationPage(new AddSupply(clickedRow, delivererCartDataGrid, loggedUser))
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
            var id = report.ReadTodayReport(deliverDate);
            var listOfSupplys = new ViewModels.DelivererCartVM().Supplies;
            await PopupNavigation.PushAsync(new CloseDeliverDay(loggedUser, listOfSupplys, deliverDate));
        }

        async void DeleteSupply(object sender, EventArgs e)
        {
            Supply x = (Supply)delivererCartDataGrid.SelectedItem;
            await x.DeleteSupply(x);
            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM().Supplies;
        }
        async void ReadAllUsers()
        {
            var x = await loggedUser.ReadAllUsers();
            usersPicker.ItemsSource = x;
            foreach(var item in x)
            {
                if (item.Equals(loggedUser.Nickname))
                {
                    usersPicker.SelectedItem = item;
                }
            }

        }

        private void usersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Xd");
        }
    }
}