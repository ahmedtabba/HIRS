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
using TripleA.Utilities.HashidsNet;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.WebApplication.Resources;
using BoulevardManagement.WebApplication.Helper;
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using static System.Collections.Specialized.BitVector32;
using BoulevardManagement.WebApplication.Resources;
namespace BoulevardManagement.WebApplication.Controllers
{

    public class TeleICUController : BaseController
    {
        private readonly IPatientBLL _patientBLL;
        readonly private ITeleICUBLL _TeleICUBLL;
        private readonly IBoulevardAttachmentBLL _boulevardAttachmentBLL;

        public TeleICUController(
            ITeleICUBLL TeleICUBLL,
            IErrorLogBLL errorLogBLL
            , IBoulevardAttachmentBLL boulevardAttachmentBLL,
            IPatientBLL patientBLL) : base(errorLogBLL)
        {
            this._TeleICUBLL = TeleICUBLL;
            _patientBLL = patientBLL;

            _boulevardAttachmentBLL = boulevardAttachmentBLL;
        }

        // GET: TeleICU
        [Permission(RoleConsistent.TeleICU.Browse)]
        public ActionResult Index()
        {
            return View();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }
        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int? patientId, Gender? gender, MaritalStatus? maritalStatus, BloodType? bloodType)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");

                IQueryable<TeleICUDTO> _res;
                _res = _TeleICUBLL.GetAllIncludedPatient();
                if (GetCurrentUser.JobRole != JobRole.IT.ToString())
                {
                    var currentUserId = GetCurrentUser.UserId;
                    _res = _res.Where(x => x.InvolvedUsers.Select(c => c.UserId).Contains(currentUserId) || x.CreatedByUserId == currentUserId);
                }
                if (patientId.HasValue && patientId.Value != 0)
                    _res = _res.Where(x => x.PatientId == patientId.Value);
                else
                {
                    if (gender.HasValue)
                    {
                        _res = _res.Where(x => x.PatientGender == gender);

                    }
                    if (maritalStatus.HasValue)
                    {
                        _res = _res.Where(x => x.PatientMaritalStatus == maritalStatus);
                    }
                    if (bloodType.HasValue)
                    {
                        _res = _res.Where(x => x.PatientBloodType == bloodType);
                    }
                }



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


