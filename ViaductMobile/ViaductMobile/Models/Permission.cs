using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViaductMobile.Models
{
    public class Permission
    {
        public int id { get; set; }
        public string Title { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<List<Permission>> ReadPermission()
        {
            return await client.GetTable<Permission>().ToListAsync();
        }
    }
}
