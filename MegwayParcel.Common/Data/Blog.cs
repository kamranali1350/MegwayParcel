using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public string ShortDetail { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FullDetail { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }

    public partial class Blog
    {
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
