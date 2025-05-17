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
    public class UserQuickActionBLL : Service<UserQuickAction>, IUserQuickActionBLL
    {
        private readonly IRepositoryAsync<UserQuickAction> _repository;
        private readonly IOperationLogBLL _OperationLogBLL;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IApplicationUserDataContext _applicationContext;
        public UserQuickActionBLL(IRepositoryAsync<UserQuickAction> repository,
               IUnitOfWorkAsync unitOfWorkAsync,
               IOperationLogBLL OperationLogBLL,
            IApplicationUserDataContext applicationContext) : base(repository, applicationContext)
        {
            _repository = repository;
            _applicationContext = applicationContext;
            _unitOfWorkAsync = unitOfWorkAsync;
            _OperationLogBLL = OperationLogBLL;
        }

        public IQueryable<UserQuickActionDTO> GetAll()
        {
            try
            {
                return _repository
                    .Query()
                    .SelectQueryable()
                    .ProjectTo<UserQuickActionDTO>();
            }
            catch { throw; }
        }

        public IQueryable<UserQuickActionDTO> GetAllByUserId(string userId)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.UserId == userId).ProjectTo<UserQuickActionDTO>();
            }
            catch { throw; }
        }

        public void Update(List<UserQuickActionDTO> actionsList, string userId)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntityList = _repository.Query().SelectQueryable().Where(x => x.UserId == userId).ToList();
                foreach (var action in obEntityList)
                {
                    _repository.DeleteAsync(action.Id);
                }

                _unitOfWorkAsync.SaveChanges();

                foreach (var action in actionsList)
                {
                    var obEntity = Mapper.Map<UserQuickActionDTO, UserQuickAction>(action);
                    obEntity.ObjectState = ObjectState.Added;
                    InsertOrUpdateGraph(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                }

                _unitOfWorkAsync.Commit();

            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
    }
}
