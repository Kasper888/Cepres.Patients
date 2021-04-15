﻿using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Cepres.Patients.Authentication.JwtBearer;
using Cepres.Patients.Configuration;
using Cepres.Patients.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;


namespace Cepres.Patients
{
  [DependsOn(
       typeof(PatientsApplicationModule),
       typeof(PatientsEntityFrameworkModule),
       typeof(AbpAspNetCoreModule),
       typeof(AbpAspNetCoreSignalRModule)
   )]
  public class PatientsWebCoreModule : AbpModule
  {
    private readonly IWebHostEnvironment _env;
    private readonly IConfigurationRoot _appConfiguration;

    public PatientsWebCoreModule(IWebHostEnvironment env)
    {
      _env = env;
      _appConfiguration = env.GetAppConfiguration();
    }

    public override void PreInitialize()
    {
      Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
          PatientsConsts.ConnectionStringName
      );

      // Use database for language management
      Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

      Configuration.Modules.AbpAspNetCore()
           .CreateControllersForAppServices(
               typeof(PatientsApplicationModule).GetAssembly()
           );

      ConfigureTokenAuth();
    }

    private void ConfigureTokenAuth()
    {
      IocManager.Register<TokenAuthConfiguration>();
      var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

      tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
      tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
      tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
      tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
      tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
    }

    public override void Initialize()
    {
      IocManager.RegisterAssemblyByConvention(typeof(PatientsWebCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
      IocManager.Resolve<ApplicationPartManager>()
          .AddApplicationPartsIfNotAddedBefore(typeof(PatientsWebCoreModule).Assembly);
    }
  }
}
