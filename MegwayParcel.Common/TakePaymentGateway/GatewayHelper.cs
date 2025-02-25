using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TakepaymentsGateway;

namespace MegwayParcel.Common.TakePaymentGateway
{
    public class GatewayHelper : IGatewayHelper
    {
        private readonly HttpClient _httpClient;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public GatewayHelper(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpContextAccessor = httpContextAccessor;

        }


        public const string MMerchantID = "233743";
        public const string MerchantKey = "3HGiK1NSH9Q15I8h319c";
        public const string GatewayUrl = "https://gw1.tponlinepayments.com";
        public readonly string RedirectUrlPath = "/home/Response/";



        public async Task<HostedPaymentForm> Hosted(MakeOrderDTO dto)
        {
            HostedPaymentForm payment = new();
            payment.Amount = (float)dto.Amount;
            payment.OrderRef = dto.OrderRef;
            payment.TransactionUnique = dto.UniqueTransectionId;
            payment.RedirectUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + $"/PaymentResponse/{dto.OrderRef}/{dto.UniqueTransectionId}";
            payment.Signature = payment.SignTransaction();
            return payment;
        }


            public async Task<HttpStatusCode> PostDataAsync<T>(string path, T data)
        {
            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(data));
            using var httpResponse = await PostAsync(path, new FormUrlEncodedContent(content));
            return httpResponse.StatusCode;
        }


        private async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var httpResponse = await _httpClient.PostAsync(url, content);
            var response = httpResponse.Content.ReadAsStringAsync();
            return httpResponse;
        }
    }
}
