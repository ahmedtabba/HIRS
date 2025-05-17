using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Helper;
using BoulevardManagement.WebApplication.Models.Home;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserQuickActionBLL _userQuickActionBLL;
        private readonly ITeleMentalHealthBLL _teleMentalHealthBLL;
        private readonly INPICUBLL _nPICUBLL;
        private readonly ITeleICUBLL _teleICUBLL;
        private readonly IPatientBLL _patientBLL;
        private readonly IDepartmentBLL _departmentBLL;
        IApplicationUserDataContext _applicationContext;
        private readonly IUserNotificationBLL _userNotificationBLL;

        public HomeController(
        IApplicationUserDataContext applicationContext,
        IUserNotificationBLL userNotificationBLL,
             IErrorLogBLL errorLogBLL,
            IUserQuickActionBLL userQuickActionBLL,
            ITeleMentalHealthBLL teleMentalHealthBLL,
            INPICUBLL nPICUBLL,
            ITeleICUBLL teleICUBLL,
            IPatientBLL patientBLL,
            IDepartmentBLL departmentBLL) : base(errorLogBLL)
        {
            _applicationContext = applicationContext;
            _userQuickActionBLL = userQuickActionBLL;
            _userNotificationBLL = userNotificationBLL;
            _teleMentalHealthBLL = teleMentalHealthBLL;
            _nPICUBLL = nPICUBLL;
            _teleICUBLL = teleICUBLL;
            _patientBLL = patientBLL;
            _departmentBLL = departmentBLL;
        }

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { this._roleManager = value; }
        }


        public ActionResult Index()
        {
            var MHDepartmentId = _departmentBLL.GetAll().Where(x => x.Name == "Mental Health").Select(x => x.Id).FirstOrDefault();
            var NPICUDepartmentId = _departmentBLL.GetAll().Where(x => x.Name == "NPICU").Select(x => x.Id).FirstOrDefault();
            var ICUDepartmentId = _departmentBLL.GetAll().Where(x => x.Name == "ICU").Select(x => x.Id).FirstOrDefault();

            var model = new DashboardVM();


            if (GetCurrentUser.DepartmentId == MHDepartmentId || User.Identity.IsIT())
            {
                model.PatientsInTeleMH = _patientBLL.GetAll().Where(x => x.DepartmentId == MHDepartmentId).Count();
                for (int i = 0; i < 6; i++)
                {
                    var cm = new CaseInMonth();
                    var currentMonth = DateTime.Now.AddMonths(i * -1).Month;
                    cm.MonthName = DateTime.Now.AddMonths(i * -1).ToString("MMM");
                    cm.CaseCount = _teleMentalHealthBLL.GetAll().Where(x => x.DateOfCreation.Year == DateTime.Now.Year && x.DateOfCreation.Month == currentMonth).Count();
                    model.TeleMHCases.Add(cm);
                }
                model.MHSPUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.ServiceProvider).Count();
                model.MHConUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.Consultant).Count();

                model.ShowMH = true;
            }

            if (GetCurrentUser.DepartmentId == NPICUDepartmentId || User.Identity.IsIT())
            {
                model.PatientsInNPICU = _patientBLL.GetAll().Where(x => x.DepartmentId == NPICUDepartmentId).Count();
                for (int i = 0; i < 6; i++)
                {
                    var cm = new CaseInMonth();
                    var currentMonth = DateTime.Now.AddMonths(i * -1).Month;
                    cm.MonthName = DateTime.Now.AddMonths(i * -1).ToString("MMM");
                    cm.CaseCount = _nPICUBLL.GetAll().Where(x => x.DateOfCreation.Year == DateTime.Now.Year && x.DateOfCreation.Month == currentMonth).Count();
                    model.NPICUCases.Add(cm);
                }
                model.NPICUSPUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.ServiceProvider).Count();
                model.NPICUConUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.Consultant).Count();

                model.ShowNPICU = true;

            }

            if (GetCurrentUser.DepartmentId == ICUDepartmentId || User.Identity.IsIT())
            {
                model.PatientsInICU = _patientBLL.GetAll().Where(x => x.DepartmentId == ICUDepartmentId).Count();

                for (int i = 0; i < 6; i++)
                {
                    var cm = new CaseInMonth();
                    var currentMonth = DateTime.Now.AddMonths(i * -1).Month;
                    cm.MonthName = DateTime.Now.AddMonths(i * -1).ToString("MMM");
                    cm.CaseCount = _teleICUBLL.GetAll().Where(x => x.DateOfCreation.Year == DateTime.Now.Year && x.DateOfCreation.Month == currentMonth).Count();
                    model.ICUCases.Add(cm);
                }
                model.ICUSPUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.ServiceProvider).Count();
                model.ICUConUsersCount = UserManager.Users.Where(x => x.DepartmentId == MHDepartmentId && x.JobRole == JobRole.Consultant).Count();

                model.ShowICU = true;

            }
            model.ITUsersCount = UserManager.Users.Where(x => x.JobRole == JobRole.IT).Count();

            return View(model);
        }


        public ActionResult GetNotification(string category)
        {

            List<UserNotificationDTO> res = new List<UserNotificationDTO>();
            var user = _applicationContext.GetApplicationUserData();
            DateTime selectedDay = DateTime.Now.AddDays(-2);
            if (user.UserName != "")
            {
                //old way
                //res = _userNotificationBLL.GetByUserId(user.UserId).Where(c => !c.IsView || c.IsUnRead).OrderByDescending(or => or.CreationDate).ToList();
                //var ListIds = res.Select(c => c.Id).ToList();
                //var previousRes = _userNotificationBLL.GetByUserId(user.UserId).Where(c => c.CreationDate >= selectedDay && c.IsView && !ListIds.Contains(c.Id)).OrderByDescending(or => or.CreationDate).ToList();
                //res.AddRange(previousRes);

                //current way

                if (category == "ALL")
                    res = _userNotificationBLL.GetByUserId(user.UserId).OrderByDescending(or => or.CreationDate).Take(10).ToList();
                if (category == "Read")
                    res = _userNotificationBLL.GetByUserId(user.UserId).Where(x => !x.IsUnRead).OrderByDescending(or => or.CreationDate).Take(10).ToList();
                if (category == "UnRead")
                    res = _userNotificationBLL.GetByUserId(user.UserId).Where(x => x.IsUnRead).OrderByDescending(or => or.CreationDate).Take(10).ToList();

            }
            return PartialView("_NotificationList", res);
        }


        public ActionResult GetUserNotifications()
        {

            List<UserNotificationDTO> res = new List<UserNotificationDTO>();
            var user = _applicationContext.GetApplicationUserData();
            DateTime selectedDay = DateTime.Now.AddDays(-2);
            if (user.UserName != "")
            {
                //old way
                //res = _userNotificationBLL.GetByUserId(user.UserId).Where(c => !c.IsView || c.IsUnRead).OrderByDescending(or => or.CreationDate).ToList();
                //var ListIds = res.Select(c => c.Id).ToList();
                //var previousRes = _userNotificationBLL.GetByUserId(user.UserId).Where(c => c.CreationDate >= selectedDay && c.IsView && !ListIds.Contains(c.Id)).OrderByDescending(or => or.CreationDate).ToList();
                //res.AddRange(previousRes);

                //current way
                res = _userNotificationBLL.GetUserNotificationsByUserId(_applicationContext.GetApplicationUserData().UserId).DistinctBy(x => x.NotificationId).Take(5).ToList();

            }
            return PartialView("_CurrentUserNotificationList", res);
        }

        public JsonResult ViewNotification(int notificationId)
        {
            try
            {

                var res = _userNotificationBLL.ViewNotification(notificationId);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult MarkUnRead(int notificationId)
        {
            try
            {
                var res = _userNotificationBLL.MarkUnRead(notificationId);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult MarkAsRead(List<int> notificationIds, bool isFavorate, bool isMarkAllPressed)
        {
            try
            {

                if (isMarkAllPressed)
                {
                    List<int> favoriteNotificationsId = null;
                    var user = _applicationContext.GetApplicationUserData();
                  

                    var res = _userNotificationBLL.GetByUserId(user.UserId, favoriteNotificationsId).Where(n => n.IsUnRead).Select(c => c.Id).ToList();
                    foreach (var notificationId in res)
                    {
                        _userNotificationBLL.ViewNotification(notificationId);
                    }
                }
                else
                {
                    if (notificationIds != null)
                    {
                        foreach (var notificationId in notificationIds)
                        {
                            _userNotificationBLL.ViewNotification(notificationId);

                        }
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ViewAllNotification()
        {
            try
            {

                var res = _userNotificationBLL.ViewAllNotification(GetCurrentUser.UserId);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Notification(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var notificationId = HashIdsManager.Decrypt(id);
                var notificationDTO = _userNotificationBLL.GetById(notificationId);
                if (notificationDTO == null)
                    return HttpNotFound();
                var notificationIdsList = _userNotificationBLL.GetByUserId(GetCurrentUser.UserId).Where(
                    c => c.ObjectType == notificationDTO.ObjectType && c.ObjectId == notificationDTO.ObjectId && c.IsUnRead).Select(c => c.Id).ToList();
                _userNotificationBLL.ViewNotification(notificationId);
                foreach (var Notification in notificationIdsList)
                {
                    _userNotificationBLL.ViewNotification(Notification);
                }
                return Redirect(notificationDTO.ActionURl);
            }
            else
                return RedirectToAction("Index");
        }



        public int GetUnRaedNotificationCount()
        {
            try
            {
                var user = _applicationContext.GetApplicationUserData();
                var res = _userNotificationBLL.GetByUserId(user.UserId).Where(x => x.IsUnRead).Count();
                return res;
            }
            catch
            {

                throw;
            }
        }






        public ActionResult AccessDenied()
        {
            ViewBag.ReturnUrl = null;

            if (Request.UrlReferrer != null)
            {
                if (!string.IsNullOrEmpty(Request.UrlReferrer.AbsolutePath))
                    ViewBag.ReturnUrl = Request.UrlReferrer.AbsolutePath;
            }
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {

            HttpCookie langCookie = new HttpCookie("culture", lang);
            langCookie.Expires.AddYears(1);
            HttpContext.Response.SetCookie(langCookie);
            return RedirectToAction("Index", "Home");
        }


        public void UpdateQuickAction(List<int> selectedActions)
        {
            try
            {
                if (selectedActions == null)
                    selectedActions = new List<int>();

                List<UserQuickActionDTO> obDTOList = new List<UserQuickActionDTO>();
                var userId = _applicationContext.GetApplicationUserData().UserId;
                foreach (var action in selectedActions)
                {
                    obDTOList.Add(new UserQuickActionDTO { UserId = userId, Action = (UserQuickActionEnum)action });
                }

                _userQuickActionBLL.Update(obDTOList, userId);


            }
            catch
            {

                throw;
            }
        }

        public ActionResult Notifications(string category)
        {
            var validCategories = new List<string> { "all", "read", "unread" };
            if (string.IsNullOrWhiteSpace(category) || !validCategories.Contains(category.ToLower()))
                category = "ALL";
            ViewBag.Category = category.ToUpper();
            return View();
        }


        [HttpPost]
        public ActionResult ReadUserNotifications([DataSourceRequest] DataSourceRequest request, string notificationStatus, bool isFavorate)
        {
            try
            {
                request.NormalizeDateFilter("CreationDate");
                var res = new List<UserNotificationDTO>().AsQueryable();
                var user = _applicationContext.GetApplicationUserData();
                request.NormalizeDateFilter("CreationDate");
                //  var s = _userNotificationBLL.GetByUserId(user.UserId).ToList();
                List<int> favoriteNotificationsId = null;



                if (user.UserName != "")
                {
                    if (notificationStatus == "-1")//-1  for All
                        res = _userNotificationBLL.GetByUserId(user.UserId, favoriteNotificationsId).Where(c => c.CreatorUserId != user.UserId);
                    if (notificationStatus == "0") // 0 for Readen
                        res = _userNotificationBLL.GetByUserId(user.UserId, favoriteNotificationsId).Where(n => !n.IsUnRead).Where(c => c.CreatorUserId != user.UserId);
                    if (notificationStatus == "1")// 1 for UnReaden
                        res = _userNotificationBLL.GetByUserId(user.UserId, favoriteNotificationsId).Where(n => n.IsUnRead).Where(c => c.CreatorUserId != user.UserId);
                }
                //    var ss = res.ToDataSourceResult(request);
                return new JsonResult() { Data = res.ToDataSourceResult(request), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }

        }

        public ActionResult GetDashBoardNotifications(string category)
        {
            try
            {
                var Notifications = _userNotificationBLL.GetByUserId(GetCurrentUser.UserId).OrderByDescending(c => c.CreationDate).Take(50);
                if (category == "UnRead")
                {
                    Notifications = _userNotificationBLL.GetByUserId(GetCurrentUser.UserId).OrderByDescending(c => c.CreationDate).Where(c => c.IsUnRead).Take(50);
                }
                else if (category == "Read")
                {
                    Notifications = _userNotificationBLL.GetByUserId(GetCurrentUser.UserId).OrderByDescending(c => c.CreationDate).Where(c => !c.IsUnRead).Take(50);
                }
                return PartialView("_NotificationDashboardTable", Notifications);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}