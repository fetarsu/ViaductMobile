using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViaductMobile.Models;

namespace ViaductMobile
{
    public class Employee
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Nickname { get; set; }
        public string Rate { get; set; }
        public string Position { get; set; }
        public DateTime WorkFrom { get; set; }
        public DateTime WorkTo { get; set; }
        public decimal DayWage { get; set; }
        public decimal Bonus { get; set; }
        public string ReportId { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");
        public async Task<bool> SaveEmployee()
        {

            try
            {
                await client.GetTable<Employee>().InsertAsync(this);
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
        public async Task<bool> UpdateEmployee(Employee employee)
        {

            try
            {
                await client.GetTable<Employee>().UpdateAsync(employee);
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
        public async Task<List<Employee>> ReadEmployeeReport(Report readReport)
        { 
            return await client.GetTable<Employee>().Where(x => x.ReportId == readReport.Id).ToListAsync();
        }

        public async Task<bool> DeleteEmployee(Employee item)
        {
            try
            {
                await client.GetTable<Employee>().DeleteAsync(item);
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

        public async Task<List<Employee>> ReadEmployeeCart(string user, DateTime date)
        {
            var a = client.GetTable<Employee>().Where(x => x.Nickname == user && x.Date.Day == date.Day && x.Date.Month == date.Month && x.Date.Year == date.Year && x.Position == "Dostawy").ToListAsync();
            return await a;
        }
        public async Task<List<Employee>> GetMonthlySalary(string nickname, int month, int year)
        {
            return await client.GetTable<Employee>().Where(x => x.Nickname == nickname && x.Date.Year == year && x.Date.Month == month).ToListAsync();
        }
    }
}

