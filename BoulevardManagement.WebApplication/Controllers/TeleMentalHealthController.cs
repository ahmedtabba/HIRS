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

namespace BoulevardManagement.WebApplication.Controllers
{

    public class TeleMentalHealthController : BaseController
    {
        private readonly IPatientBLL _patientBLL;
        private readonly IBoulevardAttachmentBLL _boulevardAttachmentBLL;
        private readonly ITelMHDiagnosisCategoryBLL _telMHDiagnosisCategoryBLL;
        private readonly ITelMHDiagnosisSubCategoryBLL _telMHDiagnosisSubCategoryBLL;
        private readonly IMedicationBLL _mostLikelyDiagnosisBLL;
        private readonly ITelMHDiagnosisBLL _telMHDiagnosisBLL;
        private readonly ITelMHMostLikelyDiagnosisBLL _telMHMostLikelyDiagnosisBLL;
        private readonly IMedicationBLL _medicationBLL;
        private readonly ICaseClosureBLL _caseClosureBLL;

        readonly private ITeleMentalHealthBLL _teleMentalHealthBLL;
        public TeleMentalHealthController(
            ITeleMentalHealthBLL teleMentalHealthBLL,
            IErrorLogBLL errorLogBLL,
            IPatientBLL patientBLL,
            IBoulevardAttachmentBLL boulevardAttachmentBLL,
            ITelMHDiagnosisCategoryBLL telMHDiagnosisCategoryBLL,
            ITelMHDiagnosisSubCategoryBLL telMHDiagnosisSubCategoryBLL,
            ITelMHDiagnosisBLL telMHDiagnosisBLL,
            ITelMHMostLikelyDiagnosisBLL telMHMostLikelyDiagnosisBLL,
            IMedicationBLL mostLikelyDiagnosisBLL,
            IMedicationBLL medicationBLL
          ,
            ICaseClosureBLL caseClosureBLL) : base(errorLogBLL)
        {
            this._teleMentalHealthBLL = teleMentalHealthBLL;
            _patientBLL = patientBLL;
            _boulevardAttachmentBLL = boulevardAttachmentBLL;
            _telMHDiagnosisCategoryBLL = telMHDiagnosisCategoryBLL;
            _telMHDiagnosisSubCategoryBLL = telMHDiagnosisSubCategoryBLL;
            _mostLikelyDiagnosisBLL = mostLikelyDiagnosisBLL;
            _telMHDiagnosisBLL = telMHDiagnosisBLL;
            _telMHMostLikelyDiagnosisBLL = telMHMostLikelyDiagnosisBLL;
            _medicationBLL = medicationBLL;
            _caseClosureBLL = caseClosureBLL;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }

        // GET: TeleMentalHealth
        [Permission(RoleConsistent.TeleMentalHealth.Browse)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int? patientId, Gender? gender, MaritalStatus? maritalStatus, BloodType? bloodType)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");

                IQueryable<TeleMentalHealthDTO> _res;
                _res = _teleMentalHealthBLL.GetAllIncludedPatient();
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


