using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL.Interfaces
{

    public interface INPICUBLL : IService<NPICU>
    {
        IQueryable<NPICUDTO> GetAll();
        IQueryable<NPICUDTO> GetAllIncludedPatient();
        int Insert(NPICUDTO obDTO);
        void Update(NPICUDTO obDTO);
        void Delete(int id);
        NPICUDTO GetById(int id);
        string IsValid(NPICUDTO obDTO);
        string ValidToBeDeleted(int id);

        void UpdateNICUConsultationForm(NICUConsultationFormDTO obDTO,bool PublishNotification=true);
        NICUConsultationFormDTO GetNICUConsultationForm(int NICUId);

        void UpdatePICUConsultationForm(PICUConsultationFormDTO obDTO, bool PublishNotification = true);
        PICUConsultationFormDTO GetPICUConsultationForm(int NICUId);

        void UpdateNICUConsultationFollowUpForm(NICUConsultationFollowUpFormDTO obDTO,bool PublishNotification=true);
        int InsertNICUConsultationFollowUpForm(NICUConsultationFollowUpFormDTO obDTO);
        NICUConsultationFollowUpFormDTO GetNICUConsultationFollowUpForm(int id);
        IQueryable<NICUConsultationFollowUpFormDTO> GetNICUConsultationFollowUpForms(int NICUId);
        void DeleteNICUConsultationFollowUpForm(int id);

        void UpdateNPICUInvestigation(NPICUInvestigationDTO obDTO, bool PublishNotification = true);
        int InsertNPICUInvestigation(NPICUInvestigationDTO obDTO);
        NPICUInvestigationDTO GetNPICUInvestigation(int id);
        IQueryable<NPICUInvestigationDTO> GetNPICUInvestigations(int NICUId);
        void DeleteNPICUInvestigation(int id);


        void UpdateNPICUConsultantSection(NPICUConsultantSectionDTO obDTO, bool PublishNotification = true);
        int InsertNPICUConsultantSection(NPICUConsultantSectionDTO obDTO);
        NPICUConsultantSectionDTO GetNPICUConsultantSection(int id);
        IQueryable<NPICUConsultantSectionDTO> GetNPICUConsultantSections(int NICUId);
        void DeleteNPICUConsultantSection(int id);

        void UpdatePICUConsultationFollowUpForm(PICUConsultationFollowUpFormDTO obDTO, bool PublishNotification = true);
        int InsertPICUConsultationFollowUpForm(PICUConsultationFollowUpFormDTO obDTO);
        PICUConsultationFollowUpFormDTO GetPICUConsultationFollowUpForm(int id);
        IQueryable<PICUConsultationFollowUpFormDTO> GetPICUConsultationFollowUpForms(int PICUId);
        void DeletePICUConsultationFollowUpForm(int id);
        void NotifyZoomMeeting(string url, int id);
        void Close(CaseClosureDTO obDTO, bool PublishNotification = true);
        void ReOpen(int caseId);
        CaseClosureDTO GetCaseClosure(int closureId);
        IQueryable<CaseClosureDTO> GetCaseClosures(int npicuId);
        int? GetCurrentlyOpendCaseIdForPatient(int patientId);
        void UpdateInvolvedUsersColors(int id);
    }

}
