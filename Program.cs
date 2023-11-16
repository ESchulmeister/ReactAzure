using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Configuration;

using reactAzure;
using reactAzure.Data;
using reactAzure.Models;
using reactAzure.Services;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddLog4Net();

        var services = builder.Services;


        //antiforgery
        services.AddAntiforgery(options => options.HeaderName = Constants.AntiForgery.Header);

        services.AddControllers()
                    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

        services.AddLogging(c => c.ClearProviders());

        services.AddMemoryCache();

        //connection resiliency
        string conn = builder.Configuration.GetConnectionString("TechCMSDB");   //db conn
        services.AddDbContext<TechCMSContext>(opt => opt.UseSqlServer(conn));

        //Azure connection
        services.AddScoped(_ =>
        {
            return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
        });


        ///Set cookie authentication 
        services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme, (opt) =>
                {
                    opt.Cookie.Name = Constants.AuthCookie;
                    opt.SlidingExpiration = true;
                    opt.ExpireTimeSpan = new TimeSpan(1, 0, 0); // Expires in 1 hour
                    opt.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };

                    opt.Cookie.HttpOnly = true;

                }
        );


        //AD user context
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHttpContextAccessor();

        // configure DI (dependency injection) for application services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();


        // configure strongly typed settings object
        services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        services.AddScoped<IAzureRepository, AzureRepository>();

        services.AddScoped<IComputerVisionService, ComputerVisionService>();

        services.AddCors();
        services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseMiddleware<JwtMiddleware>();

        //global Cors policy
        app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Tells the app to transmit the cookie through HTTPS only
        app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}