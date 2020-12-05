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
        public static readonly string version = "1.0";
        public static string macAdress;
        public static readonly List<string> permissionList = new List<string>() { "Admin", "Manager", "Pracownik" };
        public static readonly List<string> positionList = new List<string>() { "Bar", "Kuchnia", "Dostawy", "Kierownictwo" };
        public static Dictionary<String, Decimal> platformList = new Dictionary<String, Decimal>();
        public static List<Report> test;
        public static List<Employee> reportEmployeeList;
        public static List<Operation> reportOperationList;
        public static List<string> userList;
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static async void ReadAllUsers()
        {
            User user = new User();
            List<string> userListt = new List<string>();
            userListt = await user.ReadAllUsers();
            userList = userListt;
        }
        public static async void CheckVersion()
        {
            User user = new User();
            List<string> userListt = new List<string>();
            userListt = await user.ReadAllUsers();
            userList = userListt;
        }
    }
}