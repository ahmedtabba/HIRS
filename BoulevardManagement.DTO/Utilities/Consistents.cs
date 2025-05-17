using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO.Utilities
{
    public static class Consistents
    {
        public class TeleMentalHealth
        {
            public const string ClinicalStory = "clinicalstory";
            public const string WrittenPledge = "writtenpledge";
            public const string PhysicalExaminationReport = "physicalexaminationreport";
            public const string TherapeuticPlan = "therapeuticplan";
            public const string VitalSign = "vitalsign";
            public const string Chat = "chat";
        }

        public class NPICU
        {
            public const string ConsultationForm = "consultationform";
            public const string ConsultationFollowUpForm = "consultationfollowupform";
            public const string Investigation = "investigation";
            public const string ConsultantSection = "consultantsection";
            public const string Chat = "chat";
        }

        public class TeleICU
        {
            public const string ClinicalStory = "clinicalstory";
            public const string MedicationDailyScheduleTable = "medicationdailyscheduletable";
            public const string LabUnit = "labunit";
            public const string InternalConsultationForm = "internalconsultationform";
            public const string ConsultationForm = "consultationform";
            public const string PatientExitStatusReport = "patientexitstatusreport";
            public const string Chat = "chat";
        }
    }
}
