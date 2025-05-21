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
            var departments = _departmentTestBll.GetAll().ToList();
            
            return Json(departments.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (ModelState.IsValid)
            {
                var newDepartment = _departmentTestBll.Insert(input);
                
                return Json(new[] { newDepartment }.ToDataSourceResult(request, ModelState));
            }
            
            return Json(new[] { input }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (ModelState.IsValid)
                _departmentTestBll.Update(input);
            
            return Json(new[] { input }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, DepartmentTestDTO input)
        {
            if (input?.Id != null)
                _departmentTestBll.Delete(input.Id);

            return Json(new[] { input }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetInMemoryItUsers()
        {
            var allUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    FullName = "John Doe",
                    JobRole = JobRole.ServiceProvider,
                    Email = "john.doe@example.com",
                    CreationDate = DateTime.Now.AddYears(-2),
                    Gender = Gender.Male
                },
                new ApplicationUser
                {
                    Id = "2",
                    FullName = "Jane Smith",
                    JobRole = JobRole.IT,
                    Email = "jane.smith@example.com",
                    CreationDate = DateTime.Now.AddYears(-3),
                    Gender = Gender.Female
                },
                new ApplicationUser
                {
                    Id = "3",
                    FullName = "Tom Lee",
                    JobRole = JobRole.Consultant,
                    Email = "tom.lee@example.com",
                    CreationDate = DateTime.Now.AddYears(-1),
                    Gender = Gender.Male
                },
                new ApplicationUser
                {
                    Id = "4",
                    FullName = "Anna Green",
                    JobRole = JobRole.IT,
                    Email = "anna.green@example.com",
                    CreationDate = DateTime.Now.AddMonths(-10),
                    Gender = Gender.Female
                }
            };
            
            var selectListUsers = allUsers.Where(x => x.JobRole == JobRole.IT).Select(x => new SelectListItem()
            {
                Text = x.FullName,
                Value = x.Id,
            });

            return Json(selectListUsers, JsonRequestBehavior.AllowGet);
        }
    }
}