using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTMwMzc2M0AzMjMwMmUzNDJlMzBXTGVIY3JZS2NSaE81KzluWjBmQnhQeEcvRDZKTCtSTC9UZlBHZGdndzlBPQ==");

builder.Services.AddSyncfusionBlazor();

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
