
using Microsoft.AspNetCore.Identity;

namespace MegwayParcel.Database.Data
{
	public class ApplicationUser : IdentityUser
	{
		public override string Id { get { return base.Id; } }
		public string? Name { get; set; }
		public bool? IsActive { get; set; } = false;
		public bool? IsDeleted { get; set; }=false;
		public bool? IsBlocked { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime? DeletedAt { get; set; } 
	}
}
