using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using Xamarin.Forms;
using ViaductMobile.Models;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseDayNotification : PopupPage
    {
        Deliverer newDeliverer;
        Employee newEmployee;
        User loggedUser;
        string chosedUser;
        DateTime deliverDate;
        bool closed = true;
        public CloseDayNotification(Deliverer newDeliverer, Employee newEmployee, User loggedUser, DateTime deliverDate, string chosedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            this.deliverDate = deliverDate;
            this.chosedUser = chosedUser;
            this.newDeliverer = newDeliverer;
            this.newEmployee = newEmployee;
            nicknameLabel.Text = newDeliverer.Nickname;
            coursesLabel.Text = newDeliverer.Courses.ToString();
            VkLabel.Text = newDeliverer.V_k.ToString();
            VgLabel.Text = newDeliverer.V_g.ToString();
            PoLabel.Text = newDeliverer.P_o.ToString();
            PgLabel.Text = newDeliverer.P_g.ToString();
            GoLabel.Text = newDeliverer.G_o.ToString();
            GgLabel.Text = newDeliverer.G_g.ToString();
            UberGLabel.Text = newDeliverer.Uber_g.ToString();
            UberOLabel.Text = newDeliverer.Uber_o.ToString();
            SoLabel.Text = newDeliverer.S_o.ToString();
            SgLabel.Text = newDeliverer.S_g.ToString();
            VoLabel.Text = newDeliverer.Volt.ToString();
            KikLabel.Text = newDeliverer.Kik.ToString();
            delivererNumberLabel.Text = newDeliverer.DeliveriesNumber.ToString();
            bonusLabel.Text = newEmployee.Bonus.ToString();
            cashForDayLabel.Text = Math.Round(newEmployee.DayWage, 2).ToString();
            AmountToCashLabel.Text = (newDeliverer.AmountToCash - newEmployee.DayWage - newEmployee.Bonus).ToString();
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }
        [Obsolete]
        private async void End_Clicked(object sender, EventArgs e)
        {
            newDeliverer.Closed = true;
            newEmployee.DayWage = Math.Round(newEmployee.DayWage, 2);
            await newEmployee.SaveEmployee();
            await newDeliverer.UpdateDeliverer(newDeliverer);
            await PopupNavigation.PopAsync(true);
            await PopupNavigation.PopAsync(true);
            App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, newDeliverer, newEmployee, deliverDate, chosedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
        }
    }
}