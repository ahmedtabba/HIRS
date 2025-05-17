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

    public interface ILocationBLL : IService<Location>
    {
        IQueryable<LocationDTO> GetAll();
        int Insert(LocationDTO obDTO);
        void Update(LocationDTO obDTO);
        void Delete(int id);
        LocationDTO GetById(int id);
        string IsValid(LocationDTO obDTO);
        string ValidToBeDeleted(int id);
        void SetStatus(int? Id,bool IsOccupied);

    }

}
