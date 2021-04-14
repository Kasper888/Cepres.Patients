using Abp.Application.Services;
using Cepres.Patients.MultiTenancy.Dto;

namespace Cepres.Patients.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

