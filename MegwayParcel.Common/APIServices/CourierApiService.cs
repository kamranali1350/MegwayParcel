using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MegwayParcel.Common.Services
{
    public class CourierApiService
    {
        private readonly HttpClient _httpClient;

        public CourierApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // API for creating labels
        public async Task<TResponse> CreateLabelAsync<TRequest, TResponse>(TRequest requestModel)
        {
            var endpoint = "https://production.courierapi.co.uk/api/couriers/v1/MoovParcel/create-label";

            // Add headers specific to CreateLabel API
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-user", "Megway");
            _httpClient.DefaultRequestHeaders.Add("api-token", "tqlvkmzcsdgfhrbo");

            var jsonResponse = await PostAsync(endpoint, requestModel);

            // Deserialize JSON response to the provided response type
            var response = JsonSerializer.Deserialize<TResponse>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return response;
        }

        public async Task<string> CreateLabelAsyncgg<T>(T requestModel)
        {
            var endpoint = "https://production.courierapi.co.uk/api/couriers/v1/MoovParcel/create-label";

            // Add headers specific to CreateLabel API
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-user", "Megway");
            _httpClient.DefaultRequestHeaders.Add("api-token", "tqlvkmzcsdgfhrbo");

            return await PostAsync(endpoint, requestModel);
        }

        // API for getting a quote
        public async Task<string> GetQuoteAsync<T>(T requestModel)
        {
            var endpoint = "https://production.billingapi.co.uk/api/customer-routes/get-quote";

            // Add headers specific to GetQuote API
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("client_name", "Moov Parcel");
            _httpClient.DefaultRequestHeaders.Add("customer_dc_id", "HOF-0031");
            _httpClient.DefaultRequestHeaders.Add("customer_key", "d1f3fd86d746d35994d8ead6f744fd77");

            return await PostAsync(endpoint, requestModel);
        }

        // Generic POST method for sending requests
        private async Task<string> PostAsync<T>(string endpoint, T requestModel)
        {
            var payload = JsonSerializer.Serialize(requestModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);


            //if (!response.IsSuccessStatusCode)
            //{
            //    var error = await response.Content.ReadAsStringAsync();
            //    throw new Exception($"API call failed with status {response.StatusCode}: {error}");
            //}

            return await response.Content.ReadAsStringAsync();
        }
    }
}
