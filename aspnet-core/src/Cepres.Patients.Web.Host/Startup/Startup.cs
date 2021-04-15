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
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Results;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

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

      services.AddControllersWithViews(o =>
      {
        o.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
      }).AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
        {
          NamingStrategy = new CamelCaseNamingStrategy()
        };
      });
      services.AddOData(o => o.AddModel("odata", GetEdmModel()).Select().Filter().Expand().OrderBy().Count().SetMaxTop(null));
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
          Title = "Patients API"
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
