using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProjectNomad.Client;
using ProjectNomad.Client.Logic;
using ProjectNomad.Client.Logic.GameLooperManagerLogic;
using ProjectNomad.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<TasksManager>();
builder.Services.AddScoped<LocalStorageNotificationsManager>();
builder.Services.AddScoped<GameLooperManager>();
builder.Services.AddScoped<TaskOrderManager>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddSingleton<EventBroadcastService>();

//3rd library - allow to store data in browser memory
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
