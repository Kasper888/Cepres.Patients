using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Cepres.Patients.Configuration.Dto;

namespace Cepres.Patients.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : PatientsAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
