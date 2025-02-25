using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Models
{
    public class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public int? Id { get; set; }
    }
}
