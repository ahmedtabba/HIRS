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

    public interface ITelMHMostLikelyDiagnosisBLL : IService<TelMHMostLikelyDiagnosis>
    {
        IQueryable<TelMHMostLikelyDiagnosisDTO> GetAll();
        int Insert(TelMHMostLikelyDiagnosisDTO obDTO);
        void Update(TelMHMostLikelyDiagnosisDTO obDTO);
        void Delete(int id);
        TelMHMostLikelyDiagnosisDTO GetById(int id);
        string IsValid(TelMHMostLikelyDiagnosisDTO obDTO);
        string ValidToBeDeleted(int id);
        string IsValidToBeAdded(TelMHMostLikelyDiagnosisDTO obDTO);
    }

}
