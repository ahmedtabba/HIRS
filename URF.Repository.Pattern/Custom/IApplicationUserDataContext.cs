using System.Collections.Generic;

namespace Repository.Pattern
{
    public interface IApplicationUserDataContext
    {
        ApplicationUserData GetApplicationUserData();
        bool IsInRole(string roleName);
        List<ApplicationUserData> GetUsersBy(string roleName);
        ApplicationUserData GetUsersByUserId(string userId);
        List<ApplicationUserData> GetUsersByDepartmentId(int departmentId);
    }
}
