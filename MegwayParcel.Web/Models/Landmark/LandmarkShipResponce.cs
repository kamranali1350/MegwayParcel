using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LogisticERP.Web.Models.Landmark
{
    // For response of landmark API
    #region Landmark API response
    [XmlRoot(ElementName = "ShipResponse", Namespace = "")]
    public class LandmarkShipResponse
    {
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [XmlElement(ElementName = "Success")]
        public bool Success { get; set; }

        [XmlElement(ElementName = "ResultMessage")]
        public string ResultMessage { get; set; }

        [XmlElement(ElementName = "ShipmentLabelLink")]
        public string ShipmentLabelLink { get; set; }

        [XmlArray(ElementName = "Packages")]
        [XmlArrayItem(ElementName = "Package")]
        public List<ResultPackage> Packages { get; set; }
    }

    public class ResultPackage
    {
        [XmlElement(ElementName = "LabelLink")]
        public string LabelLink { get; set; }

        [XmlElement(ElementName = "TrackingNumber")]
        public string TrackingNumber { get; set; }

        [XmlElement(ElementName = "LandmarkTrackingNumber")]
        public string LandmarkTrackingNumber { get; set; }

        [XmlElement(ElementName = "PackageID")]
        public string PackageID { get; set; }

        [XmlElement(ElementName = "BarcodeData")]
        public string BarcodeData { get; set; }

        [XmlElement(ElementName = "PackageReference")]
        public string PackageReference { get; set; }
    }
    #endregion

    #region Track Response
    [XmlRoot(ElementName = "TrackResponse")]
    public class TrackResponse
    {
        [XmlArray(ElementName = "Errors")]
        [XmlArrayItem(ElementName = "Error")]
        public List<Error> Errors { get; set; }

        [XmlElement(ElementName = "TrackResult")]
        public TrackResult TrackResult { get; set; }
    }

    public class Error
    {
        [XmlElement(ElementName = "ErrorCode")]
        public string ErrorCode { get; set; }

        [XmlElement(ElementName = "ErrorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class TrackResult
    {
        [XmlElement(ElementName = "Success")]
        public bool Success { get; set; }

        [XmlElement(ElementName = "ShipmentDetails")]
        public ShipmentDetails ShipmentDetails { get; set; }

        [XmlArray(ElementName = "Packages")]
        [XmlArrayItem(ElementName = "Package")]
        public List<Package> Packages { get; set; }
    }

    public class ShipmentDetails
    {
        [XmlElement(ElementName = "EndDeliveryCarrier")]
        public string EndDeliveryCarrier { get; set; }
    }

    public class Package
    {
        [XmlElement(ElementName = "TrackingNumber")]
        public string TrackingNumber { get; set; }

        [XmlElement(ElementName = "LandmarkTrackingNumber")]
        public string LandmarkTrackingNumber { get; set; }

        [XmlElement(ElementName = "PackageReference")]
        public string PackageReference { get; set; }

        [XmlArray(ElementName = "Events")]
        [XmlArrayItem(ElementName = "Event")]
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "DateTime")]
        public DateTime DateTime { get; set; }

        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }

        [XmlElement(ElementName = "EventCode")]
        public string EventCode { get; set; }
    }
    #endregion
}
