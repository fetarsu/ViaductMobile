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
    public class PizzasAndOthers
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public static MobileServiceClient client = new MobileServiceClient(Texts.connectionString);
        public async Task<bool> SavePizzas()
        {

            try
            {
                await client.GetTable<PizzasAndOthers>().InsertAsync(this);
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
        public async Task<List<PizzasAndOthers>> ReadPizzas()
        {
            return await client.GetTable<PizzasAndOthers>().ToListAsync();
        }

        public async Task<bool> DeletePizzas(PizzasAndOthers item)
        {
            try
            {
                await client.GetTable<PizzasAndOthers>().DeleteAsync(item);
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

        public async Task<bool> UpdatePizzas(PizzasAndOthers item)
        {
            try
            {
                await client.GetTable<PizzasAndOthers>().UpdateAsync(item);
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
        public static List<string> GetSearchResults(string queryString, List<String> list)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return list.Where(f => f.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }
    }
}

