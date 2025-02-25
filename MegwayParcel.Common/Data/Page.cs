using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class Page
    {
        public int PageId { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public string Detail { get; set; }
        public int? ParentId { get; set; }
        public int? SortOrder { get; set; }
    }
}
