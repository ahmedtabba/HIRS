using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Repository.Pattern;
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
    public class UserNotificationGroupsBLL : Service<UserNotificationGroups>, IUserNotificationGroupsBLL
    {
        readonly IRepositoryAsync<UserNotificationGroups> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
        public UserNotificationGroupsBLL(
            IRepositoryAsync<UserNotificationGroups> repository,
            IUnitOfWorkAsync unitOfWorkAsync,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
        }

        public void UpdateUserGroups(List<int> Groups, string UserId)
        {
            try
            {
                var userGroups = _repository.Query().SelectQueryable().Where(f => f.UserId == UserId).ToList();
                _unitOfWorkAsync.BeginTransaction();
                foreach (int group in Groups)
                {
                    if (userGroups.Select(c => c.NotificationGroupId).Contains(group))
                        continue;
                    else
                    {
                        UserNotificationGroups userNotificationGroup = new UserNotificationGroups();
                        userNotificationGroup.NotificationGroupId = group;
                        userNotificationGroup.UserId = UserId;
                        userNotificationGroup.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added;
                        InsertOrUpdateGraph(userNotificationGroup);
                        _unitOfWorkAsync.SaveChanges();
                    }
                }
                foreach (var group in userGroups)
                {
                    if (!Groups.Contains(group.NotificationGroupId))
                    {
                        Delete(group.Id);
                        _unitOfWorkAsync.SaveChanges();
                    }
                }

                _unitOfWorkAsync.Commit();
            }
            catch { throw; }
        }

        public IQueryable<NotificationGroupDTO> GetAll(string UserId)
        {
            try
            {
                return _repository.Query().Include(x => x.NotificationGroup).SelectQueryable().Where(x => x.UserId == UserId).ProjectTo<NotificationGroupDTO>();

            }
            catch
            {

                throw;
            }
        }

        public IQueryable<UserNotificationGroupsDTO> GetAll()
        {
            try
            {
                return _repository.Query().Include(x => x.NotificationGroup).SelectQueryable().ProjectTo<UserNotificationGroupsDTO>();

            }
            catch
            {

                throw;
            }
        }
    }
}
