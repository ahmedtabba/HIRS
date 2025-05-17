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

    public interface IPatientBLL : IService<Patient>
    {
        IQueryable<PatientDTO> GetAll();
        int Insert(PatientDTO obDTO);
        void Update(PatientDTO obDTO);
        void Delete(int id);
        PatientDTO GetById(int id);
        string IsValid(PatientDTO obDTO);
        string ValidToBeDeleted(int id);

    }

}
