using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class EmployeeTestController : BaseController
    {
        private readonly IEmployeeTestBLL _employeeTestBll;

        public EmployeeTestController(IEmployeeTestBLL employeeTestBll, IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _employeeTestBll = employeeTestBll;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var departments = _employeeTestBll.GetAll();
            
            return Json(departments.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            var employee = new EmployeeTestDTO();
            
            return View("AddOrUpdate", employee);
        }
        
        public ActionResult Update(int id)
        {
            var employee = id == 0 ? new EmployeeTestDTO() : _employeeTestBll.GetById(id);
            
            return View("AddOrUpdate", employee);
        }
        
        [HttpPost]
        public ActionResult Update(EmployeeTestDTO input, SubmitFormType formType = SubmitFormType.SaveAndExist)
        {
            if (ModelState.IsValid)
            {
                if (input.Id == 0)
                {
                    input.Id = _employeeTestBll.Insert(input);
                    ShowSuccessfullyAdded();
                }
                else
                {
                    _employeeTestBll.Update(input);
                    ShowSuccessfullyUpdated();
                }
            }
            
            switch (formType)
            {
                case SubmitFormType.SaveAndExist:
                    return RedirectToAction("Index");
                case SubmitFormType.SaveAndContinue:
                    return RedirectToAction("Update", new { id = input.Id });
                case SubmitFormType.SaveAndAddNew:
                    return RedirectToAction("Create");
            }

            return View("AddOrUpdate", input);
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, EmployeeTestDTO input)
        {
            if (input?.Id != null)
                _employeeTestBll.Delete(input.Id);

            return Json(new[] { input }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetDepartmentsList()
        {
            var departments = _employeeTestBll.GetAllDepartments();
            
            var departmentsDropDownList = departments.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            
            return Json(departmentsDropDownList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadPhoto(HttpPostedFileBase photo)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var uploadPath = Server.MapPath("~/uploads/photos/");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            photo.SaveAs(filePath);

            var imageUrl = Url.Content($"~/uploads/photos/{fileName}");
            return Json(new { success = true, imageUrl });
        }
    }

    }
