using Abp.MultiTenancy;
using Cepres.Patients.Authorization.Users;

namespace Cepres.Patients.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
