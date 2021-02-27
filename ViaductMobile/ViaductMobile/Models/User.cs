using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class User
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int BarRate { get; set; }
        public int KitchenRate { get; set; }
        public int DeliverRate { get; set; }
        public string Permission { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveUser()
        {

            try
            {
                await client.GetTable<User>().InsertAsync(this);
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
        public async Task<List<User>> ReadUser()
        {
            return await client.GetTable<User>().ToListAsync();
        }
        public async Task<List<User>> ReadUser(string nickname)
        {
            return await client.GetTable<User>().Where(x => x.Nickname == nickname).ToListAsync();
        }


        public async Task<bool> UpdateUser(User item)
        {
            try
            {
                await client.GetTable<User>().UpdateAsync(item);
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
        public async Task<bool> DeleteUser(User item)
        {
            try
            {
                await client.GetTable<User>().DeleteAsync(item);
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
        public async Task<List<String>> ReadAllUsers()
        {
            var table = client.GetTable<User>();
            return await table.Select(x => x.Nickname).ToListAsync();
        }
        public async Task<List<User>> FindSinleUser(string nickname)
        {
            var table = client.GetTable<User>();
            return await table.Where(x => x.Nickname == nickname).ToListAsync();
        }
    }
}

