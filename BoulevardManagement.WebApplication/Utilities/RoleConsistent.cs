using Repository.Pattern.Ef6;
using System.Collections.Generic;

namespace BoulevardManagement.WebApplication
{
    public static class RoleConsistent
    {

        public class Project
        {
            public const string Browse = @"Project\Browse Projects";
            public const string Delete = @"Project\Delete Projects";
            public const string Add = @"Project\Add Projects";
            public const string Edit = @"Project\Edit Projects";
        }


        public class Patient
        {
            public const string Add = @"Patient\Add Patients";
            public const string Browse = @"Patient\Browse Patients";
            public const string Delete = @"Patient\Delete Patients";
            public const string Edit = @"Patient\Edit Patients";
        }

        public class UserManagement
        {
            public const string Add = @"User Management\Add New User";
            public const string Browse = @"User Management\Browse Users";
            public const string Edit = @"User Management\Edit User";
            //public const string UserLog = @"User Management\User Log";
        }

        public class UserGroup
        {
            public const string Add = @"User Permissions Groups\Add Permissions Group";
            public const string Browse = @"User Permissions Groups\Browse Permissions Group";
            public const string Delete = @"User Permissions Groups\Delete Permissions Group";
            public const string Edit = @"User Permissions Groups\Edit Permissions Group";
            public const string Manage = @"User Permissions Groups\Manage Permissions Group";
        }

        public class NotificationGroup
        {   public const string Add = @"Notification Group\Add Notification Group";
            public const string Browse = @"Notification Group\Browse Notification Group";
            public const string Delete = @"Notification Group\Delete Notification Group";
            public const string Edit = @"Notification Group\Edit Notification Group";
            //public const string AddAndEdit = @"Notification Group\Add and Edit Notification Group";
        }

        public class Notification
        {
            public const string Browse = @"Notification\Browse Notification";
            public const string Delete = @"Notification\Delete Notification";

            public const string AddAndEdit = @"Notification\Add and Edit Notification";
        }


        public class Location
        {
            public const string Add = @"Location\Add Locations";
            public const string Browse = @"Location\Browse Locations";
            public const string Delete = @"Location\Delete Locations";
            public const string Edit = @"Location\Edit Locations";
        }


        public class TeleMentalHealth
        {
            public const string Browse = @"Mental Health\Browse Mental Healths";
            public const string Delete = @"Mental Health\Delete Mental Healths";
            public const string Add = @"Mental Health\Add Mental Healths";
            public const string Edit = @"Mental Health\Edit Mental Healths";
            public const string EditSettings = @"Mental Health\Edit Settings";

            public const string ReOpen = @"Mental Health\ReOpen Mental Healths";
            public const string Close = @"Mental Health\Close Mental Healths";



            public const string EditClinicalStory = @"Mental Health Clinical Story\Edit Clinical Story";
            public const string ViewClinicalStory = @"Mental Health Clinical Story\View Clinical Story";

            public const string EditMonitoringTheVitalSigns = @"Mental Health Monitoring The Vital Signs\Edit Monitoring The Vital Signs";
            public const string ViewMonitoringTheVitalSigns = @"Mental Health Monitoring The Vital Signs\View Monitoring The Vital Signs";

            public const string EditPhysicalExaminationReport = @"Mental Health Physical Examination Report\Edit Physical Examination Reports";
            public const string ViewPhysicalExaminationReport = @"Mental Health Physical Examination Report\View Physical Examination Reports";

            public const string EditTherapeuticPlan = @"Mental Health Therapeutic Plan\Edit Therapeutic Plans";
            public const string ViewTherapeuticPlan = @"Mental Health Therapeutic Plan\View Therapeutic Plans";

            public const string EditWrittenPledge = @"Mental Health Written Pledge\Edit Written Pledges";
            public const string ViewWrittenPledge = @"Mental Health Written Pledge\View Written Pledges";

            public const string ViewChat = @"Mental Health Chat\View Chats";
        }

