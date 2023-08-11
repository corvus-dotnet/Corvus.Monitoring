// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Corvus.Monitoring. This is the only line of code needed to add Corvus.Monitoring, but this specific
// call will add the "null" implementations of IOperationsInstrumentation and IExceptionsInstrumentation.
// In a real world solution, you would use an implementation-specific call; i.e.
//     builder.services.AddApplicationInsightsInstrumentationTelemetry()
// to add the App Insights specific implementations.
builder.Services.AddInstrumentation();

WebApplication app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <summary>
/// Partial program class to make the actual one public so it can be accessed for testing purposes.
/// </summary>
public partial class Program
{
}