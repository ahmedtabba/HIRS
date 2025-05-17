using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Helper;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.WebApplication.Resources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{

    public class NPICUController : BaseController
    {
        private readonly IPatientBLL _patientBLL;
        private readonly IBoulevardAttachmentBLL _boulevardAttachmentBLL;

        readonly private INPICUBLL _NPICUBLL;
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }
        public NPICUController(
            INPICUBLL NPICUBLL,
            IErrorLogBLL errorLogBLL,
            IPatientBLL patientBLL, IBoulevardAttachmentBLL boulevardAttachmentBLL) : base(errorLogBLL)
        {
            this._NPICUBLL = NPICUBLL;
            _patientBLL = patientBLL;
            _boulevardAttachmentBLL = boulevardAttachmentBLL;
        }

        // GET: NPICU
        [Permission(RoleConsistent.NPICU.Browse)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int? patientId, Gender? gender, MaritalStatus? maritalStatus, BloodType? bloodType,NPICUType? npicuType)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");

                IQueryable<NPICUDTO> _res;
                _res = _NPICUBLL.GetAllIncludedPatient();
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
                    if (npicuType.HasValue)
                    {
                        _res = _res.Where(x => x.NPICUType == npicuType);

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

        public JsonResult Read_ConsultationFollowUpForm([DataSourceRequest] DataSourceRequest request, int NICUId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");
              
                    var _res = _NPICUBLL.GetNICUConsultationFollowUpForms((int)NICUId).ToList();

               


                foreach (var followUp in _res)
                {
                    followUp.CreatorName = UserManager.Users.Where(x => x.Id == followUp.CreatorId).Select(x => x.FullName).FirstOrDefault();
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

        public JsonResult Read_PICUConsultationFollowUpForm([DataSourceRequest] DataSourceRequest request, int PICUId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");

               
                   var _res = _NPICUBLL.GetPICUConsultationFollowUpForms((int)PICUId).ToList();

                


                foreach (var followUp in _res)
                {
                    followUp.CreatorName = UserManager.Users.Where(x => x.Id == followUp.CreatorId).Select(x => x.FullName).FirstOrDefault();
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

        public JsonResult Read_NPICUInvestigation([DataSourceRequest] DataSourceRequest request, int NICUId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _NPICUBLL.GetNPICUInvestigations(NICUId).ToList();


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

        public JsonResult Read_NPICUConsultantSection([DataSourceRequest] DataSourceRequest request, int NICUId)
        {
            try
            {
                //To NormalizeDateFilter Kendo Grid
                // request.NormalizeDateFilter("BirthDate");


                var _res = _NPICUBLL.GetNPICUConsultantSections(NICUId).ToList();


                foreach (var ConsultantSection in _res)
                {
                    ConsultantSection.CreatorName = UserManager.Users.Where(x => x.Id == ConsultantSection.CreatorId).Select(x => x.FullName).FirstOrDefault();
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

        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.NPICU.Delete)]

        public ActionResult DeleteConsultationFollowUpForm([DataSourceRequest] DataSourceRequest request, NICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {
                
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }
                
                _NPICUBLL.DeleteNICUConsultationFollowUpForm(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult DeletePICUConsultationFollowUpForm([DataSourceRequest] DataSourceRequest request, PICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _NPICUBLL.DeletePICUConsultationFollowUpForm(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult DeleteNPICUInvestigation([DataSourceRequest] DataSourceRequest request, NPICUInvestigationDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _NPICUBLL.DeleteNPICUInvestigation(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult DeleteNPICUConsultantSection([DataSourceRequest] DataSourceRequest request, NPICUConsultantSectionDTO obDTO)
        {
            try
            {

                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("Date"))
                        ModelState[key].Errors.Clear();
                }

                _NPICUBLL.DeleteNPICUConsultantSection(obDTO.Id);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        [Permission(RoleConsistent.NPICU.Edit, RoleConsistent.NPICU.Add)]
        public ActionResult Edit(string NPICUId = "", string section = "")
        {
            NPICUDTO obDTO;
            if (String.IsNullOrWhiteSpace(NPICUId))
            {
                return HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(section))
                {
                    TempData["Section"] = section;
                    return RedirectToAction("Edit", "NPICU", new { NPICUId = NPICUId });
                }
                var id = HashIdsManager.Decrypt(NPICUId);
                obDTO = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == id).FirstOrDefault();
                if (obDTO == null)
                {
                    return HttpNotFound();
                }
                obDTO.InvolvedConsultantsIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.Consultant).Select(x => x.UserId).ToList();
                obDTO.InvolvedServiceProvidersIds = obDTO.InvolvedUsers.Where(x => x.JobRole == JobRole.ServiceProvider).Select(x => x.UserId).ToList();
                ViewBag.PatientId = HashIdsManager.Encrypt(obDTO.PatientId);
                ViewBag.TeleNPICUId = obDTO.EncrptedId;
                ViewBag.CurrentStep = (int)obDTO.CurrentStep;
                
                if (!obDTO.InvolvedConsultantsIds.Contains(GetCurrentUser.UserId) && !obDTO.InvolvedServiceProvidersIds.Contains(GetCurrentUser.UserId) && obDTO.CreatedByUserId != GetCurrentUser.UserId)
                {
                    return RedirectToAction("AccessDenied", "Home");

                }
            }
            return View(obDTO);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(NPICUDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = _NPICUBLL.IsValid(obDTO);

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new NPICUUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                        obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new NPICUUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                        obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                        obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());

                        _NPICUBLL.Update(obDTO);
                        ShowSuccessfullyUpdated();

                    }
                    else
                    {

                        ShowErrorMessage(errorMessage);
                        return View(obDTO);
                    }

                    return RedirectToAction("Edit", new { NPICUId = obDTO.EncrptedId });

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }


        [Permission(RoleConsistent.NPICU.Add)]
        public ActionResult Create(NPICUDTO obDTO)
        {
            try
            {

                obDTO.InvolvedServiceProviders = obDTO.InvolvedServiceProvidersIds.Select(x => new NPICUUserDTO { UserId = x, JobRole = JobRole.ServiceProvider }).ToList();
                obDTO.InvolvedConsultants = obDTO.InvolvedConsultantsIds.Select(x => new NPICUUserDTO { UserId = x, JobRole = JobRole.Consultant }).ToList();

                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedServiceProviders.ToList());
                obDTO.InvolvedUsers.AddRange(obDTO.InvolvedConsultants.ToList());

                obDTO.Id = _NPICUBLL.Insert(obDTO);

                ShowSuccessfullyAdded();

                return RedirectToAction("Edit", "NPICU", new { NPICUId = obDTO.EncrptedId });
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.NPICU.Delete)]

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, NPICUDTO obDTO)
        {
            try
            {
                /*
                Code To Scape Date Validate Error
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("BirthDate"))
                        ModelState[key].Errors.Clear();
                }
                */
                var errors = _NPICUBLL.ValidToBeDeleted(obDTO.Id);
                if (string.IsNullOrWhiteSpace(errors))
                    _NPICUBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("Id", errors);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public JsonResult GetNPICUs(string text, int? prevId)
        {
            try
            {
                var NPICUs = _NPICUBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(text))
                    NPICUs = NPICUs.Where(x => x.PatientName.Contains(text));

                var resList = NPICUs.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_NPICUBLL.GetAll().Where(x => x.Id == prevId && x.PatientName.Contains(text)).ToList());
                        else
                            resList.AddRange(_NPICUBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }






        public JsonResult GetNPICUTypes()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(NPICUType)).Cast<Enum>();
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


        public ActionResult GetSickyMenu(string telenicuId)
        {
            try
            {
                int id = HashIdsManager.Decrypt(telenicuId);
                var currentCase = _NPICUBLL.GetAll().Where(x => x.Id == id).Select(x => new { CurrentStep = x.CurrentStep, NPICUType = x.NPICUType }).FirstOrDefault();
                var currentStep = (int)currentCase.CurrentStep;
                var caseType = currentCase.NPICUType;
                List<StickyMenuItemVM> items = new List<StickyMenuItemVM>();

                if (caseType == NPICUType.NICU)
                {
                    if (User.IsInRole(RoleConsistent.NPICU.ViewNICUConsultationForm))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = NPICUResource.ConsultationForm,
                            Id = "ConsultationFormBtn",
                            IsActive = true
                        });


                    if (User.IsInRole(RoleConsistent.NPICU.ViewNICUConsultationFollowUpForm))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = NPICUResource.ConsultationFollowUpForm,
                            Id = "ConsultationFollowUpFormBtn",
                            IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step2
                        });

                }
                else
                {
                    if (User.IsInRole(RoleConsistent.NPICU.ViewPICUConsultationForm))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = NPICUResource.ConsultationForm,
                            Id = "PICUConsultationFormBtn",
                            IsActive = true
                        });


                    if (User.IsInRole(RoleConsistent.NPICU.ViewPICUConsultationFollowUpForm))
                        items.Add(new StickyMenuItemVM
                        {
                            Text = NPICUResource.ConsultationFollowUpForm,
                            Id = "PICUConsultationFollowUpFormBtn",
                            IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step2
                        });
                }
                if (User.IsInRole(RoleConsistent.NPICU.ViewNPICUInvestigation))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = NPICUResource.NPICUInvestigation,
                        Id = "NPICUInvestigationBtn",
                        IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step2
                    });

                if (User.IsInRole(RoleConsistent.NPICU.ViewNPICUConsultantSection))
                    items.Add(new StickyMenuItemVM
                    {
                        Text = NPICUResource.NPICUConsultantSection,
                        Id = "NPICUConsultantSectionBtn",
                        IsActive = currentStep >= (int)TeleMentalHealthCurrentSteps.Step2
                    });


                if (User.IsInRole(RoleConsistent.NPICU.ViewChat))
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


        public ActionResult GetNICUConsultationForm(string telenicuId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(telenicuId);
                var model = _NPICUBLL.GetNICUConsultationForm(id);

                if (model!=null && model.PhysicalExaminationAttachmentId.HasValue)
                {
                    var PhysicalExaminationAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PhysicalExaminationAttachmentId.Value);
                    model.PhysicalExaminationFilename = PhysicalExaminationAttachment.Description;
                }

                if (model != null && model.NICUConsultationFormAttachmentId.HasValue)
                {
                    var NICUConsultationFormAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NICUConsultationFormAttachmentId.Value);
                    model.NICUConsultationFormFilename = NICUConsultationFormAttachment.Description;
                }

                if (model == null)
                    model = new NICUConsultationFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status };


                return PartialView("_NICUConsultationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPICUConsultationForm(string telenicuId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(telenicuId);
                var model = _NPICUBLL.GetPICUConsultationForm(id);
                if (model == null)
                    model = new PICUConsultationFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status  };


                if (model.PhysicalExaminationAttachmentId.HasValue)
                {
                    var PhysicalExaminationAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PhysicalExaminationAttachmentId.Value);
                    model.PhysicalExaminationFilename = PhysicalExaminationAttachment.Description;
                }

                if (model.PICUConsultationFormAttachmentId.HasValue)
                {
                    var PICUConsultationFormAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PICUConsultationFormAttachmentId.Value);
                    model.PICUConsultationFormFilename = PICUConsultationFormAttachment.Description;
                }


                return PartialView("_PICUConsultationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateNICUConsultationForm(NICUConsultationFormDTO obDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var nicu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();
                    _NPICUBLL.UpdateNICUConsultationForm(obDTO);
                    if (obDTO.HasPhysicalExaminationAttachment && !String.IsNullOrEmpty(obDTO.PhysicalExaminationFilename) && System.IO.File.Exists(obDTO.PhysicalExaminationFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NICUPhysicalExamination;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Psychosocial_History_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.PhysicalExaminationFilePath;

                       var attachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.PhysicalExaminationAttachmentId = attachment.Id;
                            obDTO.PhysicalExaminationFilename = attachment.Description;
                            obDTO.PhysicalExaminationFilePath = attachment.FilePath;
                            obDTO.HasPhysicalExaminationAttachment = false;
                        }
                    }


                    if (obDTO.HasNICUConsultationFormAttachment && !String.IsNullOrEmpty(obDTO.NICUConsultationFormFilename) && System.IO.File.Exists(obDTO.NICUConsultationFormFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NICUConsultationForm;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "NICU_Consultation_Form_For_Patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.NICUConsultationFormFilePath;

                       var attachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(attachId);
                        if (attachment != null)
                        {
                            obDTO.NICUConsultationFormAttachmentId = attachment.Id;
                            obDTO.NICUConsultationFormFilename = attachment.Description;
                            obDTO.NICUConsultationFormFilePath = attachment.FilePath;
                            obDTO.HasNICUConsultationFormAttachment = false;
                        }
                    }


                    _NPICUBLL.UpdateNICUConsultationForm(obDTO,false);
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
        public ActionResult UpdatePICUConsultationForm(PICUConsultationFormDTO obDTO)
        {
            try
            {
                var picu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    _NPICUBLL.UpdatePICUConsultationForm(obDTO);
                }
                if (obDTO.HasPhysicalExaminationAttachment && !String.IsNullOrEmpty(obDTO.PhysicalExaminationFilename) && System.IO.File.Exists(obDTO.PhysicalExaminationFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.PICUPhysicalExamination;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Psychosocial_History_for_patient" + picu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.PhysicalExaminationFilePath;

                   var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                    if (attachment != null)
                    {
                        obDTO.PhysicalExaminationAttachmentId = attachment.Id;
                        obDTO.PhysicalExaminationFilename = attachment.Description;
                        obDTO.PhysicalExaminationFilePath = attachment.FilePath;
                        obDTO.HasPhysicalExaminationAttachment = false;
                    }
                }

                if (obDTO.HasPICUConsultationFormAttachment && !String.IsNullOrEmpty(obDTO.PICUConsultationFormFilename) && System.IO.File.Exists(obDTO.PICUConsultationFormFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.PICUConsultationForm;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "PICU_Consultation_Form_For_Patient" + picu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.PICUConsultationFormFilePath;

                   var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                    if (attachment != null)
                    {
                        obDTO.PICUConsultationFormAttachmentId = attachment.Id;
                        obDTO.PICUConsultationFormFilename = attachment.Description;
                        obDTO.PICUConsultationFormFilePath = attachment.FilePath;
                        obDTO.HasPICUConsultationFormAttachment = false;
                    }
                }


                if (ModelState.IsValid)
                {

                    _NPICUBLL.UpdatePICUConsultationForm(obDTO,false);
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

        public ActionResult GetNICUConsultationFollowUpForm(string followUpId, string telenicuId)
        {
            try
            {
                NICUConsultationFollowUpFormDTO model;
                if (!string.IsNullOrWhiteSpace(followUpId))
                {
                    var fuId = HashIdsManager.Decrypt(followUpId);

                    model = _NPICUBLL.GetNICUConsultationFollowUpForm(fuId);
                }
                else
                {

                    var id = HashIdsManager.Decrypt(telenicuId);
                    //Get Last Added Follow-up
                    model = _NPICUBLL.GetNICUConsultationFollowUpForms(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model == null)
                    {
                        var consultationForm = _NPICUBLL.GetNICUConsultationForm(id);
                        model = new NICUConsultationFollowUpFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status , BirthWeight = consultationForm.BirthWeight, ChronologicalAge = consultationForm.ChronologicalAge, GestationalAge = consultationForm.GestationalAge };
                    }
                }

                if (model.NICUConsultationFollowUpFormAttachmentId.HasValue)
                {
                    var NICUConsultationFollowUpFormAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.NICUConsultationFollowUpFormAttachmentId.Value);
                    model.NICUConsultationFollowUpFormFilename = NICUConsultationFollowUpFormAttachment.Description;
                }

                return PartialView("_NICUConsultationFollowUpForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetPICUConsultationFollowUpForm(string followUpId, string telePICUId)
        {
            try
            {
                PICUConsultationFollowUpFormDTO model;
                if (!string.IsNullOrWhiteSpace(followUpId))
                {
                    var fuId = HashIdsManager.Decrypt(followUpId);
                    model = _NPICUBLL.GetPICUConsultationFollowUpForm(fuId);
                }
                else
                {

                    var id = HashIdsManager.Decrypt(telePICUId);
                    //Get Last Added Follow-up
                    model = _NPICUBLL.GetPICUConsultationFollowUpForms(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model == null)
                    {
                        var consultationForm = _NPICUBLL.GetPICUConsultationForm(id);
                        model = new PICUConsultationFollowUpFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status };
                    }
                }

                if (model.PhysicalExaminationAttachmentId.HasValue)
                {
                    var PhysicalExaminationAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.PhysicalExaminationAttachmentId.Value);
                    model.PhysicalExaminationFilename = PhysicalExaminationAttachment.Description;
                }

                return PartialView("_PICUConsultationFollowUpForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetNPICUInvestigation(string investigationId, string telenicuId)
        {
            try
            {
                NPICUInvestigationDTO model;
                if (!string.IsNullOrWhiteSpace(investigationId))
                {
                    var fuId = HashIdsManager.Decrypt(investigationId);
                    model = _NPICUBLL.GetNPICUInvestigation(fuId);
                }
                else
                {

                    var id = HashIdsManager.Decrypt(telenicuId);
                    //Get Last Added Follow-up
                    model = _NPICUBLL.GetNPICUInvestigations(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model == null)
                    {
                      //  var consultationForm = _NPICUBLL.GetNICUConsultationForm(id);
                        model = new NPICUInvestigationDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status };
                    }
                }

                if (model.BloodGasesAttachmentId.HasValue)
                {
                    var BloodGasesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.BloodGasesAttachmentId.Value);
                    model.BloodGasesFilename = BloodGasesAttachment.Description;
                }

                if (model.CBCWithDifferentialAttachmentId.HasValue)
                {
                    var CBCWithDifferentialAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CBCWithDifferentialAttachmentId.Value);
                    model.CBCWithDifferentialFilename = CBCWithDifferentialAttachment.Description;
                }

                if (model.ElectrolytesAttachmentId.HasValue)
                {
                    var ElectrolytesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ElectrolytesAttachmentId.Value);
                    model.ElectrolytesFilename = ElectrolytesAttachment.Description;
                }

                if (model.BiochemistryAttachmentId.HasValue)
                {
                    var BiochemistryAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.BiochemistryAttachmentId.Value);
                    model.BiochemistryFilename = BiochemistryAttachment.Description;
                }

                if (model.CSFStudyAttachmentId.HasValue)
                {
                    var CSFStudyAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CSFStudyAttachmentId.Value);
                    model.CSFStudyFilename = CSFStudyAttachment.Description;
                }

                if (model.ChestAndAbdomenXrayAttachmentId.HasValue)
                {
                    var ChestAndAbdomenXrayAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ChestAndAbdomenXrayAttachmentId.Value);
                    model.ChestAndAbdomenXrayFilename = ChestAndAbdomenXrayAttachment.Description;
                }

                if (model.OtherInvestigationsAttachmentId.HasValue)
                {
                    var OthersAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.OtherInvestigationsAttachmentId.Value);
                    model.OtherInvestigationsFilename = OthersAttachment.Description;
                }



                return PartialView("_NPICUInvestigationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetNPICUConsultantSection(string ConsultantSectionId, string telenicuId)
        {
            try
            {
                NPICUConsultantSectionDTO model;
                if (!string.IsNullOrWhiteSpace(ConsultantSectionId))
                {
                    var fuId = HashIdsManager.Decrypt(ConsultantSectionId);
                    model = _NPICUBLL.GetNPICUConsultantSection(fuId);
                }
                else
                {

                    var id = HashIdsManager.Decrypt(telenicuId);
                    //Get Last Added Follow-up
                    model = _NPICUBLL.GetNPICUConsultantSections(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
                    if (model == null)
                    {
                        var consultationForm = _NPICUBLL.GetNICUConsultationForm(id);
                        model = new NPICUConsultantSectionDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status  };
                    }
                }

                if (model.ConsultationNoteAttachmentId.HasValue)
                {
                    var OthersAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.ConsultationNoteAttachmentId.Value);
                    model.ConsultationNoteFilename = OthersAttachment.Description;
                }


                return PartialView("_NPICUConsultantSectionForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }



        public ActionResult GetNICUConsultationFollowUpFormForAdd(string telenicuId)
        {
            try
            {
                NICUConsultationFollowUpFormDTO model;

                var id = HashIdsManager.Decrypt(telenicuId);

                var consultationForm = _NPICUBLL.GetNICUConsultationForm(id);

                model = new NICUConsultationFollowUpFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status ,BirthWeight=consultationForm.BirthWeight,ChronologicalAge=consultationForm.ChronologicalAge,GestationalAge=consultationForm.GestationalAge };

                return PartialView("_NICUConsultationFollowUpForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPICUConsultationFollowUpFormForAdd(string telePICUId)
        {
            try
            {
                PICUConsultationFollowUpFormDTO model;

                var id = HashIdsManager.Decrypt(telePICUId);


                model = new PICUConsultationFollowUpFormDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status };

                return PartialView("_PICUConsultationFollowUpForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetNPICUInvestigationForAdd(string telenicuId)
        {
            try
            {
                NPICUInvestigationDTO model;

                var id = HashIdsManager.Decrypt(telenicuId);


                model = new NPICUInvestigationDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status };

                return PartialView("_NPICUInvestigationForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetNPICUConsultantSectionForAdd(string telenicuId)
        {
            try
            {
                NPICUConsultantSectionDTO model;

                var id = HashIdsManager.Decrypt(telenicuId);


                model = new NPICUConsultantSectionDTO { NPICUId = id,CaseStatus= _NPICUBLL.GetById(id).Status  };

                return PartialView("_NPICUConsultantSectionForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveNICUConsultationFollowUpForm(NICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var nicu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();
                    if (obDTO.Id == 0)
                    {
                        _NPICUBLL.InsertNICUConsultationFollowUpForm(obDTO);

                    }
                    else
                    {
                        _NPICUBLL.UpdateNICUConsultationFollowUpForm(obDTO);

                    }
                    if (obDTO.HasNICUConsultationFollowUpFormAttachment && !String.IsNullOrEmpty(obDTO.NICUConsultationFollowUpFormFilename) && System.IO.File.Exists(obDTO.NICUConsultationFollowUpFormFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NICUConsultationForm;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "NICU_Consultation_Form_For_Patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.NICUConsultationFollowUpFormFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.NICUConsultationFollowUpFormAttachmentId = attachment.Id;
                            obDTO.NICUConsultationFollowUpFormFilename = attachment.Description;
                            obDTO.NICUConsultationFollowUpFormFilePath = attachment.FilePath;
                            obDTO.HasNICUConsultationFollowUpFormAttachment = false;
                        }
                    }
                        _NPICUBLL.UpdateNICUConsultationFollowUpForm(obDTO,false);
                   
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
        public ActionResult SavePICUConsultationFollowUpForm(PICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var picu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();
                    if (obDTO.Id == 0)
                    {
                        _NPICUBLL.InsertPICUConsultationFollowUpForm(obDTO);

                    }
                    else
                    {
                        _NPICUBLL.UpdatePICUConsultationFollowUpForm(obDTO);

                    }
                    if (obDTO.HasPhysicalExaminationAttachment && !String.IsNullOrEmpty(obDTO.PhysicalExaminationFilename) && System.IO.File.Exists(obDTO.PhysicalExaminationFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.PICUPhysicalExaminationFollowUp;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Psychosocial_History_for_patient" + picu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.PhysicalExaminationFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.PhysicalExaminationAttachmentId = attachment.Id;
                            obDTO.PhysicalExaminationFilename = attachment.Description;
                            obDTO.PhysicalExaminationFilePath = attachment.FilePath;
                            obDTO.HasPhysicalExaminationAttachment = false;
                        }
                    }

                  
                        _NPICUBLL.UpdatePICUConsultationFollowUpForm(obDTO,false);

                    
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
        public ActionResult SaveNPICUInvestigation(NPICUInvestigationDTO obDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var nicu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();
                    if (obDTO.Id == 0)
                    {
                        _NPICUBLL.InsertNPICUInvestigation(obDTO);

                    }
                    else
                    {
                        _NPICUBLL.UpdateNPICUInvestigation(obDTO);

                    }
                    if (obDTO.HasBloodGasesAttachment && !String.IsNullOrEmpty(obDTO.BloodGasesFilename) && System.IO.File.Exists(obDTO.BloodGasesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUBloodGases;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "BloodGases_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.BloodGasesFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.BloodGasesAttachmentId = attachment.Id;
                            obDTO.BloodGasesFilename = attachment.Description;
                            obDTO.BloodGasesFilePath = attachment.FilePath;
                            obDTO.HasBloodGasesAttachment = false;
                        }
                    }

                    if (obDTO.HasCBCWithDifferentialAttachment && !String.IsNullOrEmpty(obDTO.CBCWithDifferentialFilename) && System.IO.File.Exists(obDTO.CBCWithDifferentialFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUCBCWithDifferential;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "CBCWithDifferential_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.CBCWithDifferentialFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.CBCWithDifferentialAttachmentId = attachment.Id;
                            obDTO.CBCWithDifferentialFilename = attachment.Description;
                            obDTO.CBCWithDifferentialFilePath = attachment.FilePath;
                            obDTO.HasCBCWithDifferentialAttachment = false;
                        }
                    }


                    if (obDTO.HasElectrolytesAttachment && !String.IsNullOrEmpty(obDTO.ElectrolytesFilename) && System.IO.File.Exists(obDTO.ElectrolytesFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUElectrolytes;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Electrolytes_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ElectrolytesFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.ElectrolytesAttachmentId = attachment.Id;
                            obDTO.ElectrolytesFilename = attachment.Description;
                            obDTO.ElectrolytesFilePath = attachment.FilePath;
                            obDTO.HasElectrolytesAttachment = false;
                        }
                    }


                    if (obDTO.HasBiochemistryAttachment && !String.IsNullOrEmpty(obDTO.BiochemistryFilename) && System.IO.File.Exists(obDTO.BiochemistryFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUBiochemistry;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Biochemistry_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.BiochemistryFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.BiochemistryAttachmentId = attachment.Id;
                            obDTO.BiochemistryFilename = attachment.Description;
                            obDTO.BiochemistryFilePath = attachment.FilePath;
                            obDTO.HasBiochemistryAttachment = false;
                        }
                    }

                    if (obDTO.HasCSFStudyAttachment && !String.IsNullOrEmpty(obDTO.CSFStudyFilename) && System.IO.File.Exists(obDTO.CSFStudyFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUCSFStudy;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "CSFStudy_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.CSFStudyFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.CSFStudyAttachmentId = attachment.Id;
                            obDTO.CSFStudyFilename = attachment.Description;
                            obDTO.CSFStudyFilePath = attachment.FilePath;
                            obDTO.HasCSFStudyAttachment = false;
                        }
                    }

                    if (obDTO.HasChestAndAbdomenXrayAttachment && !String.IsNullOrEmpty(obDTO.ChestAndAbdomenXrayFilename) && System.IO.File.Exists(obDTO.ChestAndAbdomenXrayFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUChestAndAbdomenXray;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "ChestAndAbdomenXray_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ChestAndAbdomenXrayFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.ChestAndAbdomenXrayAttachmentId = attachment.Id;
                            obDTO.ChestAndAbdomenXrayFilename = attachment.Description;
                            obDTO.ChestAndAbdomenXrayFilePath = attachment.FilePath;
                            obDTO.HasChestAndAbdomenXrayAttachment = false;
                        }
                    }


                    if (obDTO.HasOtherInvestigationsAttachment && !String.IsNullOrEmpty(obDTO.OtherInvestigationsFilename) && System.IO.File.Exists(obDTO.OtherInvestigationsFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUOtherInvestigations;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Others_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.OtherInvestigationsFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.OtherInvestigationsAttachmentId = attachment.Id;
                            obDTO.OtherInvestigationsFilename = attachment.Description;
                            obDTO.OtherInvestigationsFilePath = attachment.FilePath;
                            obDTO.HasOtherInvestigationsAttachment = false;
                        }
                    }
                        _NPICUBLL.UpdateNPICUInvestigation(obDTO,false);
                   
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
        public ActionResult SaveNPICUConsultantSection(NPICUConsultantSectionDTO obDTO, HttpPostedFileBase ConsultantNoteVoice)
        {
            try
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key.Contains("IsConsultationNoteVoiceDeleted")
                        )
                        ModelState[key].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    DeleteVoiceNotes(obDTO);
                    if (ConsultantNoteVoice != null)
                    {
                        var fileGuide = Guid.NewGuid();
                        var file = ConsultantNoteVoice;
                        var fileName = fileGuide.ToString() + ".mp3";
                        var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                        var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                        file.SaveAs(path);
                        obDTO.ConsultationNoteVoiceAttachPath = $"/Uploads/Voice/{fileName}";
                        //return $"{serverPath}Uploads/Voice/{fileName}";
                    }
                    var nicu = _NPICUBLL.GetAllIncludedPatient().Where(x => x.Id == obDTO.NPICUId).FirstOrDefault();

                    if (obDTO.Id == 0)
                    {
                        _NPICUBLL.InsertNPICUConsultantSection(obDTO);

                    }
                    else
                    {
                        _NPICUBLL.UpdateNPICUConsultantSection(obDTO);

                    }
                    if (obDTO.HasConsultationNoteAttachment && !String.IsNullOrEmpty(obDTO.ConsultationNoteFilename) && System.IO.File.Exists(obDTO.ConsultationNoteFilePath))
                    {
                        BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                        boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.NPICUConsultationNote;
                        boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                        boulevardAttachmentDTO.ReferenceStatus = 0;

                        boulevardAttachmentDTO.Description = "Others_for_patient" + nicu.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                        boulevardAttachmentDTO.FilePath = obDTO.ConsultationNoteFilePath;

                         var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                        var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                        if (attachment != null)
                        {
                            obDTO.ConsultationNoteAttachmentId = attachment.Id;
                            obDTO.ConsultationNoteFilename = attachment.Description;
                            obDTO.ConsultationNoteFilePath = attachment.FilePath;
                            obDTO.HasConsultationNoteAttachment = false;
                        }
                    }


                 
                        _NPICUBLL.UpdateNPICUConsultantSection(obDTO,false);

                   
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


        public ActionResult GetNICUConsultationFollowUpGrid( string telenicuId)
        {
            try
            {

                return PartialView("_NICUConsultationFollowUpGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPICUConsultationFollowUpGrid(string telePICUId)
        {
            try
            {

                return PartialView("_PICUConsultationFollowUpGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetNPICUInvestigationGrid(string telenicuId)
        {
            try
            {

                return PartialView("_NPICUInvestigationGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetNPICUConsultantSectionGrid(string telenicuId)
        {
            try
            {

                return PartialView("_NPICUConsultantSectionGrid");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetNPICUTypeEnumList()
        {
            try
            {
                var enumValues = Enum.GetValues(typeof(NPICUType)).Cast<Enum>();
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
        private void DeleteVoiceNotes(NPICUConsultantSectionDTO obDTO)
        {
            if (obDTO.IsConsultationNoteVoiceDeleted && obDTO.ConsultationNoteVoiceAttachPath != null)
            {
                string fullPath = Request.MapPath("~/Uploads/Voice/" + obDTO.ConsultationNoteVoiceAttachPath.Replace("/Uploads/Voice/", ""));
                System.IO.File.Delete(fullPath);
                obDTO.ConsultationNoteVoiceAttachPath = null;
            }

        }


        [Permission(RoleConsistent.NPICU.Close)]
        public ActionResult CloseCase(CaseClosureDTO obDTO)
        {
            try
            {
                var MentalHealth = _NPICUBLL.GetAll().Where(x => x.Id == obDTO.CaseId).FirstOrDefault();
                _NPICUBLL.Close(obDTO);
                if (obDTO.HasCaseClosureAttachment && !String.IsNullOrEmpty(obDTO.CaseClosureFilename) && System.IO.File.Exists(obDTO.CaseClosureFilePath))
                {
                    BoulevardAttachmentDTO boulevardAttachmentDTO = new BoulevardAttachmentDTO();
                    boulevardAttachmentDTO.ReferenceType = AttachmentReferenceTypes.CaseClosure;
                    boulevardAttachmentDTO.ReferenceId = obDTO.Id;
                    boulevardAttachmentDTO.ReferenceStatus = 0;

                    boulevardAttachmentDTO.Description = "Notes_for_patient" + MentalHealth.PatientName.Replace(' ', '_') + "_" + DateTime.Now.ToString();
                    boulevardAttachmentDTO.FilePath = obDTO.CaseClosureFilePath;

                     var AttachId= _boulevardAttachmentBLL.AddAttachment(boulevardAttachmentDTO);
                    var attachment = _boulevardAttachmentBLL.AttachmentGetById(AttachId);
                    if (attachment != null)
                    {
                        obDTO.CaseClosureAttachmentId = attachment.Id;
                        obDTO.CaseClosureFilename = attachment.Description;
                        obDTO.CaseClosureFilePath = attachment.FilePath;
                        obDTO.HasCaseClosureAttachment = false;
                    }
                }
                _NPICUBLL.Close(obDTO,false);
                ShowInfoMessage(CommonResource.CloseCaseMessage);
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }

        [Permission(RoleConsistent.NPICU.ReOpen)]
        public ActionResult ReOpenCase(int caseId)
        {
            try
            {
                _NPICUBLL.ReOpen(caseId);
                ShowInfoMessage(CommonResource.ReOpenCaseMessage);
                return RedirectToAction("Edit", new { NPICUId = HashIdsManager.Encrypt(caseId) });
            }
            catch (Exception ex)
            {
                // return Json(new { Success = false, Message = GetErrorMessage(ex) });

                throw;
            }
        }



        public ActionResult GetAddCaseClosureForm(string telenicuId)
        {
            try
            {
                var id = HashIdsManager.Decrypt(telenicuId);
                var model = new CaseClosureDTO
                {
                    CaseId = id,
                    CaseDepartment = DepartmentEnum.NPICU,
                    CanReOpenCase = false //User.IsInRole(RoleConsistent.TeleMentalHealth.ReOpen)
                };


                return PartialView("_CaseClosureForm", model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult GetCaseClosureForm(string closureId, string telenicuId)
        {
            try
            {
                CaseClosureDTO model;
                var id = HashIdsManager.Decrypt(telenicuId);

                if (!string.IsNullOrWhiteSpace(closureId))
                {
                    var closureIdInt = HashIdsManager.Decrypt(closureId);
                    model = _NPICUBLL.GetCaseClosure(closureIdInt);
                    if (model != null && model.CaseClosureAttachmentId.HasValue)
                    {
                        var NotesAttachment = _boulevardAttachmentBLL.AttachmentGetById(model.CaseClosureAttachmentId.Value);
                        model.CaseClosureFilename = NotesAttachment.Description;
                    }
                    model.CaseDepartment = DepartmentEnum.NPICU;
                    model.CanReOpenCase = User.IsInRole(RoleConsistent.NPICU.ReOpen) &&
                        _NPICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                }
                else
                {

                   
                    //Get Last Added Closure
                    model = _NPICUBLL.GetCaseClosures(id).OrderByDescending(x => x.Id).FirstOrDefault(); ;
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
                            CaseDepartment = DepartmentEnum.NPICU,
                            CanReOpenCase = User.IsInRole(RoleConsistent.NPICU.ReOpen) &&
                            _NPICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed
                        };
                    }
                    else
                    {
                        model.CaseDepartment = DepartmentEnum.NPICU;
                        model.CanReOpenCase = User.IsInRole(RoleConsistent.NPICU.ReOpen) &&
                            _NPICUBLL.GetAll().Where(x => x.Id == id).Select(x => x.Status).FirstOrDefault() == CaseStatus.Closed;
                    }
                }

                return PartialView("_CaseClosureForm", model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetCaseClosureGrid(string telenicuId)
        {
            try
            {

                return PartialView("_CaseClosureGrid","NPICU");

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


                var _res = _NPICUBLL.GetCaseClosures(caseId).ToList();


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