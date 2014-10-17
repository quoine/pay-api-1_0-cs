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

namespace QuoinePayAPI.GetInvoices
{
    class Program
    {
        static void Main(string[] args)
        {
            // Common testing requirement. Accept any certificate presented by Server in Dev or Test environment. 
            // Do NOT use this setting in production systems
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string url = "https://pay.quoine.com/api/invoices/";
            string key = Utility.GetAPISecretKey();
            var request = Utility.Create(url, key,string.Empty);
 
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(stream);
                    string contents = readStream.ReadToEnd();
                    dynamic json = System.Web.Helpers.Json.Decode(contents);
                    //Console.WriteLine(json.Name);
                    Console.ReadLine();
                }
            }
        }
    }
}
