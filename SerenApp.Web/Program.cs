using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SerenApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SerenApp.Web.Logic;
using SerenApp.Infrastructure.DAL.EntityFrameworkCore;
using SerenApp.Infrastructure.DAL.CosmosTableAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var dbName = builder.Configuration.GetValue<string>("DatabaseName");

    options.UseLazyLoadingProxies();
    if (builder.Environment.IsDevelopment()) options.UseInMemoryDatabase(dbName);
    else options.UseCosmos(builder.Configuration.GetConnectionString("Cosmos"), dbName);
});
builder.Services.AddScoped<TableDbContext>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AccountLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
