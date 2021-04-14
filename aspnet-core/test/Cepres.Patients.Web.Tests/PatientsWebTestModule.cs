using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cepres.Patients.EntityFrameworkCore;
using Cepres.Patients.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Cepres.Patients.Web.Tests
{
    [DependsOn(
        typeof(PatientsWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class PatientsWebTestModule : AbpModule
    {
        public PatientsWebTestModule(PatientsEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PatientsWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(PatientsWebMvcModule).Assembly);
        }
    }
}