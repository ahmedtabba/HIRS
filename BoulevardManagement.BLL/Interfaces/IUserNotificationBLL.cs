using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IUserNotificationBLL : IService<UserNotification>
    {
        bool PublishNotification(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole);
        bool PublishNotification(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole, List<string> recivingUsers);
        bool PublishNotificationForAttachment(int notificationId, AttachmentReferenceTypes referenceType, string referenceId, string userName, string operation, string jobRole);
        bool PublishNotificationForZoomMeeting(int notificationId, NotificationObjectTypes objectType, string objectId, string userName, string operation, string objectName, string jobRole,string url, List<string> recivingUsers);
        IQueryable<UserNotificationDTO> GetUserNotificationsByUserId(string userId);
        IQueryable<UserNotificationDTO> GetByUserId(string userId, List<int> favoriteNotificationsIds = null);
        UserNotificationDTO GetById(int Id);
        NotificationDTO GetByName(string name);
        bool ViewNotification(int notificationId);
        bool MarkUnRead(int notificationId);
        bool ViewAllNotification(string userId);
    }
}
