﻿using System;
using System.IO;
using System.Text;
using System.Net;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            Console.WriteLine("Which site would you like to scrape? (include http://)");
            string siteToScrape = Console.ReadLine();

            var responseFromServer = p.RequestWebsite(siteToScrape);
            p.PrintToTxtFile(responseFromServer);

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
    }
}
