using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class ExporterDetail
    {
        public int ExporterDetailId { get; set; }
        public int? OrderId { get; set; }
        public bool IsCommercialShipmet { get; set; }
        public bool IsExporterAddressAsCollectionAddress { get; set; }
        public string ExporterEorino { get; set; }
        public string ExporterVatno { get; set; }
        public string IossNumber { get; set; }

        public virtual Order Order { get; set; }
    }
}
