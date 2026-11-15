using IPFront;
using IPFront.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is missing in appsettings.json");

// Register Typed HttpClients pointing to your saxafon API
builder.Services.AddHttpClient<UserApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});
await builder.Build().RunAsync();
