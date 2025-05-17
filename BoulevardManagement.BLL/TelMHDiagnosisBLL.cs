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
using BoulevardManagement.DTO.Resources;
namespace BoulevardManagement.BLL
{

    public class TelMHDiagnosisBLL : Service<TelMHDiagnosis>, ITelMHDiagnosisBLL
    {
        readonly IRepositoryAsync<TelMHDiagnosis> _repository; 
        readonly IRepositoryAsync<TelMHMostLikelyDiagnosis> _telMHMostLikelyDiagnosisRepository; 
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public TelMHDiagnosisBLL(
     IRepositoryAsync<TelMHDiagnosis> repository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IApplicationUserDataContext applicationContext
, IRepositoryAsync<TelMHMostLikelyDiagnosis> telMHMostLikelyDiagnosisRepository) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _telMHMostLikelyDiagnosisRepository = telMHMostLikelyDiagnosisRepository;
        }


        public int Insert(TelMHDiagnosisDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelMHDiagnosisDTO, TelMHDiagnosis>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHDiagnosis).Name.ToString(),
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
        public void Update(TelMHDiagnosisDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<TelMHDiagnosisDTO, TelMHDiagnosis>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHDiagnosis).Name.ToString(),
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
                      EntityType = typeof(TelMHDiagnosis).Name.ToString(),
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

        public TelMHDiagnosisDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelMHDiagnosisDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(TelMHDiagnosisDTO obDTO)
        {
            var res = "";
            if (_repository.Query().SelectQueryable().Where(x => x.TelMHDiagnosisCategoryId == obDTO.TelMHDiagnosisCategoryId && x.TelMHClinicalStoryId == obDTO.TelMHClinicalStoryId && x.Id != obDTO.Id).Any())
                res = CommonResource.AlreadyExist;

            return res;
        }

        public string ValidToBeDeleted(TelMHDiagnosisDTO obDTO)
        {
            var res = "";
            if (_telMHMostLikelyDiagnosisRepository.Query().SelectQueryable().Where(x => x.TelMHDiagnosisCategoryId == obDTO.TelMHDiagnosisCategoryId
             && x.TelMHClinicalStoryId == obDTO.TelMHClinicalStoryId).Any())
                res = CommonResource.CannotDeleteDiagonisis;

            return res;
        }

        public IQueryable<TelMHDiagnosisDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<TelMHDiagnosisDTO>();
            }
            catch { throw; }
        }

        public string IsValidToBeChanged(TelMHDiagnosisDTO obDTO)
        {
            var res = "";
            var oldObDTO = _repository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
            if (_telMHMostLikelyDiagnosisRepository.Query().SelectQueryable().Where(x => x.TelMHDiagnosisCategoryId == oldObDTO.TelMHDiagnosisCategoryId
             && x.TelMHClinicalStoryId == oldObDTO.TelMHClinicalStoryId).Any())
                res =CommonResource.CannotChangeDiagonisis;

            return res;
        }
    }
}
