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
using TripleA.Utilities;

namespace BoulevardManagement.BLL
{
    public class UserNotificationBLL : Service<UserNotification>, IUserNotificationBLL
    {
        private readonly IRepositoryAsync<UserNotification> _repository;
        private readonly IRepositoryAsync<Notification> _NotificationRepository;
        private readonly IRepositoryAsync<NotificationGroup> _notificationGroupRepository;
        private readonly IRepositoryAsync<UserNotificationGroups> _userNotificationGroupsRepository;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IApplicationUserDataContext _applicationContext;
        //private readonly ITelegramBotBLL _telegramBotBLL;


        public UserNotificationBLL(IRepositoryAsync<UserNotification> repository,
            IRepositoryAsync<Notification> notificationRepository,
            IRepositoryAsync<NotificationGroup> notificationGroupRepository,
            //ITelegramBotBLL telegramBotBLL,
            IRepositoryAsync<UserNotificationGroups> userNotificationGroupsRepository,
            IUnitOfWorkAsync unitOfWorkAsync,
            IApplicationUserDataContext context)
            : base(repository, context)
        {
            _repository = repository;
            _notificationGroupRepository = notificationGroupRepository;
            _NotificationRepository = notificationRepository;
            _userNotificationGroupsRepository = userNotificationGroupsRepository;
            //_telegramBotBLL = telegramBotBLL;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = context;
        }

