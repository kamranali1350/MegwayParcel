using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Models
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Document
    {
        public string DocumentType { get; set; }
        public string Format { get; set; }
        public string Content { get; set; }
        public string DownloadURL { get; set; }
    }

    public class InvoiceItem
    {
        public string ItemDescription { get; set; }
        public double AmountNet { get; set; }
    }

    public class Label
    {
        public string LabelRole { get; set; }
        public string LabelFormat { get; set; }
        public string LabelSize { get; set; }
        public string AirWaybillReference { get; set; }
        public string LabelContent { get; set; }
        public string DownloadURL { get; set; }
        public string CarrierName { get; set; }
        public string ServiceName { get; set; }
    }

    public class OrderInvoice
    {
        public object InvoiceReference { get; set; }
        public double TotalNet { get; set; }
        public double Tax { get; set; }
        public double TotalGross { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }

    public class BookShipmentOutput_DTO
    {
        public string Status { get; set; }
        public List<Notification> Notifications { get; set; }
        public string OrderReference { get; set; }
        public OrderInvoice OrderInvoice { get; set; }
        public List<Label> Labels { get; set; }
        public List<Document> Documents { get; set; }
        public string TrackingURL { get; set; }
    }
    public class Notification
    {
        public string Message { get; set; }
        public string Severity { get; set; }
    }

}
