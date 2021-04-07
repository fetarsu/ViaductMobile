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
            bool correctVersion = await Methods.CheckProgramVersion();
            if (!correctVersion)
            {
                await DisplayAlert("Uwaga", "Twoja wersja jest nieaktualna, aby przejść dalej musisz zaktualizować aplikacje", "OK");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                readReportt = await Report.ReadTodayReport(chooseDay.Date);
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
                    if (loggedUser == null)
                    {
                        User user = new User()
                        {
                            Id = "25841b9f-beb4-4b99-9f0e-4727c2d9642d",
                            Nickname = "Viaduct",
                            Password = null,
                            BarRate = 0,
                            KitchenRate = 0,
                            DeliverRate = 0,
                            Permission = "Pracownik"
                        };
                        loggedUser = user;
                    }
                    App.Current.MainPage = new NavigationPage(new NewReport(readReport, loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    if (loggedUser == null)
                    {
                        User user = new User()
                        {
                            Id = "25841b9f-beb4-4b99-9f0e-4727c2d9642d",
                            Nickname = "Viaduct",
                            Password = null,
                            BarRate = 0,
                            KitchenRate = 0,
                            DeliverRate = 0,
                            Permission = "Pracownik"
                        };
                        loggedUser = user;
                    }
                    App.Current.MainPage = new NavigationPage(new NewReport(readReportt, loggedUser))
                    {
                        BarBackgroundColor = Color.FromHex("#3B3B3B"),
                        BarTextColor = Color.White
                    };

                }
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}