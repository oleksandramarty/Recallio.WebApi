using Recallio.Const.Enum;
using Recallio.Const.Providers;
using Recallio.Domain.Models.User;

namespace Recallio.Domain.Initialization;

public class InitializationProvider
{
    public static IList<Role> GetRoles()
    {
        return new List<Role>
        {
            new Role { Id = InitConst.UserRoleId, Name = RoleEnum.User.ToString(), NormalizedName = RoleProvider.User.ToUpper(), ConcurrencyStamp = InitConst.UserRoleId.ToString() },
            new Role { Id = InitConst.AdminUserId, Name = RoleEnum.Admin.ToString(), NormalizedName = RoleProvider.Admin.ToUpper(), ConcurrencyStamp = InitConst.AdminUserId.ToString() }
        };
    }
}