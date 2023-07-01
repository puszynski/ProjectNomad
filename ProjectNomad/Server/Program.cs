using AccountModule.Configuration;
using GameModule.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

AccountModuleConfiguration.DbContextConfiguration(
    builder.Services, 
    builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("No connection string provided :/"));

GameModuleConfiguration.DbContextConfiguration(builder.Services,
    builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("No connection string provided :/"));

AccountModuleConfiguration.RegisterIoC(builder.Services);
GameModuleConfiguration.RegisterIoC(builder.Services);//todo problems..

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
