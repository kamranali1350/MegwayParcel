using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class ConsignmentSummary
    {
        public int ConsignmentSummaryId { get; set; }
        public int OrderId { get; set; }
        public bool IsCustomsInvoice { get; set; }
        public string ContentSummary { get; set; }
        public string ReasonForShipment { get; set; }
        public decimal? TotalGoodsValue { get; set; }
        public decimal? ShippingCharges { get; set; }
        public decimal? TotalValueForCustoms { get; set; }
        public string DocumentUrl { get; set; }
        public string DocumentCategory { get; set; }

        public virtual Order Order { get; set; }
    }
}
