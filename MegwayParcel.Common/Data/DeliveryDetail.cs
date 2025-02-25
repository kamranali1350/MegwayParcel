using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class DeliveryDetail
    {
        public int DeliveryDetailId { get; set; }
        public int? OrderId { get; set; }
        public bool IsResidentialAddress { get; set; }
        public bool IsSignatureRequired { get; set; }
        public string ReceiverTaxId { get; set; }

        public virtual Order Order { get; set; }
    }
}
