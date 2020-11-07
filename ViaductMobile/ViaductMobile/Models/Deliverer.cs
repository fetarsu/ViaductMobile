using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile.Models
{
    public class Deliverer
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public decimal Courses { get; set; }
        public decimal V_k { get; set; }
        public decimal V_g { get; set; }
        public decimal P_o { get; set; }
        public decimal P_g { get; set; }
        public decimal G_o { get; set; }
        public decimal G_g { get; set; }
        public decimal Uber_o { get; set; }
        public decimal Uber_g { get; set; }
        public decimal S_o { get; set; }
        public decimal S_g { get; set; }
        public decimal Kik { get; set; }
        public decimal AmountToCash { get; set; }
        public int DeliveriesNumber { get; set; }
        public string ReportId { get; set; }
        public virtual Report Report { get; set; }


        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveDeliverer()
        {
            try
            {
                await client.GetTable<Deliverer>().InsertAsync(this);
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
        public async Task<List<Deliverer>> ReadDeliverer()
        {
            return await client.GetTable<Deliverer>().ToListAsync();
        }

        public async Task<bool> DeleteDeliverer(Deliverer item)
        {
            try
            {
                await client.GetTable<Deliverer>().DeleteAsync(item);
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
