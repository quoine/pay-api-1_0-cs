using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            // Common testing requirement. Accept any certificate presented by Server in Dev or Test environment. 
            // Do NOT use this setting in production systems
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var request = System.Net.WebRequest.Create("https://pay.quoine.com/api/api_secret_key/") as System.Net.HttpWebRequest;
            request.KeepAlive = false;

            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string data = new JavaScriptSerializer().Serialize(new
                {
                    email = "tinwald@gmail.com",
                    password = "Password88"
                });

                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic json = System.Web.Helpers.Json.Decode(result);
                    Console.WriteLine("Request status: " + json.status);
                    Console.WriteLine("API Secret Key: " + json.api_secret_key);
                    Console.ReadLine();
                }
            }
        }
    }
}
