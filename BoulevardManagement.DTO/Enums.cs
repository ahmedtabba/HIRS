using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BoulevardManagement.DTO.Resources;
using TripleA.Utility.DataAnnotationsAttributes;

namespace BoulevardManagement.DTO
{




    public enum AttachmentReferenceTypes
    {
        TeleMentalHealthPledgeDocument,
        TeleMentalHealthPatientIDCard,
        TeleMentalHealthExamination,
        TeleMentalHealthNotes,
        NICUPsychosocialHistory,
        NICUConsultationForm,
        ICUECG,
        ICUCxr,
        NICUPhysicalExamination,
        NPICUBloodGases,
        NPICUCBCWithDifferential,
        NPICUElectrolytes,
        NPICUBiochemistry,
        NPICUCSFStudy,
        NPICUChestAndAbdomenXray,
        NPICUOtherInvestigations,
        NPICUConsultationNote,
        PICUPhysicalExamination,
        PICUConsultationForm,
        PICUPhysicalExaminationFollowUp,
       // NPICUElectrolytes,
        ICUBloodGases,
        ICUChemistry,
        ICUCBC,
        ICUElectrolytes,
        ICUCoagulationTest,
        ICUSerology,
        ICUUrineTest,
        ICUNotes,
        CaseClosure,
        ICULabUnitGeneral

    }

    public enum SelectionTypeEnum
    {
        OneValue,
        MultiCheck
    }

    //OperationLog Enums
    public enum OperationTypeEnum
    {
        [LocalizedDescription("Add", typeof(OperationLogEnumsResource))]
        Add,

        [LocalizedDescription("Update", typeof(OperationLogEnumsResource))]
        Update,

        [LocalizedDescription("Delete", typeof(OperationLogEnumsResource))]
        Delete,

        [LocalizedDescription("Mention", typeof(OperationLogEnumsResource))]
        Mention,
        [LocalizedDescription("Chat", typeof(OperationLogEnumsResource))]
        Chat
    }



    //JobRoles
    public enum JobRole
    {
        [LocalizedDescription("IT", typeof(JobRoleEnumResource))]
        IT,
        [LocalizedDescription("ServiceProvider", typeof(JobRoleEnumResource))]
        ServiceProvider,
        [LocalizedDescription("Consultant", typeof(JobRoleEnumResource))]
        Consultant

    }


    public enum NotificationObjectTypes
    {
        [Description("User")]
        User=0,
        [Description("Notification Group")]
        NotificationGroup=1,
        [Description("Permition Group")]
        PermitionGroup=2,
        [Description("Attachment")]
        Attachment=3,
        [Description("Mental Health")]
        TeleMentalHealth=4,
        [Description("Patient")]
        Patient=5,
        [Description("N/PICU")]
        NPICU=6,
        [Description("Zoom")]
        Zoom=7,
        [Description("ICU")]
        TeleICU=8,

        //Expanded Types
        [Description("Mental Health - Clinical Story")]
        TeleMentalHealth_ClinicalStory = 9,
        [Description("Mental Health - Written Pledge")]
        TeleMentalHealth_WrittenPledge = 10,
        [Description("Mental Health - Physical Examination Report")]
        TeleMentalHealth_PhysicalExaminationReport = 11,
        [Description("Mental Health - Therapeutic Plan")]
        TeleMentalHealth_TherapeuticPlan = 12,
        [Description("Mental Health - Vital Sign")]
        TeleMentalHealth_VitalSign = 13,
        [Description("Mental Health - Chat")]
        TeleMentalHealth_Chat = 14 ,

        [Description("N/PICU - Consultation Form")]
        NPICU_ConsultationForm = 15,
        [Description("N/PICU - Consultation Follow-Up Form")]
        NPICU_ConsultationFollowUpForm = 16,
        [Description("N/PICU - Investigation")]
        NPICU_Investigation = 17,
        [Description("N/PICU - Consultant Section")]
        NPICU_ConsultantSection = 18,
        [Description("N/PICU - Chat")]
        NPICU_Chat = 19,

        [Description("ICU - Clinical Story")]
        TeleICU_ClinicalStory = 20,
        [Description("ICU - Medication Daily Schedule Table")]
        TeleICU_MedicationDailyScheduleTable = 21,
        [Description("ICU - Lab Unit")]
        TeleICU_LabUnit = 22,
        [Description("ICU - Internal Consultation Form")]
        TeleICU_InternalConsultationForm = 23,
        [Description("ICU - Consultation Form ")]
        TeleICU_ConsultationForm = 24,
        [Description("ICU - Patient Exit Status Report")]
        TeleICU_PatientExitStatusReport = 25,
        [Description("ICU - Chat")]
        TeleICU_Chat = 26,


    }


    public enum SubmitFormType
    {
        SaveAndExist,
        SaveAndContinue,
        SaveAndAddNew
    }


    public enum UserQuickActionEnum
    {
        [LocalizedDescription("AddPatient", typeof(UserQuickActionEnumsResource))]
        AddPatient
    }

