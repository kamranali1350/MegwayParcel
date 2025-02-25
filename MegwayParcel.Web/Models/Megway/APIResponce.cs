using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticERP.Web.Models.Megway
{
    
        public class Price
        {
            public List<decimal> Shipping { get; set; }
            public List<decimal> Surcharges { get; set; }
            public decimal Picking { get; set; }
            public decimal Return { get; set; }
            public List<List<string>> ParcelRules { get; set; }
            public List<decimal> PackagingCharges { get; set; }
            public decimal Total { get; set; }
        }

        public class CourierService
        {
            public string service_code { get; set; }
            public string service_name { get; set; }
            public string courier { get; set; }
            public Price Price { get; set; }
        }

    
}
