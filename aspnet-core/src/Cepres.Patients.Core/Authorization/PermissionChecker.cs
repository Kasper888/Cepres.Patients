using Abp.Authorization;
using Cepres.Patients.Authorization.Roles;
using Cepres.Patients.Authorization.Users;

namespace Cepres.Patients.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
