﻿using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using ViaductMobile.View.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xfx;

namespace ViaductMobile.View
{
    public partial class AddSupply : ContentPage, INotifyPropertyChanged
    {
        public static string supplyAddress;
        public static List<String> adressesList = new List<String>();
        public static List<String> pizzasList = new List<String>();
        Adress adress = new Adress();
        public List<Adress> adressList = new List<Adress>();
        public static List<Components> componentsList = new List<Components>();
        decimal deliveryAmount, courseAmount;
        int cartListCount;
        Supply clickedRow;
        User loggedUser;
        bool edit, add, elka;
        string components, streetName, delivererId, selectedUser, notification, asd;
        DateTime chosedDate;
        Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid;
        public AddSupply(Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser, int cartListCount, string delivererId, DateTime chosedDate, string selectedUser)
        {
            this.delivererId = delivererId;
            this.chosedDate = chosedDate;
            this.selectedUser = selectedUser;
            this.cartListCount = cartListCount;
            this.loggedUser = loggedUser;
            componentsList.Clear();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.SetTextFromListViewVM();
            Button resetPasswordButton = new Button();
            this.delivererCartDataGrid = delivererCartDataGrid;
            edit = false;
            platformPicker.ItemsSource = Methods.platformList.Keys.ToList();
            LoadAdressesAndPizzas();
        }

        public AddSupply(Supply clickedRow, Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser, int cartListCount, string delivererId, DateTime chosedDate)
        {
            this.delivererId = delivererId;
            this.chosedDate = chosedDate;
            this.loggedUser = loggedUser;
            this.cartListCount = cartListCount;
            componentsList.Clear();
            this.clickedRow = clickedRow;
            this.delivererCartDataGrid = delivererCartDataGrid;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.SetTextFromListViewVM();

            string[] co = clickedRow.Components.Split(',');
            foreach (var item in co)
            {
                if (item.Length > 1)
                {
                    var x = item.Split(' ');
                    var y = x[1].Split('x');
                    Components c = new Components();
                    for(int ij = 2; ij < x.Length; ij++)
                    {
                        c.Name += x[ij] + " ";
                    }
                    c.Name = c.Name.Remove(c.Name.Length - 1);
                    c.Count = int.Parse(y[0]);
                    componentsList.Add(c);
                }
            }
            componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
            platformPicker.ItemsSource = Methods.platformList.Keys.ToList();
            platformPicker.SelectedItem = clickedRow.Platform;
            courseEntry.Text = clickedRow.Course.ToString();
            amountEntry.Text = clickedRow.Amount.ToString();
            string[] tokens = clickedRow.Adress.Split(' ');

            if (tokens.Length == 4)
            {
                string[] tokens2 = tokens[3].Split('.');
                adressLabel.Text = "Ulica: " + tokens[0] + " " + tokens[1];
                streetName = tokens[0] + " " + tokens[1];
                buildingEntry.Text = tokens[2];
                flatEntry.Text = tokens2[1];
            }
            else
            {
                string[] tokens2 = tokens[2].Split('.');
                adressLabel.Text = "Ulica: " + tokens[0];
                streetName = tokens[0];
                buildingEntry.Text = tokens[1];
                flatEntry.Text = tokens2[1];
            }
            edit = true;
            adressLabel.IsVisible = true;
            searchResults.IsVisible = false;
            LoadAdressesAndPizzas();
        }
        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = Adress.GetSearchResults(e.NewTextValue, adressesList);
        }
        //public void OnTextChanged2(object sender, TextChangedEventArgs e)
        //{
        //    searchResults2.ItemsSource = PizzasAndOthers.GetSearchResults(e.NewTextValue, pizzasList);
        //}

