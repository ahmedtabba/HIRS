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

    public interface ITelMHDiagnosisBLL : IService<TelMHDiagnosis>
    {
        IQueryable<TelMHDiagnosisDTO> GetAll();
        int Insert(TelMHDiagnosisDTO obDTO);
        void Update(TelMHDiagnosisDTO obDTO);
        void Delete(int id);
        TelMHDiagnosisDTO GetById(int id);
        string IsValid(TelMHDiagnosisDTO obDTO);
        string ValidToBeDeleted(TelMHDiagnosisDTO obDTO);
        string IsValidToBeChanged(TelMHDiagnosisDTO obDTO);
    }

}
