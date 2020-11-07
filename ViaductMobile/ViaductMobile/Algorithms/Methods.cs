using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile.Algorithms
{
    class Methods
    {
        public static readonly List<string> permissionList = new List<string>() { "Admin", "Manager", "Pracownik" };
        public static Dictionary<String, Decimal> platformList = new Dictionary<String, Decimal>();
        public static List<Report> test;
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
