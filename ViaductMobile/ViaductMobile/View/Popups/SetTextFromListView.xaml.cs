using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using ViaductMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetTextFromListView : PopupPage
    {
        User loggedUser;
        string changedValue, property, delivererId, selectedUser;
        int cartListCount;
        DateTime chosedDate;
        public event EventHandler<object> CallbackEvent;
        public static List<String> adressesList = new List<String>();
        public static List<String> pizzasList = new List<String>();
        private Action<string> selectedItemCallback;
        private Action<Components> componentCallback;
        Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid;
        public SetTextFromListView(Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser, int cartListCount, string delivererId, DateTime chosedDate, string selectedUser, string property, Action<string> selectedItemCallback)
        {
            this.delivererCartDataGrid = delivererCartDataGrid;
            this.loggedUser = loggedUser;
            this.cartListCount = cartListCount;
            this.delivererId = delivererId;
            this.chosedDate = chosedDate;
            this.property = property;
            this.selectedItemCallback = selectedItemCallback;
            this.selectedUser = selectedUser;
            InitializeComponent();
            LoadAdresses(property);
        }

        public SetTextFromListView(Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser, int cartListCount, string delivererId, DateTime chosedDate, string selectedUser, Action<Components> componentCallback, string property)
        {
            this.delivererCartDataGrid = delivererCartDataGrid;
            this.loggedUser = loggedUser;
            this.cartListCount = cartListCount;
            this.delivererId = delivererId;
            this.chosedDate = chosedDate;
            this.property = property;
            this.componentCallback = componentCallback;
            this.selectedUser = selectedUser;
            InitializeComponent();
            LoadPizzas(property);
        }

        private void searchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            if (property.Equals("address"))
            {
                searchResults.ItemsSource = Adress.GetSearchResults(e.NewTextValue, adressesList);
            }
            else if (property.Equals("pizzas"))
            {
                searchResults.ItemsSource = PizzasAndOthers.GetSearchResults(e.NewTextValue, pizzasList);
            }
        }

        [Obsolete]
        private async void changeSearchBar(object sender, SelectedItemChangedEventArgs e)
        {
            searchBar.Text = searchResults.SelectedItem.ToString();
            if (property.Equals("pizzas"))
            {
                string result = await DisplayPromptAsync("Podaj ilość", "", keyboard: Keyboard.Numeric);
                if (result != null)
                {
                    var resultInt = int.Parse(result);
                    Components sth = new Components() { Name = searchResults.SelectedItem.ToString(), Count = resultInt };
                    componentCallback(sth);
                    await PopupNavigation.PopAsync(true);
                }
            }
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Change_Clicked(object sender, EventArgs e)
        {
            selectedItemCallback(searchBar.Text);
            await PopupNavigation.PopAsync(true);
        }

        private async void LoadPizzas(string property)
        {
            addButton.IsVisible = false;
            PizzasAndOthers pizzas = new PizzasAndOthers();
            var y = await pizzas.ReadPizzas();

            foreach (var item in y)
            {
                pizzasList.Add(item.Name);
            }
            searchResults.ItemsSource = pizzasList.Distinct().ToList();
        }
        private async void LoadAdresses(string property)
        {

            Adress ad = new Adress();
            var adressList = await ad.ReadAdress();
            var adressListDistinct = adressList.GroupBy(x => x.Street).Select(k => k.First());
            foreach (var item in adressListDistinct)
            {
                adressesList.Add(item.Street);
            }
            searchResults.ItemsSource = adressesList.Distinct().ToList();
        }
    }
}