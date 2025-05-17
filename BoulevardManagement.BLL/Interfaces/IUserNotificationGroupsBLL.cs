using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IUserNotificationGroupsBLL : IService<UserNotificationGroups>
    {
        void UpdateUserGroups(List<int> Groups, string UserId);
        IQueryable<NotificationGroupDTO> GetAll(string UserId);
        IQueryable<UserNotificationGroupsDTO> GetAll();
    }
}
