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


    // Add services to the container.
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
    
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
                    // Only enable detailed errors on a production environment for temporary troubleshooting! Setting this value
                    // to true exposes possibly sensitive error information on the console of your browser. 
                    //.AddCircuitOptions(options => options.DetailedErrors = true);
    
    builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
    builder.Services.AddSingleton<WeatherForecastService>();

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
    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

    builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}
