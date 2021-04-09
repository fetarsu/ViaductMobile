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
        Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid;
        public SetTextFromListView(Xamarin.Forms.DataGrid.DataGrid delivererCartDataGrid, User loggedUser, int cartListCount, string delivererId, DateTime chosedDate, string selectedUser, string property)
        {
            this.delivererCartDataGrid = delivererCartDataGrid;
            this.loggedUser = loggedUser;
            this.cartListCount = cartListCount;
            this.delivererId = delivererId;
            this.chosedDate = chosedDate;
            this.property = property;
            this.selectedUser = selectedUser;
            InitializeComponent();
            LoadAdressesOrPizzas(property);
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
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Change_Clicked(object sender, EventArgs e)
        {
            changedValue = searchBar.Text;
            await Navigation.PushAsync(new AddSupply(delivererCartDataGrid, loggedUser, cartListCount, delivererId, chosedDate, selectedUser, property, changedValue));
        }

        private async void LoadAdressesOrPizzas(string property)
        {
            if (property.Equals("address"))
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
            else if (property.Equals("pizzas"))
            {
                PizzasAndOthers pizzas = new PizzasAndOthers();
                var y = await pizzas.ReadPizzas();

                foreach (var item in y)
                {
                    pizzasList.Add(item.Name);
                }
                searchResults.ItemsSource = pizzasList.Distinct().ToList();
            }
        }
    }
}