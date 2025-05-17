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

    public interface IMedicationBLL : IService<Medication>
    {
        IQueryable<MedicationDTO> GetAll();
        int Insert(MedicationDTO obDTO);
        void Update(MedicationDTO obDTO);
        void Delete(int id);
        MedicationDTO GetById(int id);
        string IsValid(MedicationDTO obDTO);
        string ValidToBeDeleted(int id);

    }

}
