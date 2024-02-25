using Microsoft.AspNetCore.Identity.UI.Services;
using NationalParkWebApp_3;
using NationalParkWebApp_3.Repository;
using NationalParkWebApp_3.Repository.IRepository;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<ITrailRepository,TrailRepository>();
builder.Services.AddScoped<IBookingRepository,BookingRepository>();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["Secretkey"];

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
