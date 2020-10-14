using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViaductMobile.Utils
{
    public class DataProvider
    {
		public static MobileServiceClient client = new MobileServiceClient("https://viaductpizza.azurewebsites.net");

		public static async Task<List<User>> GetUsers()
		{
			return await client.GetTable<User>().ToListAsync();
		}
	}
}
