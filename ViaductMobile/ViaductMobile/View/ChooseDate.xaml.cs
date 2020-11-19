using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseDate : ContentPage
    {
        List<Report> reportt;
        public Report readReportt = new Report();
        public ChooseDate()
        {
            InitializeComponent();
            chooseDay.Date = DateTime.Now;
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
        private async void MoveToNewReportClicked(object sender, EventArgs e)
        {

            reportt = await readReportt.ReadTodayReport(chooseDay.Date);
            readReportt = reportt.SingleOrDefault();
            if(readReportt == null)
            {
                Report readReport = new Report();
                readReport.Start = 0;
                readReport.ReportAmount = 0;
                readReport.Terminal = 0;
                readReport.Date = chooseDay.Date.AddDays(1);
                readReport.ShouldBe = 0;
                readReport.AmountIn = 0;
                readReport.Difference = 0;
                readReport.Pizzas = 0;
                await readReport.SaveReport();
                App.Current.MainPage = new NavigationPage(new NewReport(readReport))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new NewReport(readReportt))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }
    }
}