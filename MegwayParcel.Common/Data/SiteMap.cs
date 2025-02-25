using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class SiteMap
    {
        public int SiteMapId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? SortOrder { get; set; }
        public string Detail { get; set; }
    }
}
