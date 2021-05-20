using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseDeliverDay : PopupPage
    {
        User loggedUser;
        decimal v_k, v_g, p_o, p_g, g_o, g_g, uber_o, uber_g, s_o, s_g, kik, amountToCash, courses, bonus, tips, amountToShouldBe, volt, elka, elkaCourses, cash;
        bool add;
        int deliverNumbers, v_count, p_count, g_count, uber_count, s_count, kik_count, volt_count, bolt_count, deliverNumbersToCart, elka_count;
        string reportId, selectedUser;
        Deliverer cart = new Deliverer();
        Employee newEmpoloyee = new Employee();
        Operation newOperation = new Operation();
        Operation newOperation2 = new Operation();
        List<Supply> listOfSupplys = new List<Supply>();
        TimeSpan godzinaRozpoczecia, godzinaSkonczenia;

        DateTime deliverDate;
        public CloseDeliverDay(User loggedUser, List<Supply> listOfSupplys, DateTime deliverDate, string selectedUser, string reportId, Deliverer cart)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            this.reportId = reportId;
            this.listOfSupplys = listOfSupplys;
            this.deliverDate = deliverDate;
            this.loggedUser = loggedUser;
            this.cart = cart;
            TipsEntry.Text = "0";
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void End_Clicked(object sender, EventArgs e)
        {
            add = true;
            deliverNumbers = 0;
            try { tips = decimal.Parse(TipsEntry.Text); }
            catch
            {
                await DisplayAlert("Uwaga", "Wartość w polu tipy nie jest liczbą", "OK");
                add = false;
            }
            if (add)
            {
                AddUpAmountOfDeliveriesAndCourses();
                cash = Methods.CalculateDailyWage(workFromEntry.Time, workToEntry.Time, loggedUser.DeliverRate);
                AddInformationAboutCart();
                CreateNewEmployeeAndNewOperation();  
                await PopupNavigation.PushAsync(new CloseDayNotification(cart, newEmpoloyee, loggedUser, deliverDate, selectedUser, newOperation, newOperation2));
            }
        }

        private void AddUpAmountOfDeliveriesAndCourses()
        {
            foreach (var item in listOfSupplys)
            {
                if (!item.Elka)
                {
                    if (item.Platform.Equals("Vk"))
                    {
                        v_k += item.Amount += item.Course;
                        v_count++;
                    }
                    else if (item.Platform.Equals("Vg"))
                    {
                        v_g += item.Amount += item.Course;
                        v_count++;
                    }
                    else if (item.Platform.Equals("Po"))
                    {
                        p_o += item.Amount += item.Course;
                        p_count++;
                    }
                    else if (item.Platform.Equals("Pg"))
                    {
                        p_g += item.Amount += item.Course;
                        p_count++;
                    }
                    else if (item.Platform.Equals("Gg"))
                    {
                        g_g += item.Amount += item.Course;
                        g_count++;
                    }
                    else if (item.Platform.Equals("Go"))
                    {
                        g_o += item.Amount += item.Course;
                        g_count++;
                    }
                    else if (item.Platform.Equals("Uo"))
                    {
                        uber_o += item.Amount += item.Course;
                        uber_count++;
                    }
                    else if (item.Platform.Equals("Ug"))
                    {
                        uber_g += item.Amount += item.Course;
                        uber_count++;
                    }
                    else if (item.Platform.Equals("So"))
                    {
                        s_o += item.Amount += item.Course;
                        s_count++;
                    }
                    else if (item.Platform.Equals("Sg"))
                    {
                        s_g += item.Amount += item.Course;
                        s_count++;
                    }
                    else if (item.Platform.Equals("Volt"))
                    {
                        volt += item.Amount += item.Course;
                        volt_count++;
                    }
                    else if (item.Platform.Equals("Kik"))
                    {
                        kik += item.Amount += item.Course;
                        kik_count++;
                    }
                    courses += item.Course;
                }
                else
                {
                    elkaCourses += item.Course;
                    elka += item.SumAmount;
                    elka_count++;
                }
                deliverNumbers++;
            }
            deliverNumbersToCart = deliverNumbers;
            if (deliverNumbers > 19)
            {
                bonus = 30;
                deliverNumbers = deliverNumbers - 20;
                int y = deliverNumbers / 5;
                bonus = bonus + (y * 10);
            }
        }
        public void AddInformationAboutCart()
        {
            amountToCash = -courses + v_g + p_g + g_g + uber_g + s_g - tips;
            amountToShouldBe = -p_o - g_o - uber_o - s_o - kik;
            cart.Courses = courses;
            cart.V_k = v_k;
            cart.V_g = v_g;
            cart.P_o = p_o;
            cart.P_g = p_g;
            cart.G_o = g_o;
            cart.G_g = g_g;
            cart.Uber_o = uber_o;
            cart.Uber_g = uber_g;
            cart.S_o = s_o;
            cart.S_g = s_g;
            cart.Kik = kik;
            cart.Volt = volt;
            cart.DeliveriesNumber = deliverNumbersToCart;
            cart.AmountToCash = amountToCash;
            cart.AmountToShouldBe = amountToShouldBe;
            cart.ReportId = reportId;
        }
        public void CreateNewEmployeeAndNewOperation()
        {
            newEmpoloyee.Nickname = cart.Nickname;
            newEmpoloyee.Date = deliverDate.AddDays(1);
            newEmpoloyee.Rate = loggedUser.DeliverRate.ToString();
            newEmpoloyee.WorkFrom = Convert.ToDateTime(workFromEntry.Time.ToString());
            newEmpoloyee.WorkTo = Convert.ToDateTime(workToEntry.Time.ToString());
            newEmpoloyee.Position = "Dostawy";
            newEmpoloyee.DayWage = (decimal)cash;
            newEmpoloyee.ReportId = reportId;
            newEmpoloyee.Bonus = bonus;

            newOperation.Name = "Zasilenie";
            newOperation.Authorizing = "Dostawca";
            newOperation.Date = DateTime.Now;
            newOperation.ReportId = reportId;
            newOperation.Type = "Brak faktury";
            newOperation.Amount = elka;

            newOperation2.Name = "Kursy";
            newOperation2.Authorizing = "Dostawca";
            newOperation2.Date = DateTime.Now;
            newOperation2.ReportId = reportId;
            newOperation2.Type = "Brak faktury";
            newOperation2.Amount = -elkaCourses;
        }
    }
}