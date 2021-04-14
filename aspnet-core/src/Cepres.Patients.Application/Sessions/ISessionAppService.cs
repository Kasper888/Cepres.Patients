using System.Threading.Tasks;
using Abp.Application.Services;
using Cepres.Patients.Sessions.Dto;

namespace Cepres.Patients.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
