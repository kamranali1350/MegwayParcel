using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class RoleMenu
    {
        public int RoleMenuId { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Role Role { get; set; }
    }
}
