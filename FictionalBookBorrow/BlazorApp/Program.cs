using BlazorApp.Areas.Identity;
using BlazorApp.Behaviors;
using BlazorApp.Data;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

// Configure an initial "bootstrap" logger to be able to log errors during the startup of the ASP.NET Core host.
// Note that this bootstrap logger will be completely replaced by the logger configured with "UseSerilog()" below.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("C:/temp/logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

WebApplicationBuilder builder;

try
{
    Log.Information("Starting web application...");

    builder = WebApplication.CreateBuilder(args);


    // ------------------------------------------------------------------------------------------
    //                                    Set up logging.
    // ------------------------------------------------------------------------------------------

    // Create a final logger that will replace the bootstrap logger that was created above. This is the logger that
    // will be used by the running app. The logger gets its configuration from the appsettings.json file.
    builder.Host.UseSerilog((context, services, configuration)
        => configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());


    // ------------------------------------------------------------------------------------------
    //                               Add services to the container.
    // ------------------------------------------------------------------------------------------

    RegisterCommonDependencies();


    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
    
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
                    // Only enable detailed errors on a production environment for temporary troubleshooting! Setting this value
                    // to true exposes possibly sensitive error information on the console of your browser. 
                    //.AddCircuitOptions(options => options.DetailedErrors = true);
    
    builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
    builder.Services.AddSingleton<WeatherForecastService>();

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request. 
        // Return "true" when you want your users to explicitly give their consent for non-essential cookies.  
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    WebApplication app = builder.Build();


    // ------------------------------------------------------------------------------------------
    //                            Configure the HTTP request pipeline.
    // ------------------------------------------------------------------------------------------

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCookiePolicy();

    // Configure localization immediately after the routing middle ware is added to the processing pipeline.
    RequestLocalizationOptions localizationOptions = GetSupportedLocalizationOptions();
    app.UseRequestLocalization(localizationOptions);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();

}
catch(Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


// ------------------------------------------------------------------------------------------
//                                     Helper methods.
// ------------------------------------------------------------------------------------------

// Register dependencies that are needed for all environments (Development, Staging, Production).
void RegisterCommonDependencies()
{
    // We need to register this dependency if we want access to HttpContext in custom components.
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

    builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}

RequestLocalizationOptions GetSupportedLocalizationOptions()
{
    string[] supportedCultures = GetSupportedCultures();

    var supportedOptions = new RequestLocalizationOptions();

    return supportedOptions.SetDefaultCulture(GetDefaultCulture(supportedCultures))
                           .AddSupportedCultures(supportedCultures)
                           .AddSupportedUICultures(supportedCultures);

    string[] GetSupportedCultures()
    {
        IDictionary<string, string?> cultures = builder.Configuration.GetSection("Cultures")
                                                                     .GetChildren()
                                                                     .ToDictionary(setting => setting.Key,
                                                                                   setting => setting.Value);

        return cultures.Keys.ToArray();
    }

    string GetDefaultCulture(IReadOnlyList<string> cultures)
    {
        const string dutchCulture = "nl";

        // When the Dutch culture is not specified in the app configuration file, just use the first
        // specified culture as the default.
        return cultures.Contains(dutchCulture) ? dutchCulture : cultures[0];
    }
}