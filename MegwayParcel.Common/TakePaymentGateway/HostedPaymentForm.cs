
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TakepaymentsGateway
{
    public class HostedPaymentForm
    {
        //Development ID
       // public string MerchantID { get; set; } = "234020";
        //Production ID
        public string MerchantID { get; set; } = "233743";


        public string RedirectUrl { get; set; } 


        public string ActionUrl { get; set; } = "https://gw1.tponlinepayments.com/paymentform/";


        public string TransactionType { get; set; } = "SALE";

        //Development Key
       // public string MerchantKey { get; set; } = "3HGiK1NSH9Q15I8h319c";
        //Production Key
        public string MerchantKey { get; set; } = "3HGiK1NSH9Q15I8h319c";


        public string MerchantPassword { get; set; } = "";


        public int Type { get; set; } = 1;


        public int CurrencyCode { get; set; } = 826;


        public int CountryCode { get; set; } = 826;


        public float Amount { get; set; }

        public string OrderRef { get; set; }

        public string TransactionUnique { get; set; }

        public string FormResponsive { get; set; } = "Y";


        public string RequestString { get; set; }

        public string StringToHash { get; set; }

        public string Signature { get; set; }

        public string SignTransaction()
        {
            SortedDictionary<string, string> obj = new SortedDictionary<string, string>
            {
                {
                    "merchantID",
                    MerchantID.ToString()
                },
                {
                    "action",
                    TransactionType.ToString()
                },
                {
                    "type",
                    Type.ToString()
                },
                {
                    "transactionUnique",
                    TransactionUnique.ToString()
                },
                {
                    "currencyCode",
                    CurrencyCode.ToString()
                },
                {
                    "amount",
                    Amount.ToString()
                },
                {
                    "orderRef",
                    OrderRef.ToString()
                },
                {
                    "formResponsive",
                    FormResponsive.ToString()
                },
                {
                    "countryCode",
                    CountryCode.ToString()
                },
                {
                    "redirectURL",
                    RedirectUrl.ToString()
                }
            };
            int num = 0;
            foreach (KeyValuePair<string, string> item in obj)
            {
                if (num > 0)
                {
                    RequestString += "&";
                }

                RequestString = RequestString + item.Key + "=" + UrlEncode(item.Value);
                num++;
            }

            Signature = BitConverter.ToString(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(Regex.Replace(RequestString + MerchantKey, "%0D%0A|%0A%0D|%0D", "%0A")))).Replace("-", "").ToLower();
            return Signature;
        }

        public string UrlEncode(string value)
        {
            return UpperCaseUrlEncode(value).Replace("!", "%21").Replace("*", "%2A").Replace("(", "%28")
                .Replace(")", "%29");
        }

        public string UpperCaseUrlEncode(string value)
        {
            char[] array = HttpUtility.UrlEncode(value).ToCharArray();
            for (int i = 0; i < array.Length - 2; i++)
            {
                if (array[i] == '%')
                {
                    array[i + 1] = char.ToUpper(array[i + 1]);
                    array[i + 2] = char.ToUpper(array[i + 2]);
                }
            }

            return new string(array);
        }
    }
}
#if false // Decompilation log
'35' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Web.dll'
------------------
Resolve: 'System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Configuration.dll'
------------------
Resolve: 'System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Web.ApplicationServices.dll'
#endif
