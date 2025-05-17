using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Encryption;
using TripleA.Utilities.HashidsNet;
using BoulevardManagement.BLL;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Utilities;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class AttachmentController : BaseController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IBoulevardAttachmentBLL _AttachmentBL;

        public AttachmentController(IUnitOfWorkAsync UnitOfWorkAsync, IBoulevardAttachmentBLL AttachmentBL,
             IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _unitOfWorkAsync = UnitOfWorkAsync;
            _AttachmentBL = AttachmentBL;
        }
        // GET: Attachment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int type, int objectId)
        {
            try
            {

                var res = _AttachmentBL.AttachmentGetByAttachmentTypeAndObjectId((AttachmentReferenceTypes)type, objectId);
                return Json(res.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, BoulevardAttachmentDTO ZestAttachmentDTO, int type, int parentid, int referanceStatus, string filename, string filePath)
        {
            try
            {
                if (ZestAttachmentDTO != null && !String.IsNullOrEmpty(ZestAttachmentDTO.Description) && !String.IsNullOrEmpty(filename))
                {
                    ZestAttachmentDTO.ReferenceType = (AttachmentReferenceTypes)type;
                    ZestAttachmentDTO.ReferenceId = parentid;
                    ZestAttachmentDTO.ReferenceStatus = referanceStatus;

                    //ZestAttachmentDTO.Description = filename;
                    ZestAttachmentDTO.FilePath = filePath;

                    int id = _AttachmentBL.AddAttachment(ZestAttachmentDTO);

                    ZestAttachmentDTO = _AttachmentBL.AttachmentGetById(id);
                }
                return Json(new[] { ZestAttachmentDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {


                ModelState.AddModelError("serverException", ex.ToString());

                return Json(new[] { ZestAttachmentDTO }.ToDataSourceResult(request, ModelState));
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, BoulevardAttachmentDTO ZestAttachmentDTO, int type, int parentid, int referanceStatus, string filename, string filePath)
        {
            try
            {
                if (ZestAttachmentDTO != null && !String.IsNullOrEmpty(ZestAttachmentDTO.Description) && !String.IsNullOrEmpty(filename))
                {
                    ZestAttachmentDTO.ReferenceType = (AttachmentReferenceTypes)type;
                    ZestAttachmentDTO.ReferenceId = parentid;
                    ZestAttachmentDTO.ReferenceStatus = referanceStatus;
                    //ZestAttachmentDTO.Description = filename;
                    ZestAttachmentDTO.FilePath = filePath;
                    _AttachmentBL.Update(ZestAttachmentDTO);
                }


                return Json(new[] { ZestAttachmentDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {


                ModelState.AddModelError("serverException", ex.ToString());

                return Json(new[] { ZestAttachmentDTO }.ToDataSourceResult(request, ModelState));
            }

        }

        [Authorize]
        public FileResult Download(string attachmentId)
        {
            int Id = HashIdsManager.Decrypt(attachmentId);
            BoulevardAttachmentDTO ZestAttachmentDTO = _AttachmentBL.AttachmentGetById(Id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Configurations.AttachmentsPhysicalPath + "\\" + ZestAttachmentDTO.FilePath);
            int idx = ZestAttachmentDTO.FilePath.LastIndexOf('.');
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ZestAttachmentDTO.Description + "." + ZestAttachmentDTO.FilePath.Substring(idx + 1));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int AttachmentID)
        {
            try
            {
                _AttachmentBL.DeleteAttachment(AttachmentID);

                return Json("");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult AddFile(IEnumerable<HttpPostedFileBase> files, Guid fileGuid)
        {
            try
            {
                return UploadFile(files, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddPledgeDocumentfiles(IEnumerable<HttpPostedFileBase> PledgeDocumentfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PledgeDocumentfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddPatientIDCardFile(IEnumerable<HttpPostedFileBase> PatientIDCardfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PatientIDCardfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddPhysicalExaminationFile(IEnumerable<HttpPostedFileBase> PhysicalExaminationfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PhysicalExaminationfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddPhysicalExaminationFollowUpFile(IEnumerable<HttpPostedFileBase> PhysicalExaminationFollowUpfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PhysicalExaminationFollowUpfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }



        public ActionResult AddPICUPhysicalExaminationFile(IEnumerable<HttpPostedFileBase> PhysicalExaminationfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PhysicalExaminationfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddBloodGasesFile(IEnumerable<HttpPostedFileBase> BloodGasesfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(BloodGasesfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddCaseClosureFile(IEnumerable<HttpPostedFileBase> CaseClosurefiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(CaseClosurefiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }
        
        public ActionResult AddCBCWithDifferentialFile(IEnumerable<HttpPostedFileBase> CBCWithDifferentialfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(CBCWithDifferentialfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }


        public ActionResult AddElectrolytesFile(IEnumerable<HttpPostedFileBase> Electrolytesfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Electrolytesfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddBiochemistryFile(IEnumerable<HttpPostedFileBase> Biochemistryfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Biochemistryfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddCSFStudyFile(IEnumerable<HttpPostedFileBase> CSFStudyfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(CSFStudyfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddChestAndAbdomenXrayFile(IEnumerable<HttpPostedFileBase> ChestAndAbdomenXrayfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(ChestAndAbdomenXrayfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }


        public ActionResult AddOtherInvestigationsFile(IEnumerable<HttpPostedFileBase> OtherInvestigationsfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(OtherInvestigationsfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddConsultationNoteFile(IEnumerable<HttpPostedFileBase> ConsultationNotefiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(ConsultationNotefiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddNICUConsultationFormFile(IEnumerable<HttpPostedFileBase> NICUConsultationFormfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(NICUConsultationFormfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddPICUConsultationFormFile(IEnumerable<HttpPostedFileBase> PICUConsultationFormfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(PICUConsultationFormfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }


        public ActionResult AddNICUConsultationFollowUpFormFile(IEnumerable<HttpPostedFileBase> NICUConsultationFollowUpFormfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(NICUConsultationFollowUpFormfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }


        private ActionResult UploadFile(IEnumerable<HttpPostedFileBase> filesToUpload, Guid fileGuid)
        {
            try
            {
                string attchmentPath = "";
                // The Name of the Upload component is "files"
                if (filesToUpload != null)
                {
                    foreach (var file in filesToUpload)
                    {
                        if (ValidateFile(file))
                            attchmentPath = _AttachmentBL.SaveFile(file, fileGuid);
                        else
                            throw new ApplicationException("File Not Valid");
                    }
                }

                // Return an empty string to signify success
                //return Content("");
                return Json(new { filePath = attchmentPath }, "text/plain");
            }
            catch (Exception ex)
            { throw ex; }
        }

        private bool ValidateFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 5242880)
                return false;
            else if (Path.GetExtension(file.FileName) == ".exe")
                return false;
            else
                return true;
        }


        public ActionResult AddECGFile(IEnumerable<HttpPostedFileBase> ECGfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(ECGfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult AddCxrFile(IEnumerable<HttpPostedFileBase> Cxrfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Cxrfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddClinicalBloodGasesFile(IEnumerable<HttpPostedFileBase> BloodGasesfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(BloodGasesfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddChemistryFile(IEnumerable<HttpPostedFileBase> Chemistryfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Chemistryfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddCBCFile(IEnumerable<HttpPostedFileBase> CBCfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(CBCfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddLabElectrolytesFile(IEnumerable<HttpPostedFileBase> Electrolytesfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Electrolytesfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult AddCoagulationTestFile(IEnumerable<HttpPostedFileBase> CoagulationTestfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(CoagulationTestfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddSerologyFile(IEnumerable<HttpPostedFileBase> Serologyfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Serologyfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddUrineTestFile(IEnumerable<HttpPostedFileBase> UrineTestfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(UrineTestfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ActionResult AddNotesFile(IEnumerable<HttpPostedFileBase> Notesfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(Notesfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult AddMHTherapeuticPlanNotesFile(IEnumerable<HttpPostedFileBase> MHTherapeuticPlanNotesFiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(MHTherapeuticPlanNotesFiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult AddLabUnitGeneralFile(IEnumerable<HttpPostedFileBase> LabUnitGeneralfiles, Guid fileGuid)
        {
            try
            {
                return UploadFile(LabUnitGeneralfiles, fileGuid);
            }
            catch (Exception ex)
            { throw ex; }
        }

    }

}