        [Permission(RoleConsistent.TeleICU.Edit, RoleConsistent.TeleICU.Add)]
        public ActionResult Edit(string ICUId = "", string section = "")
        {
            TeleICUDTO obDTO;
            if (String.IsNullOrWhiteSpace(ICUId))
            {
                obDTO = new TeleICUDTO();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(section))
                {
                    TempData["Section"] = section;
                    return RedirectToAction("Edit", "TeleICU", new { ICUId = ICUId });
                }

                var id = HashIdsManager.Decrypt(ICUId);
                obDTO = _TeleICUBLL.GetAllIncludedPatient().Where(x => x.Id == id).FirstOrDefault();
                if (obDTO == null)
                {
                    return HttpNotFound();
                }
                obDTO.InvolvedConsultantsIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.Consultant).Select(x => x.UserId).ToList();
                obDTO.InvolvedServiceProvidersIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.ServiceProvider).Select(x => x.UserId).ToList();
                ViewBag.PatientId = HashIdsManager.Encrypt(obDTO.PatientId);
                ViewBag.TeleICUId = obDTO.EncrptedId;
                ViewBag.CurrentStep = (int)obDTO.CurrentStep;

               

                if (!obDTO.InvolvedConsultantsIds.Contains(GetCurrentUser.UserId) && !obDTO.InvolvedServiceProvidersIds.Contains(GetCurrentUser.UserId) && obDTO.CreatedByUserId != GetCurrentUser.UserId)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            return View(obDTO);

        }

        [HttpPost]
        public ActionResult Create(TeleICUDTO obDTO)
        {
            try
            {

                obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());

                obDTO.Id = _TeleICUBLL.Insert(obDTO);

                ShowSuccessfullyAdded();

                return RedirectToAction("Edit", "TeleICU", new { ICUId = obDTO.EncrptedId });

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(TeleICUDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = _TeleICUBLL.IsValid(obDTO);

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        if (obDTO.Id == 0)
                        {
                            if (!User.IsInRole(RoleConsistent.TeleICU.Add))
                                RedirectToAction("AccessDenied", "Home");
                            obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                            obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                            obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                            obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());
                            _TeleICUBLL.Insert(obDTO);
                            ShowSuccessfullyAdded();
                        }
                        else
                        {
                            obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                            obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new TeleICUUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                            obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                            obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());
                            _TeleICUBLL.Update(obDTO);
                            ShowSuccessfullyUpdated();
                        }
                    }
                    else
                    {

                        ShowErrorMessage(errorMessage);
                        return View(obDTO);
                    }

                    return RedirectToAction("Index");

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }




        [Permission(RoleConsistent.TeleICU.Add)]
        public ActionResult Create(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                return HttpNotFound();

            var pId = HashIdsManager.Decrypt(patientId);
            var patientExist = _patientBLL.GetAll().Where(x => x.Id == pId).Any();

            if (!patientExist)
                return HttpNotFound();

            var obDTO = new TeleICUDTO()
            {
                PatientId = pId
            };

            obDTO.Id = _TeleICUBLL.Insert(obDTO);

            ShowSuccessfullyAdded();

            return RedirectToAction("Edit", "TeleICU", new { ICUId = obDTO.EncrptedId });
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.TeleICU.Delete)]

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TeleICUDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                var errors = _TeleICUBLL.ValidToBeDeleted(obDTO.Id);
                if (string.IsNullOrWhiteSpace(errors))
                    _TeleICUBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("Id", errors);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public JsonResult GetTeleICUs(string text, int? prevId)
        {
            try
            {
                var TeleICUs = _TeleICUBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(text))
                    TeleICUs = TeleICUs.Where(x => x.PatientName.Contains(text));

                var resList = TeleICUs.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_TeleICUBLL.GetAll().Where(x => x.Id == prevId && x.PatientName.Contains(text)).ToList());
                        else
                            resList.AddRange(_TeleICUBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public ActionResult GetClinicalStory(string ICUId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUId);
                var model = _TeleICUBLL.GetClinicalStory(id);
                if (model != null && model.ECGAttachmentId.HasValue)
                {
                    var ECGAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ECGAttachmentId.Value);
                    model.ECGFilename = ECGAttachment.Description;
                }
                if (model != null && model.CxrAttachmentId.HasValue)
                {
                    var CxrAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CxrAttachmentId.Value);
                    model.CxrFilename = CxrAttachment.Description;
                }
                if (model == null)
                    model = new TelICUClinicalStoryDTO
                    {
                        TeleICUId = id,
                        CaseStatus = _TeleICUBLL.GetById(id).Status
                    };
                else
                {
                    model.BPFirstPart = model.BP.Split('/')[0];
                    model.BPSecondPart = model.BP.Split('/')[1];
                }
                return PartialView("_ClinicalStory", model);

            }
            catch (Exception)
            {

                throw;
            }
        }
        //public ActionResult GetMedicationDailyScheduleTable(string ICUId)
        //{
        //    try
        //    {
        //        var id = HashIdsManager.Decrypt(ICUId);
        //        var model = _TeleICUBLL.GetClinicalStory(id);
        //        if (model == null)
        //            model = new TelICUClinicalStoryDTO { TeleICUId = id };
        //        return PartialView("_MedicationDailyScheduleTable", model);

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public ActionResult GetSickyMenu(string ICUId)
        {
            try
            {
                int id = HashIdsManager.Decrypt(ICUId);
                var currentStep = (int)_TeleICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.CurrentStep).FirstOrDefault();
                List<StickyMenuItemVM> items = new List<StickyMenuItemVM>();
                if (User.IsInRole(RoleConsistent.TelIcuClinicalStory.ViewClinicalStory))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.ClinicalStory,
                        Id = "ClinicalStoryBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TelIcuMedicationDailyScheduleTable.ViewMedicationDailyScheduleTable))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.MedicationDailyScheduleTable,
                        Id = "MedicationDailyScheduleTableBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TelIcuLabUnit.ViewLabUnit))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.LabUnit,
                        Id = "LabUnitBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TelIcuInternalConsultationForm.ViewInternalConsultationForm))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.InternalConsultationForm,
                        Id = "InternalConsultationFormBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TelIcuConsultationForm.ViewConsultationForm))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.ConsultationForm,
                        Id = "ConsultationFormBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TelIcuPatientExitStatusReport.ViewPatientExitStatusReport))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.PatientExitStatusReport,
                        Id = "ExitBtn",
                        IsActive = true
                    });
                if (User.IsInRole(RoleConsistent.TeleICU.ViewChat))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleICUResource.StickyNote,
                        Id = "StickyNoteBtn",
                        IsActive = true
                    });

                if (items.Count > 0)
                    return PartialView("_StickyMenu", items);
                else
                    return Content(@"<script>$(function(){$('#sticky-menu').hide();});</script>");


            }
            catch (Exception)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateClinicalStory(TelICUClinicalStoryDTO obDTO)
        {
            try
            {


                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("DiagnosisStatus"))
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    var nicu = _TeleICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleICUId).FirstOrDefault();
                    _TeleICUBLL.UpdateClinicalStory(obDTO);

                    if (obDTO.HasECGAttachment && !String.IsNullOrEmpty(obDTO.ECGFilename) && System.IO.File.Exists(obDTO.ECGFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUECG;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "ECG_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ECGFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.ECGAttachmentId = attachment.Id;
                            obDTO.ECGFilename = attachment.Description;
                            obDTO.ECGFilePath = attachment.FilePath;
                            obDTO.HasECGAttachment = false;
                        }
                    }

                    if (obDTO.HasCxrAttachment && !String.IsNullOrEmpty(obDTO.CxrFilename) && System.IO.File.Exists(obDTO.CxrFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUCxr;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Cxr_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.CxrFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.CxrAttachmentId = attachment.Id;
                            obDTO.CxrFilename = attachment.Description;
                            obDTO.CxrFilePath = attachment.FilePath;
                            obDTO.HasCxrAttachment = false;
                        }
                    }
                    obDTO.BP = obDTO.BPFirstPart + "/" + obDTO.BPSecondPart;
                    _TeleICUBLL.UpdateClinicalStory(obDTO, false);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }






        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_MedicationScheduler([DataSourceRequest] DataSourceRequest request, TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _TeleICUBLL.InsertMedicationScheduler(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateMedicationScheduler(TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    //   obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _TeleICUBLL.UpdateMedicationScheduler(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }

                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_MedicationScheduler([DataSourceRequest] DataSourceRequest request, TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdateMedicationScheduler(obDTO);

                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }




        public ActionResult Read_MedicationScheduler([DataSourceRequest] DataSourceRequest request, string ICUID)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUID);
                IQueryable<TelICUMedicationSchedulerDTO> res;
                res = _TeleICUBLL.GetAllMedicationSchedulersByICUId(id);

                return Json(res.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete_MedicationScheduler([DataSourceRequest] DataSourceRequest request, TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {

                _TeleICUBLL.DeleteMedicationScheduler(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult GetMedicationSchedulerForm(string MedicationSchedulerId, string ICUID)
        {
            try
            {
                var icuId = HashIdsManager.Decrypt(ICUID);
                TelICUMedicationSchedulerDTO model;
                if (string.IsNullOrWhiteSpace(MedicationSchedulerId))
                    model = new TelICUMedicationSchedulerDTO() { TeleICUId = icuId };
                else
                {
                    var id = HashIdsManager.Decrypt(MedicationSchedulerId);
                    model = _TeleICUBLL.GetMedicationScheduler(id);
                    //   model.BloodPressureFirstPart = model.BloodPressure.Split('/')[0];
                    //  model.BloodPressureSecondPart = model.BloodPressure.Split('/')[1];
                }

                return PartialView("_MedicationSchedulerForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetTelICUMedicationScheduleTable(string ICUId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUId);
                var model = _TeleICUBLL.GetMedicationDailySchedule(id);
                if (model == null)
                    model = new TelICUMedicationDailyScheduleDTO
                    {
                        TeleICUId = id,
                        CaseStatus = _TeleICUBLL.GetById(id).Status
                    };
                return PartialView("_MedicationDailyScheduleTable", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult UpdateMedicationDailySchedule(TelICUMedicationDailyScheduleDTO obDTO)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdateMedicationDailySchedule(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }














        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_DiabetesControl([DataSourceRequest] DataSourceRequest request, TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _TeleICUBLL.InsertDiabetesControl(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDiabetesControl(TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //   obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _TeleICUBLL.UpdateDiabetesControl(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }

                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_DiabetesControl([DataSourceRequest] DataSourceRequest request, TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdateDiabetesControl(obDTO);

                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }




        public ActionResult Read_DiabetesControl([DataSourceRequest] DataSourceRequest request, string ICUID)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUID);
                IQueryable<TelICUDiabetesControlDTO> res;
                res = _TeleICUBLL.GetAllDiabetesControlsByICUId(id);

                return Json(res.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete_DiabetesControl([DataSourceRequest] DataSourceRequest request, TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }
                _TeleICUBLL.DeleteDiabetesControl(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult GetDiabetesControlForm(string DiabetesControlId, string ICUID)
        {
            try
            {
                var icuId = HashIdsManager.Decrypt(ICUID);
                TelICUDiabetesControlDTO model;
                if (string.IsNullOrWhiteSpace(DiabetesControlId))
                    model = new TelICUDiabetesControlDTO() { TeleICUId = icuId };
                else
                {
                    var id = HashIdsManager.Decrypt(DiabetesControlId);
                    model = _TeleICUBLL.GetDiabetesControl(id);
                    //   model.BloodPressureFirstPart = model.BloodPressure.Split('/')[0];
                    //  model.BloodPressureSecondPart = model.BloodPressure.Split('/')[1];
                }

                return PartialView("_DiabetesControlForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }









        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_VitalSign([DataSourceRequest] DataSourceRequest request, TelICUVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _TeleICUBLL.InsertVitalSign(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateVitalSign(TelICUVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //   obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _TeleICUBLL.UpdateVitalSign(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }

                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_VitalSign([DataSourceRequest] DataSourceRequest request, TelICUVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdateVitalSign(obDTO);

                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }




        public ActionResult Read_VitalSign([DataSourceRequest] DataSourceRequest request, string ICUID)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUID);
                IQueryable<TelICUVitalSignDTO> res;
                res = _TeleICUBLL.GetAllVitalSignsByICUId(id);

                return Json(res.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete_VitalSign([DataSourceRequest] DataSourceRequest request, TelICUVitalSignDTO obDTO)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("VitalSignDate"))
                        ModelState[key].Errors.Clear();
                }
                _TeleICUBLL.DeleteVitalSign(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult GetVitalSignForm(string VitalSignId, string ICUID)
        {
            try
            {
                var icuId = HashIdsManager.Decrypt(ICUID);
                TelICUVitalSignDTO model;
                if (string.IsNullOrWhiteSpace(VitalSignId))
                    model = new TelICUVitalSignDTO() { TeleICUId = icuId };
                else
                {
                    var id = HashIdsManager.Decrypt(VitalSignId);
                    model = _TeleICUBLL.GetVitalSign(id);
                    //   model.BloodPressureFirstPart = model.BloodPressure.Split('/')[0];
                    //  model.BloodPressureSecondPart = model.BloodPressure.Split('/')[1];
                }

                return PartialView("_ICUVitalSignForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

















        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_Pump([DataSourceRequest] DataSourceRequest request, TelICUPumpDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _TeleICUBLL.InsertPump(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePump(TelICUPumpDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //   obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _TeleICUBLL.UpdatePump(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }

                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Pump([DataSourceRequest] DataSourceRequest request, TelICUPumpDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdatePump(obDTO);

                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }




        public ActionResult Read_Pump([DataSourceRequest] DataSourceRequest request, string ICUID)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUID);
                IQueryable<TelICUPumpDTO> res;
                res = _TeleICUBLL.GetAllPumpsByICUId(id);

                return Json(res.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete_Pump([DataSourceRequest] DataSourceRequest request, TelICUPumpDTO obDTO)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("PumpDate"))
                        ModelState[key].Errors.Clear();
                }
                _TeleICUBLL.DeletePump(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult GetPumpForm(string PumpId, string ICUID)
        {
            try
            {
                var icuId = HashIdsManager.Decrypt(ICUID);
                TelICUPumpDTO model;
                if (string.IsNullOrWhiteSpace(PumpId))
                    model = new TelICUPumpDTO() { TeleICUId = icuId };
                else
                {
                    var id = HashIdsManager.Decrypt(PumpId);
                    model = _TeleICUBLL.GetPump(id);
                }

                return PartialView("_PumpForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetLabUnit(string labUnitId, string ICUId)
        {
            try
            {
                TelICULabUnitDTO model;
                if (!string.IsNullOrWhiteSpace(labUnitId))
                {
                    var fuId = HashIdsManager.Decrypt(labUnitId);
                    model = _TeleICUBLL.GetLabUnit(fuId);
                    if (model != null && model.BloodGasesAttachmentId.HasValue)
                    {
                        var BloodGasesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.BloodGasesAttachmentId.Value);
                        model.BloodGasesFilename = BloodGasesAttachment.Description;
                    }
                    if (model != null && model != null && model.ChemistryAttachmentId.HasValue)
                    {
                        var ChemistryAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ChemistryAttachmentId.Value);
                        model.ChemistryFilename = ChemistryAttachment.Description;
                    }
                    if (model != null && model.CBCAttachmentId.HasValue)
                    {
                        var CBCAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CBCAttachmentId.Value);
                        model.CBCFilename = CBCAttachment.Description;
                    }
                    if (model != null && model.ElectrolytesAttachmentId.HasValue)
                    {
                        var ElectrolytesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ElectrolytesAttachmentId.Value);
                        model.ElectrolytesFilename = ElectrolytesAttachment.Description;
                    }
                    if (model != null && model.CoagulationTestAttachmentId.HasValue)
                    {
                        var CoagulationTestAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CoagulationTestAttachmentId.Value);
                        model.CoagulationTestFilename = CoagulationTestAttachment.Description;
                    }
                    if (model != null && model.SerologyAttachmentId.HasValue)
                    {
                        var SerologyAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.SerologyAttachmentId.Value);
                        model.SerologyFilename = SerologyAttachment.Description;
                    }
                    if (model != null && model.UrineTestAttachmentId.HasValue)
                    {
                        var UrineTestAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.UrineTestAttachmentId.Value);
                        model.UrineTestFilename = UrineTestAttachment.Description;
                    }
                    if (model != null && model.LabUnitGeneralAttachmentId.HasValue)
                    {
                        var LabUnitGeneralAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.LabUnitGeneralAttachmentId.Value);
                        model.LabUnitGeneralFilename = LabUnitGeneralAttachment.Description;
                    }
                }
                else
                {

                    var id = HashIdsManager.Decrypt(ICUId);
                    //Get Last Added Follow-up
                    model = _TeleICUBLL.GetLabUnits(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model == null)
                    {
                        model = new TelICULabUnitDTO { TeleICUId = id, CaseStatus = _TeleICUBLL.GetById(id).Status };
                    }
                    else
                    {
                        if (model.BloodGasesAttachmentId.HasValue)
                        {
                            var BloodGasesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.BloodGasesAttachmentId.Value);
                            model.BloodGasesFilename = BloodGasesAttachment.Description;
                        }
                        if (model.ChemistryAttachmentId.HasValue)
                        {
                            var ChemistryAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ChemistryAttachmentId.Value);
                            model.ChemistryFilename = ChemistryAttachment.Description;
                        }
                        if (model.CBCAttachmentId.HasValue)
                        {
                            var CBCAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CBCAttachmentId.Value);
                            model.CBCFilename = CBCAttachment.Description;
                        }
                        if (model.ElectrolytesAttachmentId.HasValue)
                        {
                            var ElectrolytesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ElectrolytesAttachmentId.Value);
                            model.ElectrolytesFilename = ElectrolytesAttachment.Description;
                        }
                        if (model.CoagulationTestAttachmentId.HasValue)
                        {
                            var CoagulationTestAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CoagulationTestAttachmentId.Value);
                            model.CoagulationTestFilename = CoagulationTestAttachment.Description;
                        }
                        if (model.SerologyAttachmentId.HasValue)
                        {
                            var SerologyAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.SerologyAttachmentId.Value);
                            model.SerologyFilename = SerologyAttachment.Description;
                        }
                        if (model.UrineTestAttachmentId.HasValue)
                        {
                            var UrineTestAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.UrineTestAttachmentId.Value);
                            model.UrineTestFilename = UrineTestAttachment.Description;
                        }
                        if (model.LabUnitGeneralAttachmentId.HasValue)
                        {
                            var LabUnitGeneralAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.LabUnitGeneralAttachmentId.Value);
                            model.LabUnitGeneralFilename = LabUnitGeneralAttachment.Description;
                        }
                    }
                }
                return PartialView("_LabUnit", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateLabUnit(TelICULabUnitDTO obDTO)
        {
            try
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);


                if (ModelState.IsValid)
                {
                    var nicu = _TeleICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleICUId).FirstOrDefault();
                    _TeleICUBLL.UpdateLabUnit(obDTO);
                    if (obDTO.HasBloodGasesAttachment && !String.IsNullOrEmpty(obDTO.BloodGasesFilename) && System.IO.File.Exists(obDTO.BloodGasesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUBloodGases;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "BloodGases_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.BloodGasesFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.BloodGasesAttachmentId = attachment.Id;
                            obDTO.BloodGasesFilename = attachment.Description;
                            obDTO.BloodGasesFilePath = attachment.FilePath;
                            obDTO.HasBloodGasesAttachment = false;
                        }
                    }

                    if (obDTO.HasChemistryAttachment && !String.IsNullOrEmpty(obDTO.ChemistryFilename) && System.IO.File.Exists(obDTO.ChemistryFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUChemistry;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Chemistry_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ChemistryFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.ChemistryAttachmentId = attachment.Id;
                            obDTO.ChemistryFilename = attachment.Description;
                            obDTO.ChemistryFilePath = attachment.FilePath;
                            obDTO.HasChemistryAttachment = false;
                        }
                    }
                    if (obDTO.HasCBCAttachment && !String.IsNullOrEmpty(obDTO.CBCFilename) && System.IO.File.Exists(obDTO.CBCFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUCBC;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "CBC_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.CBCFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.CBCAttachmentId = attachment.Id;
                            obDTO.CBCFilename = attachment.Description;
                            obDTO.CBCFilePath = attachment.FilePath;
                            obDTO.HasCBCAttachment = false;
                        }
                    }
                    if (obDTO.HasElectrolytesAttachment && !String.IsNullOrEmpty(obDTO.ElectrolytesFilename) && System.IO.File.Exists(obDTO.ElectrolytesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUElectrolytes;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Electrolytes_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ElectrolytesFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.ElectrolytesAttachmentId = attachment.Id;
                            obDTO.ElectrolytesFilename = attachment.Description;
                            obDTO.ElectrolytesFilePath = attachment.FilePath;
                            obDTO.HasElectrolytesAttachment = false;
                        }
                    }
                    if (obDTO.HasCoagulationTestAttachment && !String.IsNullOrEmpty(obDTO.CoagulationTestFilename) && System.IO.File.Exists(obDTO.CoagulationTestFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUCoagulationTest;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "CoagulationTest_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.CoagulationTestFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.CoagulationTestAttachmentId = attachment.Id;
                            obDTO.CoagulationTestFilename = attachment.Description;
                            obDTO.CoagulationTestFilePath = attachment.FilePath;
                            obDTO.HasCoagulationTestAttachment = false;
                        }
                    }
                    if (obDTO.HasSerologyAttachment && !String.IsNullOrEmpty(obDTO.SerologyFilename) && System.IO.File.Exists(obDTO.SerologyFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUSerology;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Serology_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.SerologyFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.SerologyAttachmentId = attachment.Id;
                            obDTO.SerologyFilename = attachment.Description;
                            obDTO.SerologyFilePath = attachment.FilePath;
                            obDTO.HasSerologyAttachment = false;
                        }
                    }
                    if (obDTO.HasUrineTestAttachment && !String.IsNullOrEmpty(obDTO.UrineTestFilename) && System.IO.File.Exists(obDTO.UrineTestFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUUrineTest;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "UrineTest_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.UrineTestFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.UrineTestAttachmentId = attachment.Id;
                            obDTO.UrineTestFilename = attachment.Description;
                            obDTO.UrineTestFilePath = attachment.FilePath;
                            obDTO.HasUrineTestAttachment = false;
                        }
                    }
                    if (obDTO.HasLabUnitGeneralAttachment && !String.IsNullOrEmpty(obDTO.LabUnitGeneralFilename) && System.IO.File.Exists(obDTO.LabUnitGeneralFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICULabUnitGeneral;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "LabUnitGeneral_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.LabUnitGeneralFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.LabUnitGeneralAttachmentId = attachment.Id;
                            obDTO.LabUnitGeneralFilename = attachment.Description;
                            obDTO.LabUnitGeneralFilePath = attachment.FilePath;
                            obDTO.HasLabUnitGeneralAttachment = false;
                        }
                    }
                    _TeleICUBLL.UpdateLabUnit(obDTO, false);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }




        public ActionResult GetInternalConsultationForm(string ICUId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUId);
                var model = _TeleICUBLL.GetInternalConsultationForm(id);
                if (model == null)
                    model = new TelICUInternalConsultationFormDTO
                    {
                        TeleICUId = id,
                        CaseStatus = _TeleICUBLL.GetById(id).Status
                    };
                return PartialView("_InternalConsultationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetConsultationForm(string ConsultationFormId, string ICUId)
        {
            try
            {
                TelICUConsultationFormDTO model;
                if (!string.IsNullOrWhiteSpace(ConsultationFormId))
                {
                    var fuId = HashIdsManager.Decrypt(ConsultationFormId);
                    model = _TeleICUBLL.GetConsultationForm(fuId);
                    if (model != null && model.NotesAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NotesAttachmentId.Value);
                        model.NotesFilename = NotesAttachment.Description;
                    }
                }
                else
                {
                    var id = HashIdsManager.Decrypt(ICUId);
                    model = _TeleICUBLL.GetConsultationForms(id).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (model == null)
                    {
                        model = new TelICUConsultationFormDTO { TeleICUId = id, CaseStatus = _TeleICUBLL.GetById(id).Status };
                    }
                    else
                    {
                        if (model.NotesAttachmentId.HasValue)
                        {
                            var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NotesAttachmentId.Value);
                            model.NotesFilename = NotesAttachment.Description;
                        }

                    }
                }

                return PartialView("_ConsultationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateInternalConsultationForm(TelICUInternalConsultationFormDTO obDTO)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    _TeleICUBLL.UpdateInternalConsultationForm(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateConsultationForm(TelICUConsultationFormDTO obDTO, HttpPostedFileBase NotesVoice)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("IsNotesVoiceDeleted")
                        )
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    DeleteVoiceNotes(obDTO);
                    if (NotesVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = NotesVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.NotesVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                        //return $"{serverPath}Uploads/Voice/{fileName}";
                    }
                    var nicu = _TeleICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleICUId).FirstOrDefault();
                    _TeleICUBLL.UpdateConsultationForm(obDTO);
                    if (obDTO.HasNotesAttachment && !String.IsNullOrEmpty(obDTO.NotesFilename) && System.IO.File.Exists(obDTO.NotesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUNotes;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Notes_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.NotesFilePath;

                        var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.NotesAttachmentId = attachment.Id;
                            obDTO.NotesFilename = attachment.Description;
                            obDTO.NotesFilePath = attachment.FilePath;
                            obDTO.HasNotesAttachment = false;
                        }
                    }
                    _TeleICUBLL.UpdateConsultationForm(obDTO, false);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }

        public ActionResult GetExit(string ICUId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUId);
                var model = _TeleICUBLL.GetExit(id);
                if (model == null)
                    model = new TelICUExitDTO
                    {
                        TeleICUId = id,
                        CaseStatus = _TeleICUBLL.GetById(id).Status
                    };
                else
                {
                    model.BloodPressureFirstPart = model.BloodPressure.Split('/')[0];
                    model.BloodPressureSecondPart = model.BloodPressure.Split('/')[1];
                }
                return PartialView("_Exit", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateExit(TelICUExitDTO obDTO)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _TeleICUBLL.UpdateExit(obDTO);
                    return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });
                }
                return Json(new { Success = false, Message = "Error!!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
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

        public JsonResult GetPositiveNegativeEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(PositiveNegativeEnum)).Cast<Enum>();
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
        public JsonResult GetSignTypeEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(SignTypeEnum)).Cast<Enum>();
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

        public JsonResult GetAppearanceEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(AppearanceEnum)).Cast<Enum>();
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

        public JsonResult GetColorEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(ColorEnum)).Cast<Enum>();
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
        public JsonResult GetUnderAnesthesiaEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(UnderAnesthesiaEnum)).Cast<Enum>();
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
        public JsonResult Read_TelIcuLabUnit([DataSourceRequest] DataSourceRequest request, int telIcuId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _TeleICUBLL.GetLabUnits(telIcuId).ToList();


                foreach (var investigation in _res)
                {
                    investigation.CreatorName = UserManager.Users.Where(x => x.Id == investigation.CreatorId).Select(x => x.FullName).FirstOrDefault();
                }


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

        public JsonResult Read_TelIcuConsultationForm([DataSourceRequest] DataSourceRequest request, int telIcuId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _TeleICUBLL.GetConsultationForms(telIcuId).ToList();


                foreach (var investigation in _res)
                {
                    investigation.CreatorName = UserManager.Users.Where(x => x.Id == investigation.CreatorId).Select(x => x.FullName).FirstOrDefault();
                }


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
        public ActionResult DeleteTelIcuLabUnit([DataSourceRequest] DataSourceRequest request, TelICULabUnitDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _TeleICUBLL.DeleteLabUnit(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public ActionResult DeleteTelIcuConsultationForm([DataSourceRequest] DataSourceRequest request, TelICUConsultationFormDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _TeleICUBLL.DeleteConsultationForm(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public ActionResult GetLabUnitGrid(string ICUId)
        {
            try
            {

                return PartialView("_LabUnitGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetConsultationFormGrid(string ICUId)
        {
            try
            {

                return PartialView("_ConsultationFormGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetTelIcuLabUnitForAdd(string ICUId)
        {
            try
            {
                TelICULabUnitDTO model;
                var id = HashIdsManager.Decrypt(ICUId);
                model = new TelICULabUnitDTO { TeleICUId = id };
                return PartialView("_LabUnit", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetConsultationFormForAdd(string ICUId)
        {
            try
            {
                TelICUConsultationFormDTO model;
                var id = HashIdsManager.Decrypt(ICUId);
                model = new TelICUConsultationFormDTO { TeleICUId = id };
                return PartialView("_ConsultationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult GetDiagonisisStatusEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(DiagnosisStatus)).Cast<Enum>();
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


        private void DeleteVoiceNotes(TelICUConsultationFormDTO obDTO)
        {
            if (obDTO.IsNotesVoiceDeleted && obDTO.NotesVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.NotesVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.NotesVoiceAttachPath = null;
            }

        }

        [Permission(RoleConsistent.TeleICU.Close)]
        public ActionResult CloseCase(CaseClosureDTO obDTO)
        {
            try
            {
                var MentalHealth = _TeleICUBLL.GetAll().Where(x => x.Id == obDTO.CaseId).FirstOrDefault();
                _TeleICUBLL.Close(obDTO);
                if (obDTO.HasCaseClosureAttachment && !String.IsNullOrEmpty(obDTO.CaseClosureFilename) && System.IO.File.Exists(obDTO.CaseClosureFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.CaseClosure;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Closure_for_patient" + MentalHealth.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.CaseClosureFilePath;

                    var attachId = _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                    if (attachment != null)
                    {
                        obDTO.CaseClosureAttachmentId = attachment.Id;
                        obDTO.CaseClosureFilename = attachment.Description;
                        obDTO.CaseClosureFilePath = attachment.FilePath;
                        obDTO.HasCaseClosureAttachment = false;
                    }
                }
                _TeleICUBLL.Close(obDTO, false);
                ShowInfoMessage(CommonResource.CloseCaseMessage);
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }

        [Permission(RoleConsistent.TeleICU.ReOpen)]
        public ActionResult ReOpenCase(int caseId)
        {
            try
            {
                _TeleICUBLL.ReOpen(caseId);
                ShowInfoMessage(CommonResource.ReOpenCaseMessage);
                return RedirectToAction("Edit", new { ICUId = HashIdsManager.Encrypt(caseId) });
            }
            catch (Exception ex)
            {
                // return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        public ActionResult GetAddCaseClosureForm(string ICUId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(ICUId);
                var model = new CaseClosureDTO
                {
                    CaseId = id,
                    CaseDepartment = DepartmentEnum.ICU,
                    CanReOpenCase = false //User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen)
                };


                return PartialView("_CaseClosureForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetCaseClosureForm(string closureId, string ICUId)
        {
            try
            {
                CaseClosureDTO model;
                var id = HashIdsManager.Decrypt(ICUId);

                if (!string.IsNullOrWhiteSpace(closureId))
                {
                    var closureIdInt = HashIdsManager.Decrypt(closureId);
                    model = _TeleICUBLL.GetCaseClosure(closureIdInt);
                    if (model != null && model.CaseClosureAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CaseClosureAttachmentId.Value);
                        model.CaseClosureFilename = NotesAttachment.Description;
                    }
                    model.CaseDepartment = DepartmentEnum.ICU;
                    model.CanReOpenCase = User.IsInRole(RoleConsistent.TeleICU.ReOpen) &&
                        _TeleICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                }
                else
                {


                    //Get Last Added Closure
                    model = _TeleICUBLL.GetCaseClosures(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model != null && model.CaseClosureAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CaseClosureAttachmentId.Value);
                        model.CaseClosureFilename = NotesAttachment.Description;
                    }
                    if (model == null)
                    {
                        model = new CaseClosureDTO
                        {
                            CaseId = id,
                            CaseDepartment = DepartmentEnum.ICU,
                            CanReOpenCase = User.IsInRole(RoleConsistent.TeleICU.ReOpen) &&
                            _TeleICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed
                        };
                    }
                    else
                    {
                        model.CaseDepartment = DepartmentEnum.ICU;
                        model.CanReOpenCase = User.IsInRole(RoleConsistent.TeleICU.ReOpen) &&
                            _TeleICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                    }
                }

                return PartialView("_CaseClosureForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetCaseClosureGrid(string ICUId)
        {
            try
            {

                return PartialView("_CaseClosureGrid", "TeleICU");

            }
            catch (Exception)
            {

                throw;
            }
        }


        public JsonResult Read_CaseClosure([DataSourceRequest] DataSourceRequest request, int caseId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _TeleICUBLL.GetCaseClosures(caseId).ToList();


                foreach (var investigation in _res)
                {
                    investigation.CreatorName = UserManager.Users.Where(x => x.Id == investigation.CreatorId).Select(x => x.FullName).FirstOrDefault();
                }


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

        public ActionResult PrintPatienExitReport(string ICUId)
        {
            try
            {
                var Id = HashIdsManager.Decrypt(ICUId);
                var IcuDto = _TeleICUBLL.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                var TelIcuExitReport = _TeleICUBLL.GetExit(Id);
                ViewBag.ExitObject = TelIcuExitReport;
                return View("ExitPrintReport", IcuDto);

            }
            catch (Exception)
            {

                throw;
            }
        }

    }

}