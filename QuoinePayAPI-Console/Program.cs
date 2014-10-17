using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoineAPI.Lib;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace QuoinePayAPI_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "https://pay.quoine.com/api/v1/invoices";
            var request = Utility.Create(url, Utility.GetAPISecretKey(), string.Empty);

            try
            {
                Console.WriteLine("Request Date: " + request.Date.ToString());
                Console.WriteLine("Request Authorization: " + request.Headers["Authorization"].ToString());
                using (WebResponse response = request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        dynamic json = System.Web.Helpers.Json.Decode(result);
                        Console.WriteLine("Request status: " + json.status);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}
