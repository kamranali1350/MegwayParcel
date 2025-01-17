using Microsoft.AspNetCore.Identity;

namespace MegwayParcel.Database.Data
{
	public class ApplicationRole : IdentityRole
	{
		public string Access { get; set; } =string.Empty;
	}
}
