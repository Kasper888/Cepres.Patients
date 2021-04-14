using Abp.Application.Services.Dto;

namespace Cepres.Patients.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

