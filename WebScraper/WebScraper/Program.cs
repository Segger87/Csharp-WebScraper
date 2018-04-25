using System;
using System.IO;
using System.Text;
using System.Net;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://engineerverse.com/");

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
            Console.ReadLine();
        }
    }
}
