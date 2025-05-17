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
    public class NotificationBLL : Service<Notification>, INotificationBLL
    {
        readonly IRepositoryAsync<Notification> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
        public NotificationBLL(IRepositoryAsync<Notification> repository,
            IUnitOfWorkAsync unitOfWorkAsync,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
        }

        public IQueryable<NotificationDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<NotificationDTO>();
            }
            catch { throw; }
        }

        public NotificationDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<NotificationDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public int Insert(NotificationDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<NotificationDTO, Notification>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Update(NotificationDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<NotificationDTO, Notification>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
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


    }
}
