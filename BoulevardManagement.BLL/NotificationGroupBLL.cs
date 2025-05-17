using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Common;
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
    public class NotificationGroupBLL : Service<NotificationGroup>, INotificationGroupBLL
    {
        readonly IRepositoryAsync<NotificationGroup> _repository;
        private readonly IUserNotificationBLL _userNotificationBLL;
        readonly IRepositoryAsync<Notification> _notificationRepository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
        private readonly IRepositoryAsync<UserNotificationGroups> _userNotificationGroupsRepository;

        public NotificationGroupBLL(
            IRepositoryAsync<NotificationGroup> repository,
            IUserNotificationBLL userNotificationBLL,
            IRepositoryAsync<Notification> notificationRepository,
            IUnitOfWorkAsync unitOfWorkAsync,
            //IUserFavoriteNotificationBLL userFavoriteNotificationBLL,
            IRepositoryAsync<UserNotificationGroups> userNotificationGroupsRepository,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _notificationRepository = notificationRepository;
            _userNotificationBLL = userNotificationBLL;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
            _userNotificationBLL = userNotificationBLL;
            _userNotificationGroupsRepository = userNotificationGroupsRepository;
        }

        public int Create(NotificationGroupDTO group)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<NotificationGroupDTO, NotificationGroup>(group);
                obEntity.ObjectState = ObjectState.Added;
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var user = _applicationContext.GetApplicationUserData();
                _userNotificationBLL.PublishNotification(_userNotificationBLL.GetByName(NotificationConsistent.NotificationGroup.Add).Id, NotificationObjectTypes.NotificationGroup, obEntity.Id.ToString(), user.FullName, "Added", obEntity.Name, user.JobRole);
                _unitOfWorkAsync.Commit();
                return obEntity.Id;
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
                var user = _applicationContext.GetApplicationUserData();
                _userNotificationBLL.PublishNotification(_userNotificationBLL.GetByName(NotificationConsistent.NotificationGroup.Delete).Id, NotificationObjectTypes.NotificationGroup, id.ToString(), user.FullName, "Deleted", obEntity.Name, user.JobRole);
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public IQueryable<NotificationGroupDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<NotificationGroupDTO>();
            }
            catch { throw; }
        }

        public NotificationGroupDTO GetById(int id)
        {
            try
            {
                var s = _repository.Query().Include(c => c.GroupNotifications).SelectQueryable().Where(f => f.Id == id).FirstOrDefault();
                var n = AutoMapper.Mapper.Map<NotificationGroupDTO>(s);
                return _repository.Query().Include(c => c.GroupNotifications).SelectQueryable().Where(f => f.Id == id).ProjectTo<NotificationGroupDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public int Update(NotificationGroupDTO group, bool isNew)
        {

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(group.Id);
                Mapper.Map<NotificationGroupDTO, NotificationGroup>(group, obEntity);
                obEntity.GroupNotifications = new List<Notification>();
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var notifications = obEntity.GroupNotifications.ToList();
                foreach (NotificationDTO notificationDto in group.GroupNotifications)
                    if (!notifications.Select(c => c.Id).Contains(notificationDto.Id))
                        notifications.Add(_notificationRepository.Find(notificationDto.Id));
                foreach (Notification notification in notifications.ToList())
                    if (!group.GroupNotifications.Select(c => c.Id).Contains(notification.Id))
                    {
                        notifications.Remove(notification);
                    }
                obEntity.GroupNotifications = new List<Notification>(notifications);
                _unitOfWorkAsync.SaveChanges();
                var user = _applicationContext.GetApplicationUserData();
                if (!isNew)
                {
                    _userNotificationBLL.PublishNotification(_userNotificationBLL.GetByName(NotificationConsistent.NotificationGroup.Edit).Id, NotificationObjectTypes.NotificationGroup, obEntity.Id.ToString(), user.FullName, "Edited", obEntity.Name, user.JobRole);

                    _unitOfWorkAsync.SaveChanges();
                }
                _unitOfWorkAsync.Commit();
                return group.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }



        }

        public IQueryable<NotificationDTO> GetNotificationByUserId(string userId)
        {
            try
            {
                var notificationGroups = _userNotificationGroupsRepository
                .Query()
                .SelectQueryable()
                .Where(c => c.UserId == userId)
                .Select(c => c.NotificationGroup.GroupNotifications).ToList();

                var notifications = new List<Notification>();
                foreach (var group in notificationGroups)
                {
                    foreach (var notification in group.ToList())
                    {
                        if (!notifications.Contains(notification))
                        {
                            notifications.Add(notification);
                        }
                    }
                }
                return notifications.AsQueryable().ProjectTo<NotificationDTO>();
            }
            catch
            {

                throw;
            }
        }
    }
}
