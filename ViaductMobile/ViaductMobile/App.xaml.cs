using System;
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
