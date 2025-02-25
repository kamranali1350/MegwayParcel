using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace MegwayParcel.Admin.Models.LandMarkViewModels
{
    public class LandMarkCountryViewModel
    {
        public int? CountryId { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string Name { get; set; }

        [DisplayName("Country ISO Like Pak")]
        public string ISO { get; set; }

        [DisplayName("Is Active?")]
        public bool IsActive { get; set; }

        [DisplayName("Enter Sort Order")]
        [Required]
        [Remote(action: "VerifySortOrder", controller: "LandMarkCountries", AdditionalFields = nameof(Name), ErrorMessage = "SortOrder already exists.")]
        public int SortOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
