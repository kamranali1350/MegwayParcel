
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MegwayParcel.Common.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		public override string Id { get { return base.Id; } }
		public string? Name { get; set; }
		public bool? IsActive { get; set; }
	}
}
