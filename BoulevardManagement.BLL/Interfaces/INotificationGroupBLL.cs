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
    public interface INotificationGroupBLL : IService<NotificationGroup>
    {
        IQueryable<NotificationGroupDTO> GetAll();
        NotificationGroupDTO GetById(int id);
        int Create(NotificationGroupDTO group);
        int Update(NotificationGroupDTO group, bool isNew);
        void Delete(int id);
        IQueryable<NotificationDTO> GetNotificationByUserId(string userId);
    }
}
