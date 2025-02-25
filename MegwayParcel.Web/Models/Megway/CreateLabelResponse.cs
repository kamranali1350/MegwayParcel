using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticERP.Web.Models.Megway
{
    public class CreateLabelResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("tracking_codes")]
        public List<string> TrackingCodes { get; set; }

        [JsonPropertyName("tracking_urls")]
        public List<string> TrackingUrls { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("dc_request_id")]
        public int DcRequestId { get; set; }

        [JsonPropertyName("tracking_request_id")]
        public int TrackingRequestId { get; set; }

        [JsonPropertyName("tracking_request_hash")]
        public long TrackingRequestHash { get; set; }

        [JsonPropertyName("label_size")]
        public string LabelSize { get; set; }

        [JsonPropertyName("courier")]
        public string Courier { get; set; }

        [JsonPropertyName("dc_service_id")]
        public string DcServiceId { get; set; }
    }


}