    public enum Gender
    {
        [LocalizedDescription("Male", typeof(GenderEnumsResource))]
        Male,
        [LocalizedDescription("Female", typeof(GenderEnumsResource))]
        Female
    }


    public enum MaritalStatus
    {
        [LocalizedDescription("Single", typeof(MaritalStatusEnumResource))]
        Single,
        [LocalizedDescription("Married", typeof(MaritalStatusEnumResource))]
        Married,
        [LocalizedDescription("Widow", typeof(MaritalStatusEnumResource))]
        Widow,
        [LocalizedDescription("Divorced", typeof(MaritalStatusEnumResource))]
        Divorced,
        [LocalizedDescription("Separated", typeof(MaritalStatusEnumResource))]
        Separated
    }


    public enum BloodType
    {
        [LocalizedDescription("O+", typeof(BloodTypeEnumResource))]
        O_Positive,
        [LocalizedDescription("O-", typeof(BloodTypeEnumResource))]
        O_Negative,
        [LocalizedDescription("A+", typeof(BloodTypeEnumResource))]
        A_Positive,
        [LocalizedDescription("A-", typeof(BloodTypeEnumResource))]
        A_Negative,
        [LocalizedDescription("B+", typeof(BloodTypeEnumResource))]
        B_Positive,
        [LocalizedDescription("B-", typeof(BloodTypeEnumResource))]
        B_Negative,
        [LocalizedDescription("AB+", typeof(BloodTypeEnumResource))]
        AB_Positive,
        [LocalizedDescription("AB-", typeof(BloodTypeEnumResource))]
        AB_Negative,
    }

