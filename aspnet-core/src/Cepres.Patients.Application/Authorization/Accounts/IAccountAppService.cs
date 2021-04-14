using System.Threading.Tasks;
using Abp.Application.Services;
using Cepres.Patients.Authorization.Accounts.Dto;

namespace Cepres.Patients.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
