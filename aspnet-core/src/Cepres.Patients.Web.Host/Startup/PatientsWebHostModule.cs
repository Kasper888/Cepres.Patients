using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cepres.Patients.Configuration;

namespace Cepres.Patients.Web.Host.Startup
{
    [DependsOn(
       typeof(PatientsWebCoreModule))]
    public class PatientsWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PatientsWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PatientsWebHostModule).GetAssembly());
        }
    }
}
