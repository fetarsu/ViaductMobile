using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class DelivererTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Deliverer> deliverers;
        private Deliverer selectedItem;
        private bool isRefreshing;
        Report readReport;
        #endregion
        #region Properties
        public List<Deliverer> Deliverers
        {
            get { return deliverers; }
            set { deliverers = value; OnPropertyChanged(nameof(Deliverers)); }
        }
        public Deliverer SelectedUser
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

        public DelivererTableVM(Report readReport)
        {
            this.readReport = readReport;
            Deliverer deliverer = new Deliverer();
            Deliverers = Task.Run(() => deliverer.ReadDelivererReport(readReport)).Result;
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
