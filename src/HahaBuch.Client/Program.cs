using HahaBuch.Client;
using HahaBuch.Client.Services;
using HahaBuch.Client.Transaction;
using HahaBuch.SharedContracts;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HahaBuch.Client.Analysis;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();
builder.Services.AddScoped<ICategoryService, ClientCategoryService>();
builder.Services.AddScoped<ITransactionService, ClientTransactionService>();
builder.Services.AddScoped<IAnalysisService, ClientAnalysisService>(); // register client analysis service
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();

await builder.Build().RunAsync();