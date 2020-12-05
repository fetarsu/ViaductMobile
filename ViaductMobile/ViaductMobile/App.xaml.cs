﻿using System;
using System.Linq;
using System.Net.NetworkInformation;
using ViaductMobile.Algorithms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#3B3B3B"),
                BarTextColor = Color.White,
            };
        }

        protected override void OnStart()
        {
            LoadStatics();
        }

        private async void LoadStatics()
        {
            User user = new User();
            Methods.userList = await user.ReadAllUsers();
            Platform adress = new Platform();
            var x = await adress.ReadPlatform();
            foreach (var item in x)
            {
                Methods.platformList.Add(item.Name, item.Course);
            }

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
