using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewReport : ContentPage
    {
        public NewReport()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = new ViewModels.ReportTableVM();
            BindingContext = new ViewModels.EmployeeTableVM();
            BindingContext = new ViewModels.OperationTableVM();
            BindingContext = new ViewModels.DelivererTableVM();
        }
        private void BackClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            return true;
        }
    }
}