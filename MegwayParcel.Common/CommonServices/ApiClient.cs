using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MegwayParcel.Common.CommonServices
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerSettings _jsonSerializerSettings;
        private readonly string _apiUser;
        private readonly string _apiToken;
        public ApiClient(string baseUrl,  string apiUser = null, string apiToken = null)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiToken = apiToken;
            _apiUser = apiUser;
        }
        public async Task<string> GetAsync(string uri)
        {
            AddHeaders();
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<TEntity> GetAsync<TEntity>(string uri)
        {
            try
            {
                var stringData = await GetAsync(uri);
                return JsonConvert.DeserializeObject<TEntity>(stringData, SerializerSettings);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deserializing the response.", ex);
            }
        }
        public async Task<HttpResponseMessage> PostAsync(string uri, object model)
        {
            AddHeaders();
            var stringData = JsonConvert.SerializeObject(model, SerializerSettings);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(uri, contentData);
        }
        public async Task<TEntity> PostAsync<TEntity>(string uri, object model)
        {
            try
            {
                var response = await PostAsync(uri, model);
                //response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TEntity>(responseData, SerializerSettings);

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        private void AddHeaders()
        {
            if (!string.IsNullOrEmpty(_apiToken))
            {
                _httpClient.DefaultRequestHeaders.Add("api-token", _apiToken);
            }

            if (!string.IsNullOrEmpty(_apiUser))
            {
                _httpClient.DefaultRequestHeaders.Add("api-user", _apiUser);
            }
        }
        private JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (_jsonSerializerSettings == null)
                {
                    _jsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver(),
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    };
                }
                return _jsonSerializerSettings;
            }
        }
    }
   public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value, SerializerSettings));
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value, SerializerSettings);
        }
        public static bool IsSessionExpire(this ISession session, string key)
        {
            var value = session.GetString(key);
            return string.IsNullOrEmpty(value);
        }
        private static JsonSerializerSettings _jsonSerializerSettings = null;
        private static JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (_jsonSerializerSettings == null)
                {
                    _jsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver(),
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    };
                }
                return _jsonSerializerSettings;
            }
        }
    }
}
