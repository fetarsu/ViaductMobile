using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViaductMobile.ViewModels
{
    public class SetTextFromListViewVM : INotifyPropertyChanged
    {
        private string _mylabelvalue;
        public string MyLabelValue
        {
            get { return _mylabelvalue; }
            set
            {
                _mylabelvalue = value;
                OnPropertyChanged(nameof(MyLabelValue));
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyLabelValue"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
