using Google.Apis.Customsearch.v1;

using Google.Apis.Customsearch.v1.Data;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

namespace WebScraper
{
	public class CrawlerApis

	{

		public static string[] Cse(string keyword)

		{

			const string apiKey = "AIzaSyCMqeIXp8iynbYQpfvHOIrzYxnEkrxgGd4";

			const string searchEngineId = "YourID";

			var query = keyword;

			string[] newUrl = new string[100];

			var count = 0;

			//const string query = "engineerverse";

			CustomsearchService customSearchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = apiKey });

			Google.Apis.Customsearch.v1.CseResource.ListRequest listRequest = customSearchService.Cse.List(query);

			listRequest.Cx = searchEngineId;

			//listRequest.Start = 1;

			for (int i = 1; i <= 90; i += 10)

			{

				listRequest.Start = i;

				listRequest.Safe = CseResource.ListRequest.SafeEnum.Medium;

				Search search = listRequest.Execute();

				foreach (var item in search.Items)

				{

					string holding = item.Link;

					newUrl[count] = holding;

					count += 1;

					Console.WriteLine("Title : " + item.Title + Environment.NewLine + "Link : " + item.Link + Environment.NewLine + Environment.NewLine);

				}

			}

			Console.ReadLine();

			return newUrl;

		}
	}


}