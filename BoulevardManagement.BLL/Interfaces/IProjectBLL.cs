using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System.Linq;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IProjectBLL : IService<Project>
    {
        int Insert(ProjectDTO obDTO);
        void Update(ProjectDTO obDTO);
        void Delete(int id);
        IQueryable<ProjectDTO> GetAll();
        ProjectDTO GetById(int id);
    }
}

