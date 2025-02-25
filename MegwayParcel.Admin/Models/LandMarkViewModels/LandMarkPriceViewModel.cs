using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace MegwayParcel.Admin.Models.LandMarkViewModels
{
    public class LandMarkPriceViewModel
    {

        public int? PriceId { get; set; }

        [DisplayName("Enter Weight in KG")]
        [Required]
        public double Weight { get; set; }

        [DisplayName("Enter Price")]
        [Required]
        public double Price { get; set; }

        [DisplayName("Is Active?")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public int CountryId { get; set; }

        [DisplayName("Enter Sort Order")]
        public int SortOrder { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
