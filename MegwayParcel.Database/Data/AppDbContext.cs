using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MegwayParcel.Database.Data
{
	
	public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
		//public DbSet<LandMarkCountries> LandMarkCountries { get; set; }
		//public DbSet<LandMarkPrices> LandMarkPrices { get; set; }
		//public DbSet<ServicesName> ServicesName { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
		}
	}
}
