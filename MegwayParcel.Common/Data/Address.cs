using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int OrderId { get; set; }
        public int? TypeId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyName { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public bool IsAddressResidential { get; set; }
        public string Eorinumber { get; set; }
        public string Country { get; set; }

        public virtual Order Order { get; set; }
    }
}
