using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class GuestUser
    {
        public int GuestUserId { get; set; }
        public string Email { get; set; }
        public string EmailCode { get; set; }
        public DateTime? EmailExpiry { get; set; }
    }
}
