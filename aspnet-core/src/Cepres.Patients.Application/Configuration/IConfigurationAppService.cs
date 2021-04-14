using System.Threading.Tasks;
using Cepres.Patients.Configuration.Dto;

namespace Cepres.Patients.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
