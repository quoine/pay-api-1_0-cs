using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using QuoinePayAPI.Lib;

namespace QuoinePayAPI.UpdateCallbackURL
{
    class Program
    {
        static void Main(string[] args)
        {
            // Do NOT use this setting in production systems
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
  
            var url = Settings.BaseTestingURL + Settings.SetPaymentsCallbackURI;

            string key = Utility.GetAPISecretKey();

            string data = new JavaScriptSerializer().Serialize(new
            {
                    callback_url = "http://test/this"
            });

            var request = Utility.Create(url, key, data, "POST");
  
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                    dynamic json = System.Web.Helpers.Json.Decode(result);
                    Console.WriteLine(json.status);
                    Console.ReadLine();
                }
            }
        }
    }
}
