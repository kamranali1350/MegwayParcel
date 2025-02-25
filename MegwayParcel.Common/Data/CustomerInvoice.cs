using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class CustomerInvoice
    {
        public int CustomerInvoiceId { get; set; }
        public int? OrderId { get; set; }
        public string CommodityCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal? ValueForCustoms { get; set; }
        public decimal? TotalGoodsValue { get; set; }
        public decimal? ShippingCharges { get; set; }
        public decimal? TotalValueForCustoms { get; set; }
        public bool IsAgree { get; set; }
        public int? CountryId { get; set; }
        public int? NoOfItems { get; set; }
        public decimal? PerItemValue { get; set; }
        public decimal? PerItemWeight { get; set; }

        public virtual Order Order { get; set; }
    }
}
