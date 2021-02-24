using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPizzas : PopupPage
    {
        PizzasAndOthers clickedRow;
        bool edit, add;
        string notification;
        int amount;
        Xamarin.Forms.DataGrid.DataGrid pizzasDataGrid;
        public AddPizzas(Xamarin.Forms.DataGrid.DataGrid pizzasDataGrid)
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.pizzasDataGrid = pizzasDataGrid;
            edit = false;
        }
        public AddPizzas(PizzasAndOthers clickedRow, Xamarin.Forms.DataGrid.DataGrid pizzasDataGrid)
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.clickedRow = clickedRow;
            this.pizzasDataGrid = pizzasDataGrid;
            nameEntry.Text = clickedRow.Name;
            priceEntry.Text = clickedRow.Price.ToString();
            edit = true;
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            BindingContext = new ViewModels.PizzasPanelVM();
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            if (nameEntry.Text == null)
            {
                add = false;
                notification += "ulica ";
            }
            try { amount = int.Parse(priceEntry.Text); }
            catch
            {
                if (priceEntry.Text == null || priceEntry.Text == "")
                    amount = 0;
                else { add = false; notification += "cena "; }
            }
            if (add == false)
            {
                await DisplayAlert("Uwaga", "Pole" + notification + " zostało źle wypełnione", "OK");
            }
            else
            {
                if (edit == false)
                {
                    PizzasAndOthers newAdress = new PizzasAndOthers()
                    {
                        Name = nameEntry.Text,
                        Price = amount
                    };
                    bool result = await newAdress.SavePizzas();
                }
                else
                {
                    clickedRow.Name = nameEntry.Text;
                    clickedRow.Price = amount;
                    bool result = await clickedRow.UpdatePizzas(clickedRow);
                }
                pizzasDataGrid.ItemsSource = new ViewModels.PizzasPanelVM().Pizzas;
            }
        }
    }
}