using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Helper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{


    public class PatientController : BaseController
    {
        private readonly IDepartmentBLL _departmentBLL;
        ITeleMentalHealthBLL _teleMentalHealthBLL;
        readonly private INPICUBLL _NPICUBLL;
        readonly private ITeleICUBLL _TeleICUBLL;


        readonly private IPatientBLL _PatientBLL;
        public PatientController(
            IPatientBLL PatientBLL,
            IErrorLogBLL errorLogBLL,
            IDepartmentBLL departmentBLL, ITeleMentalHealthBLL teleMentalHealthBLL, INPICUBLL nPICUBLL, ITeleICUBLL teleICUBLL) : base(errorLogBLL)
        {
            this._PatientBLL = PatientBLL;
            _departmentBLL = departmentBLL;
            _teleMentalHealthBLL = teleMentalHealthBLL;
            _NPICUBLL = nPICUBLL;
            _TeleICUBLL = teleICUBLL;
        }

        // GET: Patient
        [Permission(RoleConsistent.Patient.Browse)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request, DateTime? CreationDateStart, DateTime? CreationDateEnd, DateTime? BirthDateStart, DateTime? BirthDateEnd)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");

                IQueryable<PatientDTO> _res;
                _res = _PatientBLL.GetAll();
                var currentUser = GetCurrentUser;
                if (currentUser.JobRole != JobRole.IT.ToString())
                    _res = _res.Where(x => x.DepartmentId == currentUser.DepartmentId);

                //Filters
                if (CreationDateStart.HasValue)
                    _res = _res.Where(x => DbFunctions.TruncateTime(x.DateOfCreate) >= DbFunctions.TruncateTime(CreationDateStart.Value));

                if (CreationDateEnd.HasValue)
                    _res = _res.Where(x => DbFunctions.TruncateTime(x.DateOfCreate) <= DbFunctions.TruncateTime(CreationDateEnd.Value));

                if (BirthDateStart.HasValue)
                    _res = _res.Where(x => DbFunctions.TruncateTime(x.BirthDate) >= DbFunctions.TruncateTime(BirthDateStart.Value));

                if (BirthDateEnd.HasValue)
                    _res = _res.Where(x => DbFunctions.TruncateTime(x.BirthDate) <= DbFunctions.TruncateTime(BirthDateEnd.Value));

                return new JsonResult() { Data = _res.ToDataSourceResult(request), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }

        }


        [Permission(RoleConsistent.Patient.Edit, RoleConsistent.Patient.Add)]
        public ActionResult Edit(string PatientId = "")
        {
            PatientDTO obDTO;
            if (String.IsNullOrWhiteSpace(PatientId))
            {
                obDTO = new PatientDTO() { DepartmentId = GetCurrentUser.DepartmentId ?? 0 };
            }
            else
            {
                var id = HashIdsManager.Decrypt(PatientId);
                obDTO = _PatientBLL.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (obDTO==null)
                {
                     return HttpNotFound();
                }

                var MHDeptId = _departmentBLL.GetAll().Where(x => x.Name == "Mental Health").Select(x => x.Id).FirstOrDefault();
                var NPICUDeptId = _departmentBLL.GetAll().Where(x => x.Name == "NPICU").Select(x => x.Id).FirstOrDefault();
                var TELICUDeptId = _departmentBLL.GetAll().Where(x => x.Name == "ICU").Select(x => x.Id).FirstOrDefault();


                if (obDTO.DepartmentId == MHDeptId)
                    ViewBag.CanAddCase = _teleMentalHealthBLL.GetCurrentlyOpendCaseIdForPatient(obDTO.Id)==null;

                if (obDTO.DepartmentId == NPICUDeptId)
                    ViewBag.CanAddCase = _NPICUBLL.GetCurrentlyOpendCaseIdForPatient(obDTO.Id) == null;

                if (obDTO.DepartmentId == TELICUDeptId)
                    ViewBag.CanAddCase = _TeleICUBLL.GetCurrentlyOpendCaseIdForPatient(obDTO.Id) == null;

                ViewBag.MHDeptId = MHDeptId;
                ViewBag.NPICUDeptId = NPICUDeptId;
                ViewBag.TELICUDeptId = TELICUDeptId;
            }
            return View(obDTO);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(PatientDTO obDTO, SubmitFormType FormType = SubmitFormType.SaveAndExist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = _PatientBLL.IsValid(obDTO);

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        if (obDTO.Id == 0)
                        {
                            if (!User.IsInRole(RoleConsistent.Patient.Add))
                                RedirectToAction("AccessDenied", "Home");

                            //TODO::Fix This _Take DepartmentId From User Info_
                            //obDTO.DepartmentId = _departmentBLL.GetAll().Select(x => x.Id).FirstOrDefault();

                            obDTO.Id = _PatientBLL.Insert(obDTO);
                            ShowSuccessfullyAdded();
                        }
                        else
                        {
                            _PatientBLL.Update(obDTO);
                            ShowSuccessfullyUpdated();
                        }
                    }
                    else
                    {

                        ShowErrorMessage(errorMessage);
                        return View(obDTO);
                    }

                    switch (FormType)
                    {
                        case SubmitFormType.SaveAndExist:
                            return RedirectToAction("Index");
                        case SubmitFormType.SaveAndContinue:
                            return RedirectToAction("Edit", new { PatientId = obDTO.EncrptedId });
                        case SubmitFormType.SaveAndAddNew:
                            return RedirectToAction("Create");
                        default:
                            break;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }


        [Permission(RoleConsistent.Patient.Add)]
        public ActionResult Create()
        {
            var obDTO = new PatientDTO() { DepartmentId = GetCurrentUser.DepartmentId ?? 0 };

            return View("Edit", obDTO);
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.Patient.Delete)]

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, PatientDTO obDTO)
        {
            try
            {
               
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }
              
                var errors = _PatientBLL.ValidToBeDeleted(obDTO.Id);
                if (string.IsNullOrWhiteSpace(errors))
                    _PatientBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("Id", errors);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public JsonResult GetPatients(string text, int? prevId, string DeptName = "")
        {
            try
            {
                var deptId = _departmentBLL.GetAll().Where(x => x.Name == DeptName).Select(x => x.Id).FirstOrDefault();
                if(deptId!=GetCurrentUser.DepartmentId)
                    return  Json(null, JsonRequestBehavior.AllowGet);


                var Patients = _PatientBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(DeptName))
                {
                    Patients = Patients.Where(c => c.DepartmentName == DeptName);
                }
                if (!string.IsNullOrWhiteSpace(text))
                    Patients = Patients.Where(x => x.FirstName.Contains(text));

                if (!string.IsNullOrWhiteSpace(text))
                    Patients = Patients.Where(x => x.LastName.Contains(text));

                if (!string.IsNullOrWhiteSpace(text))
                    Patients = Patients.Where(x => text.Contains(x.FirstName));

                if (!string.IsNullOrWhiteSpace(text))
                    Patients = Patients.Where(x => text.Contains(x.LastName));

                var resList = Patients.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_PatientBLL.GetAll().Where(x => x.Id == prevId
                            &&( x.FirstName.Contains(text)|| x.LastName.Contains(text) || text.Contains(x.FirstName)|| text.Contains(x.LastName))).ToList());
                        else
                            resList.AddRange(_PatientBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

        public ActionResult RenderPatientInfoPartial(string patientId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(patientId);
                var patient = _PatientBLL.GetAll().Where(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Shared/_PatientInfo.cshtml", patient);
            }
            catch 
            {

                throw;
            }
        }

        public JsonResult GetGenderEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(Gender)).Cast<Enum>();
                var selectItems = enumValues.Select(s => new SelectListItem
                {
                    Text = s.GetDescription(),
                    Value = s.ToString()
                });
                return Json(selectItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }



        public JsonResult GetMaritalStatusEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(MaritalStatus)).Cast<Enum>();
                var selectItems = enumValues.Select(s => new SelectListItem
                {
                    Text = s.GetDescription(),
                    Value = s.ToString()
                });
                return Json(selectItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        public JsonResult GetBloodTypeEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(BloodType)).Cast<Enum>();
                var selectItems = enumValues.Select(s => new SelectListItem
                {
                    Text = s.GetDescription(),
                    Value = s.ToString()
                });
                return Json(selectItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

    }

}