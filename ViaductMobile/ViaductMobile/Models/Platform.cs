using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class Platform
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Course { get; set; }

        public static MobileServiceClient client = new MobileServiceClient(Texts.connectionString);

        public async Task<List<Platform>> ReadPlatform()
        {
            return await client.GetTable<Platform>().ToListAsync();
        }
    }
}

