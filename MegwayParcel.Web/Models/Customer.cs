using MegwayParcel.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MegwayParcel.Web.Models
{
    public partial class Customer
    {
        [NotMapped]
        public GuestUser GuestUser { get; set; }
        [NotMapped]
        public string ActiveMenu { get; set; }
    }
}
