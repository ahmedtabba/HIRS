using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System.Linq;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IEmployeeTestBLL : IService<EmployeeTest>
    {
        IQueryable<EmployeeTestDTO> GetAll();
        EmployeeTestDTO GetById(int id);
        int Insert(EmployeeTestDTO input);
        void Update(EmployeeTestDTO input);
        void Delete(int id);
    }
}