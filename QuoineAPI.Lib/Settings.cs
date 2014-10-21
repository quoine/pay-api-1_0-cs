using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoinePayAPI.Lib
{
    public class Settings
    {
	    // Base URL for API calls
        public static string BaseProductionURL { get { return "https://pay.quoine.com"; } }
        public static string BaseTestingURL { get { return "https://pay-stag.quoine.com"; } }
        public static string ContentType { get { return "application/json"; } }
        public static string UserAgent { get { return "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)"; } }

	    // URI parts for calling API - to be added to BaseURL per call
        public static string GetAPIKeyURI { get { return "/api/v1/api_secret_key/"; } }        // [GET] 				
        public static string NewInvoiceURI { get { return "/api/v1/invoices"; } }	            // [POST] 				
        public static string GetInvoiceURI { get { return "/api/v1/invoices"; } } 		        // [GET]				
        public static string GetInvoicesURI { get { return "/api/v1/invoices"; } }		        // [GET]				
        public static string GetAccountURI              { get { return "/api/v1/account";}}		        // [GET] 				
        public static string SetPaymentsCallbackURI { get { return "/api/v1/payments_callback_url"; } }	// [POST] 	
    }
}
