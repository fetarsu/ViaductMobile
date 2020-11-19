using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile.Models
{
    public class Supply
    {
        public string Id { get; set; }
        public string Adress { get; set; }
        public decimal Amount { get; set; }
        public decimal Course { get; set; }
        public string Platform { get; set; }
        public string Components { get; set; }
        public decimal SumAmount { get; set; }
        public string DelivererId { get; set; }
        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveSupply()
        {
            try
            {
                await client.GetTable<Supply>().InsertAsync(this);
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
        public async Task<List<Supply>> ReadSupply(string delivererId)
        {
            return await client.GetTable<Supply>().Where(x => x.DelivererId == delivererId).ToListAsync();
        }

        public async Task<bool> DeleteSupply(Supply item)
        {
            try
            {
                await client.GetTable<Supply>().DeleteAsync(item);
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

        public async Task<bool> UpdateSupply(Supply item)
        {
            try
            {
                await client.GetTable<Supply>().UpdateAsync(item);
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
    }
}

