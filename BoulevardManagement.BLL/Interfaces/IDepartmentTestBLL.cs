using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System.Linq;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IDepartmentTestBLL : IService<DepartmentTest>
    {
        IQueryable<DepartmentTestDTO> GetAll();
        DepartmentTestDTO GetById(int id);
        int Insert(DepartmentTestDTO input);
        void Update(DepartmentTestDTO input);
        void Delete(int id);
    }
}