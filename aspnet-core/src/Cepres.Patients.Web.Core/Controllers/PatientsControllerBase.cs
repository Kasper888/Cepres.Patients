using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Cepres.Patients.Controllers
{
  public abstract class PatientsControllerBase : AbpController
  {
    protected PatientsControllerBase()
    {
      LocalizationSourceName = PatientsConsts.LocalizationSourceName;
    }

    protected void CheckErrors(IdentityResult identityResult)
    {
      identityResult.CheckErrors(LocalizationManager);
    }
  }
}
