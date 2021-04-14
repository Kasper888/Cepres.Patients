using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Cepres.Patients.Configuration;
using Cepres.Patients.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.OData.Edm;
using Cepres.Patients.Patients;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.OData.Formatter;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Results;

namespace Cepres.Patients.Web.Host.Startup
{
  public class Startup
  {
    private const string _defaultCorsPolicyName = "localhost";

    private const string _apiVersion = "v1";

    private readonly IConfigurationRoot _appConfiguration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public Startup(IWebHostEnvironment env)
    {
      _hostingEnvironment = env;
      _appConfiguration = env.GetAppConfiguration();
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddOData();
   
      services.AddControllersWithViews(o =>
      {
        foreach (var outputFormatter in o.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
        {
          outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
        }
        foreach (var inputFormatter in o.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
        {
          inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
        }
        o.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
      }).AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
        {
          NamingStrategy = new CamelCaseNamingStrategy()
        };
      });
      IdentityRegistrar.Register(services);
      AuthConfigurer.Configure(services, _appConfiguration);

      services.AddSignalR();

      // Configure CORS for angular2 UI
      services.AddCors(
          options => options.AddPolicy(
              _defaultCorsPolicyName,
              builder => builder
                  .WithOrigins(
                      // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                      _appConfiguration["App:CorsOrigins"]
                          .Split(",", StringSplitOptions.RemoveEmptyEntries)
                          .Select(o => o.RemovePostFix("/"))
                          .ToArray()
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()
          )
      );

      // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc(_apiVersion, new OpenApiInfo
        {
          Version = _apiVersion,
          Title = "Patients API",
          Description = "Patients",
          // uncomment if needed TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Patients",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/aspboilerplate"),
          },
          License = new OpenApiLicense
          {
            Name = "MIT License",
            Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
          }
        });
        options.DocInclusionPredicate((docName, description) => true);

        // Define the BearerAuth scheme that's in use
        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
        {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey
        });
      });

      // Configure Abp and Dependency Injection
      return services.AddAbp<PatientsWebHostModule>(
          // Configure Log4Net logging
          options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
              f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                      ? "log4net.config"
                      : "log4net.Production.config"
                  )
          )
      );
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

      app.UseCors(_defaultCorsPolicyName); // Enable CORS!

      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAbpRequestLocalization();

      app.UseUnitOfWork(options =>
      {
        options.Filter = httpContext => httpContext.Request.Path.Value.StartsWith("/odata", StringComparison.OrdinalIgnoreCase);
      });
      app.UseODataBatching();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<AbpCommonHub>("/signalr");
        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
        endpoints.MapODataRoute("odataPrefix", "odata", GetEdmModel()).Count().Filter().Expand().OrderBy().Select().MaxTop(null).SkipToken();
      });

      // Enable middleware to serve generated Swagger as a JSON endpoint
      app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

      // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
      app.UseSwaggerUI(options =>
      {
        // specifying the Swagger JSON endpoint.
        options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"Patients API {_apiVersion}");
        options.IndexStream = () => Assembly.GetExecutingAssembly()
                  .GetManifestResourceStream("Cepres.Patients.Web.Host.wwwroot.swagger.ui.index.html");
        options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.  
      }); // URL: /swagger
    }
    private static IEdmModel GetEdmModel()
    {
      ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
      builder.EntitySet<Patient>("Patient");
      builder.EntitySet<Visit>("Visit");

      return builder.GetEdmModel();
    }
  }
}
