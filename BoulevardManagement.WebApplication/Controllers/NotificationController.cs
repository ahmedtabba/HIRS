using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class NotificationController : BaseController
    {
        INotificationBLL _notificationBLL;

        public NotificationController(INotificationBLL notificationBLL,
             IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _notificationBLL = notificationBLL;
        }
        [Permission(RoleConsistent.Notification.Browse)]
        public ActionResult Index()
        {
            return View();
        }


        #region Notification

        [Permission(RoleConsistent.Notification.AddAndEdit)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, NotificationDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _notificationBLL.Insert(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        [Permission(RoleConsistent.Notification.AddAndEdit)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, NotificationDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _notificationBLL.Update(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        [Permission(RoleConsistent.Notification.Delete)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, NotificationDTO obDTO)
        {
            try
            {
                _notificationBLL.Delete(obDTO.Id);
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return Json(_notificationBLL.GetAll().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        #endregion
    }
}