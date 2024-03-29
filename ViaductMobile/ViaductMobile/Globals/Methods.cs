﻿using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;
using ViaductMobile.Globals;
using Acr.UserDialogs;
using System;
using System.Linq;
using ViaductMobile.Algorithms;
using ViaductMobile.Globals;
using ViaductMobile.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViaductMobile.Algorithms
{
    public class Methods
    {
        public static readonly string version = "1.4.2";
        public static string macAdress;
        public static readonly List<string> permissionList = new List<string>() { "Admin", "Manager", "Pracownik" };
        public static readonly List<string> positionList = new List<string>() { "Bar", "Kuchnia", "Dostawy", "Kierownictwo" };
        public static readonly List<string> operationTypeList = new List<string>() { "Brak faktury", "Faktura" };
        public static Dictionary<String, Decimal> platformList = new Dictionary<String, Decimal>() { { "Vg", 0}, { "Vk", 0 }, { "Po", 5 }, { "Pg", 5 }, { "Gg", 7 },
        { "Go", 7},{ "Ug", 7},{ "Uo", 7},{ "Sg", 0},{ "So", 0},{ "KiK", 2},};
        public static List<Employee> reportEmployeeList;
        public static List<Operation> reportOperationList;
        public static List<string> userList;
        private static System.Random random = new System.Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task<bool> CheckProgramVersion()
        {
            var version = await Configuration.ReadConfigurationParameter(TextResources.version);
            return version.Equals(TextResources.programVersion) ? true : false;
        }

        public static bool getMacAddress()
        {
            string macAddress = string.Empty;

            var all = Collections.List(Java.Net.NetworkInterface.NetworkInterfaces);

            foreach (var interfaces in all)
            {
                if (!(interfaces as Java.Net.NetworkInterface).Name.Contains("wlan0")) continue;

                var macBytes = (interfaces as
                Java.Net.NetworkInterface).GetHardwareAddress();
                if (macBytes == null) continue;

                var sb = new System.Text.StringBuilder();
                foreach (var b in macBytes)
                {
                    string convertedByte = string.Empty;
                    convertedByte = (b & 0xFF).ToString("X2") + ":";

                    if (convertedByte.Length == 1)
                    {
                        convertedByte.Insert(0, "0");
                    }
                    sb.Append(convertedByte);
                }

                macAddress = sb.ToString().Remove(sb.Length - 1);

                if (macAddress.Equals(Texts.tabletMacAddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static decimal CalculateDailyWage(TimeSpan startHour, TimeSpan endHour, int delivererRate)
        {
            TimeSpan hour24 = new TimeSpan(24, 00, 00);
            double startHourAsDouble = startHour.TotalHours;
            double endHourAsDouble = endHour.TotalHours;
            double difference, minusStart;

            if (startHourAsDouble >= 0 && endHourAsDouble < 8)
            {
                minusStart = (hour24 - startHour).TotalHours;
                difference = minusStart + endHourAsDouble;
            }
            else
            {
                difference = (endHour - startHour).TotalHours;
            }
            return delivererRate * (decimal)difference;
        }
    }
}