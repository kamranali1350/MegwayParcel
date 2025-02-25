using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class Order
    {
        public Order()
        {
            Addresses = new HashSet<Address>();
            ConsignmentSummaries = new HashSet<ConsignmentSummary>();
            CustomerInvoices = new HashSet<CustomerInvoice>();
            DeliveryDetails = new HashSet<DeliveryDetail>();
            ExporterDetails = new HashSet<ExporterDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? CollectionDate { get; set; }
        public string CollectionTime { get; set; }
        public string RequestQuote { get; set; }
        public string SelectQuote { get; set; }
        public bool? IsAddCart { get; set; }
        public bool? IsRemoveFromBasket { get; set; }
        public int? CustomerId { get; set; }
        public int? CollectionOptionId { get; set; }
        public string OrderResponse { get; set; }
        public decimal InsuranceValue { get; set; }
        public decimal InsuranceCharges { get; set; }
        public decimal CollectionCharges { get; set; }
        public decimal ShippingCharges { get; set; }
        public decimal Surcharges { get; set; }
        public decimal RemoteSurcharges { get; set; }
        public decimal Vatcharges { get; set; }
        public decimal TotalCharges { get; set; }
        public string ErrorResponse { get; set; }
        public string RefrenceNo { get; set; }
        public bool IsPaymentReceived { get; set; }
        public string PaymentRefrenceNo { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsSameCountry { get; set; }
        public string PaymentResponse { get; set; }
        public string SelectedCompanyLogo { get; set; }
        public decimal SignaturePrice { get; set; }
        public bool? IsCollectionEmailSent { get; set; }
        public string CollectionEmailError { get; set; }
        public bool? IsDeliveryEmailSent { get; set; }
        public string DeliveryEmailError { get; set; }
        public bool? IsInvoiceEmailSent { get; set; }
        public string InvoiceEmailError { get; set; }
        public string AirWaybillReference { get; set; }
        public decimal? ResidentialSurcharge { get; set; }
        public string OrderEmail { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceCode { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<ConsignmentSummary> ConsignmentSummaries { get; set; }
        public virtual ICollection<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; }
        public virtual ICollection<ExporterDetail> ExporterDetails { get; set; }
    }
}
