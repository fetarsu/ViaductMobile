using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViaductMobile.Models
{
    public class Logs
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string DeletedTable { get; set; }
        public DateTime Date { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveLogs()
        {
            try
            {
                await client.GetTable<Logs>().InsertAsync(this);
                return true;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                var response = await msioe.Response.Content.ReadAsStringAsync();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Logs>> ReadAdress()
        {
            return await client.GetTable<Logs>().ToListAsync();
        }
    }
}
