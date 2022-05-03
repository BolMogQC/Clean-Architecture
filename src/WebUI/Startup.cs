using System.Globalization;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.WebUI.Filters;
using CleanArchitecture.WebUI.Services;
using FluentValidation.AspNetCore;
using Infrastructure.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using WebUI.Models;

namespace CleanArchitecture.WebUI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        
        services.AddControllersWithViews(options => options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                opts.JsonSerializerOptions.Converters.Add(new ValidationProblemDetailsJsonConverter());
                opts.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages()
            .AddViewLocalization()
            .AddRazorRuntimeCompilation();

        services.AddAuthentication()
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
        
        services.ConfigureApplicationCookie(options =>
        {
            // See https://github.com/aspnet/AspNetCore/blob/5a64688d8e192cacffda9440e8725c1ed41a30cf/src/Identity/src/Identity/IdentityServiceCollectionExtensions.cs#L56
            // https://stackoverflow.com/questions/45700454/net-core-middleware-return-blank-result
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });
        
        services.Configure<JwtOptions>(Configuration.GetSection("jwt"));
        services.AddLocalization();
        services.AddMvc()
            .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase) // quick hack, review this.
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();
        
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo> { new("fr"), new("en") };

            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => 
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "CleanArchitecture API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var supportedCultures = new[] { "fr", "en" };

        app.UseCors(x =>
        {
            x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });
        
        app.UseRequestLocalization(
            new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures));
        
        var cultureInfo = new CultureInfo("ca-FR");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}
