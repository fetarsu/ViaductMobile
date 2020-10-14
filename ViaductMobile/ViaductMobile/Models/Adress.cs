using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class Adress
    {
        public string Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveAdress()
        {

            try
            {
                await client.GetTable<Adress>().InsertAsync(this);
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
        public async Task<List<Adress>> ReadAdress()
        {
            return await client.GetTable<Adress>().ToListAsync();
        }

        public async Task<bool> DeleteAdress(Adress item)
        {
            try
            {
                await client.GetTable<Adress>().DeleteAsync(item);
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

        public async Task<bool> UpdateAdress(Adress item)
        {
            try
            {
                await client.GetTable<Adress>().UpdateAsync(item);
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