        public class StickyNote
        {
            //public const string Browse = @"Customer\Browse Customer";
            public const string Record = @"Note\Record Note";
            public const string Delete = @"Note\Delete Note";
            public const string DownloadRecord = @"Note\Download Record";


        }
        public class TeleICU
        {
            public const string Browse = @"ICU\Browse ICUs";
            public const string Delete = @"ICU\Delete ICUs";
            public const string Add = @"ICU\Add ICUs";
            public const string Edit = @"ICU\Edit ICUs";
            public const string ReOpen = @"ICU\ReOpen ICUs";
            public const string Close = @"ICU\Close ICUs";
            public const string ViewChat = @"ICU Chat\View Chats";
            public const string EditSettings = @"ICU\Edit Settings";

        }
        public class TelIcuClinicalStory
        {
            public const string EditClinicalStory = @"Icu Clinical Story\Edit Clinical Story";
            public const string ViewClinicalStory = @"Icu Clinical Story\View Clinical Story";
        }
        public class TelIcuMedicationDailyScheduleTable
        {
            public const string EditMedicationDailyScheduleTable = @"Icu Medication Daily Schedule Table\Edit Medication Daily Schedule Table";
            public const string ViewMedicationDailyScheduleTable = @"Icu Medication Daily Schedule Table\View Medication Daily Schedule Table";
        }
        public class TelIcuLabUnit
        {
            public const string EditLabUnit = @"Icu Lab Unit\Edit Lab Unit";
            public const string ViewLabUnit = @"Icu Lab Unit\View Lab Unit";
        }
        public class TelIcuInternalConsultationForm
        {
            public const string EditInternalConsultationForm = @"Icu Internal Consultation Form\Edit Internal Consultation Form";
            public const string ViewInternalConsultationForm = @"Icu Internal Consultation Form\View Internal Consultation Form";
        }
        public class TelIcuConsultationForm
        {
            public const string EditConsultationForm = @"Icu  Consultation Form\Edit  Consultation Form";
            public const string ViewConsultationForm = @"Icu  Consultation Form\View  Consultation Form";
        }
        public class TelIcuPatientExitStatusReport
        {
            public const string EditPatientExitStatusReport = @"Icu Patient Exit Status Report\Edit Patient Exit Status Report";
            public const string ViewPatientExitStatusReport = @"Icu Patient Exit Status Report\View Patient Exit Status Report";
        }
        public class NPICU
        {
            public const string Browse = @"N/PICU\Browse N/PICUs";
            public const string Delete = @"N/PICU\Delete N/PICUs";
            public const string Add = @"N/PICU\Add N/PICUs";
            public const string Edit = @"N/PICU\Edit N/PICUs";
            public const string EditSettings = @"N/PICU\Edit Settings";

            public const string ReOpen = @"N/PICU\ReOpen N/PICUs";
            public const string Close = @"N/PICU\Close N/PICUs";


            public const string ViewNICUConsultationForm = @"NICU Consultation Form\View Consultation Form";
            public const string EditNICUConsultationForm = @"NICU Consultation Form\Edit Consultation Form";

            public const string ViewPICUConsultationForm = @"PICU Consultation Form\View Consultation Form";
            public const string EditPICUConsultationForm = @"PICU Consultation Form\Edit Consultation Form";

            public const string ViewNPICUInvestigation = @"NPICU Investigation\View Investigation";
            public const string EditNPICUInvestigation = @"NPICU Investigation\Edit Investigation";

            public const string ViewNPICUConsultantSection = @"NPICU Consultant Section\View Consultant Section";
            public const string EditNPICUConsultantSection = @"NPICU Consultant Section\Edit Consultant Section";

            public const string ViewNICUConsultationFollowUpForm = @"NICU Consultation Follow-up Form\View Consultation Follow-up Form";
            public const string EditNICUConsultationFollowUpForm = @"NICU Consultation Follow-up Form\Edit Consultation Follow-up Form";

            public const string ViewPICUConsultationFollowUpForm = @"PICU Consultation Follow-up Form\View Consultation Follow-up Form";
            public const string EditPICUConsultationFollowUpForm = @"PICU Consultation Follow-up Form\Edit Consultation Follow-up Form";

            public const string ViewChat = @"N/PICU Chat\View Chats";
        }


        public static Dictionary<string, List<string>> Groups = new Dictionary<string, List<string>>();

        static RoleConsistent()
        {
            Groups.Add("Patients", new List<string>() { "Patient" });
            Groups.Add("Locations", new List<string>() { "Location" });
            Groups.Add("Mental Healths", new List<string>() { "Mental Health", "Mental Health Clinical Story", "Mental Health Monitoring The Vital Signs", "Mental Health Physical Examination Report", "Mental Health Therapeutic Plan", "Mental Health Written Pledge" , "Mental Health Chat" });
            Groups.Add("ICU", new List<string>() { "ICU", "Icu Clinical Story", "Icu Medication Daily Schedule Table", "Icu Lab Unit", "Icu Internal Consultation Form", "Icu  Consultation Form", "Icu Patient Exit Status Report", "ICU Chat" });
            Groups.Add("N/PICUs", new List<string>() { "N/PICU", "NICU Consultation Form", "PICU Consultation Form", "NPICU Investigation", "NPICU Consultant Section", "NICU Consultation Follow-up Form" , "PICU Consultation Follow-up Form" , "N/PICU Chat" });
            Groups.Add("Notes", new List<string>() { "Note" });
            Groups.Add("User Management", new List<string>() { "User Management", "User Permissions Groups", "Notification Group" });
        }
    }
}