        private async void LoadAdressesAndPizzas()
        {
            PizzasAndOthers pizzas = new PizzasAndOthers();
            adressList = await adress.ReadAdress();
            var adressListDistinct = adressList.GroupBy(x => x.Street).Select(k => k.First());
            var y = await pizzas.ReadPizzas();
            foreach (var item in adressListDistinct)
            {
                adressesList.Add(item.Street);
            }
            foreach (var item in y)
            {
                pizzasList.Add(item.Name);
            }
            adressesList = adressesList.Distinct().ToList();
            pizzasList = pizzasList.Distinct().ToList();

        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            if (streetName == null)
            {
                add = false;
                notification += "nazwa ulica ";
            }
            if (buildingEntry.Text == null)
            {
                add = false;
                notification += "numer bloku ";
            }
            if (platformPicker.SelectedItem == null)
            {
                add = false;
                notification += "platforma ";
            }
            if (flatEntry.Text == null)
            {
                flatEntry.Text = "";
            }
            try { deliveryAmount = decimal.Parse(amountEntry.Text); }
            catch
            {
                if (amountEntry.Text == null || amountEntry.Text == "")
                    deliveryAmount = 0;
                else { add = false; notification += "kwota dostawy "; }
            }
            try { courseAmount = decimal.Parse(courseEntry.Text); }
            catch
            {
                if (courseEntry.Text == null || courseEntry.Text == "")
                    courseAmount = 0;
                else { add = false; notification += "kwota kursu "; }
            }
            if (add == false)
            {
                await DisplayAlert("Uwaga", "Pole " + notification + " zostało źle wypełnione", "OK");
            }
            if (platformPicker.SelectedItem.Equals("Vg"))
            {
                elka = await DisplayAlert("Pytanie", "Czy ta dostawa to elka?", "Tak", "Nie");
            }

            var delivererCartt = await Deliverer.ReadDelivererCartt(chosedDate, loggedUser.Nickname);
            if (delivererCartt is null)
            {
                Deliverer d = new Deliverer();
                d.Nickname = selectedUser;
                d.Date = chosedDate.Date.AddDays(1);
                d.Closed = false;
                await d.SaveDeliverer();
                delivererId = d.Id;
            }
            else
            {
                delivererId = delivererCartt.Id;
            }
            components = " ";
            IEnumerable<Components> componentsIEnumerable = (IEnumerable<Components>)componentsDataGrid.ItemsSource;
            if (componentsIEnumerable != null)
            {
                foreach (var item in componentsIEnumerable)
                {
                    components += item.Count + "x " + item.Name + ", ";
                }
            }

            var newAdress = adressList.Where(x => x.Street.Equals(streetName.ToLower()) && x.Number.Equals(buildingEntry.Text)).FirstOrDefault();
            if (newAdress == null)
            {
                if (platformPicker.SelectedItem.ToString().Equals("Po") || platformPicker.SelectedItem.ToString().Equals("Pg") || platformPicker.SelectedItem.ToString().Equals("Go")
                    || platformPicker.SelectedItem.ToString().Equals("Uo") || platformPicker.SelectedItem.ToString().Equals("Gg") || platformPicker.SelectedItem.ToString().Equals("Volt") || platformPicker.SelectedItem.ToString().Equals("Ug"))
                {
                    Adress item = new Adress()
                    {
                        Street = streetName,
                        Number = buildingEntry.Text,
                        Amount = 0
                    };
                    bool result = await item.SaveAdress();
                }
                else
                {
                    Adress item = new Adress()
                    {
                        Street = streetName,
                        Number = buildingEntry.Text,
                        Amount = decimal.Parse(amountEntry.Text)
                    };
                    bool result = await item.SaveAdress();
                }
            }
            else
            {
                if (newAdress.Amount == 0)
                {
                    newAdress.Amount = decimal.Parse(courseEntry.Text);
                    bool result = await newAdress.UpdateAdress(newAdress);
                }
            }

            if (edit == false)
            {
                Supply newSupply = new Supply()
                {
                    Adress = streetName + " " + buildingEntry.Text + " m." + flatEntry.Text,
                    Amount = deliveryAmount,
                    Course = courseAmount,
                    Platform = platformPicker.SelectedItem.ToString(),
                    Components = components,
                    SumAmount = deliveryAmount + courseAmount,
                    Elka = elka,
                    DelivererId = delivererId
                };
                bool result = await newSupply.SaveSupply();
            }
            else
            {
                clickedRow.Adress = streetName + " " + buildingEntry.Text + " m." + flatEntry.Text;
                clickedRow.Amount = deliveryAmount;
                clickedRow.Course = courseAmount;
                clickedRow.Platform = platformPicker.SelectedItem.ToString();
                clickedRow.Components = components;
                clickedRow.SumAmount = deliveryAmount + courseAmount;
                clickedRow.DelivererId = delivererId;
                bool result = await clickedRow.UpdateSupply(clickedRow);
            }
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, chosedDate))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };

        }

        //private void changeSearchBar2(object sender, SelectedItemChangedEventArgs e)
        //{
        //    changeSearchBarAsync();
        //}
        //private async void changeSearchBarAsync()
        //{
        //    string result = await DisplayPromptAsync("Podaj ilość", "", keyboard: Keyboard.Numeric);
        //    var resultInt = int.Parse(result);
        //    Components sth = new Components() { Name = searchResults2.SelectedItem.ToString(), Count = resultInt };
        //    componentsList.Add(sth);
        //    BindingContext = new ViewModels.ComponentsTableVM();
        //    componentsDataGrid.ItemsSource = null;
        //    componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
        //    searchResults2.IsVisible = false;
        //    HideListView2();

        //}
        [Obsolete]
        async void addAdressButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new SetTextFromListView(delivererCartDataGrid, loggedUser, cartListCount,
                delivererId, chosedDate, selectedUser, "address", (s) => { addressLabel.Text = "Ulica: " + s; streetName = s; }));
        }
        [Obsolete]
        async void ShowListView2(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new SetTextFromListView(delivererCartDataGrid, loggedUser, cartListCount,
                delivererId, chosedDate, selectedUser, (ds) =>
                {
                    componentsList.Add(ds);
                    BindingContext = new ViewModels.ComponentsTableVM();
                    componentsDataGrid.ItemsSource = null;
                    componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
                }, "pizzas"));
        }

        //private void HideListView2(object sender, EventArgs e)
        //{
        //    HideListView2();
        //}
        //private void HideListView2()
        //{
        //    componentLabel.IsVisible = true;
        //    searchResults2.IsVisible = false;
        //    hideButton.IsVisible = false;
        //    componentsDataGrid.IsVisible = true;
        //}

        private async void platformPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (platformPicker.SelectedItem.ToString().Equals("Ug") || platformPicker.SelectedItem.ToString().Equals("Uo")
                || platformPicker.SelectedItem.ToString().Equals("Go") || platformPicker.SelectedItem.ToString().Equals("Gg")
                || platformPicker.SelectedItem.ToString().Equals("Volt"))
            {
                courseEntry.Text = "7";
            }
            else if (platformPicker.SelectedItem.ToString().Equals("Po") || platformPicker.SelectedItem.ToString().Equals("Pg"))
            {
                courseEntry.Text = "5";
            }
            else if (platformPicker.SelectedItem.ToString().Equals("Kik"))
            {
                courseEntry.Text = "2";
            }
            else
            {
                courseEntry.Text = adressList.Where(x => x.Street.Equals(streetName) && x.Number.Equals(buildingEntry.Text)).Select(k => k.Amount).FirstOrDefault().ToString();
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Components x = (Components)componentsDataGrid.SelectedItem;
            componentsList.Remove(x);
            componentsDataGrid.ItemsSource = null;
            componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
        }
        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, chosedDate))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser, chosedDate))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }
    }
}