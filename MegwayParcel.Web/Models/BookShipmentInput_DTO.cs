using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BookAccessory
    {
        public string Code { get; set; }
    }

    public class BookDetails
    {
        public int ServiceID { get; set; }
        public string YourReference { get; set; }
        public string VATNumber { get; set; }
        public string EORINumber { get; set; }
        public string IMEINumber { get; set; }
        public Collection Collection { get; set; }
        public List<BookAccessory> BookAccessories { get; set; }
        public Insurance Insurance { get; set; }
    }

    public class Collection
    {
        public string CollectionDate { get; set; }
        public string ReadyFrom { get; set; }
        public int CollectionOptionID { get; set; }
    }

    public class BookCollectionAddress
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public Country Country { get; set; }
        public bool IsAddressResidential { get; set; }
        public string EORINumber { get; set; }
    }

    public class CommodityDetail
    {
        public string CommodityCode { get; set; }
        public string CommodityDescription { get; set; }
        public CountryOfOrigin CountryOfOrigin { get; set; }
        public int NumberOfUnits { get; set; }
        public double UnitValue { get; set; }
        public double UnitWeight { get; set; }
    }

    public class BookConsignment
    {
        public string ItemType { get; set; }
        public bool ItemsAreStackable { get; set; }
        public string ConsignmentSummary { get; set; }
        public double ConsignmentValue { get; set; }
        public List<BookPackage> Packages { get; set; }
    }

    public class BookCountry
    {
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
    }

    public class CountryOfOrigin
    {
        public string CountryCode { get; set; }
        public int? CountryID { get; set; }
    }

    public class BookCredentials
    {
        public string APIKey { get; set; }
        public string Password { get; set; }
    }

    public class BookDeliveryAddress
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public BookCountry Country { get; set; }
        public bool IsAddressResidential { get; set; }
        public string EORINumber { get; set; }
    }

    public class Insurance
    {
        public double CoverValue { get; set; }
        public double ExcessValue { get; set; }
        public bool GoodsAreNew { get; set; }
        public bool GoodsAreFragile { get; set; }
    }

    public class BookPackage
    {
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<CommodityDetail> CommodityDetails { get; set; }
    }

    public class BookShipmentInput_DTO
    {
        public BookCredentials Credentials { get; set; }
        public BookShipment Shipment { get; set; }
        public BookDetails BookDetails { get; set; }
    }

    public class BookShipment
    {
        public BookConsignment Consignment { get; set; }
        public BookCollectionAddress CollectionAddress { get; set; }
        public BookDeliveryAddress DeliveryAddress { get; set; }
    }


}
