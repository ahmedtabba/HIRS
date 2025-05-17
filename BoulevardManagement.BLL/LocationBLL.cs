using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.DTO.Resources;
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

    public class LocationBLL : Service<Location>, ILocationBLL
    {
        readonly IRepositoryAsync<Location> _repository;
        readonly IRepositoryAsync<TeleICU> _teleICURepository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public LocationBLL(
         IRepositoryAsync<Location> repository,
         IUnitOfWorkAsync unitOfWorkAsync,
         IOperationLogBLL operationLogBLL,
         IApplicationUserDataContext applicationContext,
         IRepositoryAsync<TeleICU> teleICURepository) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _teleICURepository = teleICURepository;
        }


        public int Insert(LocationDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<LocationDTO, Location>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(Location).Name.ToString(),
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
        public void Update(LocationDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<LocationDTO, Location>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(Location).Name.ToString(),
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
                      EntityType = typeof(Location).Name.ToString(),
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

        public LocationDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<LocationDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(LocationDTO obDTO)
        {
            var res = "";
            if (_repository.Query().SelectQueryable().Where(x => x.Code == obDTO.Code && x.Id != obDTO.Id).Any())
                res = LocationResource.CodeRequired;

            if (_repository.Query().SelectQueryable().Where(x => x.Name == obDTO.Name && x.Id != obDTO.Id).Any())
                res = LocationResource.NameRequired;

            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";
            if (_teleICURepository.Query().SelectQueryable().Where(x => x.LocationId == id).Any())
                res = LocationResource.CannotDelete;

            return res;
        }

        public IQueryable<LocationDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<LocationDTO>();
            }
            catch { throw; }
        }

        /// <summary>
        /// Set Occupied Status for the location, this must be called from other bll with active unitOfWork
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <param name="IsOccupied">Location new status</param>
        public void SetStatus(int? id,bool IsOccupied)
        {
            try
            {
                if (id!=null)
                {
                    var obEntity = _repository.Query().SelectQueryable().Where(x => x.Id == id).FirstOrDefault();
                    obEntity.Occupied = IsOccupied;
                    obEntity.ObjectState = ObjectState.Modified;
                    _unitOfWorkAsync.SaveChanges();
                }
           
            }
            catch
            {

                throw;
            }
        }
    }
}
