using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraper
{
	class Program
	{
		private static void Main(string[] args)
		{

			Console.WriteLine("Which search would you like to scrape? (include http://)");
			string siteToScrape = Console.ReadLine();

			Console.WriteLine("Would you lke to output in: \n 1) Txt File \n 2) Html");
			int outputType = Convert.ToInt32(Console.ReadLine());

			var responseFromServer = RequestWebsite(siteToScrape);

			if (outputType == 1)
			{
				PrintToTxtFile(responseFromServer);
			}
			else
			{
				PrintToHtmlFile(responseFromServer);
			}

			var filteredList = PullOutLinksFromGoogleAndAddToAList().Where(x => x.Contains("google") == false).ToList();
			PrintAListOfSitesFromGoogleToATxtFile(filteredList);

			for (var i = 0; i < filteredList.Count(); i++)
			{
				var pushSiteToIndividualTxtFile = RequestWebsite(filteredList[i]);
				OutPutAllResultsIntoSeperateTxtFiles(i.ToString(), pushSiteToIndividualTxtFile);
			}

			Console.ReadLine();
		}

		public static string RequestWebsite(string url)
		{
			WebRequest request = WebRequest.Create(url);
			request.Method = "GET";
			request.Credentials = CredentialCache.DefaultCredentials;
			string responseFromServer = "";
			((HttpWebRequest)request).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
												  "Chrome/65.0.3325.181 Safari/537.36";
			try
			{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Console.WriteLine(response.StatusDescription);
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
				Console.WriteLine(responseFromServer);
				reader.Close();
				dataStream.Close();
				response.Close();

			}
			catch (Exception ex)
			{
				responseFromServer = ex.ToString();
			}
			return responseFromServer;
		}
		private static void PrintToTxtFile(string response)
		{
			File.WriteAllText(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webData.txt", response);
		}

		private static void PrintToHtmlFile(string response)
		{
			File.WriteAllText(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webDataHTML.html", response);
		}

		public static List<string> PullOutLinksFromGoogleAndAddToAList()
		{
			Console.WriteLine("Extracting links....................");

			using (StreamReader r = new StreamReader(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webData.txt"))
			{
				string readAllText = r.ReadToEnd();

				List<string> listOfSites = new List<string>();


				string search = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
				MatchCollection match = Regex.Matches(readAllText, search);

				foreach (Match m in match)
				{
					var urlsExtracted = String.Concat(m.Groups[1], "://", m.Groups[2].Value, m.Groups[3]);
					urlsExtracted = urlsExtracted.Replace("&amp", "");
					listOfSites.Add(urlsExtracted);
					Console.WriteLine(urlsExtracted);
					Console.WriteLine();
				}

				return listOfSites;
			}
		}

		public static void PrintAListOfSitesFromGoogleToATxtFile(List<string> filteredListOfSites)
		{
			File.WriteAllLines(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webDataListOfSites.txt", filteredListOfSites);
		}

		public static List<StreamWriter> OutPutAllResultsIntoSeperateTxtFiles(string filename, string contentFromSites)
		{
			var streamWriters = new List<StreamWriter>();
			streamWriters.Add(new StreamWriter(@"C:\Work\Training\WebScraper\Csharp-WebScraper\WebDadi\" + filename + ".txt"));

			Parallel.ForEach(streamWriters, s => { s.Write(contentFromSites); s.Dispose(); });

			return streamWriters;
		}
	}
}
