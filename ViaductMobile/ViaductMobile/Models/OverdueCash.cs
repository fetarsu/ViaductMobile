using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class OverdueCash
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }

        public static MobileServiceClient client = new MobileServiceClient(Texts.connectionString);
        public async Task<bool> SaveOverdueCash()
        {

            try
            {
                await client.GetTable<OverdueCash>().InsertAsync(this);
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
        public async Task<bool> DeleteOverdueCash(OverdueCash item)
        {
            try
            {
                await client.GetTable<OverdueCash>().DeleteAsync(item);
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

        public async Task<List<OverdueCash>> ReadOverdueCash(User user)
        {
            return await client.GetTable<OverdueCash>().Where(x => x.UserId == user.Id).ToListAsync();
        }

    }
}