    public enum TeleMentalHealthCurrentSteps
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
    }

    public enum TeleICUCurrentSteps
    {
        ClinicalStory,
        MedicationDailyScheduleTable,
        LabUnit,
        InternalConsultationForm,
        PatientExitStatusReport,
    }
    public enum StickyNoteMessageTypeEnum
    {
        [Description("Text")]
        TextMessageNote,
        [Description("Voice")]
        VoiceMessageNote
    }
    public enum ChannelEnum
    {
        General,
        Lab
    }

    public enum NPICUCurrentSteps
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
    }

    public enum NPICUType
    {
        [LocalizedDescription("NICU", typeof(NPICUTypeEnumResource))]
        NICU,
        [LocalizedDescription("PICU", typeof(NPICUTypeEnumResource))]
        PICU
    }

    public enum LabourAndDelivery
    {
        [LocalizedDescription("VaginalDelivery", typeof(LabourAndDeliveryEnumResource))]
        VaginalDelivery,
        [LocalizedDescription("C_S", typeof(LabourAndDeliveryEnumResource))]
        C_S,
        [LocalizedDescription("Vacuum", typeof(LabourAndDeliveryEnumResource))]
        Vacuum,
        [LocalizedDescription("Forceps", typeof(LabourAndDeliveryEnumResource))]
        Forceps
    }

    public enum BornPlace
    {
        [LocalizedDescription("Inborn", typeof(BornPlaceEnumResource))]
        Inborn,
        [LocalizedDescription("Outborn", typeof(BornPlaceEnumResource))]
        Outborn
    }

    [Flags]
    public enum NICUResuscitation 
    {
        [LocalizedDescription("Drying", typeof(NICUResuscitationEnumResource))]
        Drying=1,
        [LocalizedDescription("Heating", typeof(NICUResuscitationEnumResource))]
        Heating=2,
        [LocalizedDescription("SecretionsSuctioning", typeof(NICUResuscitationEnumResource))]
        SecretionsSuctioning = 4,
        [LocalizedDescription("ColdClamping", typeof(NICUResuscitationEnumResource))]
        ColdClamping = 8,
        [LocalizedDescription("SkinToskin", typeof(NICUResuscitationEnumResource))]
        SkinToskin = 16,
        [LocalizedDescription("HFNC", typeof(NICUResuscitationEnumResource))]
        HFNC = 32,
        [LocalizedDescription("CPAP", typeof(NICUResuscitationEnumResource))]
        CPAP = 64,
        [LocalizedDescription("PPV", typeof(NICUResuscitationEnumResource))]
        PPV = 128,
        [LocalizedDescription("Intubation", typeof(NICUResuscitationEnumResource))]
        Intubation = 256,
        [LocalizedDescription("UVC", typeof(NICUResuscitationEnumResource))]
        UVC = 512,
        [LocalizedDescription("ChestCompression", typeof(NICUResuscitationEnumResource))]
        ChestCompression = 1024,
        [LocalizedDescription("AdrenalineDoses", typeof(NICUResuscitationEnumResource))]
        AdrenalineDoses = 2048,
        [LocalizedDescription("VolumeExpanders", typeof(NICUResuscitationEnumResource))]
        VolumeExpanders = 4096,
        [LocalizedDescription("BloodTransfusion", typeof(NICUResuscitationEnumResource))]
        BloodTransfusion = 8192,
        [LocalizedDescription("Wrappolyethylene", typeof(NICUResuscitationEnumResource))]
        Wrappolyethylene = 16384,
        [LocalizedDescription("CordBloodGases", typeof(NICUResuscitationEnumResource))]
        CordBloodGases = 32768,
    }
    public enum SignTypeEnum
    {
        [LocalizedDescription("SingleMinus", typeof(SignTypeEnumResource))]
        SingleMinus,
        [LocalizedDescription("SinglePlus", typeof(SignTypeEnumResource))]
        SinglePlus,
        [LocalizedDescription("DoublePlus", typeof(SignTypeEnumResource))]
        DoublePlus,
        [LocalizedDescription("TriplePlus", typeof(SignTypeEnumResource))]
        TriplePlus,
    }
    public enum PositiveNegativeEnum
    {
        [LocalizedDescription("Negative", typeof(PositiveNegativeEnumResource))]
        Negative,
        [LocalizedDescription("Positive", typeof(PositiveNegativeEnumResource))]
        Positive,

    }
    public enum AppearanceEnum
    {
        [LocalizedDescription("Clear", typeof(AppearanceEnumResource))]
        Clear,
        [LocalizedDescription("turbid", typeof(AppearanceEnumResource))]
        turbid,
    }
    public enum ColorEnum
    {
        [LocalizedDescription("Yellow", typeof(ColorEnumResource))]
        Yellow,
        [LocalizedDescription("Reddish", typeof(ColorEnumResource))]
        Reddish,
        [LocalizedDescription("Brownish", typeof(ColorEnumResource))]
        Brownish,
    }
    public enum UnderAnesthesiaEnum{
        [LocalizedDescription("General", typeof(UnderAnesthesiaEnumResource))]
        General,
        [LocalizedDescription("Partially", typeof(UnderAnesthesiaEnumResource))]
        Partially

    }


    public enum BloodGases
    {
        [LocalizedDescription("ABG", typeof(BloodGasesEnumResource))]
        ABG,
        [LocalizedDescription("VBG", typeof(BloodGasesEnumResource))]
        VBG,
        [LocalizedDescription("CBG", typeof(BloodGasesEnumResource))]
        CBG
    }


    public enum GeneralAppearance1
    {
        [LocalizedDescription("Well", typeof(GeneralAppearance1EnumResource))]
        Well,
        [LocalizedDescription("Poor", typeof(GeneralAppearance1EnumResource))]
        Poor,
        [LocalizedDescription("Grave", typeof(GeneralAppearance1EnumResource))]
        Grave
    }

    [Flags]
    public enum GeneralAppearance2
    {
        [LocalizedDescription("Cyanosis", typeof(GeneralAppearance1EnumResource))]
        Cyanosis,
        [LocalizedDescription("Pallor", typeof(GeneralAppearance1EnumResource))]
        Pallor,
        [LocalizedDescription("Jaundice", typeof(GeneralAppearance1EnumResource))]
        Jaundice
    }



    [Flags]
    public enum RiskFactorsEnum
    {
    
        [LocalizedDescription("Diabetes", typeof(RiskFactorsEnumResource))]
        Diabetes = 1,
        [LocalizedDescription("Stress", typeof(RiskFactorsEnumResource))]
        Stress = 2,
        [LocalizedDescription("Smoking", typeof(RiskFactorsEnumResource))]
        Smoking = 4,
        [LocalizedDescription("Obesity", typeof(RiskFactorsEnumResource))]
        Obesity = 8,
        [LocalizedDescription("FamilyStory", typeof(RiskFactorsEnumResource))]
        FamilyStory = 16,
        [LocalizedDescription("Fats", typeof(RiskFactorsEnumResource))]
        Fats = 32
            
    }

    public enum DepartmentEnum
    {
        MH,
        NPICU,
        ICU
    }
    public enum DiagnosisStatus
    {
        [LocalizedDescription("Confirmed", typeof(DiagnosisStatusEnumResource))]
        Confirmed,
        [LocalizedDescription("Default", typeof(DiagnosisStatusEnumResource))]
        Default,
        [LocalizedDescription("FirstTime", typeof(DiagnosisStatusEnumResource))]
        FirstTime
    }

    public enum CaseStatus
    {
        [LocalizedDescription("Open", typeof(CaseStatusEnumResource))]
        Open,
        [LocalizedDescription("Closed", typeof(CaseStatusEnumResource))]
        Closed
    }

    public enum CaseCloseReason
    {
        [LocalizedDescription("NormalExit", typeof(CaseCloseReasonEnumResource))]
        NormalExit,
        [LocalizedDescription("Death", typeof(CaseCloseReasonEnumResource))]
        Death,
        [LocalizedDescription("Referral", typeof(CaseCloseReasonEnumResource))]
        Referral,
        [LocalizedDescription("PersonalResponsibilityExit", typeof(CaseCloseReasonEnumResource))]
        PersonalResponsibilityExit,
    }
}
