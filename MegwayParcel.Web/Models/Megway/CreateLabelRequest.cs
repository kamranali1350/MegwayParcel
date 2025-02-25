using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LogisticERP.Web.Models.Megway
{
    [XmlRoot("CreateLabelRequest")]
    public class CreateLabelRequest
    {
        public string auth_company { get; set; }
        public bool format_address_default { get; set; }
        public string request_id { get; set; }

        [XmlElement("shipment")]
        public CreateLabelRequestShipment Shipment { get; set; }
    }

    public class CreateLabelRequestShipment
    {
        public string label_size { get; set; }
        public string label_format { get; set; }
        public bool generate_invoice { get; set; }
        public bool generate_packing_slip { get; set; }
        public CreateLabelRequestCourier courier { get; set; }
        public DateTime collection_date { get; set; }
        public string dc_service_id { get; set; }
        public string reference { get; set; }
        public string reference_2 { get; set; }
        public string delivery_instructions { get; set; }

        [XmlElement("ship_from")]
        public CreateLabelRequestShipFrom ship_from { get; set; }

        [XmlElement("ship_to")]
        public CreateLabelRequestShipTo ship_to { get; set; }

        [XmlArray("parcels")]
        [XmlArrayItem("parcel")]
        public List<CreateLabelRequestParcel> parcels { get; set; }
    }

    public class CreateLabelRequestCourier
    {
        public string auth_company { get; set; }
    }

    public class CreateLabelRequestShipFrom
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string company_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string address_3 { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string country_iso { get; set; }
        public string company_id { get; set; }
        public string tax_id { get; set; }
        public string eori_id { get; set; }
        public string ioss_number { get; set; }
    }

    public class CreateLabelRequestShipTo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string company_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string address_3 { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string country_iso { get; set; }
        public string tax_id { get; set; }
    }

    public class CreateLabelRequestParcel
    {
        public int dim_width { get; set; }
        public int dim_height { get; set; }
        public int dim_length { get; set; }
        public string dim_unit { get; set; }

        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<CreateLabelRequestItem> items { get; set; }
    }

    public class CreateLabelRequestItem
    {
        public string description { get; set; }
        public string origin_country { get; set; }
        public int    quantity { get; set; }
        public string value_currency { get; set; }
        public double weight { get; set; }
        public string weight_unit { get; set; }
        public string sku { get; set; }
        public string hs_code { get; set; }
        public string value { get; set; }
        public string extended_description { get; set; }
    }
}
