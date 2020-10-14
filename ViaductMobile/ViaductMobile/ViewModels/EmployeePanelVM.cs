using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;

namespace ViaductMobile.ViewModels
{
    class EmployeePanelVM : INotifyPropertyChanged
    {
        #region fields
        private List<User> users;
        private User selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(nameof(Users)); }
        }
        public User SelectedUser
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                System.Diagnostics.Debug.WriteLine("User Selected : " + value?.Nickname);
            }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }
        public ICommand RefreshCommand { get; set; }
        #endregion

        public EmployeePanelVM()
        {
            User user = new User();
            Users = Task.Run(() => user.ReadUser()).Result;
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
