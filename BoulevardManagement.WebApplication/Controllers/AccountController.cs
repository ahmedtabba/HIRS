using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.Utilities;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.DTO;
using TripleA.Utilities.HashidsNet;
using BoulevardManagement.Model.Common;
using BoulevardManagement.WebApplication.Helper;

namespace BoulevardManagement.WebApplication.Controllers
{

    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationGroupManager _groupManager;
        public AccountController() : base(null)
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, ApplicationGroupManager groupManager, IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            GroupManager = groupManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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
                return this._roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { this._roleManager = value; }
        }

        public ApplicationGroupManager GroupManager
        {
            get
            {
                return this._groupManager ?? HttpContext.GetOwinContext().Get<ApplicationGroupManager>();
            }
            private set { this._groupManager = value; }
        }

        public ActionResult Edit()
        {
            var id = User.Identity.GetUserId();
            var user = UserManager.Users.Where(u => u.Id == id).FirstOrDefault();

            var userVM = new UsersViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = Url.Content(string.Format("{0}{1}.jpg?timestamp=" + DateTime.Now.Ticks, Configurations.ProfileVirtualPath, user.Id)),
            };

            return View(userVM);
        }

        [HttpPost]
        public ActionResult Edit(UsersViewModel userVM, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    var s = context.Users.Find(userVM.Id);
                    s.FullName = userVM.FullName;
                    s.PhoneNumber = userVM.PhoneNumber;
                    if (upload != null)
                    {
                        // System.IO.File.Delete( userVM.PhotoUrl.Substring(0,userVM.PhotoUrl.IndexOf('?')));
                        string extention = upload.FileName.Substring(upload.FileName.IndexOf('.'));
                        string path = Path.Combine(/*Server.MapPath(Configurations.ProfilePhysicalPath)*/Server.MapPath("~/Uploads/Profile"), userVM.Id + ".jpg");
                        upload.SaveAs(path);
                        s.HasPhoto = true;
                    }
                    context.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    ShowSuccessfullyUpdated($"user ({userVM.FullName})");

                }
            }
            return RedirectToAction("Index", "Home");

        }



        public JsonResult GetUsers(string text)
        {
            try
            {
                var users = from user in UserManager.Users
                            select user;

                if (!string.IsNullOrEmpty(text))
                    users = users.Where(x => x.UserName.Contains(text));
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

        public IQueryable<ApplicationUser> GetUsersInRole(string role)
        {
            return from user in UserManager.Users
                   where user.Roles.Any(r => r.RoleId == role)
                   select user;
        }

        public JsonResult GetUsersBasedOnJobAndDepartmentId(JobRole jobRole, int DepartmentId)
        {
            try
            {
                var users = from user in UserManager.Users
                            where user.JobRole == jobRole && user.DepartmentId == DepartmentId
                            select user;
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }
        public JsonResult GetUsersBasedOnJob(JobRole jobRole)
        {
            try
            {
                var users = from user in UserManager.Users
                            where user.JobRole == jobRole
                            select user;
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        public IQueryable<ApplicationRole> GetRoles()
        {
            return RoleManager.Roles;
        }

        public JsonResult GetApplicationRoles(string text)
        {

            var UserRoles = RoleManager.Roles.ToList();
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPermisions(string id)
        {
            var userRoles = RoleManager.Roles.Where(c => c.Users.Any(u => u.UserId == id)).ToList().BuildTree(isGroomingEnabled: false);
            //userRoles.RemoveAll(d => !d.Items.Any());
            var treeViewItemModel = new TreeViewItemModel() { Text = "Permissions", Expanded = true };
            treeViewItemModel.Items.AddRange(userRoles);
            return Json(userRoles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationGroups(string text)
        {
            var list = GroupManager.Groups.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserRoles(string roles)
        {
            //UserManager.Users
            List<string> rolesLst = roles.Split(',').Select(c => c.Trim()).ToList();
            rolesLst.ForEach(c => c.Trim());
            var UserRoles = RoleManager.Roles.Where(c => rolesLst.Contains(c.Name)).ToList();


            return Json(UserRoles.Select(c => c.Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserGroups(string groupIds)
        {
            List<ApplicationGroup> userGroups = new List<ApplicationGroup>();

            if (!string.IsNullOrEmpty(groupIds))
            {
                List<string> groupList = groupIds.Split(',').Select(c => c.Trim()).ToList();
                userGroups = GroupManager.Groups.Where(c => groupList.Contains(c.Id)).ToList();
            }

            return Json(userGroups.Select(c => c.Id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int? notificationGroupId)
        {
            try
            {
                var UsersVM = new List<UsersViewModel>();
                var DepartmentService = DependencyResolver.Current.GetService(typeof(IDepartmentBLL));

                foreach (var user in UserManager.Users.ToList())
                {
                    UsersViewModel userVM = new UsersViewModel { Email = user.Email, FullName = user.FullName, Id = user.Id, PhoneNumber = user.PhoneNumber, HasPhoto = user.HasPhoto, IsLockedOut = user.LockoutEndDateUtc != null ,Gender=user.Gender,Descreption=user.Descreption };
                    if (user.HasPhoto)
                        userVM.PhotoUrl = Url.Content(string.Format("{0}{1}.jpg?timestamp=" + DateTime.Now.Ticks, Configurations.ProfileVirtualPath, user.Id));
                    else
                        userVM.PhotoUrl = Url.Content(string.Format("{0}user.png", Configurations.ProfileVirtualPath));

                    var roleNames = UserManager.GetRoles(user.Id).ToList();
                    var roles = RoleManager.Roles.Where(r => roleNames.Any(c => c == r.Name)).ToList();
                    userVM.RoleNames = string.Join(", ", roles.Select(d => d.Description));
                    var userGroups = this.GroupManager.GetUserGroups(user.Id);
                    userVM.GroupNames = string.Join(", ", userGroups.Select(d => d.Name).ToList());
                    userVM.GroupIds = string.Join(", ", userGroups.Select(d => d.Id).ToList());
                    userVM.DepartmentId = user.DepartmentId;
                    if (userVM.DepartmentId != null)
                    {
                        if (System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
                        {
                            userVM.DepartmentName = (DepartmentService as BLL.DepartmentBLL).GetAll().Where(c => c.Id == userVM.DepartmentId.Value).Select(c => c.ArabicName).FirstOrDefault();
                        }
                        else
                        {
                            userVM.DepartmentName = (DepartmentService as BLL.DepartmentBLL).GetAll().Where(c => c.Id == userVM.DepartmentId.Value).Select(c => c.Name).FirstOrDefault();

                        }
                    }
                    userVM.CreationDate = user.CreationDate;
                    var userNotificationGroupService = DependencyResolver.Current.GetService(typeof(IUserNotificationGroupsBLL));
                    var notificationGroups = (userNotificationGroupService as BLL.UserNotificationGroupsBLL).GetAll(user.Id).ToList();

                    //NotificationGroup Filter
                    #region NotificationGroup Filter
                    if (notificationGroupId != null && notificationGroups.Where(x => x.Id == notificationGroupId).FirstOrDefault() == null)
                        continue;
                    #endregion

                    userVM.NotificationGroupsNames = string.Join(", ", notificationGroups.Select(d => d.Name).ToList());

                    userVM.JobRole = user.JobRole;
                    UsersVM.Add(userVM);
                }
                return Json(UsersVM.AsQueryable().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }
        }

        [Permission(RoleConsistent.UserManagement.Browse)]
        public ActionResult GetUsersInNotifcationGroup([DataSourceRequest] DataSourceRequest request, string notificationGroupId)
        {
            try
            {
                var UsersVM = new List<UsersViewModel>();

                var users = UserManager.Users.ToList();

                foreach (var user in users)
                {
                    UsersViewModel userVM = new UsersViewModel { Email = user.Email, FullName = user.FullName, Id = user.Id, PhoneNumber = user.PhoneNumber, HasPhoto = user.HasPhoto, IsLockedOut = user.LockoutEndDateUtc != null,Gender=user.Gender,Descreption=user.Descreption};


                    var roleNames = UserManager.GetRoles(user.Id).ToList();
                    var roles = RoleManager.Roles.Where(r => roleNames.Any(c => c == r.Name)).ToList();
                    userVM.RoleNames = string.Join(", ", roles.Select(d => d.Description));
                    var userGroups = this.GroupManager.GetUserGroups(user.Id);
                    userVM.JobRole = user.JobRole;


                    var notificationGroupPlanId = HashIdsManager.Decrypt(notificationGroupId);




                    var userNotificationGroupService = DependencyResolver.Current.GetService(typeof(IUserNotificationGroupsBLL));
                    var notificationGroups = (userNotificationGroupService as BLL.UserNotificationGroupsBLL).GetAll().Where(x => x.NotificationGroupId == notificationGroupPlanId && x.UserId == user.Id).ToList();

                    if (notificationGroups.Count > 0)
                        UsersVM.Add(userVM);
                }



                return Json(UsersVM.AsQueryable().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }
        }

        [Permission(RoleConsistent.UserManagement.Browse)]
        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ChangeUserRole(string Id, List<string> userRoles)
        {
            var res = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var currentRoles = UserManager.Users.Where(u => u.Id == Id).FirstOrDefault().Roles.Select(p => p.RoleId);

                    foreach (var userRole in userRoles)
                    {
                        if (!currentRoles.Contains(userRole))
                        {
                            var newRole = RoleManager.FindById(userRole);
                            UserManager.AddToRole(Id, newRole.Name);
                        }
                    }
                    foreach (var currentRole in currentRoles.ToList())
                    {
                        if (!userRoles.Contains(currentRole))
                        {
                            var newRole = RoleManager.FindById(currentRole);
                            UserManager.RemoveFromRole(Id, newRole.Name);
                        }
                    }
                }
                else
                {
                    foreach (var k in ModelState.Keys)
                    {
                        if (ModelState[k].Errors.Count > 0)
                        {
                            res += ModelState[k].Errors.ToList().Select(x => x.ErrorMessage).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ChangeUserGroup(string userId, List<string> userGroups)
        {
            var res = "";
            try
            {
                if (userGroups == null)
                    userGroups = new List<string>();

                if (ModelState.IsValid)
                {
                    GroupManager.SetUserGroups(userId, userGroups.ToArray());
                    var user = UserManager.Users.Where(u => u.Id == userId).FirstOrDefault();
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();
                }
                else
                {
                    foreach (var k in ModelState.Keys)
                    {
                        if (ModelState[k].Errors.Count > 0)
                        {
                            res += ModelState[k].Errors.ToList().Select(x => x.ErrorMessage).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult ChangeUserRole(UserRoleViewModel model)
        //{
        //    var res = "";
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            var role = RoleManager.FindById(model.RoleId);
        //            if (role != null)
        //            {
        //                var roles = UserManager.GetRoles(model.Id);
        //                roles.ToList().ForEach(x =>
        //                {

        //                    UserManager.RemoveFromRole(model.Id, x);
        //                });
        //                UserManager.AddToRole(model.Id, role.Name);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var k in ModelState.Keys)
        //            {
        //                if (ModelState[k].Errors.Count > 0)
        //                {
        //                    res += ModelState[k].Errors.ToList().Select(x => x.ErrorMessage).FirstOrDefault();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        res = e.Message;
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);

        //}


        [HttpPost]
        public async Task<ActionResult> ChangeUserPhoto(string PhotoUserId, string filename = "", string filePath = "")
        {
            var res = "";
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var file = new FileInfo(filePath);
                    string destenationFileName = PhotoUserId.ToString() + file.Extension;
                    System.IO.File.Copy(file.FullName, Path.Combine(Configurations.ProfilePhysicalPath, destenationFileName), true);
                    var user = UserManager.Users.FirstOrDefault(p => p.Id == PhotoUserId);

                    user.HasPhoto = true;

                    var result = await UserManager.UpdateAsync(user);
                }
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUserPhoto(string PhotoUserId)
        {
            var res = "";
            try
            {

                //var file = string.Format("{0}{1}.jpg?timestamp=" + DateTime.Now.Ticks, Configurations.ProfileVirtualPath, PhotoUserId);
                //file = file.Replace("/", "//");
                ////string destenationFileName = PhotoUserId.ToString() + file.Extension;
                //System.IO.File.Delete(file);
                var user = UserManager.Users.FirstOrDefault(p => p.Id == PhotoUserId);

                user.HasPhoto = false;

                var result = await UserManager.UpdateAsync(user);

            }
            catch (Exception e)
            {
                res = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        //[LayoutInjecter("_Layout")]
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl)&&returnUrl.Contains("LogOffBySys"))
                returnUrl = "";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[LayoutInjecter("_Layout")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.RememberMe = false;
            foreach (var key in ModelState.Keys)
                if (key.Contains("RememberMe"))
                    ModelState[key].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {

                case SignInStatus.Success:
                    //var ctx = System.Web.HttpContext.Current;
                    //ctx.Session["useaname"] = User.Identity.GetFullName();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:

                    ModelState.AddModelError("", System.Globalization.CultureInfo.CurrentCulture.TextInfo.IsRightToLeft? "معلومات تسجيل الدخول غير صحيحة":"Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        public ActionResult Register(string id)
        {
            RegisterViewModel model;
            if (!String.IsNullOrEmpty(id))
            {
                var user = UserManager.Users.FirstOrDefault(p => p.Id == id);
                var userId = user.Id;
                model = new RegisterViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    IsLockedOut = user.LockoutEndDateUtc != null,
                    JobRole = user.JobRole,
                    DepartmentId = user.DepartmentId,
                    CreationDate = user.CreationDate,
                    Gender=user.Gender,
                    Descreption=user.Descreption
               
                };
                model.SelectedNotificationGroups = ApplicationUserManager.GetUserNotificationGroups(id).Select(c => c.NotificationGroup).ToList();
                var userGroups = this.GroupManager.GetUserGroups(user.Id);
                model.UserGroups = this.GroupManager.GetUserGroups(user.Id).ToList();
                model.UserGroups = this.GroupManager.GetUserGroups(user.Id).ToList();
                model.HasPhoto = user.HasPhoto;

                model.PhotoUrl = model.HasPhoto ? Url.Content(string.Format("{0}{1}.jpg?timestamp=" + DateTime.Now.Ticks, Configurations.ProfileVirtualPath, user.Id)) : "";

                var _service = DependencyResolver.Current.GetService(typeof(INotificationGroupBLL));
                model.NotificationGroups.AddRange((_service as BLL.NotificationGroupBLL).GetAll().Where(c => (model.SelectedNotificationGroups.Contains(c.Id))));

                ViewBag.Title = string.Format("Edit User: {0}", user.FullName);

                var _QAService = DependencyResolver.Current.GetService(typeof(IUserQuickActionBLL));
                if (User.Identity.GetUserId() == user.Id)
                {
                    model.SelectedQuickActions.AddRange((_QAService as BLL.UserQuickActionBLL).GetAll().Where(c => c.UserId == userId).ToList().Select(x => (int)x.Action).ToList());

                }
                ViewBag.CurrentUserId = User.Identity.GetUserId();
            }
            else
            {
                model = new RegisterViewModel() { CreationDate = DateTime.Now };
                ViewBag.Title = "Create New User";

            }
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model, List<string> Roles)
        //public async Task<ActionResult> Register(RegisterViewModel model, string filename = "", string filePath = "")
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase upload)
        {
            try
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (!string.IsNullOrWhiteSpace(model.Id))
                {
                    ModelState.Remove("Password");
                    foreach (var key in ModelState.Keys)
                    {
                        if (key.Contains("Name"))
                            ModelState[key].Errors.Clear();

                        if (key.Contains("Date"))
                            ModelState[key].Errors.Clear();

                    }
                }
                //if (model!=null && !model.HasPhoto)
                //{
                //    switch (model.Gender)
                //    {
                //        case Gender.Male:
                //            model.PhotoUrl = "/Content/img/uossm-01.png";
                //            model.HasPhoto = true;
                //            break;
                //        case Gender.Female:
                //            model.PhotoUrl = "/Content/img/uossm-01.png";
                //            model.HasPhoto = true;
                //            break;
                //        default:
                //            break;
                //    }
                //}
                if (!User.IsInRole(RoleConsistent.UserManagement.Edit) && model.Id == User.Identity.GetUserId())
                {
                    if (model != null && ModelState.IsValid)
                    {
                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {

                            var s = context.Users.Find(model.Id);
                            var Gender = s.Gender;
                            s.FullName = model.FullName;
                            s.PhoneNumber = model.PhoneNumber;
                            s.DepartmentId = model.DepartmentId;
                            s.CreationDate = model.CreationDate;
                            s.Gender = model.Gender;
                            s.Descreption = model.Descreption;
                            if (upload != null)
                            {
                                // System.IO.File.Delete( userVM.PhotoUrl.Substring(0,userVM.PhotoUrl.IndexOf('?')));
                                string extention = upload.FileName.Substring(upload.FileName.IndexOf('.'));
                                string path = Path.Combine(/*Server.MapPath(Configurations.ProfilePhysicalPath)*/Server.MapPath("~/Uploads/Profile"), model.Id + ".jpg");
                                upload.SaveAs(path);
                                s.HasPhoto = true;
                            }
                         
                           
                            context.Entry(s).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            ShowSuccessfullyUpdated();

                            if (upload ==null)
                            {
                                if (model.Gender!= Gender)
                                {
                                  //  LogOffBySys();
                                 return   RedirectToAction("LogOffBySys", "Account", new { Area = "" });
                                }
                            }
                            else
                            {
                                return RedirectToAction("LogOffBySys", "Account", new { Area = "" });
                            }
                            if (User.IsInRole(RoleConsistent.UserManagement.Browse))
                                return RedirectToAction("Users", "Account", new { Area = "" });
                            else
                                return RedirectToAction("Index", "Home", new { Area = "" });


                        }
                    }
                }


                if (model != null && ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(model.Id))
                    {

                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, PhoneNumber = model.PhoneNumber, JobRole = model.JobRole, DepartmentId = model.DepartmentId, CreationDate = model.CreationDate, HasPhoto = upload != null ,Gender=model.Gender,Descreption=model.Descreption};

                        List<int> selectedNotificationGroups = model.SelectedNotificationGroups.ToList();
                        foreach (int notificationGroup in selectedNotificationGroups)
                            user.NotificationGroups.Add(new UserNotificationGroup() { NotificationGroup = notificationGroup });

                        List<string> userGroups = model.GroupIds.ToList();
                        foreach (string uGroup in userGroups)
                            user.Groups.Add(new ApplicationUserGroup() { ApplicationGroupId = uGroup, ApplicationUserId = user.Id });

                        var _service = DependencyResolver.Current.GetService(typeof(INotificationGroupBLL));
                        model.NotificationGroups.AddRange((_service as BLL.NotificationGroupBLL).GetAll().Where(c => (model.SelectedNotificationGroups.Contains(c.Id))));

                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var UserNotificationGroupsService = DependencyResolver.Current.GetService(typeof(IUserNotificationGroupsBLL));
                            (UserNotificationGroupsService as BLL.UserNotificationGroupsBLL).UpdateUserGroups(selectedNotificationGroups, user.Id);

                            var userId = User.Identity.GetUserId();
                            if (upload != null)
                            {
                                // System.IO.File.Delete( userVM.PhotoUrl.Substring(0,userVM.PhotoUrl.IndexOf('?')));
                                string extention = upload.FileName.Substring(upload.FileName.IndexOf('.'));
                                string path = Path.Combine(/*Server.MapPath(Configurations.ProfilePhysicalPath)*/Server.MapPath("~/Uploads/Profile"), user.Id + ".jpg");
                                upload.SaveAs(path);
                            }
                            var currentUser = UserManager.Users.Where(s => s.Id == userId).FirstOrDefault();
                            var userNotificationService = DependencyResolver.Current.GetService(typeof(IUserNotificationBLL));
                            (userNotificationService as BLL.UserNotificationBLL).PublishNotification((userNotificationService as BLL.UserNotificationBLL).GetByName(NotificationConsistent.UserManagement.Add).Id, NotificationObjectTypes.User, user.Id, currentUser.FullName, "Added", user.FullName, currentUser.JobRole.ToString());
                            GroupManager.SetUserGroups(user.Id, model.GroupIds.ToArray());
                            ShowSuccessfullyAdded();

                            var tokens = new Dictionary<string, string>();
                            tokens.Add("@Email", model.Email);
                            tokens.Add("@FullName", model.FullName);
                            tokens.Add("@PhoneNumber", model.PhoneNumber);
                            tokens.Add("@Role", model.JobRole.GetDescription());
                            EmailUtil.SendMail(MailTemplates.UserAdded, new List<string> { model.Email }, tokens);

                        
                            if (User.IsInRole(RoleConsistent.UserManagement.Browse))
                                return RedirectToAction("Users", "Account", new { Area = "" });
                            else
                                return RedirectToAction("Index", "Home", new { Area = "" });
                        }
                        AddErrors(result);
                    }
                    else
                    {
                        var user = UserManager.Users.FirstOrDefault(p => p.Id == model.Id);
                        var Gender = user.Gender;
                        var userNotificationGroups = ApplicationUserManager.GetUserNotificationGroups(user.Id);
                        if (upload != null)
                        {
                            // System.IO.File.Delete( userVM.PhotoUrl.Substring(0,userVM.PhotoUrl.IndexOf('?')));
                            string extention = upload.FileName.Substring(upload.FileName.IndexOf('.'));
                            string path = Path.Combine(/*Server.MapPath(Configurations.ProfilePhysicalPath)*/Server.MapPath("~/Uploads/Profile"), model.Id + ".jpg");
                            upload.SaveAs(path);
                            user.HasPhoto = true;
                        }
                        user.UserName = model.Email;
                        user.Email = model.Email;
                        user.FullName = model.FullName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.DepartmentId = model.DepartmentId;
                        user.Gender = model.Gender;
                        user.Descreption = model.Descreption;
                        List<int> notificationGroupLst = new List<int>();
                        if (model.SelectedNotificationGroups != null)
                            notificationGroupLst = model.SelectedNotificationGroups.ToList();

                        user.JobRole = model.JobRole;
                        if (model.IsLockedOut && user.LockoutEndDateUtc == null)
                        {
                            user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(50);
                            //UserManager.SetLockoutEnabled
                        }
                        else if (!model.IsLockedOut)
                        {
                            user.LockoutEndDateUtc = null;
                        }

                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {
                            foreach (UserNotificationGroup userNotificationGroup in userNotificationGroups.Where(c => !notificationGroupLst.Contains(c.NotificationGroup)))
                            {
                                var s = context.UserNotificationGroups.Find(userNotificationGroup.Id);
                                context.Entry(s).State = System.Data.Entity.EntityState.Deleted;
                                context.SaveChanges();
                            }
                        }

                        foreach (int notificationGroup in notificationGroupLst)
                            user.NotificationGroups.Add(new UserNotificationGroup() { NotificationGroup = notificationGroup });

                        var result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            ShowSuccessfullyUpdated();
                            var currentUserId = User.Identity.GetUserId();
                            var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();

                            var userNotificationService = DependencyResolver.Current.GetService(typeof(IUserNotificationBLL));
                            (userNotificationService as BLL.UserNotificationBLL).PublishNotification((userNotificationService as BLL.UserNotificationBLL).GetByName(NotificationConsistent.UserManagement.Edit).Id, NotificationObjectTypes.User, user.Id, currentUser.FullName, "Edited", user.FullName, currentUser.JobRole.ToString());

                            //var container = new Microsoft.Practices.Unity.UnityContainer();

                            //DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));

                            GroupManager.SetUserGroups(model.Id, model.GroupIds.ToArray());

                            object UserNotificationGroupsService = DependencyResolver.Current.GetService(typeof(IUserNotificationGroupsBLL));
                            //UserNotificationGroupsService = DependencyResolver.Current.GetService(typeof(IUserNotificationGroupsBLL));
                            (UserNotificationGroupsService as BLL.UserNotificationGroupsBLL).UpdateUserGroups(notificationGroupLst, user.Id);
                            if (upload == null && model.Id == GetCurrentUser.UserId)
                            {
                                if (model.Gender != Gender)
                                {
                                 //   LogOffBySys();
                                   return RedirectToAction("LogOffBySys", "Account", new { Area = "" });
                                }
                            }
                            if (upload!=null &&model.Id==GetCurrentUser.UserId)
                            {
                                return RedirectToAction("LogOffBySys", "Account", new { Area = "" });
                            }
                            if (User.IsInRole(RoleConsistent.UserManagement.Browse))
                                return RedirectToAction("Users", "Account", new { Area = "" });
                            else
                                return RedirectToAction("Index", "Home", new { Area = "" });

                        }
                        AddErrors(result);
                    }

                }
                ViewBag.Errors = "Error";
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                var currentUserId = User.Identity.GetUserId();
                var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();
                var userNotificationService = DependencyResolver.Current.GetService(typeof(IUserNotificationBLL));
                (userNotificationService as BLL.UserNotificationBLL).PublishNotification((userNotificationService as BLL.UserNotificationBLL).GetByName(NotificationConsistent.UserManagement.ChangePermitionGroupForUser).Id, NotificationObjectTypes.User, user.Id, currentUser.FullName, "Changeed Permition Group For User", user.FullName, currentUser.JobRole.ToString());
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult LogOffBySys()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }





        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}