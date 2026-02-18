using GymSystem.Data;
using GymSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // configure options.Password, options.Lockout, etc.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagementAccess", policy => policy.RequireRole("Admin", "Staff"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Events.OnRedirectToLogin = ctx =>
    {
        var returnUrl = ctx.Request.Path + ctx.Request.QueryString;
        if (ctx.Request.Path.StartsWithSegments("/Management", StringComparison.OrdinalIgnoreCase))
        {
            var redirect = $"/Management/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
            ctx.Response.Redirect(redirect);
        }
        else
        {
            var redirect = $"/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
            ctx.Response.Redirect(redirect);
        }
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = ctx =>
    {
        var returnUrl = ctx.Request.Path + ctx.Request.QueryString;
        if (ctx.Request.Path.StartsWithSegments("/Management", StringComparison.OrdinalIgnoreCase))
        {
            var redirect = $"/Management/Account/AccessDenied?returnUrl={Uri.EscapeDataString(returnUrl)}";
            ctx.Response.Redirect(redirect);
        }
        else
        {
            var redirect = $"/Account/AccessDenied?returnUrl={Uri.EscapeDataString(returnUrl)}";
            ctx.Response.Redirect(redirect);
        }
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

await GymSystem.Data.SeedData.EnsureRolesAndAdminAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
