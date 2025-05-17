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

    public class TelMHDiagnosisCategoryBLL : Service<TelMHDiagnosisCategory>, ITelMHDiagnosisCategoryBLL
    {
        readonly IRepositoryAsync<TelMHDiagnosisCategory> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public TelMHDiagnosisCategoryBLL(
     IRepositoryAsync<TelMHDiagnosisCategory> repository,
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


        public int Insert(TelMHDiagnosisCategoryDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelMHDiagnosisCategoryDTO, TelMHDiagnosisCategory>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHDiagnosisCategory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Update(TelMHDiagnosisCategoryDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<TelMHDiagnosisCategoryDTO, TelMHDiagnosisCategory>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHDiagnosisCategory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Delete(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Find(id);
                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelMHDiagnosisCategory).Name.ToString(),
                      ObjectId = obEntity.Id,
                      ObjectRefernceNO = obEntity.Id.ToString(),
                      OperationType = OperationTypeEnum.Delete,
                      LogDescription = String.Format("{0} is deleted",
                      obEntity.Id.ToString())
                  });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TelMHDiagnosisCategoryDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelMHDiagnosisCategoryDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(TelMHDiagnosisCategoryDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";


            return res;
        }

        public IQueryable<TelMHDiagnosisCategoryDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<TelMHDiagnosisCategoryDTO>();
            }
            catch { throw; }
        }
    }
}
