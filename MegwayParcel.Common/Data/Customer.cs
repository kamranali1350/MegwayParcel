using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string BusinessType { get; set; }
        public string AcountName { get; set; }
        public string Title { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNo { get; set; }
        public string AddTitle { get; set; }
        public string AddForeName { get; set; }
        public string AddSurName { get; set; }
        public string AddLine1 { get; set; }
        public string AddLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public bool IsResidentialAddress { get; set; }
        public bool IsVatRegistered { get; set; }
        public string MyVatNumber { get; set; }
        public string Eorinumber { get; set; }
        public bool IsShippingOrder { get; set; }
        public bool IsNewsOffers { get; set; }
        public int? CompanySettingId { get; set; }
        public string EmailCode { get; set; }
        public DateTime? EmailExpiry { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool IsWithoutPayment { get; set; }
        public string DefForename { get; set; }
        public string DefSurname { get; set; }
        public string DefEmailAddress { get; set; }
        public string DefTelephoneNumber { get; set; }
        public string DefMobileNumber { get; set; }
        public string DefCompanyName { get; set; }
        public string DefAddressLineOne { get; set; }
        public string DefAddressLineTwo { get; set; }
        public string DefCity { get; set; }
        public string DefCounty { get; set; }
        public string DefPostcode { get; set; }
        public string DefTitle { get; set; }
        public string IossNumber { get; set; }

        public virtual CompanySetting CompanySetting { get; set; }
    }


    public partial class Customer
    {
        [NotMapped]
        public GuestUser GuestUser { get; set; }
        [NotMapped]
        public string ActiveMenu { get; set; }
    }
}
