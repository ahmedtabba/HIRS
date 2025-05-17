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

    public interface ITeleMentalHealthBLL : IService<TeleMentalHealth>
    {
        IQueryable<TeleMentalHealthDTO> GetAll();
        IQueryable<TeleMentalHealthDTO> GetAllIncludedPatient();
        int Insert(TeleMentalHealthDTO obDTO);
        void Update(TeleMentalHealthDTO obDTO);
        void Delete(int id);
        TeleMentalHealthDTO GetById(int id);
        string IsValid(TeleMentalHealthDTO obDTO);
        string ValidToBeDeleted(int id);

        TelMHClinicalStoryDTO GetClinicalStory(int teleMentalHealthId);
        int UpdateClinicalStory(TelMHClinicalStoryDTO obDTO,bool ToNextStep = true);
        TelMHWrittenPledgeDTO GetWrittenPledge(int teleMentalHealthId);
        void UpdateWrittenPledge(TelMHWrittenPledgeDTO obDTO,bool PublishNotification= true);
        TelMHPhysicalExaminationReportDTO GetPhysicalExaminationReport(int teleMentalHealthId);
        void UpdatePhysicalExaminationReport(TelMHPhysicalExaminationReportDTO obDTO, bool PublishNotification = true);
        TelMHTherapeuticPlanDTO GetTherapeuticPlan(int planId);
        int InsertVitalSign(TelMHVitalSignDTO obDTO);
        void UpdateVitalSign(TelMHVitalSignDTO obDTO);
        IQueryable<TelMHVitalSignDTO> GetAllVitalSignsByMentalHelthId(int mentalHealthId);
        TelMHVitalSignDTO GetVitalSign(int id);
        void DeleteVitalSign(int id);
        void NotifyZoomMeeting(string url, int id);

        IQueryable<TelMHTherapeuticPlanDTO> GetTherapeuticPlans(int mHealthId);
        void UpdateTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO,bool PublishNotification=true);
        int InsertTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO);
        void DeleteTherapeuticPlan(int id);
        void Close(CaseClosureDTO obDTO, bool PublishNotification = true);
        void ReOpen(int caseId);
        CaseClosureDTO GetCaseClosure(int closureId);
        IQueryable<CaseClosureDTO> GetCaseClosures(int mHealthId);
        int? GetCurrentlyOpendCaseIdForPatient(int patientId);
        void UpdateInvolvedUsersColors(int id);
        void UpdateTherapeuticPlanForAttachment(TelMHTherapeuticPlanDTO obDTO);
    }

}
