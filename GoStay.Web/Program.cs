using DAL.Entities;
using DAL.Helpers.PageHelpers;
using GoStay.Api.MiddleWare;
using GoStay.Api.Providers;
using GoStay.Common.Configuration;
using GoStay.DataAccess.DBContext;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Repositories;
using GoStay.DataAccess.UnitOfWork;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using GoStay.Web.Configurations;
using GoStay.Web.Helpers;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Q101.ServiceCollectionExtensions.ServiceCollectionExtensions;
using RewriteRules;
using Serilog;
using System.Globalization;
using System.Text;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
AppConfigs.LoadAll(config);

Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(config)
      .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//--register CommonDBContext
// Add framework services.
builder.Services
    .AddControllersWithViews()
    // Maintain property names during serialization. See:
    // https://github.com/aspnet/Announcements/issues/194
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
// Add Kendo UI services to the services container
builder.Services.AddKendo();

builder.Services.AddDbContext<CommonDBContext>(options =>
            options.UseSqlServer(AppConfigs.SqlConnection, options => { }),
            ServiceLifetime.Scoped
            );

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = AppConfigs.GoogleClientId;
        options.ClientSecret = AppConfigs.GoogleClientSecret;
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    })
    .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
     {
         options.ClientId = AppConfigs.FacebookClientId;
         options.ClientSecret = AppConfigs.FacebookClientSecret;
     })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

//--register Singleton
builder.Services.AddTransient(typeof(ICommonRepository<>), typeof(CommonRepository<>));
builder.Services.AddTransient(typeof(ICommonUoW), typeof(CommonUoW));
builder.Services.AddSingleton<IDataContextHelper, DataContextHelper>();
builder.Services.AddSingleton<IConstants, Constants>();
builder.Services.AddSingleton<ISessionManager, SessionManager>();
//--register Service
builder.Services.RegisterAssemblyTypesByName(typeof(IAdminServices).Assembly,
     name => name.EndsWith("Services"))
     .AsScoped()
     .AsImplementedInterfaces()
     .Bind();
//--register Api Service
builder.Services.RegisterAssemblyTypesByName(typeof(IHotelApiServices).Assembly,
     name => name.EndsWith("Services"))
     .AsScoped()
     .AsImplementedInterfaces()
     .Bind();

//--For session and cookies purpose
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<AdminAuthorization>();
//builder.Services.AddScoped<TeacherAuthorization>();
//builder.Services.AddScoped<StudentAuthorization>();
builder.Services.AddHttpClient<IMyTypedClientServices, MyTypedClientServices>();

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("vi-VN"),
        new CultureInfo("en-US"),
        new CultureInfo("zh-CN"),
    };
    options.DefaultRequestCulture = new RequestCulture("vi-VN");
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddCommonServices();


//--Session and cookies settings
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time  in seconds, minutes 
});

builder.Services.AddHttpClient("https://210.211.99.55:9000").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

});
var app = builder.Build();

using (StreamReader iisUrlRewriteStreamReader =
    File.OpenText("IISUrlRewrite.xml"))
{
    var options = new RewriteOptions()
        .AddRedirect("redirect-rule/(.*)", "redirected/$1")
        .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2", skipRemainingRules: true)
        .AddIISUrlRewrite(iisUrlRewriteStreamReader)
        .Add(MethodRules.RedirectXmlFileRequests)
        .Add(MethodRules.RewriteTextFileRequests);
    //.Add(new RedirectImageRequests(".png", "/png-images"))
    //.Add(new RedirectImageRequests(".jpg", "/jpg-images"));

    app.UseRewriter(options);
}

    StaticServiceProvider.Provider = app.Services;
    UpdateTimer.Init();

app.UseMiddleware<JwtMiddleware>();

app.Use(async (context, next) =>
{
    if (context.Request.Query.Count() > 0 &&
    context.Request.Query["culture"].ToString() != "")
    {

        System.Threading.Thread.CurrentThread.CurrentCulture =
        System.Threading.Thread.CurrentThread.CurrentUICulture =
        new CultureInfo(context.Request.Query["culture"].ToString());
        // Save current culture cookie
        context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue
                (new RequestCulture(context.Request.Query["culture"].ToString())),
                new CookieOptions() { Expires = DateTime.Now.AddYears(1) }
            );
    }
    await next.Invoke();
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
#region routing
//app.MapAreaControllerRoute(
//    name: "MyAreaAdmin",
//    areaName: "Admin",
//    pattern: "Admin/{controller=AdminAccount}/{action=Login}");

app.MapAreaControllerRoute(
    name: "AdminLogin",
    areaName: "Admin",
    pattern: "Admin/Login",
     defaults: new { controller = "AdminAccount", action = "Login" });

app.MapAreaControllerRoute(
    name: "AdminDashboard",
    areaName: "Admin",
    pattern: "Admin/Home",
     defaults: new { controller = "AdminHome", action = "Dashboard" });
app.MapAreaControllerRoute(
    name: "MyAreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=AdminAccount}/{action=Login}");

app.MapControllerRoute(
    name: "khachsan",
    pattern: "khach-san",
    defaults: new { controller = "Hotels", action = "Index" });

app.MapControllerRoute(
    name: "order",
    pattern: "order",
    defaults: new { controller = "Orders", action = "Index" });

app.MapControllerRoute(
    name: "order/payments",
    pattern: "order/payments",
    defaults: new { controller = "Orders", action = "XacnhanPTThanhToan" });

app.MapControllerRoute(
    name: "order/successful",
    pattern: "order/successful",
    defaults: new { controller = "Orders", action = "Xacnhanhdatphong" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "tintuc",
    pattern: "tin-tuc",
    defaults: new { controller = "News", action = "Index" });
app.MapControllerRoute(
    name: "tin-tuc",
    pattern: "tin-tuc",
    defaults: new { controller = "News", action = "NewsDetails", id = "", slug = "" });
app.MapControllerRoute(
    name: "danh-muc-tin",
    pattern: "danh-muc-tin",
    defaults: new { controller = "News", action = "ListNews", idCategory = "", idTopic = "" });
app.MapControllerRoute(
    name: "danh-muc-tin",
    pattern: "danh-muc-tin",
    defaults: new { controller = "News", action = "ListNewsTopic", idTopic = "" });
app.MapControllerRoute(
    name: "khach-san",
    pattern: "khach-san",
    defaults: new { controller = "HotelDetail", action = "Index", id = "", slug = "" });
app.MapControllerRoute(
    name: "tours",
    pattern: "tours",
    defaults: new { controller = "Tours", action = "TourContent", id = "", slug = "" });
app.MapControllerRoute(
    name: "video",
    pattern: "video",
    defaults: new { controller = "News", action = "ListVideoNews", IdCategory = "", slug = "" });
#endregion

app.Run();
