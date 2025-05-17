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
using BoulevardManagement.DTO.Resources;
namespace BoulevardManagement.BLL
{

    public class TelMHMostLikelyDiagnosisBLL : Service<TelMHMostLikelyDiagnosis>, ITelMHMostLikelyDiagnosisBLL
    {
        readonly IRepositoryAsync<TelMHMostLikelyDiagnosis> _repository;
        readonly IRepositoryAsync<TelMHDiagnosis> _diagonisisRepository;

        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public TelMHMostLikelyDiagnosisBLL(
     IRepositoryAsync<TelMHMostLikelyDiagnosis> repository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IApplicationUserDataContext applicationContext,
      IRepositoryAsync<TelMHDiagnosis> diagonisisRepository
  ) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _diagonisisRepository = diagonisisRepository;

        }


        public int Insert(TelMHMostLikelyDiagnosisDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelMHMostLikelyDiagnosisDTO, TelMHMostLikelyDiagnosis>(obDTO);


                obEntity.TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);

                var TelMHMostLikelySubDiagnoses = Mapper.Map<ICollection<TelMHMostLikelySubDiagnosisDTO>, ICollection<TelMHMostLikelySubDiagnosis>>(obDTO.TelMHMostLikelySubDiagnoses);
                TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var diagnos in TelMHMostLikelySubDiagnoses)
                    obEntity.TelMHMostLikelySubDiagnoses.Add(diagnos);
                obEntity.TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.TelMHMostLikelyDiagnosisId = obEntity.Id);

                obEntity.ObjectState = ObjectState.Added;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHMostLikelyDiagnosis).Name.ToString(),
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
        public void Update(TelMHMostLikelyDiagnosisDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Query().Include(x => x.TelMHMostLikelySubDiagnoses).SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<TelMHMostLikelyDiagnosisDTO, TelMHMostLikelyDiagnosis>(obDTO, obEntity);
                obEntity.TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);

                var TelMHMostLikelySubDiagnoses = Mapper.Map<ICollection<TelMHMostLikelySubDiagnosisDTO>, ICollection<TelMHMostLikelySubDiagnosis>>(obDTO.TelMHMostLikelySubDiagnoses);
                TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var diagnos in TelMHMostLikelySubDiagnoses)
                    obEntity.TelMHMostLikelySubDiagnoses.Add(diagnos);
                obEntity.TelMHMostLikelySubDiagnoses.ToList().ForEach(c => c.TelMHMostLikelyDiagnosisId = obEntity.Id);
                obEntity.ObjectState = ObjectState.Modified;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHMostLikelyDiagnosis).Name.ToString(),
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
                var obEntity = _repository.Query().Include(x=>x.TelMHMostLikelySubDiagnoses).SelectQueryable().Where(x=>x.Id==id).FirstOrDefault();

                obEntity.TelMHMostLikelySubDiagnoses.ToList().ForEach(x => x.ObjectState = ObjectState.Deleted);
                _unitOfWorkAsync.SaveChanges();

                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelMHMostLikelyDiagnosis).Name.ToString(),
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

        public TelMHMostLikelyDiagnosisDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelMHMostLikelyDiagnosisDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(TelMHMostLikelyDiagnosisDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";


            return res;
        }

        public IQueryable<TelMHMostLikelyDiagnosisDTO> GetAll()
        {
            try
            {
                return _repository.Query().Include(x=>x.TelMHMostLikelySubDiagnoses).SelectQueryable().ProjectTo<TelMHMostLikelyDiagnosisDTO>();
            }
            catch { throw; }
        }

        public string IsValidToBeAdded(TelMHMostLikelyDiagnosisDTO obDTO)
        {
            string res = "";
            if (!_diagonisisRepository.Query().SelectQueryable().Where(c=>c.TelMHClinicalStoryId==obDTO.TelMHClinicalStoryId&&obDTO.TelMHDiagnosisCategoryId==obDTO.TelMHDiagnosisCategoryId).Any())
            {
                res =CommonResource.NoRelatedDiagnosis;
            }
            return res;
        }
    }
}
