using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;

namespace ViaductMobile.ViewModels
{
    class PizzasPanelVM : INotifyPropertyChanged
    {
        #region fields
        private List<PizzasAndOthers> pizzas;
        private PizzasAndOthers selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<PizzasAndOthers> Pizzas
        {
            get { return pizzas; }
            set { pizzas = value; OnPropertyChanged(nameof(Pizzas)); }
        }
        public PizzasAndOthers SelectedPizza
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }
        public ICommand RefreshCommand { get; set; }
        #endregion

        public PizzasPanelVM()
        {
            PizzasAndOthers pizzas = new PizzasAndOthers();
            Pizzas = Task.Run(() => pizzas.ReadPizzas()).Result;
            RefreshCommand = new Command(CmdRefresh);
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(3000);
            IsRefreshing = false;
        }
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
