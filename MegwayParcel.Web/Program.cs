using MegwayParcel.Common.Services;
using MegwayParcel.Common.CommonServices;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.TakePaymentGateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<IGatewayHelper, GatewayHelper>();
builder.Services.AddHttpClient<CourierApiService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddTransient<IGatewayHelper, GatewayHelper>();
builder.Services.AddScoped<CommonFunctions>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<LogisticERPContext>();


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
app.UseSession();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
