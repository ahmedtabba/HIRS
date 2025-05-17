using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IErrorLogBLL : IService<ErrorLog>
    {
        int Insert(ErrorLogDTO obDTO);
        void Delete(int id);
        void Clean();
        IQueryable<ErrorLogDTO> GetAll();
    }
}
