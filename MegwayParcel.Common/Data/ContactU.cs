using System;
using System.Collections.Generic;

#nullable disable

namespace MegwayParcel.Common.Data
{
    public partial class ContactU
    {
        public int ContactUsId { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
