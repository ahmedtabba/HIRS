using BoulevardManagement.DTO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities;
using TripleA.Utilities.Helpers;

namespace BoulevardManagement.DTO
{
    public class UserNotificationDTO : EntityDTO
    {
        public string UserId { get; set; }

        public int NotificationId { get; set; }

        public virtual NotificationDTO Notification { get; set; }

        public string NotificationString { get; set; }

        public string NotificationName { get; set; }

        public string ObjectId { get; set; }

        public NotificationObjectTypes ObjectType { get; set; }

        public bool IsView { get; set; }

        public bool IsUnRead { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime CreationTime { get { return CreationDate; } }
        public string CreatorUserId { get; set; }

        public string CreatorFullName { get; set; }
        public string DiractLink { get; set; }


        //For View Grid
        public bool MarkAsRead { get; set; }

        public string ActionURl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DiractLink))
                    return DiractLink;
                string url = "";
                if (ObjectType == NotificationObjectTypes.TeleMentalHealth)
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                else if (ObjectType == NotificationObjectTypes.Patient)
                    url = "/Patient/Edit?PatientId=";
                else if (ObjectType == NotificationObjectTypes.TeleICU)
                    url = "/TeleIcu/Edit?ICUId=";
                else if (ObjectType == NotificationObjectTypes.TeleICU_ClinicalStory)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.ClinicalStory;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_Chat)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.Chat;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_ConsultationForm)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.ConsultationForm;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_InternalConsultationForm)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.InternalConsultationForm;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_LabUnit)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.LabUnit;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.MedicationDailyScheduleTable;
                }
                else if (ObjectType == NotificationObjectTypes.TeleICU_PatientExitStatusReport)
                {
                    url = "/TeleIcu/Edit?ICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleICU.PatientExitStatusReport;
                }
                else if (ObjectType == NotificationObjectTypes.NPICU)
                    url = "/NPICU/Edit?NPICUId=";
                else if (ObjectType == NotificationObjectTypes.NPICU_ConsultationForm)
                {
                    url = "/NPICU/Edit?NPICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.NPICU.ConsultationForm;
                }
                else if (ObjectType == NotificationObjectTypes.NPICU_ConsultationFollowUpForm)
                {
                    url = "/NPICU/Edit?NPICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.NPICU.ConsultationFollowUpForm;
                }
                else if (ObjectType == NotificationObjectTypes.NPICU_Investigation)
                {
                    url = "/NPICU/Edit?NPICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.NPICU.Investigation;
                }
                else if (ObjectType == NotificationObjectTypes.NPICU_ConsultantSection)
                {
                    url = "/NPICU/Edit?NPICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.NPICU.ConsultantSection;
                }
                else if (ObjectType == NotificationObjectTypes.NPICU_Chat)
                {
                    url = "/NPICU/Edit?NPICUId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.NPICU.Chat;
                }

                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_ClinicalStory)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.ClinicalStory;
                }
                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_WrittenPledge)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.WrittenPledge;
                }
                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.PhysicalExaminationReport;
                }
                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.TherapeuticPlan;
                }
                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_VitalSign)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.VitalSign;
                }
                else if (ObjectType == NotificationObjectTypes.TeleMentalHealth_Chat)
                {
                    url = "/TeleMentalHealth/Edit?mentalHealthId=";
                    return HelperMethods.getActionURL(url, ObjectId.ToString()) + "&section=" + Consistents.TeleMentalHealth.Chat;
                }
                return HelperMethods.getActionURL(url, ObjectId.ToString());
            }
        }
        public string ObjectTypeString
        {
            get
            {
                switch (ObjectType)
                {
                    case NotificationObjectTypes.User:
                        return "<span class='badge badge-danger' style='float:left; vertical-align:middle; margin-right:2px'>USR</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NotificationGroup:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>NG</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.PermitionGroup:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>PG</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.Attachment:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ATT</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth:
                        return "<span class='badge badge-warning' style='float:left; vertical-align:middle; margin-right:2px'>MH</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.Patient:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>PA</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>NPI</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.Zoom:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>ZOM</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_ClinicalStory:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-CS</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_WrittenPledge:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-WP</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-PER</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-TP</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_VitalSign:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-VS</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-CH</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CF</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationFollowUpForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CFF</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU_Investigation:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-I</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU_ConsultantSection:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NPCS</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.NPICU_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CH</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_ClinicalStory:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CS</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-MDS</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_LabUnit:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-LU</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_InternalConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-ICF</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_ConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CF</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_PatientExitStatusReport:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-PES</span>" + ObjectType.GetEnumDescription();
                        break;
                    case NotificationObjectTypes.TeleICU_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CH</span>" + ObjectType.GetEnumDescription();
                        break;
                    default:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>-</span>" + ObjectType.GetEnumDescription();
                        break;
                }
            }
        }

        public string ObjectTypeBadge
        {
            get
            {
                switch (ObjectType)
                {
                    case NotificationObjectTypes.User:
                        return "<span class='badge badge-danger' style='float:left; vertical-align:middle; margin-right:2px'>USR</span>";
                        break;
                    case NotificationObjectTypes.NotificationGroup:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>NG</span>";
                        break;
                    case NotificationObjectTypes.PermitionGroup:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>PG</span>";
                        break;
                    case NotificationObjectTypes.Attachment:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ATT</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth:
                        return "<span class='badge badge-warning' style='float:left; vertical-align:middle; margin-right:2px'>MH</span>";
                        break;
                    case NotificationObjectTypes.Patient:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>PA</span>";
                        break;
                    case NotificationObjectTypes.NPICU:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>NPI</span>";
                        break;
                    case NotificationObjectTypes.Zoom:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>ZOM</span>";
                        break;
                    case NotificationObjectTypes.TeleICU:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_ClinicalStory:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-CS</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_WrittenPledge:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-WP</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-PER</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-TP</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_VitalSign:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-VS</span>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>MH-CH</span>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CF</span>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationFollowUpForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CFF</span>";
                        break;
                    case NotificationObjectTypes.NPICU_Investigation:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-I</span>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultantSection:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NPCS</span>";
                        break;
                    case NotificationObjectTypes.NPICU_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>NP-CH</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_ClinicalStory:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CS</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-MDS</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_LabUnit:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-LU</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_InternalConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-ICF</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_ConsultationForm:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CF</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_PatientExitStatusReport:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-PES</span>";
                        break;
                    case NotificationObjectTypes.TeleICU_Chat:
                        return "<span class='badge badge-primary' style='float:left; vertical-align:middle; margin-right:2px'>ICU-CH</span>";
                        break;
                    default:
                        return "<span class='badge badge-success' style='float:left; vertical-align:middle; margin-right:2px'>-</span>";
                        break;
                }
            }
        }

        public string ObjectTypeIcon
        {
            get
            {
                switch (ObjectType)
                {
                    case NotificationObjectTypes.User:
                        return "<i class='icon-2x  flaticon-profile-1 text-info icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NotificationGroup:
                        return "<i class='icon-2x text-dark-50 flaticon2-notification icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.PermitionGroup:
                        return "<i class='icon-2x text-dark-50 flaticon2-lock icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.Attachment:
                        return "<i class='icon-2x text-dark-50 fa fa-file icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.Patient:
                        return "<i class='icon-2x flaticon-avatar text-success icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NPICU:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.Zoom:
                        return "<i class='icon-xl fas fa-camera text-primary'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_ClinicalStory:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_WrittenPledge:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_VitalSign:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.TeleMentalHealth_Chat:
                        return "<i class='icon-xl  text-dark-50 flaticon2-heart-rate-monitor'></i>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationForm:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultationFollowUpForm:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NPICU_Investigation:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NPICU_ConsultantSection:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.NPICU_Chat:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_ClinicalStory:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_LabUnit:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_InternalConsultationForm:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_ConsultationForm:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_PatientExitStatusReport:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    case NotificationObjectTypes.TeleICU_Chat:
                        return "<i class='icon-2x text-dark-50 fa fa-bed icon-lg'></i>";
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }


        public string NotificationURL { get { return string.Format(@"/home/notification/{0}", EncrptedId); } }

        public UserNotificationDTO()
        {

        }
    }
}
