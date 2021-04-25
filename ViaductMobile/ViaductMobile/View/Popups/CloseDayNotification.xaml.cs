﻿using Rg.Plugins.Popup.Pages;
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
using ViaductMobile.Globals;
using Acr.UserDialogs;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseDayNotification : PopupPage
    {
        Deliverer newDeliverer;
        Employee newEmployee;
        Operation newOperation, newOperation2;
        User loggedUser;
        string chosedUser;
        DateTime deliverDate;
        bool closed = true;
        public CloseDayNotification(Deliverer newDeliverer, Employee newEmployee, User loggedUser, DateTime deliverDate, string chosedUser, Operation newOperation, Operation newOperation2)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            this.deliverDate = deliverDate;
            this.chosedUser = chosedUser;
            this.newDeliverer = newDeliverer;
            this.newEmployee = newEmployee;
            this.newOperation = newOperation;
            this.newOperation2 = newOperation2;
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
            elkiLabel.Text = newOperation.Amount.ToString();
            delivererElkiLabel.Text = Math.Abs(newOperation2.Amount).ToString();
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
            UserDialogs.Instance.ShowLoading(Texts.loadingMessage);
            await Task.Delay(100);
            newDeliverer.Closed = true;
            newEmployee.DayWage = Math.Round(newEmployee.DayWage, 2);
            await newEmployee.SaveEmployee();
            if(newOperation.Amount != 0)
            {
                await newOperation.SaveOperations();
            }
            if (newOperation2.Amount != 0)
            {
                await newOperation2.SaveOperations();
            }
            await newOperation2.SaveOperations();
            await newDeliverer.UpdateDeliverer(newDeliverer);
            await PopupNavigation.PopAsync(true);
            await PopupNavigation.PopAsync(true);
            App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, newDeliverer, deliverDate, chosedUser))
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White
            };
            UserDialogs.Instance.HideLoading();
        }
    }
}