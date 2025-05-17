using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System.Linq;

namespace BoulevardManagement.BLL.Interfaces
{

    public interface ITeleICUBLL : IService<TeleICU>
    {
        IQueryable<TeleICUDTO> GetAll();
        IQueryable<TeleICUDTO> GetAllIncludedPatient();
        int Insert(TeleICUDTO obDTO);
        void Update(TeleICUDTO obDTO);
        void Delete(int id);
        TeleICUDTO GetById(int id);
        string IsValid(TeleICUDTO obDTO);
        string ValidToBeDeleted(int id);

        TelICUClinicalStoryDTO GetClinicalStory(int teleICUId);
        void UpdateClinicalStory(TelICUClinicalStoryDTO obDTO,bool PublishNotification=true);

        TelICUMedicationDailyScheduleDTO GetMedicationDailySchedule(int teleICUId);
        void UpdateMedicationDailySchedule(TelICUMedicationDailyScheduleDTO obDTO);

        int InsertMedicationScheduler(TelICUMedicationSchedulerDTO obDTO);
        void UpdateMedicationScheduler(TelICUMedicationSchedulerDTO obDTO);
        IQueryable<TelICUMedicationSchedulerDTO> GetAllMedicationSchedulersByICUId(int ICUId);
        TelICUMedicationSchedulerDTO GetMedicationScheduler(int id);
        void DeleteMedicationScheduler(int id);
        int InsertDiabetesControl(TelICUDiabetesControlDTO obDTO);
        void UpdateDiabetesControl(TelICUDiabetesControlDTO obDTO);
        IQueryable<TelICUDiabetesControlDTO> GetAllDiabetesControlsByICUId(int ICUId);
        TelICUDiabetesControlDTO GetDiabetesControl(int id);
        void DeleteDiabetesControl(int id);
        int InsertVitalSign(TelICUVitalSignDTO obDTO);
        void UpdateVitalSign(TelICUVitalSignDTO obDTO);
        IQueryable<TelICUVitalSignDTO> GetAllVitalSignsByICUId(int ICUId);
        TelICUVitalSignDTO GetVitalSign(int id);
        void DeleteVitalSign(int id);

        int InsertPump(TelICUPumpDTO obDTO);
        void UpdatePump(TelICUPumpDTO obDTO);
        IQueryable<TelICUPumpDTO> GetAllPumpsByICUId(int ICUId);
        TelICUPumpDTO GetPump(int id);
        void DeletePump(int id);


        TelICULabUnitDTO GetLabUnit(int telelabunitId);
        void UpdateLabUnit(TelICULabUnitDTO obDTO, bool PuplishNotification = true);
        IQueryable<TelICULabUnitDTO> GetLabUnits(int telIcuId);
        IQueryable<TelICUConsultationFormDTO> GetConsultationForms(int telIcuId);

        void DeleteLabUnit(int id);
        void DeleteConsultationForm(int id);

        TelICUInternalConsultationFormDTO GetInternalConsultationForm(int teleICUId);
        void UpdateInternalConsultationForm(TelICUInternalConsultationFormDTO obDTO);

        TelICUConsultationFormDTO GetConsultationForm(int teleConsultationForm);
        void UpdateConsultationForm(TelICUConsultationFormDTO obDTO, bool PublishNotification = true);

        TelICUExitDTO GetExit(int teleICUId);
        void UpdateExit(TelICUExitDTO obDTO);
        void NotifyZoomMeeting(string url, int id);

        void Close(CaseClosureDTO obDTO, bool PublishNotification = true);
        void ReOpen(int caseId);
        CaseClosureDTO GetCaseClosure(int closureId);
        IQueryable<CaseClosureDTO> GetCaseClosures(int ICUId);
        int? GetCurrentlyOpendCaseIdForPatient(int patientId);
        string ValidToCloseCase(int id);
        void UpdateInvolvedUsersColors(int id);
    }

}
