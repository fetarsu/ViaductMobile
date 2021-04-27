using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Globals;
using ViaductMobile.Models;

namespace ViaductMobile.Models
{
    public class Operation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Authorizing { get; set; }
        public string DocumentNumber { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string ReportId { get; set; }

        public static MobileServiceClient client = new MobileServiceClient(Texts.connectionString);
        public async Task<bool> SaveOperations()
        {

            try
            {
                await client.GetTable<Operation>().InsertAsync(this);
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
        public async Task<bool> UpdateOpetarions(Operation operation)
        {

            try
            {
                await client.GetTable<Operation>().UpdateAsync(operation);
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
        public async Task<List<Operation>> ReadOperationsReport(Report readReport)
        {
            return await client.GetTable<Operation>().Where(x => x.ReportId == readReport.Id).ToListAsync();
        }

        public async Task<bool> DeleteOperations(Operation item)
        {
            try
            {
                await client.GetTable<Operation>().DeleteAsync(item);
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
