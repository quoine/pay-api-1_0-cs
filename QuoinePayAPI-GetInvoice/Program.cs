using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using QuoineAPI.Lib;

namespace QuoinePayAPI.GetInvoice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Common testing requirement. Accept any certificate presented by Server in Dev or Test environment. 
            // nb. Do NOT use this setting in production systems
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var key = Utility.GetAPISecretKey();
            string url = String.Format("https://pay.quoine.com/api/v1/invoices/{0}", "33");
            var request = Utility.Create(url, key, string.Empty);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(stream);
                    string contents = (String)readStream.ReadToEnd();
                    Console.WriteLine(contents);
                    dynamic json = System.Web.Helpers.Json.Decode(contents);
                    Console.ReadLine();
                }
            }
        }
    }
}
