using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuoineAPI.Lib
{
    public class Utility
    {
        public static HttpWebRequest Create(string url, string key, string content, string method = "GET")
        {
            var timestamp = DateTime.UtcNow;
            var request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
            request.KeepAlive = false;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = method;
            request.ContentType = "application/json";
            request.Date = new DateTime( 
                    timestamp.ToUniversalTime().Year,
                    timestamp.ToUniversalTime().Month,
                    timestamp.ToUniversalTime().Day,
                    timestamp.ToUniversalTime().Hour,
                    timestamp.ToUniversalTime().Minute,
                    timestamp.ToUniversalTime().Second,
                    DateTimeKind.Utc);
            request.Headers.Add("Content-MD5", Utility.GetMD5(string.Empty));
            request.Headers.Add("Authorization", Utility.GetAuthorization(key, Utility.GetMD5(content), timestamp.ToString("r"), url));
            return request;
        }

        public static string GetAPISecretKey()
        {
            return ConfigurationSettings.AppSettings["Quoine_API_Key"] ?? Guid.NewGuid().ToString();
        }

        public static string GetUserId()
        {
            return ConfigurationSettings.AppSettings["Quoine_API_UserId"] ?? Guid.NewGuid().ToString();
        }
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetMD5(string content)
        {
            // byte array representation of that string
            byte[] encodedString = new UTF8Encoding().GetBytes(content);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedString);

            return Convert.ToBase64String(hash);
        }

        private static string GetAuthorizationText(string hash, string userid)
        {
            var txt = string.Format("APIAuth {0}:{1}",userid, hash);
            return txt;
        }

        private static string GetInputString(string type, string contentMD5, string url, string date)
        {
            var action = new Uri(url).AbsolutePath;
            var cstr = string.Format("{0},{1},{2},{3}", type, contentMD5, action, date);
            return cstr;
        }

        /// <summary>
        /// <remarks>
        /// signature: a Base64 encoded SHA1 HMAC of carnonical_string
        /// canonical_string contains "content-type,content-MD5,request URI,date" e.g. "application/json,1B2M2Y8AsgTpgAmY7PhCfg==,/api/invoices,Sat, 27 Sep 2014 04:55:17 GMT"
        /// </remarks>
        /// </summary>
        /// <param name="content">Content-MD5 (optional) MD5 hash of the request content e.g. "34rc3n4fun489wfnw4urfw4381ddj91hwnv9usduvbsdfbvisenv"</param>
        /// <param name="date">Date - Timestamp of the request send time e.g. "Sat, 25 September 2014 08:00:01 GMT"</param>
        /// <param name="url">API URL e.g. https://pay.quoine.com/api/v1/invoices </param>
        /// <returns></returns>
        public static string GetAuthorization(string key, string content, string date, string url)
        {
            string encoded = string.Empty;
            byte[] hash = null;

            var data = Utility.GetInputString("application/json", Utility.GetMD5(content), url, date);
            try
            {
                // byte array representation of that string
                byte[] encodedString = new UTF8Encoding().GetBytes(data);

                //var hashedKey = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                var hashedKey = Encoding.UTF8.GetBytes(key);
                // Initialize the keyed hash object. 
                using (HMACSHA1 hmac = new HMACSHA1(hashedKey))
                {
                    hash = hmac.ComputeHash(encodedString);
                }
                Console.WriteLine(Convert.ToBase64String(hash));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while generating SHA1 hash:" + ex.Message);
                throw;
            }            // string representation 
            return Utility.GetAuthorizationText(Convert.ToBase64String(hash),GetUserId());
        }
    }
}
