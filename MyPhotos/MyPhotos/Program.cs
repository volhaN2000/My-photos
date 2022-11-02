using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MyPhotos.Areas.Identity.Data;
using MyPhotos.Services;
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;



// See https://aka.ms/new-console-template for more information
const string SUBSCRIPTION_KEY = "dfa5543e585e429fafb3ffd14b02d197";
const string ENDPOINT = "https://facial-recognition-tms.cognitiveservices.azure.com/";



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyPhotosDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MyPhotosDbContextConnection' not found.");

builder.Services.AddDbContext<MyPhotosDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MyPhotosUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MyPhotosDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

