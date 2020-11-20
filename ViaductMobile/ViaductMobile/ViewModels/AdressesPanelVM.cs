using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;

namespace ViaductMobile.ViewModels
{
    class AdressesPanelVM : INotifyPropertyChanged
    {
        #region fields
        private List<Adress> adresses;
        private Adress selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Adress> Adresses
        {
            get { return adresses; }
            set { adresses = value; OnPropertyChanged(nameof(Adresses)); }
        }
        public Adress SelectedAdress
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

        public AdressesPanelVM()
        {
            Adress adress = new Adress();
            Adresses = Task.Run(() => adress.ReadAdress()).Result;
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
