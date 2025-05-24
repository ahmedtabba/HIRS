using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Repository.Pattern;
using Service.Pattern;
using System.Collections.Generic;
using System.Linq;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IDepartmentTestBLL : IService<DepartmentTest>
    {
        IEnumerable<DepartmentTestDTO> GetAll();
        DepartmentTestDTO GetById(int id);
        DepartmentTestDTO Insert(DepartmentTestDTO input);
        DepartmentTestDTO Update(DepartmentTestDTO input);
        void Delete(int id);
        List<ApplicationUserData> GetItUsers();
    }
}