using BlazorApp.Components;
using BlazorApp.Services;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FIX:
// 1. Remove manual HttpClient creation and use the standard AddHttpClient.
// 2. We register IPostService and IUserService to use a named HttpClient configuration.

builder.Services.AddHttpClient<IUserService, HttpUserService>(client =>
{
    // Configure the base address for the UserService
    client.BaseAddress = new Uri("https://localhost:7243");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // FIX: This is the SSL bypass, applied correctly within the handler configuration.
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddHttpClient<IPostService, HttpPostService>(client =>
{
    // Configure the base address for the PostService
    client.BaseAddress = new Uri("https://localhost:7243");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // FIX: Apply the SSL bypass to the PostService as well
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// IMPORTANT: We no longer need separate singleton/scoped registrations for IUserService/IPostService
// because AddHttpClient<TInterface, TImplementation> handles both the DI registration
// AND the HttpClient creation/injection.

// Ensure ICommentService is registered if you have one (or remove if unused)
// If ICommentService is implemented with an HttpCommentService, it should use the same pattern.
// Assuming for now it's correctly handled via a similar registration if needed.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// IMPORTANT: Ensure this line is commented out if using the HTTP profile (7204)
// If using HTTPS (7243) and the above bypass, this line is usually fine.
// app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


/*
using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FIX: Creating a custom HttpClientHandler to ignore SSL certificate errors
// for local development, which prevents the "SSL connection could not be established" error.
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

// FIX: Registering the HttpClient using the API's HTTPS port (7243) and the custom handler.
builder.Services.AddScoped(sp => new HttpClient(handler)
{
    BaseAddress = new Uri("https://localhost:7243")
});


//builder.Services.AddScoped( services=> new HttpClient
  //      {
            //BaseAddress = new Uri("https://localhost:7243")
          //port address of the webApi, from the launchSettings.json file
            //beware that the https port is defined in the same line as the http port
            //switching baseaddress to https may cause issues if the certificate is not trusted, so moving to http for now
    //        BaseAddress = new Uri("http://localhost:7204")
      //  }
   // );


//registering the services
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

*/