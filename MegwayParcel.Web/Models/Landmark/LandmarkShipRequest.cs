using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LogisticERP.Web.Models.Landmark
{
    [XmlRoot("ShipRequest")]
    public class LandmarkShipRequest
    {
        public Login Login { get; set; }
        public bool Test { get; set; }
        public string ClientID { get; set; }
        public string AccountNumber { get; set; }
        public string Reference { get; set; }
        public ShipTo ShipTo { get; set; }
        public ShippingLane ShippingLane { get; set; }
        public string ShipMethod { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal OrderInsuranceFreightTotal { get; set; }
        public decimal ShipmentInsuranceFreight { get; set; }
        public string ItemsCurrency { get; set; }
        public bool IsCommercialShipment { get; set; }
        public string LabelFormat { get; set; }
        public string LabelEncoding { get; set; }
        public VendorInformation VendorInformation { get; set; }

        [XmlArray("Packages")]
        [XmlArrayItem("Package")]
        public List<ShipPackage> Packages { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<Item> Items { get; set; }
    }

    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ShipTo
    {
        public string Name { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ConsigneeTaxID { get; set; }
    }

    public class ShippingLane
    {
        public string Region { get; set; }
    }

    public class VendorInformation
    {
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorCity { get; set; }
        public string VendorState { get; set; }
        public string VendorPostalCode { get; set; }
        public string VendorCountry { get; set; }
        public string VendorBusinessNumber { get; set; }
        public string VendorRGRNumber { get; set; }
        public string VendorIOSSNumber { get; set; }
        public string VendorEORINumber { get; set; }
    }

    public class ShipPackage
    {
        public string WeightUnit { get; set; }
        public decimal Weight { get; set; }
        public string DimensionsUnit { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string PackageReference { get; set; }
    }

    public class Item
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string HSCode { get; set; }
        public string CountryOfOrigin { get; set; }
    }

    [XmlRoot("TrackRequest")]
    public class TrackRequest
    {
        public Login Login { get; set; }
        public bool Test { get; set; }
        public int ClientID { get; set; }
        public string Reference { get; set; }
        public string TrackingNumber { get; set; }
        public string PackageReference { get; set; }
        public string RetrievalType { get; set; }
    }
}
/*
 using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LogisticERP.Web.Models.Landmark
{
    [XmlRoot("ShipRequest")]
    public class LandmarkShipRequest
    {
        public Login Login { get; set; } = new Login
        {
            Username = "Megway_Logistics_API",
            Password = "Kotli1977@pakistan"
        };

        public bool Test { get; set; } = false;
        public string ClientID { get; set; } = "2208";
        public string AccountNumber { get; set; } = "L2208A";
        public string Reference { get; set; } = "3245325";

        public ShipTo ShipTo { get; set; } = new ShipTo
        {
            Name = "Test Company",
            Attention = "Test Consignee",
            Address1 = "1234 Example Drive",
            Address2 = "Building #C",
            Address3 = "Unit 1",
            City = "London",
            State = string.Empty,
            PostalCode = "TW18 8EE",
            Country = "GB",
            Phone = "1-519-737-9101",
            Email = "orders@test.com",
            ConsigneeTaxID = "12345"
        };

        public ShippingLane ShippingLane { get; set; } = new ShippingLane
        {
            Region = "Client UK"
        };

        public string ShipMethod { get; set; } = "LGINTSTD";
        public decimal OrderTotal { get; set; } = 187.98m;
        public decimal OrderInsuranceFreightTotal { get; set; } = 20.65m;
        public decimal ShipmentInsuranceFreight { get; set; } = 20.65m;
        public string ItemsCurrency { get; set; } = "USD";
        public bool IsCommercialShipment { get; set; } = false;
        public string LabelFormat { get; set; } = "PDF";
        public string LabelEncoding { get; set; } = "LINKS";

        public VendorInformation VendorInformation { get; set; } = new VendorInformation
        {
            VendorName = "Test Company Legal Name",
            VendorPhone = "12223334444",
            VendorEmail = "contact@vendor.com",
            VendorAddress1 = "Sample Company Street",
            VendorAddress2 = "Suite 135",
            VendorCity = "Santa Barbara",
            VendorState = "CA",
            VendorPostalCode = "93101",
            VendorCountry = "US",
            VendorBusinessNumber = "12345",
            VendorRGRNumber = "123",
            VendorIOSSNumber = "IM1234567891",
            VendorEORINumber = "12345"
        };

        [XmlArray("Packages")]
        [XmlArrayItem("Package")]
        public List<ShipPackage> Packages { get; set; } = new List<ShipPackage>
        {
            new ShipPackage
            {
                WeightUnit = "LB",
                Weight = 4.5m,
                DimensionsUnit = "IN",
                Length = 12m,
                Width = 12m,
                Height = 12m,
                PackageReference = "98233312"
            }
        };

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<Item> Items { get; set; } = new List<Item>
        {
            new Item
            {
                Sku = "7224059",
                Quantity = 2,
                UnitPrice = 93.99m,
                Description = "Women's Shoes",
                HSCode = "640399.30.00",
                CountryOfOrigin = "CN"
            }
        };
    }

    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ShipTo
    {
        public string Name { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ConsigneeTaxID { get; set; }
    }

    public class ShippingLane
    {
        public string Region { get; set; }
    }

    public class VendorInformation
    {
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorCity { get; set; }
        public string VendorState { get; set; }
        public string VendorPostalCode { get; set; }
        public string VendorCountry { get; set; }
        public string VendorBusinessNumber { get; set; }
        public string VendorRGRNumber { get; set; }
        public string VendorIOSSNumber { get; set; }
        public string VendorEORINumber { get; set; }
    }

    public class ShipPackage
    {
        public string WeightUnit { get; set; }
        public decimal Weight { get; set; }
        public string DimensionsUnit { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string PackageReference { get; set; }
    }

    public class Item
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string HSCode { get; set; }
        public string CountryOfOrigin { get; set; }
    }
        
    [XmlRoot("TrackRequest")]
    public class TrackRequest
    {
        public Login Login { get; set; }
        public bool Test { get; set; }
        public int ClientID { get; set; }
        public string Reference { get; set; }
        public string TrackingNumber { get; set; }
        public string PackageReference { get; set; }
        public string RetrievalType { get; set; }
    }
}
 
 */