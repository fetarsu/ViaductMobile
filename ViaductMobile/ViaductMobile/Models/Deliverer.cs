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
        public DateTime Date { get; set; }
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
        public bool Closed { get; set; }
        public decimal AmountToCash { get; set; }
        public decimal AmountToShouldBe { get; set; }
        public int DeliveriesNumber { get; set; }
        public string ReportId { get; set; }

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
        public async Task<List<Deliverer>> ReadDelivererReport(Report readReport)
        {
            return await client.GetTable<Deliverer>().Where(x => x.ReportId == readReport.Id).ToListAsync();
        }
        public async Task<bool> UpdateDeliverer(Deliverer item)
        {
            try
            {
                await client.GetTable<Deliverer>().UpdateAsync(item);
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

        public async Task<List<Deliverer>> ReadDelivererCart(DateTime datee, string userr)
        {
            string t = userr;
            var table = client.GetTable<Deliverer>();
            return await table.Where(x => x.Nickname == userr && x.Date.Day == datee.Day && x.Date.Month == datee.Month && x.Date.Year == datee.Year).ToListAsync();
        }
    }
}
