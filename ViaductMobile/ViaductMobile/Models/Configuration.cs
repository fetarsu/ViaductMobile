using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class Configuration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parameter { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");

        public async Task<List<Configuration>> ReadConfigurationParameter(string name)
        {
            return await client.GetTable<Configuration>().Where(x => x.Name == name).ToListAsync();
        }
    }
}

