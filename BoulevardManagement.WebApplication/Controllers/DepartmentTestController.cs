using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class DepartmentTestController : BaseController
    {
        private readonly IDepartmentTestBLL _departmentTestBll;

        public DepartmentTestController(IDepartmentTestBLL departmentTestBll, IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _departmentTestBll = departmentTestBll;
        }

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var departments = _departmentTestBll.GetAll();
            
            return Json(departments.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (ModelState.IsValid)
            {
                var newDepartment = _departmentTestBll.Insert(input);
                
                return Json(new[] { newDepartment }.ToDataSourceResult(request, ModelState));
            }
            
            return Json(new[] { input }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (ModelState.IsValid)
                _departmentTestBll.Update(input);
            
            return Json(new[] { input }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (input?.Id != null)
                _departmentTestBll.Delete(input.Id);

            return Json(new[] { input }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetItUsersList()
        {
            var itUsers = _departmentTestBll.GetItUsers();

            var selectListUsers = itUsers.Select(x => new SelectListItem()
            {
                Text = x.FullName,
                Value = x.UserId,
            });

            return Json(selectListUsers, JsonRequestBehavior.AllowGet);
        }
    }
}