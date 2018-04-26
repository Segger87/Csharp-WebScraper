﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace WebScraper
{
	class Program
	{
		static void Main(string[] args)
		{
			Program p = new Program();

			Console.WriteLine("Which site would you like to scrape? (include http://)");
			string siteToScrape = Console.ReadLine();

			Console.WriteLine("Would you lke to output in: \n 1) Txt File \n 2) Html");
			string outputType = Console.ReadLine();

			var responseFromServer = p.RequestWebsite(siteToScrape);

			if (outputType == "1")
			{
				p.PrintToTxtFile(responseFromServer);

			}
			else
			{
				p.PrintToHtmlFile(responseFromServer);

			}

			p.PullOutLinksFromGoogle();

			Console.ReadLine();
		}

		public string RequestWebsite(string url)
		{
			WebRequest request = WebRequest.Create(url);
			request.Credentials = CredentialCache.DefaultCredentials;
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Console.WriteLine(response.StatusDescription);
			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			Console.WriteLine(responseFromServer);
			reader.Close();
			dataStream.Close();
			response.Close();
			return responseFromServer;

		}
		private void PrintToTxtFile(string response)
		{
			File.WriteAllText(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webData.txt", response);
		}

		private void PrintToHtmlFile(string response)
		{
			File.WriteAllText(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webDataHTML.html", response);
		}

		private void PullOutLinksFromGoogle()
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
					var urlsExtracted = String.Concat(m.Groups[2].Value, m.Groups[3]);
					urlsExtracted = urlsExtracted.Replace("&amp", "");
					listOfSites.Add(urlsExtracted);
					Console.WriteLine(urlsExtracted);
					Console.WriteLine();
				}

				File.WriteAllLines(@"C:\Work\Training\WebScraper\Csharp-WebScraper\webDataListOfSites.txt", listOfSites);

			}
		}
	}
}
