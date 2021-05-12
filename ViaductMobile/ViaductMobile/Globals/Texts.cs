using System;
using System.Collections.Generic;
using System.Text;

namespace ViaductMobile.Globals
{
    class Texts
    {
        //Settings
        public static readonly string tabletMacAddress = "D0:B1:28:D5:87:E9";
        public static readonly string emailToSendReportAddress = "r.switucha@gmail.com";
        public static readonly string userNicknameOnTablet = "Viaduct";
        public static readonly string programVersion = "dev";
        public static readonly string connectionString = "https://viaductdev.azurewebsites.net";

        //Testing settings
        public static readonly string tabletMacAddressTesting = "A8:9C:ED:C7:48:3E";
        public static readonly string emailToSendReportAddressTesting = "r.switucha@gmail.com";

        //DisplayAlert
        public static readonly string attentionDisplayAlertHeader = "Uwaga";
        public static readonly string errorDisplayAlertHeader = "Błąd";
        public static readonly string fatalErrorDisplayAlertHeader = "Błąd krytyczny";

        public static readonly string okDisplayAlertMessage = "OK";
        public static readonly string acceptDisplayAlertMessage = "Tak";
        public static readonly string dismissDisplayAlertMessage = "Nie";

        public static readonly string wrongLoginDetailsDisplayAlertMessage = "Zły login lub hasło";

        //Acr.UserDialogs
        public static readonly string loadingMessage = "Proszę czekać...";

        //XAML
        public static readonly string appBackgroundColor = "#3B3B3B";
    }
}
