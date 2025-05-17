using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Repository.Pattern;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL
{

    public class CaseClosureBLL : Service<CaseClosure>, ICaseClosureBLL
    {
        readonly IRepositoryAsync<CaseClosure> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public CaseClosureBLL(
     IRepositoryAsync<CaseClosure> repository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IApplicationUserDataContext applicationContext
  ) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;

        }


        public void Insert(CaseClosureDTO obDTO)
        {
            try
            {
                var obEntity = Mapper.Map<CaseClosureDTO, CaseClosure>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(CaseClosure).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });

            }
            catch {throw; }
        }
        

        public CaseClosureDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<CaseClosureDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public IQueryable<CaseClosureDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<CaseClosureDTO>();
            }
            catch { throw; }
        }

        public IQueryable<CaseClosureDTO> GetAll(DepartmentEnum department, int caseId)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(x=>x.CaseDepartment==(int)department&&x.CaseId==caseId).ProjectTo<CaseClosureDTO>();
            }
            catch { throw; }
        }

        public CaseClosureDTO GetLastClosure(DepartmentEnum department, int caseId)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(x => x.CaseDepartment == (int)department && x.CaseId == caseId)
                    .OrderByDescending(x=>x.Id).ProjectTo<CaseClosureDTO>().FirstOrDefault();
            }
            catch { throw; }
        }
    }
}
