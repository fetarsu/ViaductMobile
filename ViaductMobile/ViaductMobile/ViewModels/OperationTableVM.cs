using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class OperationTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Operation> operations;
        private Operation selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Operation> Operations
        {
            get { return operations; }
            set { operations = value; OnPropertyChanged(nameof(Operations)); }
        }
        public Operation SelectedOperation
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

        public OperationTableVM()
        {
            Operation operations = new Operation();
            Operations = Task.Run(() => operations.ReadOperations()).Result;
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
