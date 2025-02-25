
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Serialization;
using RestSharp;

namespace MegwayParcel.Common.Services
{
    public interface IShippingService
    {
        Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, string endpoint)
            where TRequest : class
            where TResponse : class;
    }

    public class ShippingService : IShippingService
    {
        private readonly RestClient _client;
        private readonly ILogger<ShippingService> _logger;

        public ShippingService(ILogger<ShippingService> logger)
        {
            var options = new RestClientOptions("https://api.landmarkglobal.com")
            {
                MaxTimeout = -1,
            };
            _client = new RestClient(options);
            _logger = logger;
        }

        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, string endpoint)
            where TRequest : class
            where TResponse : class
             {
            try
            {
                var restRequest = new RestRequest(endpoint, Method.Post);
                restRequest.AddHeader("Content-Type", "application/xml");

                // Serialize the request to XML
                var xmlBody = SerializeWithoutXmlHeader(request);
                restRequest.AddParameter("application/xml", xmlBody, ParameterType.RequestBody);

                // Execute the request
                RestResponse response = await _client.ExecuteAsync(restRequest);

                // Check for unsuccessful response
                if (!response.IsSuccessful)
                {
                    _logger.LogError("Error from API: {StatusCode} - {Content}", response.StatusCode, response.Content);
                    throw new ExternalServiceException($"Error from API: {response.StatusCode} - {response.Content}");
                }

                // Deserialize the response content into the specified type
                var responseObject = DeserializeXml<TResponse>(response.Content);
                return responseObject;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the request.");
                throw new ServiceException("An error occurred while sending the request.", ex);
            }
        }

        // Method to deserialize XML content to the specified object type
        private static T DeserializeXml<T>(string xmlContent)
        {
            var serializer = new XmlSerializer(typeof(T));
            try
            {
                using (var stringReader = new System.IO.StringReader(xmlContent))
                using (var reader = new System.Xml.XmlTextReader(stringReader))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while deserializing the XML response.", ex);
            }
        }

        // Method to serialize an object to XML string without the XML header
        private static string SerializeWithoutXmlHeader<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
    }

    // Custom StringWriter with Encoding
    public class StringWriterWithEncoding : System.IO.StringWriter
    {
        private readonly Encoding _encoding;
        public StringWriterWithEncoding(Encoding encoding) => _encoding = encoding;
        public override Encoding Encoding => _encoding;
    }

    // Custom Exceptions
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message) { }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
