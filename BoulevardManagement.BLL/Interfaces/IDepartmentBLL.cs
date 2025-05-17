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

    public interface IDepartmentBLL : IService<Department>
    {
        IQueryable<DepartmentDTO> GetAll();
        int Insert(DepartmentDTO obDTO);
        void Update(DepartmentDTO obDTO);
        void Delete(int id);
        DepartmentDTO GetById(int id);
        string IsValid(DepartmentDTO obDTO);
        string ValidToBeDeleted(int id);

    }

}
