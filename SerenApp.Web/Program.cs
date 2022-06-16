using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SerenApp.Core.Interfaces;
using SerenApp.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var dbName = builder.Configuration.GetValue<string>("DatabaseName");

    if (builder.Environment.IsDevelopment()) options.UseInMemoryDatabase(dbName);
    else options.UseCosmos(builder.Configuration.GetConnectionString("Cosmos"), dbName);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
