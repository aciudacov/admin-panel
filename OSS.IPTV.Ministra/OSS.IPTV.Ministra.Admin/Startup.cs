using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Services;
using OSS.IPTV.Ministra.Admin.Services.AppUser;
using OSS.IPTV.Ministra.Repository;
using System.Security.Claims;
using ILogger = OSS.IPTV.Ministra.Admin.Interfaces.ILogger;

namespace OSS.IPTV.Ministra.Admin;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices();
        services.AddScoped<LayoutService>();
        services.AddSingleton<LocalizationService>();
        services.AddLocalization();
        
        ConfigureApplicationServices(services);
        //ConfigureAuthenticationServices(services);
    }

    private void ConfigureApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IChannel, TvChannelService>();
        services.AddScoped<IPackage, TvPackageService>();
        services.AddScoped<ILogger, LoggerService>();
        services.AddScoped<AppUserProvider>();

        services.AddDbContext<IptvContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("Default"));
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        var context = app.ApplicationServices.GetRequiredService<IptvContext>();
        context.Database.Migrate();

        //app.UseAuthentication();
        //app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }

    private void ConfigureAuthenticationServices(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { options.LoginPath = "/account/login"; })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                Configuration.Bind("Auth:OIDC", options);

                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("email");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("roles");

                options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;

                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