        [Permission(RoleConsistent.TeleMentalHealth.Edit, RoleConsistent.TeleMentalHealth.Add)]
        public ActionResult Edit(string mentalHealthId = "",string section="")
        {
            TeleMentalHealthDTO obDTO;
            if (String.IsNullOrWhiteSpace(mentalHealthId))
            {
                return HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(section))
                {
                    TempData["Section"] = section;
                    return RedirectToAction("Edit", "TeleMentalHealth", new { mentalHealthId = mentalHealthId });
                }
                var id = HashIdsManager.Decrypt(mentalHealthId);
                obDTO = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == id).FirstOrDefault();
                if (obDTO==null)
                {
                    return HttpNotFound();
                }
                obDTO.InvolvedConsultantsIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.Consultant).Select(x => x.UserId).ToList();
                obDTO.InvolvedServiceProvidersIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.ServiceProvider).Select(x => x.UserId).ToList();
                ViewBag.PatientId = HashIdsManager.Encrypt(obDTO.PatientId);
                ViewBag.TeleMHId = obDTO.EncrptedId;
                ViewBag.CurrentStep = (int)obDTO.CurrentStep;
                

                if (!obDTO.InvolvedConsultantsIds.Contains(GetCurrentUser.UserId) && !obDTO.InvolvedServiceProvidersIds.Contains(GetCurrentUser.UserId) && obDTO.CreatedByUserId != GetCurrentUser.UserId)
                {
                    return RedirectToAction("AccessDenied", "Home");

                }
            }
            return View(obDTO);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(TeleMentalHealthDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = _teleMentalHealthBLL.IsValid(obDTO);

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new TeleMentalHealthUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                        obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new TeleMentalHealthUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                        obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                        obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());

                        _teleMentalHealthBLL.Update(obDTO);
                        ShowSuccessfullyUpdated();

                    }
                    else
                    {

                        ShowErrorMessage(errorMessage);
                        return View(obDTO);
                    }

                    return RedirectToAction("Edit", new { mentalHealthId = obDTO.EncrptedId });

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }




        [Permission(RoleConsistent.TeleMentalHealth.Add)]
        [HttpPost]
        public ActionResult Create(TeleMentalHealthDTO obDTO)
        {
            try
            {

                //if()

                obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new TeleMentalHealthUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new TeleMentalHealthUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());

                obDTO.Id = _teleMentalHealthBLL.Insert(obDTO);

                ShowSuccessfullyAdded();

                return RedirectToAction("Edit", "TeleMentalHealth", new { mentalHealthId = obDTO.EncrptedId });
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.TeleMentalHealth.Delete)]

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TeleMentalHealthDTO obDTO)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }
                var errors = _teleMentalHealthBLL.ValidToBeDeleted(obDTO.Id);
                if (string.IsNullOrWhiteSpace(errors))
                    _teleMentalHealthBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("Id", errors);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public JsonResult GetTeleMentalHealths(string text, int? prevId)
        {
            try
            {
                var TeleMentalHealths = _teleMentalHealthBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(text))
                    TeleMentalHealths = TeleMentalHealths.Where(x => x.PatientName.Contains(text));

                var resList = TeleMentalHealths.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_teleMentalHealthBLL.GetAll().Where(x => x.Id == prevId && x.PatientName.Contains(text)).ToList());
                        else
                            resList.AddRange(_teleMentalHealthBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        public ActionResult GetClinicalStory(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var model = _teleMentalHealthBLL.GetClinicalStory(id);
                if (model == null)
                {
                    model = new TelMHClinicalStoryDTO { TeleMentalHealthId = id,
                        CaseStatus=_teleMentalHealthBLL.GetById(id).Status
                    };
                    model.Id = _teleMentalHealthBLL.UpdateClinicalStory(model, false);
                }
                //model.MostLikelyDiagnosisIds = model.MostLikelyDiagnosis.Select(x => x.MostLikelyDiagnosisId).ToList();
                return PartialView("_ClinicalStory", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetWrittenPledge(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var model = _teleMentalHealthBLL.GetWrittenPledge(id);
                if (model == null)
                    model = new TelMHWrittenPledgeDTO { TeleMentalHealthId = id,
                        CaseStatus = _teleMentalHealthBLL.GetById(id).Status
                    };
                else
                {
                    if (model.PledgeDocumentAttachmentId.HasValue)
                    {
                        var PledgeDocumentAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PledgeDocumentAttachmentId.Value);
                        model.PledgeDocumentFilename = PledgeDocumentAttachment.Description;
                    }

                    if (model.PatientIDCardAttachmentId.HasValue)
                    {
                        var PatientIDCardAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PatientIDCardAttachmentId.Value);
                        model.PatientIDCardFilename = PatientIDCardAttachment.Description;
                    }
                }

                return PartialView("_WrittenPledge", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetTherapeuticPlan(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var model = _teleMentalHealthBLL.GetTherapeuticPlan(id);
                if (model != null && model.NotesAttachmentId.HasValue)
                {
                    var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NotesAttachmentId.Value);
                    model.NotesFilename = NotesAttachment.Description;
                }
                if (model == null)
                    model = new TelMHTherapeuticPlanDTO { TeleMentalHealthId = id,
                        CaseStatus = _teleMentalHealthBLL.GetById(id).Status
                    };


                return PartialView("_TherapeuticPlan", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetTMonitoringTheVitalSigns(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var hmStatus = _teleMentalHealthBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault();
                //var model = _teleMentalHealthBLL.GetTherapeuticPlan(id);
                //if (model == null)
                var model = new MonitoringTheVitalSignsVM { TeleMentalHealthId = id,CaseStatus=hmStatus };
                return PartialView("_MonitoringTheVitalSigns", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPhysicalExaminationReport(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var patientName = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == id).Select(x => x.PatientName).FirstOrDefault();
                var model = _teleMentalHealthBLL.GetPhysicalExaminationReport(id);
                if (model == null)
                    model = new TelMHPhysicalExaminationReportDTO { TeleMentalHealthId = id ,
                        CaseStatus = _teleMentalHealthBLL.GetById(id).Status
                    };
                else
                {
                    if (model.ReportOfThePhysicalExaminationAttachmentId.HasValue)
                    {
                        var ReportOfThePhysicalExaminationAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ReportOfThePhysicalExaminationAttachmentId.Value);
                        model.ReportOfThePhysicalExaminationFilename = ReportOfThePhysicalExaminationAttachment.Description;
                    }

                }

                model.PatientName = patientName;

                return PartialView("_PhysicalExaminationReport", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetSickyMenu(string mentalHealthId)
        {
            try
            {
                int id = HashIdsManager.Decrypt(mentalHealthId);
                var _MH = _teleMentalHealthBLL.GetAll().Where(x => x.Id == id).Select(x => new { CurrentStep = x.CurrentStep, Remotely = x.Remotely }).FirstOrDefault();
                var currentStep = (int)_MH.CurrentStep;
                var isRemotely = _MH.Remotely;
                List<StickyMenuItemVM> items = new List<StickyMenuItemVM>();

                if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewClinicalStory))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleMentalHealthResource.ClinicalStory,
                        Id = "ClinicalStoryBtn",
                        IsActive = true
                    });

                if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewWrittenPledge))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleMentalHealthResource.WrittenPledge,
                        Id = "WrittenPledgeBtn",
                        //IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step2
                        IsActive = true
                    });

                if (!isRemotely)
                {
                    if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewPhysicalExaminationReport))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = TeleMentalHealthResource.PhysicalExaminationReport,
                            Id = "PhysicalExaminationReportBtn",
                            //IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step3
                            IsActive = true
                        });

                    if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewTherapeuticPlan))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = TeleMentalHealthResource.TherapeuticPlan,
                            Id = "TherapeuticPlanBtn",
                            //IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step4
                            IsActive = true
                        });


                    if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewMonitoringTheVitalSigns))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = TeleMentalHealthResource.MonitoringTheVitalSigns,
                            Id = "MonitoringTheVitalSignsBtn",
                            //IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step5
                            IsActive = true
                        });
                }

                if (User.IsInRole(RoleConsistent.TeleMentalHealth.ViewChat))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = TeleMentalHealthResource.StickyNote,
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
        public ActionResult UpdateClinicalStory(TelMHClinicalStoryDTO obDTO, HttpPostedFileBase remarksVoice, HttpPostedFileBase CurrentComplaintVoice, HttpPostedFileBase RiskAssessmentVoice, HttpPostedFileBase CaseSummaryVoice)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("IsRemarksVoiceDeleted") ||
                        key.Contains("IsCurrentComplaintVoiceDeleted") ||
                        key.Contains("IsRiskAssessmentVoiceDeleted") ||
                         key.Contains("IsCaseSummaryVoiceDeleted")
                        )
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    DeleteVoiceNotes(obDTO);
                    if (remarksVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = remarksVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.RemarksVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                        //return $"{serverPath}Uploads/Voice/{fileName}";
                    }
                    if (CurrentComplaintVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = CurrentComplaintVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.CurrentComplaintVoiceAttachPath = $"/Uploads/Voice/{fileName}";

                    }
                    if (RiskAssessmentVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = RiskAssessmentVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.RiskAssessmentVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                    }
                    if (CaseSummaryVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = CaseSummaryVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.CaseSummaryVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                    }
                    // obDTO.MostLikelyDiagnosis = obDTO.MostLikelyDiagnosisIds.Select(x => new TelMHMostLikelyDiagnosisDTO { MostLikelyDiagnosisId = x, TelMHClinicalStoryId = obDTO.Id }).ToList();
                    _teleMentalHealthBLL.UpdateClinicalStory(obDTO);
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

        private void DeleteVoiceNotes(TelMHClinicalStoryDTO obDTO)
        {
            if (obDTO.IsRemarksVoiceDeleted && obDTO.RemarksVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.RemarksVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.RemarksVoiceAttachPath = null;
            }
            if (obDTO.IsCurrentComplaintVoiceDeleted && obDTO.CurrentComplaintVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.CurrentComplaintVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.CurrentComplaintVoiceAttachPath = null;
            }
            if (obDTO.IsRiskAssessmentVoiceDeleted && obDTO.RiskAssessmentVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.RiskAssessmentVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.RiskAssessmentVoiceAttachPath = null;
            }
            if (obDTO.IsCaseSummaryVoiceDeleted && obDTO.CaseSummaryVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.CaseSummaryVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.CaseSummaryVoiceAttachPath = null;
            }

        }
        private void DeleteVoiceNotes(TelMHTherapeuticPlanDTO obDTO)
        {
            if (obDTO.IsTherapeuticPlanVoiceDeleted && obDTO.TherapeuticPlanVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.TherapeuticPlanVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.TherapeuticPlanVoiceAttachPath = null;
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO, HttpPostedFileBase TherapeuticPlanVoice)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("IsTherapeuticPlanVoiceDeleted"))
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    DeleteVoiceNotes(obDTO);
                    if (TherapeuticPlanVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = TherapeuticPlanVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.TherapeuticPlanVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                        //return $"{serverPath}Uploads/Voice/{fileName}";
                    }
                    var MentalHealth = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleMentalHealthId).FirstOrDefault();

                    if (obDTO.HasNotesAttachment && !String.IsNullOrEmpty(obDTO.NotesFilename) && System.IO.File.Exists(obDTO.NotesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.ICUNotes;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Notes_for_patient" + MentalHealth.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.NotesFilePath;

                        _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetByAttachmentTypeAndObjectId(AttachmentReferenceTypes.TeleMentalHealthNotes, obDTO.Id).FirstOrDefault();
                        if (attachment != null)
                        {
                            obDTO.NotesAttachmentId = attachment.Id;
                            obDTO.NotesFilename = attachment.Description;
                            obDTO.NotesFilePath = attachment.FilePath;
                            obDTO.HasNotesAttachment = false;
                        }
                    }
                      _teleMentalHealthBLL.UpdateTherapeuticPlan(obDTO);
                    _teleMentalHealthBLL.UpdateTherapeuticPlanForAttachment(obDTO);
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
        public ActionResult UpdateWrittenPledge(TelMHWrittenPledgeDTO obDTO)
        {
            try
            {

                var telMH = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleMentalHealthId).FirstOrDefault();
                _teleMentalHealthBLL.UpdateWrittenPledge(obDTO);
                if (obDTO.HasNewPledgeDocumentAttachment && !String.IsNullOrEmpty(obDTO.PledgeDocumentFilename) && System.IO.File.Exists(obDTO.PledgeDocumentFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.TeleMentalHealthPledgeDocument;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Pledge_Document_for_patient" + telMH.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.PledgeDocumentFilePath;

                    var attachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                    if (attachment != null)
                    {
                        obDTO.PledgeDocumentAttachmentId = attachment.Id;
                        obDTO.PledgeDocumentFilename = attachment.Description;
                        obDTO.PledgeDocumentFilePath = attachment.FilePath;
                        obDTO.HasNewPledgeDocumentAttachment = false;
                    }
                }

                if (obDTO.HasNewPatientIDCardAttachment && !String.IsNullOrEmpty(obDTO.PatientIDCardFilename) && System.IO.File.Exists(obDTO.PatientIDCardFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.TeleMentalHealthPatientIDCard;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Patient_ID_Card_for_patient" + telMH.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.PatientIDCardFilePath;

                  var attachId=  _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                    if (attachment != null)
                    {
                        obDTO.PatientIDCardAttachmentId = attachment.Id;
                        obDTO.PatientIDCardFilename = attachment.Description;
                        obDTO.PatientIDCardFilePath = attachment.FilePath;
                        obDTO.HasNewPatientIDCardAttachment = false;
                    }
                }


                _teleMentalHealthBLL.UpdateWrittenPledge(obDTO,false);
                return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });



            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePhysicalExaminationReport(TelMHPhysicalExaminationReportDTO obDTO)
        {
            try
            {

                var telMH = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleMentalHealthId).FirstOrDefault();
                _teleMentalHealthBLL.UpdatePhysicalExaminationReport(obDTO);

                if (obDTO.HasNewReportOfThePhysicalExaminationAttachment && !String.IsNullOrEmpty(obDTO.ReportOfThePhysicalExaminationFilename) && System.IO.File.Exists(obDTO.ReportOfThePhysicalExaminationFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.TeleMentalHealthExamination;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Report_Of_The_Physical_Examination" + telMH.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.ReportOfThePhysicalExaminationFilePath;

                  var attachId=  _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                    if (attachment != null)
                    {
                        obDTO.ReportOfThePhysicalExaminationAttachmentId = attachment.Id;
                        obDTO.ReportOfThePhysicalExaminationFilename = attachment.Description;
                        obDTO.ReportOfThePhysicalExaminationFilePath = attachment.FilePath;
                        obDTO.HasNewReportOfThePhysicalExaminationAttachment = false;
                    }
                }




                _teleMentalHealthBLL.UpdatePhysicalExaminationReport(obDTO,false);
                return Json(new { Success = true, Message = CommonResource.SuccessfullyAddedMSG });



            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_VitalSign([DataSourceRequest] DataSourceRequest request, TelMHVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.Id = _teleMentalHealthBLL.InsertVitalSign(obDTO);
                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateVitalSign(TelMHVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obDTO.BloodPressure = obDTO.BloodPressureFirstPart + "/" + obDTO.BloodPressureSecondPart;
                    _teleMentalHealthBLL.UpdateVitalSign(obDTO);
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
        public ActionResult Edit_VitalSign([DataSourceRequest] DataSourceRequest request, TelMHVitalSignDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _teleMentalHealthBLL.UpdateVitalSign(obDTO);

                }
                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }




        public ActionResult Read_VitalSign([DataSourceRequest] DataSourceRequest request, string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                IQueryable<TelMHVitalSignDTO> res;
                res = _teleMentalHealthBLL.GetAllVitalSignsByMentalHelthId(id);

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
        public ActionResult Delete_VitalSign([DataSourceRequest] DataSourceRequest request, TelMHVitalSignDTO obDTO)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("TimeOfMeasurement") || key.Contains("BloodPressureFirstPart") || key.Contains("BloodPressureSecondPart"))
                        ModelState[key].Errors.Clear();
                }
                _teleMentalHealthBLL.DeleteVitalSign(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult GetVitalSignForm(string vitalSignId, string mentalHealthId)
        {
            try
            {
                var mhId = HashIdsManager.Decrypt(mentalHealthId);
                TelMHVitalSignDTO model;
                if (string.IsNullOrWhiteSpace(vitalSignId))
                    model = new TelMHVitalSignDTO() { TeleMentalHealthId = mhId, TimeOfMeasurement = DateTime.Now };
                else
                {
                    var id = HashIdsManager.Decrypt(vitalSignId);
                    model = _teleMentalHealthBLL.GetVitalSign(id);
                    model.BloodPressureFirstPart = model.BloodPressure.Split('/')[0];
                    model.BloodPressureSecondPart = model.BloodPressure.Split('/')[1];
                }

                return PartialView("_VitalSignForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public JsonResult GetDiagnosisCategorys_ar(string text)
        {
            try
            {

                var res = _telMHDiagnosisCategoryBLL.GetAll();
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.ArabicName.Contains(text));
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        public JsonResult GetDiagnosisCategorys(string text)
        {
            try
            {

                var res = _telMHDiagnosisCategoryBLL.GetAll();
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));


                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public JsonResult GetDiagnosisCategorysForMLD(string text, int clinicalStoryId)
        {
            try
            {
                var validDiagnosisIds = _telMHDiagnosisBLL.GetAll().Where(x => x.TelMHClinicalStoryId == clinicalStoryId).Select(x => x.TelMHDiagnosisCategoryId).ToList();
                var res = _telMHDiagnosisCategoryBLL.GetAll().Where(x => validDiagnosisIds.Contains(x.Id));
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));


                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        public JsonResult GetDiagnosisCategorysForMLD_ar(string text, int clinicalStoryId)
        {
            try
            {
                var validDiagnosisIds = _telMHDiagnosisBLL.GetAll().Where(x => x.TelMHClinicalStoryId == clinicalStoryId).Select(x => x.TelMHDiagnosisCategoryId).ToList();
                var res = _telMHDiagnosisCategoryBLL.GetAll().Where(x => validDiagnosisIds.Contains(x.Id));
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));


                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        public JsonResult GetDiagnosisSubCategorys_ar(string text, int diagnosisCategoryId)
        {
            try
            {

                var res = _telMHDiagnosisSubCategoryBLL.GetAll().Where(x => x.TelMHDiagnosisCategoryId == diagnosisCategoryId);
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.ArabicName.Contains(text));
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        public JsonResult GetDiagnosisSubCategorys(string text, int? diagnosisCategoryId)
        {
            try
            {

                var res = _telMHDiagnosisSubCategoryBLL.GetAll().Where(x => x.TelMHDiagnosisCategoryId == diagnosisCategoryId);
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));

                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public JsonResult GetMostLikelyDiagnosis(string text)
        {
            try
            {

                var res = _mostLikelyDiagnosisBLL.GetAll();
                if (!string.IsNullOrEmpty(text))
                    res = res.Where(x => x.Name.Contains(text));
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public JsonResult Read_TelMHTherapeuticPlan([DataSourceRequest] DataSourceRequest request, int mHealthId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _teleMentalHealthBLL.GetTherapeuticPlans(mHealthId).ToList();


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


        public ActionResult DeleteTelMHTherapeuticPlan([DataSourceRequest] DataSourceRequest request, TelMHTherapeuticPlanDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _teleMentalHealthBLL.DeleteTherapeuticPlan(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


        public ActionResult GetTelMHTherapeuticPlan(string planId, string mentalHealthId)
        {
            try
            {
                TelMHTherapeuticPlanDTO model;
                if (!string.IsNullOrWhiteSpace(planId))
                {
                    var fuId = HashIdsManager.Decrypt(planId);
                    model = _teleMentalHealthBLL.GetTherapeuticPlan(fuId);
                    if (model != null && model.NotesAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NotesAttachmentId.Value);
                        model.NotesFilename = NotesAttachment.Description;
                    }
                }
                else
                {

                    var id = HashIdsManager.Decrypt(mentalHealthId);
                    //Get Last Added Follow-up
                    model = _teleMentalHealthBLL.GetTherapeuticPlans(id).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (model!=null)
                    {
                        model = _teleMentalHealthBLL.GetTherapeuticPlan(model.Id);
                    }
                    if (model != null && model.NotesAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NotesAttachmentId.Value);
                        model.NotesFilename = NotesAttachment.Description;
                    }
                    if (model == null)
                    {
                        model = new TelMHTherapeuticPlanDTO { TeleMentalHealthId = id ,
                            CaseStatus = _teleMentalHealthBLL.GetById(id).Status
                        };
                    }
                }

                return PartialView("_TherapeuticPlan", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetTelMHTherapeuticPlanForAdd(string mentalHealthId)
        {
            try
            {
                TelMHTherapeuticPlanDTO model;
                var id = HashIdsManager.Decrypt(mentalHealthId);
                model = new TelMHTherapeuticPlanDTO { TeleMentalHealthId = id , CaseStatus = _teleMentalHealthBLL.GetById(id).Status };
                return PartialView("_TherapeuticPlan", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTelMHTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO, HttpPostedFileBase TherapeuticPlanVoice)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("IsTherapeuticPlanVoiceDeleted"))
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    DeleteVoiceNotes(obDTO);
                    if (TherapeuticPlanVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = TherapeuticPlanVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.TherapeuticPlanVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                        //return $"{serverPath}Uploads/Voice/{fileName}";
                    }
                    var MentalHealth = _teleMentalHealthBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.TeleMentalHealthId).FirstOrDefault();
                    if (obDTO.Id == 0)
                    {
                        _teleMentalHealthBLL.InsertTherapeuticPlan(obDTO);

                    }
                    else
                    {
                        _teleMentalHealthBLL.UpdateTherapeuticPlan(obDTO);

                    }
                    if (obDTO.HasNotesAttachment && !String.IsNullOrEmpty(obDTO.NotesFilename) && System.IO.File.Exists(obDTO.NotesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.TeleMentalHealthNotes;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Notes_for_patient" + MentalHealth.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.NotesFilePath;

                       var attachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.NotesAttachmentId = attachment.Id;
                            obDTO.NotesFilename = attachment.Description;
                            obDTO.NotesFilePath = attachment.FilePath;
                            obDTO.HasNotesAttachment = false;
                        }
                    }
                   // obDTO.Medications = _teleMentalHealthBLL.GetTherapeuticPlan(obDTO.Id).Medications;
                    _teleMentalHealthBLL.UpdateTherapeuticPlanForAttachment(obDTO);


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


        public ActionResult Diagnosis_Read([DataSourceRequest] DataSourceRequest request, int TelMHClinicalStoryId)
        {
            return Json(_telMHDiagnosisBLL.GetAll().Where(x => x.TelMHClinicalStoryId == TelMHClinicalStoryId).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diagnosis_Create([DataSourceRequest] DataSourceRequest request, TelMHDiagnosisDTO obDTO)
        {
            if (obDTO != null && ModelState.IsValid)
            {
                var errors = _telMHDiagnosisBLL.IsValid(obDTO);
                if (string.IsNullOrWhiteSpace(errors))
                    obDTO.Id= _telMHDiagnosisBLL.Insert(obDTO);
                else
                    ModelState.AddModelError("e", errors);
            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diagnosis_Update([DataSourceRequest] DataSourceRequest request, TelMHDiagnosisDTO obDTO)
        {
            if (obDTO != null && ModelState.IsValid)
            {
                var errors = _telMHDiagnosisBLL.IsValidToBeChanged(obDTO);
                if (string.IsNullOrWhiteSpace(errors))
                    _telMHDiagnosisBLL.Update(obDTO);
                else
                    ModelState.AddModelError("e", errors);
            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diagnosis_Destroy([DataSourceRequest] DataSourceRequest request, TelMHDiagnosisDTO obDTO)
        {
            if (obDTO != null)
            {
                var errors = _telMHDiagnosisBLL.ValidToBeDeleted(obDTO);
                if (string.IsNullOrWhiteSpace(errors))
                    _telMHDiagnosisBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("e", errors);
            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }




        public ActionResult MostLikelyDiagnosis_Read([DataSourceRequest] DataSourceRequest request, int TelMHClinicalStoryId)
        {
            var res = _telMHMostLikelyDiagnosisBLL.GetAll().Where(x => x.TelMHClinicalStoryId == TelMHClinicalStoryId).ToList();
            foreach (var item in res)
            {
                item.TelMHMostLikelySubDiagnosesIds = item.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryId).ToList();
                item.TelMHMostLikelySubDiagnosesNames = item.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryName).ToList();
                item.TelMHMostLikelySubDiagnosesArabicNames = item.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryArabicName).ToList();
            }
            return Json(res.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MostLikelyDiagnosis_Create([DataSourceRequest] DataSourceRequest request, TelMHMostLikelyDiagnosisDTO obDTO)
        {
            if (obDTO != null && ModelState.IsValid)
            {
                var errors = _telMHMostLikelyDiagnosisBLL.IsValidToBeAdded(obDTO);

                if (string.IsNullOrWhiteSpace(errors))
                {
                    obDTO.TelMHMostLikelySubDiagnoses = obDTO.TelMHMostLikelySubDiagnosesIds.Select(x => new TelMHMostLikelySubDiagnosisDTO { TelMHDiagnosisSubCategoryId = x }).ToList();
                    obDTO.Id = _telMHMostLikelyDiagnosisBLL.Insert(obDTO);
                    obDTO = _telMHMostLikelyDiagnosisBLL.GetAll().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    obDTO.TelMHMostLikelySubDiagnosesIds = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryId).ToList();
                    obDTO.TelMHMostLikelySubDiagnosesNames = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryName).ToList();
                    obDTO.TelMHMostLikelySubDiagnosesArabicNames = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryArabicName).ToList();

                }
                else
                {
                    ModelState.AddModelError("e", errors);
                }
            
            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MostLikelyDiagnosis_Update([DataSourceRequest] DataSourceRequest request, TelMHMostLikelyDiagnosisDTO obDTO)
        {
            if (obDTO != null && ModelState.IsValid)
            {
                var errors = _telMHMostLikelyDiagnosisBLL.IsValidToBeAdded(obDTO);
                if (string.IsNullOrEmpty(errors))
                {
                    obDTO.TelMHMostLikelySubDiagnoses = obDTO.TelMHMostLikelySubDiagnosesIds.Select(x => new TelMHMostLikelySubDiagnosisDTO { TelMHDiagnosisSubCategoryId = x }).ToList();
                    _telMHMostLikelyDiagnosisBLL.Update(obDTO);
                    obDTO = _telMHMostLikelyDiagnosisBLL.GetAll().Where(x => x.Id == obDTO.Id).FirstOrDefault();

                    obDTO.TelMHMostLikelySubDiagnosesIds = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryId).ToList();
                    obDTO.TelMHMostLikelySubDiagnosesNames = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryName).ToList();
                    obDTO.TelMHMostLikelySubDiagnosesArabicNames = obDTO.TelMHMostLikelySubDiagnoses.Select(x => x.TelMHDiagnosisSubCategoryArabicName).ToList();

                }else
                    ModelState.AddModelError("e", errors);

            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MostLikelyDiagnosis_Destroy([DataSourceRequest] DataSourceRequest request, TelMHMostLikelyDiagnosisDTO obDTO)
        {
            if (obDTO != null)
            {
                _telMHMostLikelyDiagnosisBLL.Delete(obDTO.Id);
            }

            return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetTelMHTherapeuticPlanGrid(string mentalHealthId)
        {
            try
            {

                return PartialView("_TherapeuticPlanGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult GetMedications(string text, int? previousId)
        {
            try
            {

                var res = _medicationBLL.GetAll().Take(20);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    res = _medicationBLL.GetAll().Where(x => x.Name.Contains(text)).Take(20);
                }
                var resList = res.ToList();

                if (previousId.HasValue && !resList.Select(x => x.Id).Contains((int)previousId))
                {
                    resList.AddRange(_medicationBLL.GetAll().Where(x => x.Id == previousId && (x.Name.Contains(text))));
                }
                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        [Permission(RoleConsistent.TeleMentalHealth.Close)]
        public ActionResult CloseCase(CaseClosureDTO obDTO)
        {
            try
            {
                var MentalHealth = _teleMentalHealthBLL.GetAll().Where(x => x.Id == obDTO.CaseId).FirstOrDefault();
                _teleMentalHealthBLL.Close(obDTO);
                if (obDTO.HasCaseClosureAttachment && !String.IsNullOrEmpty(obDTO.CaseClosureFilename) && System.IO.File.Exists(obDTO.CaseClosureFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.CaseClosure;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Notes_for_patient" + MentalHealth.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.CaseClosureFilePath;

                  var attachId=  _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                    if (attachment != null)
                    {
                        obDTO.CaseClosureAttachmentId = attachment.Id;
                        obDTO.CaseClosureFilename = attachment.Description;
                        obDTO.CaseClosureFilePath = attachment.FilePath;
                        obDTO.HasCaseClosureAttachment = false;
                    }
                }
                _teleMentalHealthBLL.Close(obDTO,false);
                ShowInfoMessage(CommonResource.CloseCaseMessage);
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }

        [Permission(RoleConsistent.TeleMentalHealth.ReOpen)]
        public ActionResult ReOpenCase(int caseId)
        {
            try
            {
                _teleMentalHealthBLL.ReOpen(caseId);
                ShowInfoMessage(CommonResource.ReOpenCaseMessage);
                return RedirectToAction("Edit", new { mentalHealthId = HashIdsManager.Encrypt(caseId) });
            }
            catch (Exception ex)
            {
               // return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        public ActionResult GetAddCaseClosureForm(string mentalHealthId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(mentalHealthId);
                var model = new CaseClosureDTO
                {
                    CaseId = id,
                    CaseDepartment = DepartmentEnum.MH,
                    CanReOpenCase = false //User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen)
                };


                return PartialView("_CaseClosureForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetCaseClosureForm(string closureId, string mentalHealthId)
        {
            try
            {
                CaseClosureDTO model;
                var id = HashIdsManager.Decrypt(mentalHealthId);

                if (!string.IsNullOrWhiteSpace(closureId))
                {
                    var closureIdInt = HashIdsManager.Decrypt(closureId);
                    model = _teleMentalHealthBLL.GetCaseClosure(closureIdInt);
                    if (model != null && model.CaseClosureAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CaseClosureAttachmentId.Value);
                        model.CaseClosureFilename = NotesAttachment.Description;
                    }
                    model.CaseDepartment = DepartmentEnum.MH;
                    model.CanReOpenCase = User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen) &&
                        _teleMentalHealthBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                }
                else
                {

                    //Get Last Added Closure
                    model = _teleMentalHealthBLL.GetCaseClosures(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
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
                            CaseDepartment = DepartmentEnum.MH,
                            CanReOpenCase = User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen) &&
                            _teleMentalHealthBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed
                        };
                    }
                    else
                    {
                        model.CaseDepartment = DepartmentEnum.MH;
                        model.CanReOpenCase=User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen) &&
                            _teleMentalHealthBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                    }
                }

                return PartialView("_CaseClosureForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetCaseClosureGrid(string mentalHealthId)
        {
            try
            {

                return PartialView("_CaseClosureGrid", "TeleMentalHealth");

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


                var _res = _teleMentalHealthBLL.GetCaseClosures(caseId).ToList();


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
    }

}