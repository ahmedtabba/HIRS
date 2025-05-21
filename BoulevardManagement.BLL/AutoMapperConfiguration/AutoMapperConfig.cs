using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Repository.Pattern.Ef6;

namespace BoulevardManagement.BLL.AutoMapperConfiguration
{
    public static class AutoMapperConfiguration
    {
        public static void RegisterAllMappers()
        {


            Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();


                #region Project
                cfg.CreateMap<Project, ProjectDTO>();
                cfg.CreateMap<ProjectDTO, Project>();
                #endregion



                #region ErrorLog
                cfg.CreateMap<ErrorLog, ErrorLogDTO>();
                cfg.CreateMap<ErrorLogDTO, ErrorLog>();
                #endregion


                #region OperationLog
                cfg.CreateMap<OperationLog, OperationLogDTO>()
                     .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => (OperationTypeEnum)src.OperationType));

                cfg.CreateMap<OperationLogDTO, OperationLog>()
                .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => (int)src.OperationType));
                #endregion
                
                #region DepartmentTest

                cfg.CreateMap<DepartmentTest, DepartmentTestDTO>()
                    .ReverseMap();
                
                #endregion

                #region Patient
                cfg.CreateMap<Patient, PatientDTO>()
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentArabicName, opt => opt.MapFrom(src => src.Department.ArabicName))
                   .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department.Code))
                   .ForMember(dest => dest.PatientCode, opt => opt.MapFrom(src => src.Department.Code+"-"+src.NumberStr))
                   .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender))
                     .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => (BloodType?)src.BloodType))
                   .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => (MaritalStatus?)src.MaritalStatus));

                cfg.CreateMap<PatientDTO, Patient>()
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => (int?)src.BloodType))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => (int?)src.MaritalStatus))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (int)src.Gender));
                #endregion

                #region Notifications

                cfg.CreateMap<Notification, NotificationDTO>();
                cfg.CreateMap<NotificationDTO, Notification>();

                cfg.CreateMap<NotificationGroup, NotificationGroupDTO>();
                cfg.CreateMap<NotificationGroupDTO, NotificationGroup>();

                cfg.CreateMap<UserNotification, UserNotificationDTO>()
                       .ForMember(dest => dest.CreatorUserId, opt => opt.MapFrom(src => src.CreatedBy))
                 .ForMember(dest => dest.ObjectType, opt => opt.MapFrom(src => (NotificationObjectTypes)src.ObjectType)); 

                cfg.CreateMap<UserNotificationDTO, UserNotification>()
                 .ForMember(dest => dest.ObjectType, opt => opt.MapFrom(src => (int)src.ObjectType)); ;

                cfg.CreateMap<UserNotificationGroups, UserNotificationGroupsDTO>();
                cfg.CreateMap<UserNotificationGroupsDTO, UserNotificationGroups>();

                cfg.CreateMap<UserNotificationGroups, NotificationGroupDTO>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NotificationGroup.Name));
                #endregion


                #region Department
                cfg.CreateMap<Department, DepartmentDTO>();

                cfg.CreateMap<DepartmentDTO, Department>();
                #endregion


                #region Location
                cfg.CreateMap<Location, LocationDTO>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentArabicName, opt => opt.MapFrom(src => src.Department.ArabicName))
                   .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department.Code))
                   ;

                cfg.CreateMap<LocationDTO, Location>();
                #endregion

                #region UserQuickAction
                cfg.CreateMap<UserQuickAction, UserQuickActionDTO>()
                      .ForMember(dest => dest.Action, opt => opt.MapFrom(src => (UserQuickActionEnum)src.Action));

                cfg.CreateMap<UserQuickActionDTO, UserQuickAction>()
                      .ForMember(dest => dest.Action, opt => opt.MapFrom(src => (int)src.Action));
                #endregion


                #region TelementalHealth
                cfg.CreateMap<TeleMentalHealth, TeleMentalHealthDTO>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName+" "+src.Patient.LastName))
                .ForMember(dest => dest.PatientDepartmentId, opt => opt.MapFrom(src => src.Patient.DepartmentId))
                .ForMember(dest => dest.PatientFatherName, opt => opt.MapFrom(src => src.Patient.FatherName))
                 .ForMember(dest => dest.PatientCode, opt => opt.MapFrom(src => src.Patient.Department.Code + "-" + src.Patient.NumberStr))
                .ForMember(dest => dest.PatientMotherName, opt => opt.MapFrom(src => src.Patient.MotherName))
                .ForMember(dest => dest.PatientPhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.PatientPlaceOfBirth, opt => opt.MapFrom(src => src.Patient.PlaceOfBirth))
                .ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.Patient.BirthDate))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.PatientBloodType, opt => opt.MapFrom(src => (BloodType?)src.Patient.BloodType))
                .ForMember(dest => dest.PatientMaritalStatus, opt => opt.MapFrom(src => (MaritalStatus?)src.Patient.MaritalStatus))
                .ForMember(dest => dest.CurrentStep, opt => opt.MapFrom(src => (TeleMentalHealthCurrentSteps)src.CurrentStep))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (CaseStatus)src.Status))
                   .ForMember(dest => dest.PatientGender, opt => opt.MapFrom(src => (Gender)src.Patient.Gender));

                cfg.CreateMap<TeleMentalHealthDTO, TeleMentalHealth>()
                .ForMember(dest => dest.CurrentStep, opt => opt.MapFrom(src => (int)src.CurrentStep))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                 .ForMember(dest => dest.InvolvedUsers, g => g.Ignore());
                #endregion


                #region TelMHClinicalStory
                cfg.CreateMap<TelMHClinicalStory, TelMHClinicalStoryDTO>()
                     .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src =>(CaseStatus) src.TeleMentalHealth.Status));

                cfg.CreateMap<TelMHClinicalStoryDTO, TelMHClinicalStory>()
                    .ForMember(dest => dest.Diagnosis, g => g.Ignore())
                    .ForMember(dest => dest.MostLikelyDiagnosis, g => g.Ignore());
                #endregion

                #region TelMHWrittenPledge
                cfg.CreateMap<TelMHWrittenPledge, TelMHWrittenPledgeDTO>()
                   .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleMentalHealth.Status));

                cfg.CreateMap<TelMHWrittenPledgeDTO, TelMHWrittenPledge>();
                #endregion


                #region BoulevardAttachment
                cfg.CreateMap<BoulevardAttachment, BoulevardAttachmentDTO>()
                .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => (AttachmentReferenceTypes)src.ReferenceType));
                cfg.CreateMap<BoulevardAttachmentDTO, BoulevardAttachment>()
                .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => (int)src.ReferenceType));
                #endregion


                #region TelMHPhysicalExaminationReport
                cfg.CreateMap<TelMHPhysicalExaminationReport, TelMHPhysicalExaminationReportDTO>()
                  .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleMentalHealth.Status));

                cfg.CreateMap<TelMHPhysicalExaminationReportDTO, TelMHPhysicalExaminationReport>();
                #endregion


                #region TelMHTherapeuticPlan
                cfg.CreateMap<TelMHTherapeuticPlan, TelMHTherapeuticPlanDTO>()
                .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy))

                   .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleMentalHealth.Status));

                cfg.CreateMap<TelMHTherapeuticPlanDTO, TelMHTherapeuticPlan>();
                #endregion



                #region TelMHVitalSign
                cfg.CreateMap<TelMHVitalSign, TelMHVitalSignDTO>();

                cfg.CreateMap<TelMHVitalSignDTO, TelMHVitalSign>();
                #endregion



                #region TelMHDiagnosisCategory
                cfg.CreateMap<TelMHDiagnosisCategory, TelMHDiagnosisCategoryDTO>();

                cfg.CreateMap<TelMHDiagnosisCategoryDTO, TelMHDiagnosisCategory>();
                #endregion



                #region TelMHDiagnosisSubCategory
                cfg.CreateMap<TelMHDiagnosisSubCategory, TelMHDiagnosisSubCategoryDTO>();

                cfg.CreateMap<TelMHDiagnosisSubCategoryDTO, TelMHDiagnosisSubCategory>();
                #endregion


                #region TelMHDiagnosis
                cfg.CreateMap<TelMHDiagnosis, TelMHDiagnosisDTO>()
                   .ForMember(dest => dest.TelMHDiagnosisCategoryName, opt => opt.MapFrom(src => src.TelMHDiagnosisCategory.Name))
                    .ForMember(dest => dest.TelMHDiagnosisCategoryArabicName, opt => opt.MapFrom(src => src.TelMHDiagnosisCategory.ArabicName));

                cfg.CreateMap<TelMHDiagnosisDTO, TelMHDiagnosis>();
                #endregion


                #region Medication
                cfg.CreateMap<Medication, MedicationDTO>();

                cfg.CreateMap<MedicationDTO, Medication>();
                #endregion


                #region TelMHMostLikelyDiagnosis
                cfg.CreateMap<TelMHMostLikelyDiagnosis, TelMHMostLikelyDiagnosisDTO>();

                cfg.CreateMap<TelMHMostLikelyDiagnosisDTO, TelMHMostLikelyDiagnosis>()
                .ForMember(c => c.TelMHMostLikelySubDiagnoses, opt => opt.Ignore());
                
                #endregion

                #region StickyNote
                cfg.CreateMap<StickyNote, StickyNoteDTO>()
                       .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (ChannelEnum)src.Channel))
                        .ForMember(dest => dest.StickyNoteMessageType, opt => opt.MapFrom(src => (StickyNoteMessageTypeEnum)src.StickyNoteMessageType));
                //.ForMember(c => c.CreationDate, src => src.MapFrom(c => c.CreationDate.ToString("dd-MMM-yyyy hh:mm")));
                cfg.CreateMap<StickyNoteDTO, StickyNote>()
                .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (int)src.Channel))
                .ForMember(dest => dest.StickyNoteMessageType, opt => opt.MapFrom(src => (int)src.StickyNoteMessageType))
                .ForMember(c => c.CreationDate, opt => opt.Ignore());


                cfg.CreateMap<StickyNote, NewsFeedCommentDTO>()
        .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (ChannelEnum)src.Channel))
        .ForMember(dest => dest.StickyNoteMessageType, opt => opt.MapFrom(src => (StickyNoteMessageTypeEnum)src.StickyNoteMessageType))
         .ForMember(c => c.CommentDate, src => src.MapFrom(c => c.CreationDate));
                cfg.CreateMap<NewsFeedCommentDTO, StickyNote>()
                .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (int)src.Channel))
                .ForMember(c => c.CreationDate, opt => opt.Ignore()); 
                #endregion


                #region TeleMentalHealthUser
                cfg.CreateMap<TeleMentalHealthUser, TeleMentalHealthUserDTO>()
                   .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (JobRole)src.JobRole));

                cfg.CreateMap<TeleMentalHealthUserDTO, TeleMentalHealthUser>()
                   .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (int)src.JobRole));
                #endregion


                #region TeleICU
                cfg.CreateMap<TeleICU, TeleICUDTO>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.PatientDepartmentId, opt => opt.MapFrom(src => src.Patient.DepartmentId))
                .ForMember(dest => dest.PatientFatherName, opt => opt.MapFrom(src => src.Patient.FatherName))
                 .ForMember(dest => dest.PatientCode, opt => opt.MapFrom(src => src.Patient.Department.Code + "-" + src.Patient.NumberStr))
                .ForMember(dest => dest.PatientMotherName, opt => opt.MapFrom(src => src.Patient.MotherName))
                .ForMember(dest => dest.PatientPhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.PatientPlaceOfBirth, opt => opt.MapFrom(src => src.Patient.PlaceOfBirth))
                .ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.Patient.BirthDate))
                 .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.PatientBloodType, opt => opt.MapFrom(src => (BloodType?)src.Patient.BloodType))
                .ForMember(dest => dest.PatientMaritalStatus, opt => opt.MapFrom(src => (MaritalStatus?)src.Patient.MaritalStatus))
                .ForMember(dest => dest.CurrentStep, opt => opt.MapFrom(src => (TeleICUCurrentSteps)src.CurrentStep))
                 .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.CreationDate))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (CaseStatus)src.Status))
                .ForMember(dest => dest.PatientGender, opt => opt.MapFrom(src => (Gender)src.Patient.Gender));

                cfg.CreateMap<TeleICUDTO, TeleICU>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.InvolvedUsers, g => g.Ignore()); 
                #endregion


                #region TelICUClinicalStory
                cfg.CreateMap<TelICUClinicalStory, TelICUClinicalStoryDTO>()
                .ForMember(dest => dest.DiagnosisStatus, opt => opt.MapFrom(src => (DiagnosisStatus?)src.DiagnosisStatus))
                 .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status))
                                .ForMember(dest => dest.RiskFactors, opt => opt.MapFrom(src => (RiskFactorsEnum)src.RiskFactors));
                cfg.CreateMap<TelICUClinicalStoryDTO, TelICUClinicalStory>()
                .ForMember(dest => dest.DiagnosisStatus, opt => opt.MapFrom(src => (int?)src.DiagnosisStatus))
                .ForMember(dest => dest.RiskFactors, opt => opt.MapFrom(src => (int)src.RiskFactors));
                #endregion

                #region TelICUMedicationScheduler
                cfg.CreateMap<TelICUMedicationScheduler, TelICUMedicationSchedulerDTO>();
                cfg.CreateMap<TelICUMedicationSchedulerDTO, TelICUMedicationScheduler>();
                #endregion

                #region TelICUDiabetesControl
                cfg.CreateMap<TelICUDiabetesControl, TelICUDiabetesControlDTO>();
                cfg.CreateMap<TelICUDiabetesControlDTO, TelICUDiabetesControl>();
                #endregion

                #region NPICU
                cfg.CreateMap<NPICU, NPICUDTO>().ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.PatientFatherName, opt => opt.MapFrom(src => src.Patient.FatherName))
                 .ForMember(dest => dest.PatientCode, opt => opt.MapFrom(src => src.Patient.Department.Code + "-" + src.Patient.NumberStr))
                .ForMember(dest => dest.PatientDepartmentId, opt => opt.MapFrom(src => src.Patient.DepartmentId))
                .ForMember(dest => dest.PatientMotherName, opt => opt.MapFrom(src => src.Patient.MotherName))
                .ForMember(dest => dest.PatientPhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.PatientPlaceOfBirth, opt => opt.MapFrom(src => src.Patient.PlaceOfBirth))
                .ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.Patient.BirthDate))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.PatientBloodType, opt => opt.MapFrom(src => (BloodType?)src.Patient.BloodType))
                .ForMember(dest => dest.PatientMaritalStatus, opt => opt.MapFrom(src => (MaritalStatus?)src.Patient.MaritalStatus))
                .ForMember(dest => dest.CurrentStep, opt => opt.MapFrom(src => (NPICUCurrentSteps)src.CurrentStep))
                .ForMember(dest => dest.NPICUType, opt => opt.MapFrom(src => (NPICUType)src.NPICUType))
                 .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.CreationDate))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (CaseStatus)src.Status))
                   .ForMember(dest => dest.PatientGender, opt => opt.MapFrom(src => (Gender)src.Patient.Gender));

                cfg.CreateMap<NPICUDTO, NPICU>()
                .ForMember(dest => dest.NPICUType, opt => opt.MapFrom(src => (int)src.NPICUType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                 .ForMember(dest => dest.InvolvedUsers, g => g.Ignore());
                #endregion

                #region NPICUUser
                cfg.CreateMap<NPICUUser, NPICUUserDTO>()
                   .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (JobRole)src.JobRole));

                cfg.CreateMap<NPICUUserDTO, NPICUUser>()
                     .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (int)src.JobRole));
                #endregion

                #region TelICUVitalSign
                cfg.CreateMap<TelICUVitalSign, TelICUVitalSignDTO>();
                cfg.CreateMap<TelICUVitalSignDTO, TelICUVitalSign>();
                #endregion
                #region TelICUPump
                cfg.CreateMap<TelICUPump, TelICUPumpDTO>();
                cfg.CreateMap<TelICUPumpDTO, TelICUPump>();
                #endregion
                #region TelICUMedicationDailySchedule
                cfg.CreateMap<TelICUMedicationDailySchedule, TelICUMedicationDailyScheduleDTO>()
                .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status));
                cfg.CreateMap<TelICUMedicationDailyScheduleDTO, TelICUMedicationDailySchedule>();
                #endregion


                #region TelICULabUnit
                cfg.CreateMap<TelICULabUnit, TelICULabUnitDTO>()
                   .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status))
                 .ForMember(dest => dest.CBCBloodGroup, opt => opt.MapFrom(src => (BloodType?)src.CBCBloodGroup))
                 .ForMember(dest => dest.SerologyHBSAG, opt => opt.MapFrom(src => (PositiveNegativeEnum?)src.SerologyHBSAG))
                 .ForMember(dest => dest.SerologyHCV, opt => opt.MapFrom(src => (PositiveNegativeEnum?)src.SerologyHCV))
                 .ForMember(dest => dest.SerologyHIV, opt => opt.MapFrom(src => (PositiveNegativeEnum?)src.SerologyHIV))
                 .ForMember(dest => dest.UrineGlucose, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineGlucose))
                 .ForMember(dest => dest.UrineAppearance, opt => opt.MapFrom(src => (AppearanceEnum?)src.UrineAppearance))
                  .ForMember(dest => dest.UrintHemoglobin, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrintHemoglobin))
                  .ForMember(dest => dest.UrineColor, opt => opt.MapFrom(src => (ColorEnum?)src.UrineColor))
                  .ForMember(dest => dest.UrineBilllrubin, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineBilllrubin))
                  .ForMember(dest => dest.UrineKitones, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineKitones))
                  .ForMember(dest => dest.UrineCalciumOxalate, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineCalciumOxalate))
                  .ForMember(dest => dest.UrineAmorphousUrate, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineAmorphousUrate))
                  .ForMember(dest => dest.UrineUACrystals, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineUACrystals))
                  .ForMember(dest => dest.UrineMucus, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineMucus))
                  .ForMember(dest => dest.UrineCandida, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineCandida))
                  .ForMember(dest => dest.UrineBacteria, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineBacteria))
                  .ForMember(dest => dest.UrineGCylinders, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineGCylinders))
                  .ForMember(dest => dest.UrineProtein, opt => opt.MapFrom(src => (SignTypeEnum?)src.UrineProtein))
                  .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                         .ForMember(dest => dest.BloodGases, opt => opt.MapFrom(src => (BloodGases?)src.BloodGases))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy));

                cfg.CreateMap<TelICULabUnitDTO, TelICULabUnit>()
                 .ForMember(dest => dest.CBCBloodGroup, opt => opt.MapFrom(src => (int?)src.CBCBloodGroup))
                  .ForMember(dest => dest.SerologyHBSAG, opt => opt.MapFrom(src => (int?)src.SerologyHBSAG))
                      .ForMember(dest => dest.SerologyHCV, opt => opt.MapFrom(src => (int?)src.SerologyHCV))
                  .ForMember(dest => dest.SerologyHIV, opt => opt.MapFrom(src => (int?)src.SerologyHIV))
                    .ForMember(dest => dest.UrineGlucose, opt => opt.MapFrom(src => (int?)src.UrineGlucose))
                  .ForMember(dest => dest.UrineAppearance, opt => opt.MapFrom(src => (int?)src.UrineAppearance))
                      .ForMember(dest => dest.UrintHemoglobin, opt => opt.MapFrom(src => (int?)src.UrintHemoglobin))
                    .ForMember(dest => dest.UrineColor, opt => opt.MapFrom(src => (int?)src.UrineColor))
                  .ForMember(dest => dest.UrineBilllrubin, opt => opt.MapFrom(src => (int?)src.UrineBilllrubin))
                      .ForMember(dest => dest.UrineKitones, opt => opt.MapFrom(src => (int?)src.UrineKitones))
                  .ForMember(dest => dest.UrineCalciumOxalate, opt => opt.MapFrom(src => (int?)src.UrineCalciumOxalate))
                    .ForMember(dest => dest.UrineAmorphousUrate, opt => opt.MapFrom(src => (int?)src.UrineAmorphousUrate))
                  .ForMember(dest => dest.UrineUACrystals, opt => opt.MapFrom(src => (int?)src.UrineUACrystals))
                      .ForMember(dest => dest.UrineMucus, opt => opt.MapFrom(src => (int?)src.UrineMucus))
                  .ForMember(dest => dest.UrineCandida, opt => opt.MapFrom(src => (int?)src.UrineCandida))
                             .ForMember(dest => dest.UrineBacteria, opt => opt.MapFrom(src => (int?)src.UrineBacteria))
                  .ForMember(dest => dest.UrineGCylinders, opt => opt.MapFrom(src => (int?)src.UrineGCylinders))
                     .ForMember(dest => dest.UrineProtein, opt => opt.MapFrom(src => (int?)src.UrineProtein))
                    .ForMember(dest => dest.BloodGases, opt => opt.MapFrom(src => (int?)src.BloodGases))
                ;
                #endregion



                #region NICUConsultationForm
                cfg.CreateMap<NICUConsultationForm, NICUConsultationFormDTO>()
                   .ForMember(dest => dest.BornPlace, opt => opt.MapFrom(src => (BornPlace)src.BornPlace))
                   .ForMember(dest => dest.Resuscitation, opt => opt.MapFrom(src => (NICUResuscitation)src.Resuscitation))
                      .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.NPICU.Status))
                   .ForMember(dest => dest.LabourAndDelivery, opt => opt.MapFrom(src => (LabourAndDelivery)src.LabourAndDelivery));

                cfg.CreateMap<NICUConsultationFormDTO, NICUConsultationForm>()
                    .ForMember(dest => dest.BornPlace, opt => opt.MapFrom(src => (int)src.BornPlace))
                   .ForMember(dest => dest.Resuscitation, opt => opt.MapFrom(src => (int)src.Resuscitation))
                   .ForMember(dest => dest.LabourAndDelivery, opt => opt.MapFrom(src => (int)src.LabourAndDelivery));
                #endregion


                #region NICUConsultationFollowUpForm
                cfg.CreateMap<NICUConsultationFollowUpForm, NICUConsultationFollowUpFormDTO>()
                 .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src =>src.CreationDate))
                 .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src =>(CaseStatus)src.NPICU.Status))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy));

                cfg.CreateMap<NICUConsultationFollowUpFormDTO, NICUConsultationFollowUpForm>()
                 .ForMember(dest => dest.ChronologicalAge, g => g.Ignore())
                 .ForMember(dest => dest.BirthWeight, g => g.Ignore())
                 .ForMember(dest => dest.GestationalAge, g => g.Ignore());
                #endregion


                #region TelICUInternalConsultationForm
                cfg.CreateMap<TelICUInternalConsultationForm, TelICUInternalConsultationFormDTO>()
                    .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status))
                 .ForMember(dest => dest.UndeAnesthesia, opt => opt.MapFrom(src => (UnderAnesthesiaEnum)src.UndeAnesthesia));
                cfg.CreateMap<TelICUInternalConsultationFormDTO, TelICUInternalConsultationForm>()
                 .ForMember(dest => dest.UndeAnesthesia, opt => opt.MapFrom(src => (int?)src.UndeAnesthesia));
                #endregion

                #region TelICUConsultationForm
                cfg.CreateMap<TelICUConsultationForm, TelICUConsultationFormDTO>()
                 .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status))
                 .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy))
                  .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))

            ;
                cfg.CreateMap<TelICUConsultationFormDTO, TelICUConsultationForm>()
                ;
                #endregion

                #region TelICUExit
                cfg.CreateMap<TelICUExit, TelICUExitDTO>()
                   .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.TeleICU.Status));
                cfg.CreateMap<TelICUExitDTO, TelICUExit>();
                #endregion


                #region NPICUInvestigation
                cfg.CreateMap<NPICUInvestigation, NPICUInvestigationDTO>()
                 .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy))
                          .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.NPICU.Status))
                   .ForMember(dest => dest.BloodGases, opt => opt.MapFrom(src => (BloodGases)src.BloodGases));

                cfg.CreateMap<NPICUInvestigationDTO, NPICUInvestigation>()
                   .ForMember(dest => dest.BloodGases, opt => opt.MapFrom(src => (int)src.BloodGases));
                #endregion



                #region NPICUConsultantSection
                cfg.CreateMap<NPICUConsultantSection, NPICUConsultantSectionDTO>()
                .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                   .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.NPICU.Status))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy));

                cfg.CreateMap<NPICUConsultantSectionDTO, NPICUConsultantSection>();
                #endregion



                #region PICUConsultationForm
                cfg.CreateMap<PICUConsultationForm, PICUConsultationFormDTO>()
                   .ForMember(dest => dest.GeneralAppearance1, opt => opt.MapFrom(src => (GeneralAppearance1)src.GeneralAppearance1))
                    .ForMember(dest => dest.GeneralAppearance2, opt => opt.MapFrom(src => (GeneralAppearance2)src.GeneralAppearance2))
                     .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.NPICU.Status));

                cfg.CreateMap<PICUConsultationFormDTO, PICUConsultationForm>()
               .ForMember(dest => dest.GeneralAppearance1, opt => opt.MapFrom(src => (int?)src.GeneralAppearance1))
                    .ForMember(dest => dest.GeneralAppearance2, opt => opt.MapFrom(src => (int?)src.GeneralAppearance2));
                #endregion




                #region TeleICUUser
                cfg.CreateMap<TeleICUUser, TeleICUUserDTO>()
                   .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (JobRole)src.JobRole));

                cfg.CreateMap<TeleICUUserDTO, TeleICUUser>()
                   .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => (int)src.JobRole));
                #endregion


                #region PICUConsultationFollowUpForm
                cfg.CreateMap<PICUConsultationFollowUpForm, PICUConsultationFollowUpFormDTO>()
                .ForMember(dest => dest.GeneralAppearance1, opt => opt.MapFrom(src => (GeneralAppearance1)src.GeneralAppearance1))
                    .ForMember(dest => dest.GeneralAppearance2, opt => opt.MapFrom(src => (GeneralAppearance2)src.GeneralAppearance2))
                 .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                      .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => (CaseStatus)src.NPICU.Status))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy));

                cfg.CreateMap<PICUConsultationFollowUpFormDTO, PICUConsultationFollowUpForm>()
                  .ForMember(dest => dest.GeneralAppearance1, opt => opt.MapFrom(src => (int?)src.GeneralAppearance1))
                    .ForMember(dest => dest.GeneralAppearance2, opt => opt.MapFrom(src => (int?)src.GeneralAppearance2));
                #endregion





                #region TelMHMostLikelySubDiagnosis
                cfg.CreateMap<TelMHMostLikelySubDiagnosis, TelMHMostLikelySubDiagnosisDTO>()
                .ForMember(dest => dest.TelMHDiagnosisSubCategoryName, opt => opt.MapFrom(src => src.TelMHDiagnosisSubCategory.Name))
                .ForMember(dest => dest.TelMHDiagnosisSubCategoryArabicName, opt => opt.MapFrom(src => src.TelMHDiagnosisSubCategory.ArabicName));

                cfg.CreateMap<TelMHMostLikelySubDiagnosisDTO, TelMHMostLikelySubDiagnosis>();
                #endregion

                #region Medication_MHTherapeuticPlan
                cfg.CreateMap<Medication_MHTherapeuticPlan, Medication_MHTherapeuticPlanDTO>()
                .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.Medication.Name));

                ;
                cfg.CreateMap<Medication_MHTherapeuticPlanDTO, Medication_MHTherapeuticPlan>();
                #endregion



                #region CaseClosure
                cfg.CreateMap<CaseClosure, CaseClosureDTO>()
                 .ForMember(dest => dest.DateOfCreate, opt => opt.MapFrom(src => src.CreationDate))
                  .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy ))
                   .ForMember(dest => dest.CaseDepartment, opt => opt.MapFrom(src => (DepartmentEnum)src.CaseDepartment))
                   .ForMember(dest => dest.CaseCloseReason, opt => opt.MapFrom(src => (CaseCloseReason)src.CaseCloseReason));

                cfg.CreateMap<CaseClosureDTO, CaseClosure>()
                .ForMember(dest => dest.CaseDepartment, opt => opt.MapFrom(src => (int)src.CaseDepartment))
                   .ForMember(dest => dest.CaseCloseReason, opt => opt.MapFrom(src => (int)src.CaseCloseReason));
                #endregion


            });


        }
    }
}
