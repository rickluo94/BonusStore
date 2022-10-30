using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Text.Json.Serialization;
using BonusStore.DataAccess.Data;
using BonusStore.Model;
using BonusStore.DataAccess.Repository.IRepository;
using BonusStore.DataAccess.DbInitializer;
using BonusStore.DataAccess.Repository;

// Client IP safelist for ASP.NET Core
//https://learn.microsoft.com/zh-tw/aspnet/core/security/ip-safelist?view=aspnetcore-6.0

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    //https://learn.microsoft.com/zh-tw/aspnet/core/mvc/models/validation?view=aspnetcore-6.0
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "The field is required.");
}).AddJsonOptions(options =>
{
    //PascalCase https://khalilstemmler.com/blogs/camel-case-snake-case-pascal-case/

    //The default casing for JSON strings in ASP.NET Core is camelCase.
    //The Telerik UI components that are data-bound depend on PascalCase formatted response from the server.
    //If the JSON serialization isn't configured properly, the UI components will display wrong data.
    //To find out how to configure the application to return the data in Pascal-case,
    //refer to the JSON Serialization article.
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    //options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    //options.JsonSerializerOptions.WriteIndented = true;
    //https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties?pivots=dotnet-6-0
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

//https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/http-context?view=aspnetcore-6.0
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Injection AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/add-user-data?view=aspnetcore-6.0&tabs=visual-studio
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddRazorPages().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio
//https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0
//https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0
builder.Services.Configure<IdentityOptions>(options =>
{
    // Lockout settings.
    // date time UTC 
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //https://learn.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.builder.sessionoptions?view=aspnetcore-6.0
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization();

app.UseRouting();
SeedDatabase();

app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}