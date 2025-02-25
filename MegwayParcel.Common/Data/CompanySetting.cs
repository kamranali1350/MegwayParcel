using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class CompanySetting
    {
        public CompanySetting()
        {
            Customers = new HashSet<Customer>();
        }

        public int CompanySettingId { get; set; }
        public bool IsActive { get; set; }
        public decimal InsuranceCharges { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal ShippingPercent { get; set; }
        public decimal SurchargePercent { get; set; }
        public decimal RemoteSurchargePercent { get; set; }
        public decimal VatchargePercent { get; set; }
        public string SettingName { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
