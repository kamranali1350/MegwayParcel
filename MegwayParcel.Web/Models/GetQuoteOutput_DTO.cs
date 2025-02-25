using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AvailableSize
    {
        public string Size { get; set; }
    }

    public class CollectionOption
    {
        public int CollectionOptionID { get; set; }
        public string CollectionOptionTitle { get; set; }
        public double CollectionCharge { get; set; }
        public string SameDayCollectionCutOffTime { get; set; }
        public ExpectedLabel ExpectedLabel { get; set; }
    }

    public class ExpectedLabel
    {
        public string LabelRole { get; set; }
        public string LabelFormat { get; set; }
        public string LabelGenerateStatus { get; set; }
        public List<AvailableSize> AvailableSizes { get; set; }
    }

    public class ExpectedLabel2
    {
        public string LabelRole { get; set; }
        public string LabelFormat { get; set; }
        public string LabelGenerateStatus { get; set; }
        public List<AvailableSize> AvailableSizes { get; set; }
    }

    public class OptionalExtra
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
    }

    public class GetQuoteOutput_DTO
    {
        public string Status { get; set; }
        public List<ErrorObject> Notifications { get; set; }
        public int QuoteID { get; set; }
        public List<ServiceResult> ServiceResults { get; set; }
    }
    public class ErrorObject
    {
        public string Message { get; set; }
        public string Severity { get; set; }
    }

    public class ServicePriceBreakdown
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
    }

    public class ServiceResult
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string CarrierName { get; set; }
        public double ChargeableWeight { get; set; }
        public string TransitTimeEstimate { get; set; }
        public bool IsWarehouseService { get; set; }
        public TotalCost TotalCost { get; set; }
        public List<ServicePriceBreakdown> ServicePriceBreakdown { get; set; }
        public List<OptionalExtra> OptionalExtras { get; set; }
        public bool SignatureRequiredAvailable { get; set; }
        public List<ExpectedLabel> ExpectedLabels { get; set; }
        public List<CollectionOption> CollectionOptions { get; set; }
        public string ServiceType { get; set; }
        public string SameDayCollectionCutOffTime { get; set; }
    }

    public class TotalCost
    {
        public double TotalCostNetWithCollection { get; set; }
        public double TotalCostNetWithoutCollection { get; set; }
        public double TotalCostGrossWithCollection { get; set; }
        public double TotalCostGrossWithoutCollection { get; set; }
    }


}
