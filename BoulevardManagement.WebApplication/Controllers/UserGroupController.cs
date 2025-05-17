using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.DTO;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class UserGroupController : BaseController
    {
        public UserGroupController(IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {

        }
        private ApplicationGroupManager _groupManager;
        public ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? new ApplicationGroupManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var list = GroupManager.Groups;
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

        public ActionResult Edit(string id = "")
        {
            UserGroupVM model = new UserGroupVM();
            var selectedRoles = new List<ApplicationRole>();
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationGroup applicationgroup = this.GroupManager.Groups.FirstOrDefault(g => g.Id == id);

                if (applicationgroup == null)
                    return RedirectToAction("Index");

                selectedRoles = this.GroupManager.GetGroupRoles(applicationgroup.Id).ToList().OrderBy(c => c.Name).ToList();

                model = new UserGroupVM();
                model.Id = applicationgroup.Id;
                model.Name = applicationgroup.Name;
            }

            model.Roles = RoleManager.Roles.OrderBy(d => d.Name).ToList().BuildTree(selectedRoles);
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UserGroupVM model, FormCollection values, SubmitFormType FormType = SubmitFormType.SaveAndExist)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result;
                ApplicationGroup group;
                if (string.IsNullOrEmpty(model.Id))
                {
                    group = new ApplicationGroup() { Name = model.Name };
                    result = this.GroupManager.CreateGroup(group);
                    if (result.Succeeded)
                    {
                        var currentUserId = User.Identity.GetUserId();
                        var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();
                        ShowSuccessfullyAdded();
                    }
                }
                else
                {
                    group = this.GroupManager.Groups.Single(d => d.Id == model.Id);
                    group.Name = model.Name;
                    result = this.GroupManager.UpdateGroup(group);
                    if (result.Succeeded)
                    {
                        var currentUserId = User.Identity.GetUserId();
                        var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();
                        ShowSuccessfullyUpdated();
                    }
                }

                var finalRoles = RoleManager.Roles.Where(s => model.FinalSelectedRoles.Any(d => d == s.Id));

                if (result.Succeeded)
                {

                    GroupManager.SetGroupRoles(group.Id, finalRoles.Select(s => s.Name).ToArray());

                }

                switch (FormType)
                {
                    case SubmitFormType.SaveAndExist:
                        return RedirectToAction("Index");
                    case SubmitFormType.SaveAndContinue:
                        return RedirectToAction("Edit", new { id = group.Id });
                    case SubmitFormType.SaveAndAddNew:
                        return RedirectToAction("Edit", new { id ="" });
                    default:
                        break;
                }
            }

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ApplicationGroup model)
        {
            try
            {
                GroupManager.DeleteGroup(model.Id);
                var currentUserId = User.Identity.GetUserId();
                var currentUser = UserManager.Users.Where(s => s.Id == currentUserId).FirstOrDefault();
                ModelState.Clear();
            }
            catch
            {
              //  throw new TripleA.Utilities.Exceptions.KendoException(ex);
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
    }
}