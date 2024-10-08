using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SHARKNA;
using SHARKNA.Models;
using System.Configuration;
using SHARKNA.Domain;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<SHARKNAContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Users/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                //options.LoginPath = "/accounts/ErrorNotLoggedIn";
                //options.LogoutPath = "account/logout";
            });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAdmin", p => p.RequireAuthenticatedUser().RequireRole("Admin"));
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.None;
});


builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();

builder.Services.AddScoped<UserDomain>();
builder.Services.AddScoped<BoardDomain>();
builder.Services.AddScoped<EventRegistrationsDomain>();
builder.Services.AddScoped<RequestStatusDomain>();
builder.Services.AddScoped<EventDomain>();
builder.Services.AddScoped<BoardTalRequestsDomain>();
builder.Services.AddScoped<RoleDomain>();
builder.Services.AddScoped<PermissionDomain>();
builder.Services.AddScoped<BoardRolesDomain>();
builder.Services.AddScoped<EventAttendenceDomain>();
builder.Services.AddScoped<BoardRequestsDomain>();
builder.Services.AddScoped<BoardMembersDomain>();
builder.Services.AddScoped<EventRequestsDomain>();


var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSession();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();

//test
//test