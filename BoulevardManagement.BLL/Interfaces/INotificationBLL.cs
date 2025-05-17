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
    public interface INotificationBLL : IService<Notification>
    {
        IQueryable<NotificationDTO> GetAll();
        NotificationDTO GetById(int id);
        int Insert(NotificationDTO obDTO);
        void Update(NotificationDTO obDTO);
        void Delete(int id);
    }
}
