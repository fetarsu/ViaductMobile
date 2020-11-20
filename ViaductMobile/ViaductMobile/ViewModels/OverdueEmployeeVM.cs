using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;

namespace ViaductMobile.ViewModels
{
    class OverdueEmployeeVM : INotifyPropertyChanged
    {
        #region fields
        private List<OverdueCash> overdues;
        User user;
        private OverdueCash selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<OverdueCash> Overdues
        {
            get { return overdues; }
            set { overdues = value; OnPropertyChanged(nameof(Overdues)); }
        }
        public OverdueCash SelectedOverdue
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

        public OverdueEmployeeVM(User user)
        {
            this.user = user;
            OverdueCash overdue = new OverdueCash();
            Overdues = Task.Run(() => overdue.ReadOverdueCash(user)).Result;
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
