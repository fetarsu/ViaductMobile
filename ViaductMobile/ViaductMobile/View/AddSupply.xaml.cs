using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xfx;

namespace ViaductMobile.View
{
    public partial class AddSupply : ContentPage
    {
        public static List<String> adressesList = new List<String>();
        public static List<String> pizzasList = new List<String>();
        public static List<Components> componentsList = new List<Components>();
        Supply clickedRow;
        User loggedUser;
        bool edit;
        string components, streetName;
        Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid;
        public AddSupply(Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser)
        {
            this.loggedUser = loggedUser;
            componentsList.Clear();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            Button resetPasswordButton = new Button();
            this.delivererCartDataGrid = delivererCartDataGrid;
            edit = false;
            platformPicker.ItemsSource = Methods.platformList.Keys.ToList();
            LoadAdressesAndPizzas();
        }
        public AddSupply(Supply clickedRow, Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser)
        {
            this.loggedUser = loggedUser;
            componentsList.Clear();
            this.clickedRow = clickedRow;
            this.delivererCartDataGrid = delivererCartDataGrid;
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();

            string[] co = clickedRow.Components.Split(',');
            foreach (var item in co)
            {
                if(item.Length > 1)
                {
                    var x = item.Split(' ');
                    var y = x[1].Split('x');
                    Components c = new Components();
                    c.Name = x[2];
                    c.Count = int.Parse(y[0]);
                    componentsList.Add(c);
                }
            }
            componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
            courseEntry.Text = clickedRow.Course.ToString();
            amountEntry.Text = clickedRow.Amount.ToString();
            platformPicker.ItemsSource = Methods.platformList.Keys.ToList();
            platformPicker.SelectedItem = clickedRow.Platform;
            string[] tokens = clickedRow.Adress.Split(' ');
            string[] tokens2 = tokens[2].Split('.');
            adressLabel.Text = tokens[0];
            streetName = tokens[0];
            buildingEntry.Text = tokens[1];
            flatEntry.Text = tokens2[1];
            edit = true;
            addAdressButton.Text = "Usuń";
            searchBar.IsVisible = false;
            adressLabel.IsVisible = true;
            searchResults.IsVisible = false;
            LoadAdressesAndPizzas();
        }
        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = Adress.GetSearchResults(e.NewTextValue, adressesList);
        }
        public void OnTextChanged2(object sender, TextChangedEventArgs e)
        {
            searchResults2.ItemsSource = PizzasAndOthers.GetSearchResults(e.NewTextValue, pizzasList);
        }

        private async void LoadAdressesAndPizzas()
        {
            Adress adress = new Adress();
            PizzasAndOthers pizzas = new PizzasAndOthers();
            var x = await adress.ReadAdress();
            var y = await pizzas.ReadPizzas();
            foreach (var item in x)
            {
                adressesList.Add(item.Street);
            }
            foreach (var item in y)
            {
                pizzasList.Add(item.Name);
            }

        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            components = " ";
            IEnumerable<Components> componentsIEnumerable = (IEnumerable<Components>)componentsDataGrid.ItemsSource;
            foreach (var item in componentsIEnumerable)
            {
                components += item.Count + "x " + item.Name + ", ";
            }

            //var x = componentss.GetEnumerator();

            if (edit == false)
            {
                Supply newSupply = new Supply()
                {
                    Adress = streetName + " " + buildingEntry.Text + " m." + flatEntry.Text,
                    Amount = decimal.Parse(amountEntry.Text),
                    Course = decimal.Parse(courseEntry.Text),
                    Platform = platformPicker.SelectedItem.ToString(),
                    Components = components,
                    SumAmount = decimal.Parse(amountEntry.Text) + decimal.Parse(courseEntry.Text),
                    DelivererId = loggedUser.Id
                };
                bool result = await newSupply.SaveSupply();
            }
            else
            {
                clickedRow.Adress = streetName + " " + buildingEntry.Text + " m." + flatEntry.Text;
                clickedRow.Amount = decimal.Parse(amountEntry.Text);
                clickedRow.Course = decimal.Parse(courseEntry.Text);
                clickedRow.Platform = platformPicker.SelectedItem.ToString();
                clickedRow.Components = components;
                clickedRow.SumAmount = decimal.Parse(amountEntry.Text) + decimal.Parse(courseEntry.Text);
                clickedRow.DelivererId = loggedUser.Id;
                bool result = await clickedRow.UpdateSupply(clickedRow);
            }
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        private void changeSearchBar(object sender, SelectedItemChangedEventArgs e)
        {
            searchBar.Text = searchResults.SelectedItem.ToString();
        }
        private void changeSearchBar2(object sender, SelectedItemChangedEventArgs e)
        {
            changeSearchBarAsync();
        }
        private async void changeSearchBarAsync()
        {
            string result = await DisplayPromptAsync("Podaj ilość", "", keyboard: Keyboard.Numeric);
            var resultInt = int.Parse(result);
            Components sth = new Components() { Name = searchResults2.SelectedItem.ToString(), Count = resultInt };
            componentsList.Add(sth);
            BindingContext = new ViewModels.ComponentsTableVM();
            componentsDataGrid.ItemsSource = null;
            componentsDataGrid.ItemsSource = new ViewModels.ComponentsTableVM().Components;
            searchResults2.IsVisible = false;
        }

        private void addAdressButton_Clicked(object sender, EventArgs e)
        {
            if (addAdressButton.Text.Equals("Dodaj"))
            {
                streetName = null;
                addAdressButton.Text = "Usuń";
                searchBar.IsVisible = false;
                adressLabel.Text = "Adres: " + searchBar.Text;
                streetName = searchBar.Text;
                adressLabel.IsVisible = true;
                searchResults.IsVisible = false;
            }
            else
            {
                addAdressButton.Text = "Dodaj";
                searchBar.Text = null;
                adressLabel.Text = null;
                searchBar.IsVisible = true;
                adressLabel.IsVisible = false;
            }
        }
        private void ShowListView(object sender, EventArgs e)
        {
            searchResults.IsVisible = true;
        }
        private void ShowListView2(object sender, EventArgs e)
        {
            searchResults2.IsVisible = true;
            hideButton.IsVisible = true;
            componentLabel.IsVisible = false;
        }

        private void HideListView2(object sender, EventArgs e)
        {
            componentLabel.IsVisible = true;
            searchResults2.IsVisible = false;
            hideButton.IsVisible = false;
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
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new DelivererCart(loggedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }
    }
}