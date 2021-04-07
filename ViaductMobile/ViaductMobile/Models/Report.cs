using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Algorithms;
using ViaductMobile.Models;

namespace ViaductMobile.Models
{
    public class Report
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Start { get; set; }
        public decimal ReportAmount { get; set; }
        public decimal Terminal { get; set; }
        public decimal ShouldBe { get; set; }
        public decimal AmountIn { get; set; }
        public decimal Difference { get; set; }
        public bool Closed { get; set; }
        public int Pizzas { get; set; }
        public int EmployeePizzas { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveReport()
        {
            try
            {
                await client.GetTable<Report>().InsertAsync(this);
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
        public async Task<bool> UpdateReport()
        {
            try
            {
                await client.GetTable<Report>().UpdateAsync(this);
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
        public async Task<List<Report>> ReadReport()
        {
            return await client.GetTable<Report>().ToListAsync();
        }

        public static async Task<List<Report>> ReadTodayReport(Report readReport)
        {
            return await client.GetTable<Report>().Where(x => x.Date.Day == readReport.Date.Day && x.Date.Month == readReport.Date.Month && x.Date.Year == readReport.Date.Year).ToListAsync();
        }

        public static async Task<Report> ReadTodayReport(DateTime date)
        {
            var reportList = await client.GetTable<Report>().Where(x => x.Date.Month == date.Month && x.Date.Day == date.Day && x.Date.Year == date.Year).ToListAsync();
            return reportList.SingleOrDefault();
        }
    }
}
