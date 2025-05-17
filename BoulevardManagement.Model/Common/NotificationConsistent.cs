using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Common
{
    public static class NotificationConsistent
    {
        public class Patient
        {
            public const string Add = @"Patient\Add Patient";
            public const string Edit = @"Patient\Edit Patient";
            public const string Delete = @"Patient\Delete Patient";
        }

        public class Zoom
        {
            public const string JoinMeeting = @"Zoom\Join Meeting";
        }

        public class UserManagement
        {
            public const string Add = @"User Management\Add New User";
            public const string Edit = @"User Management\Edit User";
            public const string Delete = @"User Management\Delete User";
            public const string ChangePermitionGroupForUser = @"User Management\Change Permition Group For User";
            public const string ChangePasswordForUser = @"User Management\Change Password For User";
        }

        public class UserGroup
        {
            public const string Add = @"User Permissions Groups\Add Permissions Group";
            public const string Delete = @"User Permissions Groups\Delete Permissions Group";
            public const string Edit = @"User Permissions Groups\Edit Permissions Group";
        }

        public class NotificationGroup
        {
            public const string Add = @"Notification Group\Add Notification Group";
            public const string Delete = @"Notification Group\Delete Notification Group";
            public const string Edit = @"Notification Group\Edit Notification Group";
        }

        public class TeleMentalHealth
        {
            public const string Add = @"Mental Health\Add Mental Health";
            public const string Delete = @"Mental Health\Delete Mental Health";
            public const string Edit = @"Mental Health\Edit Mental Health";
            public const string ReOpen = @"Mental Health\ReOpen Mental Health";
            public const string Close = @"Mental Health\Close Mental Health";
            public const string UpdateCllinicalStory = @"Mental Health\Update Clinical Story";
            public const string UpdateWrittenPledge = @"Mental Health\Update Written Pledge";
            public const string UpdatePhysicalExaminationReport = @"Mental Health\Update Physical Examination Report";
            public const string UpdateTherapeuticPlan = @"Mental Health\Update Therapeutic Plan";
            public const string EditVitalSign = @"Mental Health\Edit Vital Sign";
            public const string AddVitalSign = @"Mental Health\Add Vital Sign";
            public const string DeleteVitalSign = @"Mental Health\Delete Vital Sign";
            public const string ChatOnMentalHealth = @"Mental Health\Chat On Mental Health";

        }


        public class NPICU
        {
            public const string Edit = @"NPICU\Edit NPICU";
            public const string Add = @"NPICU\Add NPICU";
            public const string ReOpen = @"NPICU\ReOpen NPICU";
            public const string Close = @"NPICU\Close NPICU";
            public const string UpdateNICUConsultationForm = @"NPICU\Update NICU Consultation Form";
            public const string UpdatePICUConsultationForm = @"NPICU\Update PICU Consultation Form";
            public const string UpdateNICUConsultationFollowUpForm = @"NPICU\Update NICU Consultation Follow-up Form";
            public const string AddNICUConsultationFollowUpForm = @"NPICU\Add NICU Consultation Follow-up Form";
            public const string UpdatePICUConsultationFollowUpForm = @"NPICU\Update PICU Consultation Follow-up Form";
            public const string AddPICUConsultationFollowUpForm = @"NPICU\Add PICU Consultation Follow-up Form";
            public const string DeletePICUConsultationFollowUpForm = @"NPICU\Delete PICU Consultation Follow-up Form";
            public const string DeleteNICUConsultationFollowUpForm = @"NPICU\Delete NICU Consultation Follow-up Form";
            public const string AddNPICUInvestigation = @"NPICU\Add NPICU Investigation";
            public const string UpdateNPICUInvestigation = @"NPICU\Update NPICU Investigation";
            public const string AddNPICUConsultantSection = @"NPICU\Add NPICU Consultant Section";
            public const string UpdateNPICUConsultantSection = @"NPICU\Update NPICU Consultant Section";
            public const string ChatOnNPICU = @"NPICU\Chat On NPICU";

        }



        public class TeleIcu
        {
            public const string Add = @"ICU\Add ICU";
            public const string Delete = @"ICU\Delete ICU";
            public const string Edit = @"ICU\Edit ICU";
            public const string ReOpen = @"ICU\ReOpen ICU";
            public const string Close = @"ICU\Close ICU";
            public const string UpdateCllinicalStory = @"ICU\Update Clinical Story";
            public const string UpdateMedicationDailyScheduleTable = @"ICU\Update Medication Daily Schedule Table";
            public const string UpdateLabUnit = @"ICU\Update Lab Unit";
            public const string UpdateInternalConsultationForm = @"ICU\Update Internal Consultation Form";
            public const string UpdateConsultationForm = @"ICU\Update  Consultation Form";
            public const string UpdatePatientExitStatusReport = @"ICU\Update Patient Exit Status Report";
            public const string ChatOnICU = @"ICU\Chat On ICU";

        }
        public class TeleIcuMedicationScheduler
        {
            public const string EditMedicationScheduler = @"ICU Medication Scheduler\Edit Medication Scheduler";
            public const string AddMedicationScheduler = @"ICU Medication Scheduler\Add Medication Scheduler";
            public const string DeleteMedicationScheduler = @"ICU Medication Scheduler\Delete Medication Scheduler";
        }
        public class TeleIcuDiabetesControl
        {
            public const string EditDiabetesControl = @"ICU Diabetes Control\Edit Diabetes Control";
            public const string AddDiabetesControl = @"ICU Diabetes Control\Add Diabetes Control";
            public const string DeleteDiabetesControl = @"ICU Diabetes Control\Delete Diabetes Control";
        }
        public class TeleIcuVitalSign
        {
            public const string EditVitalSign = @"ICU Vital Sign\Edit Vital Sign";
            public const string AddVitalSign = @"ICU Vital Sign\Add Vital Sign";
            public const string DeleteVitalSign = @"ICU Vital Sign\Delete Vital Sign";
        }
        public class TeleIcuPump
        {
            public const string EditPump = @"ICU Pump\Edit Pump";
            public const string AddPump = @"ICU Pump\Add Pump";
            public const string DeletePump = @"ICU Pump\Delete Pump";
        }
        




        public static Dictionary<string, List<string>> Groups = new Dictionary<string, List<string>>();

        static NotificationConsistent()
        {
            Groups.Add("Patient", new List<string>() { "Patient"});
            Groups.Add("Mental Health", new List<string>() { "Mental Health" });
            Groups.Add("ICU", new List<string>() { "ICU", "ICU Medication Scheduler", "ICU Diabetes Control", "ICU Vital Sign", "ICU Pump" });
            Groups.Add("NPICU", new List<string>() { "NPICU" });
            Groups.Add("Zoom", new List<string>() { "Zoom" });
            Groups.Add("User Management", new List<string>() { "User Management", "User Permissions Groups", "Notification Group" });

        }
    }
}
