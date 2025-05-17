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

    public interface ITelMHDiagnosisCategoryBLL : IService<TelMHDiagnosisCategory>
    {
        IQueryable<TelMHDiagnosisCategoryDTO> GetAll();
        int Insert(TelMHDiagnosisCategoryDTO obDTO);
        void Update(TelMHDiagnosisCategoryDTO obDTO);
        void Delete(int id);
        TelMHDiagnosisCategoryDTO GetById(int id);
        string IsValid(TelMHDiagnosisCategoryDTO obDTO);
        string ValidToBeDeleted(int id);

    }

}
