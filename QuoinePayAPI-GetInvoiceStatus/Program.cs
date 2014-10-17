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

namespace QuoinePayAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string sampleUserId = "59";           
            string url = string.Format( "https://pay.quoine.com/api/invoices/(0)/status", "9");
            string key = Utility.GetAPISecretKey();

            // Common testing requirement. Accept any certificate presented by Server in Dev or Test environment. 
            // Do NOT use this setting in production systems
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
            request.KeepAlive = false;

            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Date = DateTime.Now;
            request.Headers.Add("Content-MD5", Utility.GetMD5(string.Empty));
            request.Headers.Add("Authorization", Utility.GetAuthorization(key, Utility.GetMD5("BODY CONTENT HERE"), DateTime.Now.ToString("r"), url, sampleUserId));

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(stream);
                    string contents = readStream.ReadToEnd();
                    Console.WriteLine(contents);
                    //dynamic json = System.Web.Helpers.Json.Decode(contents);
                    //Console.WriteLine(json.Name);
                    Console.ReadLine();
                }
            }
        }
    }
}
