﻿using Rg.Plugins.Popup.Pages;
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
        decimal v_k, v_g, p_o, p_g, g_o, g_g, uber_o, uber_g, s_o, s_g, kik, amountToCash, courses, bonus;
        int deliverNumbers, v_count, p_count, g_count, uber_count, s_count, kik_count, deliverNumbers2;
        string reportId;
        Deliverer cart = new Deliverer();
        List<Supply> listOfSupplys = new List<Supply>();
        TimeSpan godzinaRozpoczecia, godzinaSkonczenia;

        DateTime deliverDate;
        public CloseDeliverDay(User loggedUser, List<Supply> listOfSupplys, DateTime deliverDate, string reportId, Deliverer cart)
        {
            this.reportId = reportId;
            this.listOfSupplys = listOfSupplys;
            this.deliverDate = deliverDate;
            this.loggedUser = loggedUser;
            InitializeComponent();
            this.cart = cart;
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void End_Clicked(object sender, EventArgs e)
        {
            var x = Methods.test;
            godzinaRozpoczecia = workFromEntry.Time;
            godzinaSkonczenia = workToEntry.Time;
            decimal tips = decimal.Parse(TipsEntry.Text);
            var platfromList = Methods.platformList.Keys.ToList();
            var platfromListstrigs = Methods.platformList.Keys.ToList();
            foreach (var item in listOfSupplys)
            {
                if (item.Platform.Equals("Vk"))
                {
                    v_k += item.SumAmount;
                    v_count++;
                }
                else if (item.Platform.Equals("Vg"))
                {
                    v_g += item.SumAmount;
                    v_count++;
                }
                else if (item.Platform.Equals("Po"))
                {
                    p_o += item.SumAmount;
                    p_count++;
                }
                else if (item.Platform.Equals("Pg"))
                {
                    p_g += item.SumAmount;
                    p_count++;
                }
                else if (item.Platform.Equals("Gg"))
                {
                    g_g += item.SumAmount;
                    g_count++;
                }
                else if (item.Platform.Equals("Go"))
                {
                    g_o += item.SumAmount;
                    g_count++;
                }
                else if (item.Platform.Equals("Uo"))
                {
                    uber_o += item.SumAmount;
                    uber_count++;
                }
                else if (item.Platform.Equals("Ug"))
                {
                    uber_g += item.SumAmount;
                    uber_count++;
                }
                else if (item.Platform.Equals("So"))
                {
                    s_o += item.SumAmount;
                    s_count++;
                }
                else if (item.Platform.Equals("Sg"))
                {
                    s_g += item.SumAmount;
                    s_count++;
                }
                else if (item.Platform.Equals("Kik"))
                {
                    kik += item.SumAmount;
                    kik_count++;
                }
                deliverNumbers++;
                courses += item.Course;
            }
            deliverNumbers2 = deliverNumbers;
            if (deliverNumbers > 19)
            {
                bonus = 30;
                deliverNumbers = -20;
                int y = deliverNumbers / 5;
                bonus = +(y * 10);
            }
            TimeSpan godzina24 = new TimeSpan(24, 00, 00);
            double start_liczba = godzinaRozpoczecia.TotalHours;
            double koniec_liczba = godzinaSkonczenia.TotalHours;
            double roznica, minusRozpoczecie;

            if (koniec_liczba >= 0 && koniec_liczba < 8)
            {
                minusRozpoczecie = (godzina24 - godzinaRozpoczecia).TotalHours;
                roznica = minusRozpoczecie + koniec_liczba;
            }
            else
            {
                roznica = (godzinaSkonczenia - godzinaRozpoczecia).TotalHours;
            }
            decimal roznica2 = (decimal)(roznica);
            var cash = loggedUser.DeliverRate * roznica2;
            amountToCash = -courses + v_g + p_g + g_g + uber_g + s_g - tips - bonus;
            var tee = courses;
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
            cart.DeliveriesNumber = deliverNumbers2;
            cart.AmountToCash = amountToCash;
            cart.ReportId = reportId;
            Employee newEmpoloyee = new Employee()
            {
                Nickname = cart.Nickname,
                Date = deliverDate,
                Rate = loggedUser.DeliverRate.ToString(),
                WorkFrom = Convert.ToDateTime(workFromEntry.Time.ToString()),
                WorkTo = Convert.ToDateTime(workToEntry.Time.ToString()),
                Position = "Deliverer",
                DayWage = cash,
                ReportId = reportId,
                Bonus = bonus
            };
            await PopupNavigation.PushAsync(new CloseDayNotification(cart, newEmpoloyee, loggedUser));
        }
    }
}