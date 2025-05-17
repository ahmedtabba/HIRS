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
    public interface IOperationLogBLL : IService<OperationLog>
    {
        IQueryable<OperationLogDTO> GetAll(string EntityType, int ObjectId);

        IQueryable<OperationLogDTO> GetAll(string EntityType, int ObjectId, DateTime fromDate, DateTime toDate, int operationType, string searchText);

        int Insert(OperationLogDTO obDTO);
    }
}
