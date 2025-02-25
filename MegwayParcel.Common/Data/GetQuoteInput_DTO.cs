using System.Collections.Generic;

namespace MegwayParcel.Common.Data
{
    public class GetQuoteInput_DTO
    {
        public Credentials Credentials { get; set; }
        public Shipment Shipment { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CollectionAddress
    {
        public string Postcode { get; set; }
        public Country1 Country { get; set; }
        public string CountryName { get; set; }
    }

    public class Consignment
    {
        public string ItemType { get; set; }
        public List<Package> Packages { get; set; }
    }

    public class Country1
    {
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
    }

    //public class Credentials
    //{
    //    public string APIKey { get; set; }
    //    public string Password { get; set; }
    //}

    public class DeliveryAddress
    {
        public string Postcode { get; set; }
        public Country1 Country { get; set; }
        public string CountryName { get; set; }
    }

    public class Package
    {
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }



    public class Shipment
    {
        public Consignment Consignment { get; set; }
        public CollectionAddress CollectionAddress { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public bool IsSameCountry { get; set; }
		public string? ServiceCode { get; set; }
	}
}
