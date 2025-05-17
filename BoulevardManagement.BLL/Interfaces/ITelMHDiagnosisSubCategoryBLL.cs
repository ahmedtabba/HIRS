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

    public interface ITelMHDiagnosisSubCategoryBLL : IService<TelMHDiagnosisSubCategory>
    {
        IQueryable<TelMHDiagnosisSubCategoryDTO> GetAll();
        int Insert(TelMHDiagnosisSubCategoryDTO obDTO);
        void Update(TelMHDiagnosisSubCategoryDTO obDTO);
        void Delete(int id);
        TelMHDiagnosisSubCategoryDTO GetById(int id);
        IQueryable<TelMHDiagnosisSubCategoryDTO> GetByDiagnosisId(int id);
        string IsValid(TelMHDiagnosisSubCategoryDTO obDTO);
        string ValidToBeDeleted(int id);

    }

}
