using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using QuoinePayAPI.Lib;

namespace QuoinePayAPI_Console
{
    /// <summary>
    /// Sanmple code for calling the Quoine API method to list invoices.
    /// Use the dynamic json object to inspect the invoices returned, a more detailed example is available in the GetInvoices project
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var url = Settings.BaseTestingURL + Settings.GetInvoicesURI;
            string key = Utility.GetAPISecretKey();
            var request = Utility.Create(url, key, string.Empty);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(stream);
                    string contents = readStream.ReadToEnd();
                    dynamic json = System.Web.Helpers.Json.Decode(contents);
                    Console.ReadLine();
                }
            }
        }
    }
}
