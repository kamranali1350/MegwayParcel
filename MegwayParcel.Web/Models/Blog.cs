using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MegwayParcel.Web.Models
{
    public partial class Blog
    {
        [NotMapped] 
        public IFormFile Image { get; set; }
    }
}
