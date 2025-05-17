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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL
{
    public class OperationLogBLL: Service<OperationLog>, IOperationLogBLL
    {
        readonly IRepositoryAsync<OperationLog> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;

        public OperationLogBLL(
            IRepositoryAsync<OperationLog> repository,
            IUnitOfWorkAsync unitOfWorkAsync,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
        }

        public IQueryable<OperationLogDTO> GetAll(string EntityType, int ObjectId)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(c => (c.ObjectId == ObjectId || ObjectId == 0) && (c.EntityType == EntityType || String.IsNullOrEmpty(EntityType))).ProjectTo<OperationLogDTO>();
            }
            catch { throw; }
        }

        public IQueryable<OperationLogDTO> GetAll(string EntityType, int ObjectId, DateTime fromDate, DateTime toDate, int operationType, string searchText)
        {
            try
            {
                var otype = Convert.ToInt32(operationType);
                return _repository
                    .Query()
                    .SelectQueryable()
                    .Where(c => (c.ObjectId == ObjectId || ObjectId == 0)
                    && (c.EntityType == EntityType || ( c.EntityType.Contains(EntityType)) || String.IsNullOrEmpty(EntityType))
                    && (c.OperationType == otype || otype == -1)
                    && (searchText == "" || (searchText != "" && (c.LogDescription.Contains(searchText) || c.ObjectRefernceNO.Contains(searchText) || c.MentionedObjectRefernceNO.Contains(searchText) || c.EntityType.Contains(searchText))))
                    && (DbFunctions.TruncateTime(c.CreationDate) >= fromDate.Date && DbFunctions.TruncateTime(c.CreationDate) <= toDate.Date))
                    .OrderByDescending(c => c.CreationDate).ProjectTo<OperationLogDTO>();
            }
            catch { throw; }
        }

        public int Insert(OperationLogDTO obDTO)
        {
            try
            {
                var obEntity = Mapper.Map<OperationLogDTO, OperationLog>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                obEntity.UserName = _applicationContext.GetApplicationUserData().FullName;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                return obEntity.Id;
            }
            catch { throw; }
        }
    }
}
