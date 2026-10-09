using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuctionLab.Data;
using AuctionLab.Web.Data;
using AuctionLab.Business.Interfaces;
using AuctionLab.Business.Services;
using AuctionLab.Data.Interfaces;
using AuctionLab.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AuctionDb")));


builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppIdentityDbContext>();


builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();


builder.Services.AddScoped<IAuctionService, AuctionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auctions}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
