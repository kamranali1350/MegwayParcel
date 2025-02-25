using MegwayParcel.Admin.Filters;
using MegwayParcel.Admin.Models;
using MegwayParcel.Admin.Services;
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
//google login
builder.Services.AddAuthentication()
.AddGoogle(options =>
{
#pragma warning disable CS8601 // Possible null reference assignment.
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
#pragma warning restore CS8601 // Possible null reference assignment.
});
//services.AddSession();
builder.Services.AddSession(options =>
{
    // Set the session timeout to 30 minutes
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddDbContext<LogisticERPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Add Razor Pages services

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<LogisticERPContext>()
    .AddDefaultTokenProviders();

// Add application services.
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IMvcControllerDiscovery, MvcControllerDiscovery>();
builder.Services.AddSingleton(new DynamicAuthorizationOptions { DefaultAdminUser = "test123@yahoo.com" });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(DynamicAuthorizationFilter));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
