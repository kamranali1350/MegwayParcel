using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace MegwayParcel.Common.Data
{
    public class LandMarkPrices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PriceId { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; } = false;
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries LandMarkCountries { get; set; }
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public ServicesName ServicesName { get; set; }
    }
}
