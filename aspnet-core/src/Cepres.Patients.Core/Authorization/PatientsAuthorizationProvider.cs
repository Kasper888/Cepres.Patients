using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Cepres.Patients.Authorization
{
  public class PatientsAuthorizationProvider : AuthorizationProvider
  {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
      context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
      context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
      context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
      context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

      var patient = context.CreatePermission(PermissionNames.Patient_Get);
      patient.CreateChildPermission(PermissionNames.Patient_List);
      patient.CreateChildPermission(PermissionNames.Patient_Update);
      patient.CreateChildPermission(PermissionNames.Patient_Delete);
      patient.CreateChildPermission(PermissionNames.Patient_Create);

      var reporting = context.CreatePermission(PermissionNames.Reporting);
      reporting.CreateChildPermission(PermissionNames.Reporting_PatientStatistics);

    }

    private static ILocalizableString L(string name)
    {
      return new LocalizableString(name, PatientsConsts.LocalizationSourceName);
    }
  }
}
