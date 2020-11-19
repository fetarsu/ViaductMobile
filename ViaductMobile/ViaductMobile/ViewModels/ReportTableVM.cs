using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViaductMobile.Models;
using Xamarin.Forms;


namespace ViaductMobile.ViewModels
{
    class ReportTableVM : INotifyPropertyChanged
    {
        #region fields
        private List<Report> reports;
        private Report selectedItem;
        Report readReport;
        private bool isRefreshing;
        #endregion
        #region Properties
        public List<Report> Reports
        {
            get { return reports; }
            set { reports = value; OnPropertyChanged(nameof(Reports)); }
        }
        public Report SelectedReport
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

        public ReportTableVM(Report readReport)
        {
            this.readReport = readReport;
            Report report = new Report();
            List<Report> reportList = new List<Report>();
            reportList.Add(readReport);
            Reports = reportList;
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
