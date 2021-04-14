using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cepres.Patients.Authorization;

namespace Cepres.Patients
{
    [DependsOn(
        typeof(PatientsCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PatientsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PatientsAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PatientsApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
