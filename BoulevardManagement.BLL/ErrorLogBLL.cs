using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.Model.Entities;
using BoulevardManagement.DTO;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern;
using AutoMapper;
using Repository.Pattern.Infrastructure;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;

namespace BoulevardManagement.BLL
{
    public class ErrorLogBLL : Service<ErrorLog>, IErrorLogBLL
    {
        readonly IRepositoryAsync<ErrorLog> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
        public ErrorLogBLL(IRepositoryAsync<ErrorLog> repository,
            IUnitOfWorkAsync unitOfWorkAsync,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _applicationContext = applicationContex;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public void Clean()
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var errorLogs = _repository.Query().SelectQueryable().Where(x => DbFunctions.DiffDays(x.CreationDate, DateTime.Now) > BoulevardManagement.Utilities.Configurations.MaxDaysToCleanErrorLog).ToList();
                foreach (var log in errorLogs)
                    _repository.Delete(log);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public IQueryable<ErrorLogDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<ErrorLogDTO>();
            }
            catch { throw; }
        }

        public int Insert(ErrorLogDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<ErrorLogDTO, ErrorLog>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
    }
}
