﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class DelivererCartVM : INotifyPropertyChanged
    {
        #region fields
        private List<Supply> supplies;
        private Supply selectedItem;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Supply> Supplies
        {
            get { return supplies; }
            set { supplies = value; OnPropertyChanged(nameof(Supplies)); }
        }
        public Supply SelectedSupply
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

        public DelivererCartVM()
        {
            Supply deliverer = new Supply();
            Supplies = Task.Run(() => deliverer.ReadSupply()).Result;
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
