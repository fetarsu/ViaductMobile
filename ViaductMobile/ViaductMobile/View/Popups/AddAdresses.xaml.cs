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
    public partial class AddAdresses : PopupPage
    {
        Adress clickedRow;
        bool edit, add;
        string notification;
        int amount;
        Xamarin.Forms.DataGrid.DataGrid adressesDataGrid;
        public AddAdresses(Xamarin.Forms.DataGrid.DataGrid adressesDataGrid)
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.adressesDataGrid = adressesDataGrid;
            edit = false;
        }
        public AddAdresses(Adress clickedRow, Xamarin.Forms.DataGrid.DataGrid adressesDataGrid)
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            this.clickedRow = clickedRow;
            this.adressesDataGrid = adressesDataGrid;
            streetNameEntry.Text = clickedRow.Street;
            streetNumberEntry.Text = clickedRow.Number;
            deliveryCostEntry.Text = clickedRow.Amount.ToString();
            edit = true;
        }
        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            BindingContext = new ViewModels.AdressesPanelVM();
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            if (streetNameEntry.Text == null)
            {
                add = false;
                notification += "ulica ";
            }
            try { amount = int.Parse(deliveryCostEntry.Text); }
            catch
            {
                if (deliveryCostEntry.Text == null || deliveryCostEntry.Text == "")
                    amount = 0;
                else { add = false; notification += "kwota "; }
            }
            if (add == false)
            {
                await DisplayAlert("Uwaga", "Pole" + notification + " zostało źle wypełnione", "OK");
            }
            else
            {
                if (edit == false)
                {
                    Adress newAdress = new Adress()
                    {
                        Street = streetNameEntry.Text,
                        Number = streetNumberEntry.Text,
                        Amount = amount
                    };
                    bool result = await newAdress.SaveAdress();
                }
                else
                {
                    clickedRow.Street = streetNameEntry.Text;
                    clickedRow.Number = streetNumberEntry.Text;
                    clickedRow.Amount = amount;
                    bool result = await clickedRow.UpdateAdress(clickedRow);
                }
                adressesDataGrid.ItemsSource = new ViewModels.AdressesPanelVM().Adresses;
            }    
        }
    }
}