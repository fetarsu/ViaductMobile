using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using ViaductMobile.View;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class ComponentsTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Components> components;
        private Components selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Components> Components
        {
            get { return components; }
            set { components = value; OnPropertyChanged(nameof(Components)); }
        }
        public Components SelectedComponent
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

        public ComponentsTableVM()
        {
            Components = AddSupply.componentsList;
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
