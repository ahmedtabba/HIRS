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
    public interface IUserQuickActionBLL : IService<UserQuickAction>
    {
        IQueryable<UserQuickActionDTO> GetAll();
        void Update(List<UserQuickActionDTO> actionsList, string userId);
        IQueryable<UserQuickActionDTO> GetAllByUserId(string userId);
    }
}
