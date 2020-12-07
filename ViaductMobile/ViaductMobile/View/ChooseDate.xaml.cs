using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseDate : ContentPage
    {
        List<Report> reportt;
        User loggedUser;
        public Report readReportt = new Report();
        public ChooseDate()
        {
            InitializeComponent();
            chooseDay.Date = DateTime.Now;
        }
        public ChooseDate(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            chooseDay.Date = DateTime.Now;
        }
        protected override bool OnBackButtonPressed()
        {
            if (loggedUser != null)
            {
                App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            return true;
        }
        private void BackClicked(object sender, EventArgs e)
        {
            if (loggedUser != null)
            {
                App.Current.MainPage = new NavigationPage(new MainPage(loggedUser))
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#3B3B3B"),
                    BarTextColor = Color.White
                };
            }
        }
        private async void MoveToNewReportClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Proszę czekać...");
            Configuration c = new Configuration();
            var configList = await c.ReadConfigurationParameter("version");
            var config = configList.SingleOrDefault();
            if (!config.Parameter.Equals(Methods.version))
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Uwaga", "Twoja wersja jest nieaktualna, aby przejść dalej musisz zaktualizować aplikacje", "OK");
            }
            else
            {
                reportt = await readReportt.ReadTodayReport(chooseDay.Date);
                readReportt = reportt.SingleOrDefault();
                if (readReportt == null)
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
                    if (loggedUser != null)
                    {
                        App.Current.MainPage = new NavigationPage(new NewReport(readReport, loggedUser))
                        {
                            BarBackgroundColor = Color.FromHex("#3B3B3B"),
                            BarTextColor = Color.White
                        };
                    }
                    else
                    {
                        App.Current.MainPage = new NavigationPage(new NewReport(readReport))
                        {
                            BarBackgroundColor = Color.FromHex("#3B3B3B"),
                            BarTextColor = Color.White
                        };
                    }
                }
                else
                {
                    if (loggedUser != null)
                    {
                        App.Current.MainPage = new NavigationPage(new NewReport(readReportt, loggedUser))
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
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}