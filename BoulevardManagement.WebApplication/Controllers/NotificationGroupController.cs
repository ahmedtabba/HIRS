using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class NotificationGroupController : BaseController
    {
        INotificationBLL _notificationBLL;
        INotificationGroupBLL _notificationGroupBLL;

        public NotificationGroupController
            (
            INotificationBLL notificationBLL,
            INotificationGroupBLL notificationGroupBLL,
             IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _notificationBLL = notificationBLL;
            _notificationGroupBLL = notificationGroupBLL;
        }



        [Permission(RoleConsistent.NotificationGroup.Browse)]
        public ActionResult Index()
        {
            return View();
        }


        [Permission(RoleConsistent.NotificationGroup.Add,RoleConsistent.NotificationGroup.Edit)]
        public ActionResult Create(string id)
        {
            NotificationGroupVM vmModel = new NotificationGroupVM();
            var selectedNotifications = new List<NotificationDTO>();
            if (!string.IsNullOrEmpty(id))
            {
                var dtoModel = _notificationGroupBLL.GetById(HashIdsManager.Decrypt(id));
                if (dtoModel == null)
                    return HttpNotFound();

                vmModel.Name = dtoModel.Name;
                vmModel.Id = dtoModel.EncrptedId;
                //vmModel.selected.AddRange(dtoModel.GroupNotifications.Select(c => c.Id.ToString()));
                vmModel.FinalSelectedNotifications = dtoModel.GroupNotifications.Select(c => c.Id.ToString()).ToArray();
                vmModel.Notifications = _notificationBLL.GetAll().ToList().BuildTree(dtoModel.GroupNotifications.ToList());

            }
            else
                vmModel.Notifications = _notificationBLL.GetAll().ToList().BuildTree(null);
            return View(vmModel);
        }


        [HttpPost]
        public ActionResult Create(NotificationGroupVM vmModel, SubmitFormType FormType = SubmitFormType.SaveAndExist)
        {
            bool isNew = false;
            if (ModelState.IsValid)
            {
                NotificationGroupDTO group;
                var notifications = _notificationBLL.GetAll().Select(c => c.Id.ToString()).ToList();
                if (string.IsNullOrEmpty(vmModel.Id))
                {
                    group = new NotificationGroupDTO() { Name = vmModel.Name };
                    group.Id = _notificationGroupBLL.Create(group);
                    ShowSuccessfullyAdded();
                    isNew = true;
                }
                else
                {
                    group = _notificationGroupBLL.GetById(HashIdsManager.Decrypt(vmModel.Id));
                    group.Name = vmModel.Name;
                    //result = _notificationGroupBLL.Update(group);
                }


                if (group.Id != 0)
                {
                    group.GroupNotifications = new List<NotificationDTO>();

                    foreach (string selectedNotification in vmModel.FinalSelectedNotifications.ToList().Where(c => notifications.Contains(c)))
                        group.GroupNotifications.Add(_notificationBLL.GetById(Convert.ToInt32(selectedNotification)));
                    _notificationGroupBLL.Update(group, isNew);
                    if (!isNew)
                        ShowSuccessfullyUpdated();
                }
                switch (FormType)
                {
                    case SubmitFormType.SaveAndExist:
                        return RedirectToAction("Index");
                    case SubmitFormType.SaveAndContinue:
                        return RedirectToAction("Create", new { id = group.EncrptedId });
                    case SubmitFormType.SaveAndAddNew:
                        return RedirectToAction("Create", new { id = "" });
                    default:
                        break;
                }
            }

            return View(vmModel);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var list = _notificationGroupBLL.GetAll();
                return Json(list.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.UserGroup.Delete)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, NotificationGroupDTO model)
        {
            try
            {
                _notificationGroupBLL.Delete(model.Id);
                ModelState.Clear();
            }
            catch
            {
                // throw new TripleA.Utilities.Exceptions.KendoException(ex);
                ModelState.Clear();
                var errorMessage = "";
                if (System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
                {
                    errorMessage = "هناك مستخدمين في هذا الجروب";
                }
                else
                {
                    errorMessage = "There are users in this Group.";
                }
                ModelState.AddModelError("DeleteException", errorMessage);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetNotifications()
        {
            //var groupNotificationsLst = _notificationGroupBLL.GetById(Convert.ToInt32(QueryStringEncrypting.Decrypt(groupId))).GroupNotifications.Select(c => c.Id).ToList();
            //.Where(c => !groupNotificationsLst.Contains(c.Id))
            List<NotificationDTO> notifications = _notificationBLL.GetAll().ToList();

            return Json(notifications, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNotificationGroups(string text)
        {
            try
            {
                var res = _notificationGroupBLL.GetAll();
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
    }
}