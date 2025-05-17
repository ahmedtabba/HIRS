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

    public interface ICaseClosureBLL : IService<CaseClosure>
    {
        IQueryable<CaseClosureDTO> GetAll();
        IQueryable<CaseClosureDTO> GetAll(DepartmentEnum department,int caseId);
        CaseClosureDTO GetLastClosure(DepartmentEnum department,int caseId);
        void Insert(CaseClosureDTO obDTO);
        CaseClosureDTO GetById(int id);
    }

}