        public NotificationDTO GetByName(string name)
        {
            try
            {
                return _NotificationRepository.Query().SelectQueryable().Where(f => f.Name == name).ProjectTo<NotificationDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public IQueryable<UserNotificationDTO> GetByUserId(string userId, List<int> favoriteNotificationsIds = null)
        {
            try
            {
                if (favoriteNotificationsIds == null)
                    return _repository.Query().Include(c => c.Notification).SelectQueryable().Where(f => f.UserId == userId).ProjectTo<UserNotificationDTO>();
                else
                    return _repository.Query().Include(c => c.Notification).SelectQueryable().Where(f => f.UserId == userId && favoriteNotificationsIds.Contains(f.NotificationId)).ProjectTo<UserNotificationDTO>();

            }
            catch { throw; }
        }



        public UserNotificationDTO GetById(int Id)
        {
            try
            {
                return _repository.Query()
                    .Include(c => c.Notification)
                    .SelectQueryable()
                    .Where(f => f.Id == Id)
                    .ProjectTo<UserNotificationDTO>()
                    .FirstOrDefault();
            }
            catch { throw; }
        }

        public bool MarkUnRead(int notificationId)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();

                var obEntity = Find(notificationId);
                obEntity.IsUnRead = true;
                obEntity.ObjectState = ObjectState.Modified;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return true;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public bool PublishNotification(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole)
        {

            try
            {

                var notificationString = "";

                var users = GetUsersWithPermissionToReciveNotification(notificationId);

                //_unitOfWorkAsync.BeginTransaction();



                notificationString = GenerateNotificationString(objectType, userName, operation, objectName, jobRole);



                foreach (var user in users)
                {

                    var userJobRole = _applicationContext.GetUsersByUserId(user).JobRole;
                    var userCanGetNotification = true;


                    if (userCanGetNotification)
                    {
                        GenerateNotification(notificationId, objectType, objectId, operation, notificationString, user, userName);
                    }
                }
                //_unitOfWorkAsync.Commit();

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }

        }





        public bool PublishNotification(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole, List<string> recivingUsers)
        {

            try
            {
                var users = GetUsersWithPermissionToReciveNotification(notificationId);

                //_unitOfWorkAsync.BeginTransaction();


                //Add Managers To Users Who recives Notifications
                var managerUsers = _applicationContext.GetUsersBy("Manager").Select(x => x.UserId).ToList();

                foreach (var user in managerUsers)
                {
                    if (!recivingUsers.Contains(user))
                        recivingUsers.Add(user);
                }

                var notificationString = GenerateNotificationString(objectType, userName, operation, objectName, jobRole);

                foreach (var user in users)
                {

                    var userCanGetNotification = true;

                    if (!recivingUsers.Contains(user))
                        userCanGetNotification = false;


                    if (userCanGetNotification)
                    {
                        GenerateNotification(notificationId, objectType, objectId, operation, notificationString, user, userName);
                    }
                }
                //_unitOfWorkAsync.Commit();

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        public bool PublishNotificationForAttachment(int notificationId, AttachmentReferenceTypes referenceType, string referenceId, string userName, string operation, string jobRole)
        {

            try
            {
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }




        private static string GenerateNotificationString(NotificationObjectTypes objectType, string userName, string operation, string objectName, string jobRole)
        {
            string notificationString;
            if (objectType == NotificationObjectTypes.Zoom)
                notificationString = $"{jobRole} ({userName}) {operation} For ({objectName})";
            else
                notificationString = $"{jobRole} ({userName}) {operation} {objectType.GetEnumDescription()} ({objectName})";
            return notificationString;
        }


        private static string GenerateNotificationStringForAttachment(string userName, string operation, AttachmentReferenceTypes referenceType, string objectNo, string referenceId, string jobRole)
        {
            string notificationString;

            notificationString = $"{jobRole} ({userName}) {operation}  Attach file on  {referenceType} {objectNo} ";

            return notificationString;
        }

        private List<string> GetUsersWithPermissionToReciveNotification(int notificationId, bool ForZoom = false)
        {
            //Get NotificationGroups which can listens to the notification which its id is notificationId
            var groups = _notificationGroupRepository
                .Query()
                .Include(c => c.GroupNotifications)
                .SelectQueryable()
                .Where(c => c.GroupNotifications.Select(d => d.Id).Contains(notificationId))
                .Select(c => c.Id).ToList();

            if (!ForZoom)
            {
                //Get the userId of the user who created the notofocation
                var senderUserId = _applicationContext.GetApplicationUserData().UserId;

                //Get All Users whose belongs to one of the notification groups which can listens to the notification notificationId But not the user who created the notofocation
                var users = _userNotificationGroupsRepository
                    .Query()
                    .SelectQueryable()
                    .Where(c => groups.Contains(c.NotificationGroupId) && c.UserId != senderUserId)
                    .Select(c => c.UserId).Distinct().ToList();
                return users;
            }
            else
            {
                var users = _userNotificationGroupsRepository
                   .Query()
                   .SelectQueryable()
                   .Where(c => groups.Contains(c.NotificationGroupId))
                   .Select(c => c.UserId).Distinct().ToList();
                return users;
            }
        }

        private void GenerateNotification(int notificationId, NotificationObjectTypes objectType, string objectId, string operation, string notificationString, string user, string username, string diractLink="")
        {
            UserNotification userNotification = new UserNotification()
            {
                NotificationId = notificationId,
                ObjectId = objectId,
                UserId = user,
                ObjectType = (int)objectType,
                NotificationString = notificationString,
                ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added,
                IsView = false,
                IsUnRead = true,
                DiractLink=diractLink
            };

            _repository.Insert(userNotification);
            _unitOfWorkAsync.SaveChanges();
            var notificationDTO = Mapper.Map<UserNotification, UserNotificationDTO>(userNotification);

            if (notificationString.Contains("Added Note on") || notificationString.Contains("Note in"))
                notificationString = notificationString.Replace("Note", "*Note*");
            //if (operation == "Delete")
            //    _telegramBotBLL.SendMessage("*🔔 Notification:* \n" + notificationString + "\n", user);
            //else
            //    _telegramBotBLL.SendMessage("*🔔 Notification:* \n" + notificationString + "\n" + ConfigurationManager.AppSettings["SystemURL"] + notificationDTO.NotificationURL.Substring(1, notificationDTO.NotificationURL.Length - 1), user);
        }

        public bool ViewAllNotification(string userId)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();

                var obEntityList = _repository.Query().SelectQueryable().Where(c => (c.IsView == false || c.IsUnRead) && c.UserId == userId).ToList();

                obEntityList.ToList().ForEach(c =>
                {
                    c.IsView = true;
                    c.ObjectState = ObjectState.Modified;
                    InsertOrUpdateGraph(c);
                }
                );

                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return true;

            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public bool ViewNotification(int notificationId)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();

                var obEntity = Find(notificationId);
                obEntity.IsView = true;
                obEntity.IsUnRead = false;
                obEntity.ObjectState = ObjectState.Modified;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return true;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public IQueryable<UserNotificationDTO> GetUserNotificationsByUserId(string userId)
        {
            try
            {
                return _repository.Query().Include(c => c.Notification).SelectQueryable().Where(f => f.CreatedBy == userId).OrderByDescending(x => x.CreationDate).ProjectTo<UserNotificationDTO>();

            }
            catch { throw; }
        }

        public bool PublishNotificationForZoomMeeting(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole,string url, List<string> recivingUsers)
        {
            try
            {
                var users = GetUsersWithPermissionToReciveNotification(notificationId, ForZoom: true);

                var notificationString = GenerateNotificationString(objectType, userName, operation, objectName, jobRole);

                foreach (var user in users)
                {

                    var userCanGetNotification = true;

                    if (!recivingUsers.Contains(user))
                        userCanGetNotification = false;


                    if (userCanGetNotification)
                    {
                        GenerateNotification(notificationId, objectType, objectId, operation, notificationString, user, userName,url);
                    }
                }
                //_unitOfWorkAsync.Commit();

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
