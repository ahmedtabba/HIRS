using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Common;
using BoulevardManagement.Model.Entities;
using Repository.Pattern;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BoulevardManagement.BLL
{

    public class TeleMentalHealthBLL : Service<TeleMentalHealth>, ITeleMentalHealthBLL
    {
        private readonly UserNotificationBLL _userNotificationBLL;
        private readonly ICaseClosureBLL _caseClosureBLL;
        readonly IRepositoryAsync<TeleMentalHealth> _repository;
        readonly IRepositoryAsync<TeleMentalHealthUser> _teleMentalHealthUserRepository;
        readonly IRepositoryAsync<Patient> _patientRepository;
        readonly IRepositoryAsync<TelMHClinicalStory> _telMHClinicalStoryRepository;
        readonly IRepositoryAsync<TelMHWrittenPledge> _telMHWrittenPledgeRepository;
        readonly IRepositoryAsync<TelMHTherapeuticPlan> _telMHTherapeuticPlanRepository;
        readonly IRepositoryAsync<CaseClosure> _caseClosureRepository;
        readonly IRepositoryAsync<TelMHVitalSign> _telMHVitalSignRepository;
        readonly IRepositoryAsync<TelMHPhysicalExaminationReport> _telMHPhysicalExaminationReportRepository;
        readonly IRepositoryAsync<Medication_MHTherapeuticPlan> _telMHMedicationTherapeuticPlan;
        readonly IRepositoryAsync<TeleMentalHealthUser> _telmhUsersRepository;

        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;
        List<string> Colors = new List<string>() { "#CFAFAF", "#EDA2A2", "#FCAF94",
            "#FFC388", "#EAEDC9", "#E5F494",
            "#CCF0C8", "#B7E2DB", "#93E4FF",
            "#CEC0EA","#ff373742","#ff379d36","#6837ff1c","#3799ff61","#ffcb372b"

        };
        public TeleMentalHealthBLL(
     IRepositoryAsync<TeleMentalHealth> repository,
     IRepositoryAsync<TeleMentalHealthUser> teleMentalHealthUserRepository,
     IRepositoryAsync<Patient> patientRepository,
     IRepositoryAsync<TelMHClinicalStory> telMHClinicalStoryRepository,
     IRepositoryAsync<TelMHWrittenPledge> telMHWrittenPledgeRepository,
     IRepositoryAsync<TelMHPhysicalExaminationReport> telMHPhysicalExaminationReportRepository,
     IRepositoryAsync<TelMHTherapeuticPlan> telMHTherapeuticPlanRepository,
     IRepositoryAsync<TelMHVitalSign> telMHVitalSignRepository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IRepositoryAsync<Medication_MHTherapeuticPlan> telMHMedicationTherapeuticPlan,
      IApplicationUserDataContext applicationContext
  ,
      IRepositoryAsync<TeleMentalHealthUser> telmhUsersRepository,
            UserNotificationBLL userNotificationBLL,
            ICaseClosureBLL caseClosureBLL, IRepositoryAsync<CaseClosure> caseClosureRepository) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _teleMentalHealthUserRepository = teleMentalHealthUserRepository;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _telMHClinicalStoryRepository = telMHClinicalStoryRepository;
            _telMHWrittenPledgeRepository = telMHWrittenPledgeRepository;
            _telMHVitalSignRepository = telMHVitalSignRepository;
            _telMHPhysicalExaminationReportRepository = telMHPhysicalExaminationReportRepository;
            _telMHTherapeuticPlanRepository = telMHTherapeuticPlanRepository;
            _userNotificationBLL = userNotificationBLL;
            _patientRepository = patientRepository;
            _telMHMedicationTherapeuticPlan = telMHMedicationTherapeuticPlan;
            _caseClosureBLL = caseClosureBLL;
            _caseClosureRepository = caseClosureRepository;
            _telmhUsersRepository = telmhUsersRepository;
        }


        public int Insert(TeleMentalHealthDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TeleMentalHealthDTO, TeleMentalHealth>(obDTO);
                obEntity.ObjectState = ObjectState.Added;



                obEntity.InvolvedUsers = Mapper.Map<ICollection<TeleMentalHealthUserDTO>, ICollection<TeleMentalHealthUser>>(obDTO.InvolvedUsers);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);




                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();


                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.Add).Id,
                   NotificationObjectTypes.TeleMentalHealth,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Added",
                   $"Mental Health Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);


                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TeleMentalHealth).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Update(TeleMentalHealthDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Query()
                                    .Include(x => x.InvolvedUsers)
                                    .SelectQueryable()
                                    .Where(c => c.Id == obDTO.Id).FirstOrDefault();


                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);
                var involvedUsers = Mapper.Map<ICollection<TeleMentalHealthUserDTO>, ICollection<TeleMentalHealthUser>>(obDTO.InvolvedUsers);
                involvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var involvedUser in involvedUsers)
                    obEntity.InvolvedUsers.Add(involvedUser);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.TeleMentalHealthId = obEntity.Id);


                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();


                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.Edit).Id,
                   NotificationObjectTypes.TeleMentalHealth,
                   obEntity.Id.ToString(),
                   user.FullName, "Edited",
                  $"Mental Health Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);


                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TeleMentalHealth).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Delete(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Query()
                                    .Include(x => x.InvolvedUsers)
                                    .SelectQueryable()
                                    .Where(c => c.Id == id).FirstOrDefault();

                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);

                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);


                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var clinicalStory = _telMHClinicalStoryRepository.Query().SelectQueryable().Where(c => c.TeleMentalHealthId == obEntity.Id).FirstOrDefault();
                if (clinicalStory != null)
                {
                    _telMHClinicalStoryRepository.Delete(clinicalStory.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var PhysicalExamination = _telMHPhysicalExaminationReportRepository.Query().SelectQueryable().Where(c => c.TeleMentalHealthId == obEntity.Id).FirstOrDefault();
                if (PhysicalExamination != null)
                {
                    _telMHPhysicalExaminationReportRepository.Delete(PhysicalExamination.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var writtenPledge = _telMHWrittenPledgeRepository.Query().SelectQueryable().Where(c => c.TeleMentalHealthId == obEntity.Id).FirstOrDefault();
                if (writtenPledge != null)
                {
                    _telMHWrittenPledgeRepository.Delete(writtenPledge.Id);
                    _unitOfWorkAsync.SaveChanges();
                }


                var TherapeuticPlans = _telMHTherapeuticPlanRepository.Query().Include(c => c.Medications).SelectQueryable().Where(c => c.TeleMentalHealthId == obEntity.Id).ToList();
                foreach (var TherapeuticPlan in TherapeuticPlans)
                {
                    var Medications = _telMHMedicationTherapeuticPlan.Query().SelectQueryable().Where(c => c.TelMHTherapeuticPlanId == TherapeuticPlan.Id).ToList();
                    foreach (var Medication in Medications)
                    {
                        _telMHMedicationTherapeuticPlan.Delete(Medication.Id);
                        _unitOfWorkAsync.SaveChanges();

                    }
                    _telMHTherapeuticPlanRepository.Delete(TherapeuticPlan.Id);
                }
                _unitOfWorkAsync.SaveChanges();
                var VitalSigns = _telMHVitalSignRepository.Query().SelectQueryable().Where(c => c.TeleMentalHealthId == obEntity.Id).ToList();
                foreach (var VitalSign in VitalSigns)
                {
                    _telMHVitalSignRepository.Delete(VitalSign.Id);
                }
                _unitOfWorkAsync.SaveChanges();
                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
               _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.Delete).Id,
               NotificationObjectTypes.TeleMentalHealth,
               id.ToString(),
               user.FullName,
               "Deleted",
               $"Mental Health Case For Patient {patientFullName }",
               user.JobRole, usersInCase);


                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TeleMentalHealth).Name.ToString(),
                      ObjectId = obEntity.Id,
                      ObjectRefernceNO = obEntity.Id.ToString(),
                      OperationType = OperationTypeEnum.Delete,
                      LogDescription = String.Format("{0} is deleted",
                      obEntity.Id.ToString())
                  });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TeleMentalHealthDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TeleMentalHealthDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(TeleMentalHealthDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";


            return res;
        }

        public IQueryable<TeleMentalHealthDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<TeleMentalHealthDTO>();
            }
            catch { throw; }
        }

        public IQueryable<TeleMentalHealthDTO> GetAllIncludedPatient()
        {
            try
            {
                return _repository.Query().Include(x => x.Patient).Include(x => x.InvolvedUsers).SelectQueryable().ProjectTo<TeleMentalHealthDTO>();
            }
            catch { throw; }
        }

        public TelMHClinicalStoryDTO GetClinicalStory(int teleMentalHealthId)
        {
            try
            {
                return _telMHClinicalStoryRepository.Query().Include(x => x.Diagnosis).Include(x => x.MostLikelyDiagnosis).SelectQueryable().Where(f => f.TeleMentalHealthId == teleMentalHealthId).ProjectTo<TelMHClinicalStoryDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public TelMHWrittenPledgeDTO GetWrittenPledge(int teleMentalHealthId)
        {
            try
            {
                return _telMHWrittenPledgeRepository.Query().SelectQueryable().Where(f => f.TeleMentalHealthId == teleMentalHealthId).ProjectTo<TelMHWrittenPledgeDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public int UpdateClinicalStory(TelMHClinicalStoryDTO obDTO, bool ToNextStep = true)
        {
            try
            {
                TelMHClinicalStory obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _telMHClinicalStoryRepository.Query().Include(x => x.Diagnosis).Include(x => x.MostLikelyDiagnosis).SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelMHClinicalStoryDTO, TelMHClinicalStory>(obDTO, obEntity);

                    //obEntity.Diagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);

                    //var Diagnosis = Mapper.Map<ICollection<TelMHDiagnosisDTO>, ICollection<TelMHDiagnosis>>(obDTO.Diagnosis);
                    //Diagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                    //foreach (var diagnos in Diagnosis)
                    //    obEntity.Diagnosis.Add(diagnos);
                    //obEntity.Diagnosis.ToList().ForEach(c => c.TelMHClinicalStoryId = obEntity.Id);


                    //obEntity.MostLikelyDiagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);

                    //var MostLikelyDiagnosis = Mapper.Map<ICollection<TelMHMostLikelyDiagnosisDTO>, ICollection<TelMHMostLikelyDiagnosis>>(obDTO.MostLikelyDiagnosis);
                    //MostLikelyDiagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                    //foreach (var diagnos in MostLikelyDiagnosis)
                    //    obEntity.MostLikelyDiagnosis.Add(diagnos);
                    //obEntity.MostLikelyDiagnosis.ToList().ForEach(c => c.TelMHClinicalStoryId = obEntity.Id);


                    obEntity.ObjectState = ObjectState.Modified;
                    _telMHClinicalStoryRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    if (ToNextStep)
                        _userNotificationBLL.PublishNotification(
                           _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateCllinicalStory).Id,
                           NotificationObjectTypes.TeleMentalHealth_ClinicalStory,
                           _MHCase.Id.ToString(),
                           user.FullName, "Edited",
                          $"Mental Health Case For Patient {patientFullName }",
                           user.JobRole,
                          usersInCase);


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHClinicalStory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelMHClinicalStoryDTO, TelMHClinicalStory>(obDTO);

                    obEntity.Diagnosis = Mapper.Map<ICollection<TelMHDiagnosisDTO>, ICollection<TelMHDiagnosis>>(obDTO.Diagnosis);
                    obEntity.Diagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Added);

                    obEntity.MostLikelyDiagnosis = Mapper.Map<ICollection<TelMHMostLikelyDiagnosisDTO>, ICollection<TelMHMostLikelyDiagnosis>>(obDTO.MostLikelyDiagnosis);
                    obEntity.MostLikelyDiagnosis.ToList().ForEach(c => c.ObjectState = ObjectState.Added);

                    obEntity.ObjectState = ObjectState.Added;
                    _telMHClinicalStoryRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (ToNextStep)
                    {
                        _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateCllinicalStory).Id,
              NotificationObjectTypes.TeleMentalHealth_ClinicalStory,
              _MHCase.Id.ToString(),
              user.FullName, "Edited",
             $"Mental Health Case For Patient {patientFullName }",
              user.JobRole,
             usersInCase);
                    }
           

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHClinicalStory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();

                if (ToNextStep)
                    UpdateTeleMentalHealthCurrentStep(obEntity.TeleMentalHealthId, TeleMentalHealthCurrentSteps.Step2);



                _unitOfWorkAsync.Commit();

                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        private void UpdateTeleMentalHealthCurrentStep(int teleMentalHealthId, TeleMentalHealthCurrentSteps newStep)
        {
            try
            {
                var teleMentalHealth = Find(teleMentalHealthId);
                teleMentalHealth.CurrentStep = teleMentalHealth.CurrentStep < (int)newStep ? (int)newStep : teleMentalHealth.CurrentStep;
                teleMentalHealth.ObjectState = ObjectState.Modified;
                Update(teleMentalHealth);
                _unitOfWorkAsync.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void UpdateWrittenPledge(TelMHWrittenPledgeDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                TelMHWrittenPledge obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _telMHWrittenPledgeRepository.Find(obDTO.Id);
                    Mapper.Map<TelMHWrittenPledgeDTO, TelMHWrittenPledge>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _telMHWrittenPledgeRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();


                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {


                        _userNotificationBLL.PublishNotification(
                           _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateWrittenPledge).Id,
                           NotificationObjectTypes.TeleMentalHealth_WrittenPledge,
                           _MHCase.Id.ToString(),
                           user.FullName, "Edited",
                          $"Mental Health Case For Patient {patientFullName }",
                           user.JobRole,
                          usersInCase);
                    }

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHWrittenPledge).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelMHWrittenPledgeDTO, TelMHWrittenPledge>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _telMHWrittenPledgeRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    obDTO.Id = obEntity.Id;
                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {

                        _userNotificationBLL.PublishNotification(
                           _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateWrittenPledge).Id,
                           NotificationObjectTypes.TeleMentalHealth_WrittenPledge,
                           _MHCase.Id.ToString(),
                           user.FullName, "Edited",
                          $"Mental Health Case For Patient {patientFullName }",
                           user.JobRole,
                          usersInCase);

                    }
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHWrittenPledge).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();

                if (obEntity.PatientIDCardAttachmentId.HasValue && obEntity.PledgeDocumentAttachmentId.HasValue)
                    UpdateTeleMentalHealthCurrentStep(obEntity.TeleMentalHealthId, TeleMentalHealthCurrentSteps.Step3);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TelMHPhysicalExaminationReportDTO GetPhysicalExaminationReport(int teleMentalHealthId)
        {
            try
            {
                return _telMHPhysicalExaminationReportRepository.Query().SelectQueryable().Where(f => f.TeleMentalHealthId == teleMentalHealthId).ProjectTo<TelMHPhysicalExaminationReportDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdatePhysicalExaminationReport(TelMHPhysicalExaminationReportDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                TelMHPhysicalExaminationReport obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _telMHPhysicalExaminationReportRepository.Find(obDTO.Id);
                    Mapper.Map<TelMHPhysicalExaminationReportDTO, TelMHPhysicalExaminationReport>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _telMHPhysicalExaminationReportRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();


                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                          _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdatePhysicalExaminationReport).Id,
                                          NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport,
                                          _MHCase.Id.ToString(),
                                          user.FullName, "Edited",
                                         $"Mental Health Case For Patient {patientFullName }",
                                          user.JobRole,
                                         usersInCase);
                    }



                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHPhysicalExaminationReport).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelMHPhysicalExaminationReportDTO, TelMHPhysicalExaminationReport>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _telMHPhysicalExaminationReportRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    obDTO.Id = obEntity.Id;
                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                         _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdatePhysicalExaminationReport).Id,
                                         NotificationObjectTypes.TeleMentalHealth_PhysicalExaminationReport,
                                         _MHCase.Id.ToString(),
                                         user.FullName, "Edited",
                                        $"Mental Health Case For Patient {patientFullName }",
                                         user.JobRole,
                                        usersInCase);
                    }


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHPhysicalExaminationReport).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();

                if (obEntity.ReportOfThePhysicalExaminationAttachmentId.HasValue)
                    UpdateTeleMentalHealthCurrentStep(obEntity.TeleMentalHealthId, TeleMentalHealthCurrentSteps.Step4);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TelMHTherapeuticPlanDTO GetTherapeuticPlan(int planId)
        {
            try
            {
                return _telMHTherapeuticPlanRepository.Query().SelectQueryable().Where(f => f.Id == planId).ProjectTo<TelMHTherapeuticPlanDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        //public void UpdateTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO)
        //{
        //    try
        //    {
        //        TelMHTherapeuticPlan obEntity;
        //        _unitOfWorkAsync.BeginTransaction();
        //        if (obDTO.Id > 0)
        //        {
        //            obEntity = _telMHTherapeuticPlanRepository.Find(obDTO.Id);
        //            Mapper.Map<TelMHTherapeuticPlanDTO, TelMHTherapeuticPlan>(obDTO, obEntity);
        //            obEntity.ObjectState = ObjectState.Modified;
        //            _telMHTherapeuticPlanRepository.Update(obEntity);
        //            _unitOfWorkAsync.SaveChanges();


        //            var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

        //            var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
        //            usersInCase.Add(_MHCase.CreatedBy);


        //            var user = _applicationContext.GetApplicationUserData();
        //            var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FullName).FirstOrDefault();

        //            _userNotificationBLL.PublishNotification(
        //               _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan).Id,
        //               NotificationObjectTypes.TeleMentalHealth,
        //               _MHCase.Id.ToString(),
        //               user.FullName, "Edited",
        //              $"Mental Health Case For Patient {patientFullName }",
        //               user.JobRole,
        //              usersInCase);


        //            _operationLogBLL.Insert(
        //            new OperationLogDTO()
        //            {
        //                EntityType = typeof(TelMHTherapeuticPlan).Name.ToString(),
        //                ObjectId = obEntity.Id,
        //                ObjectRefernceNO = obEntity.Id.ToString(),
        //                OperationType = OperationTypeEnum.Update,
        //                LogDescription = String.Format("{0} is updated",
        //                obEntity.Id.ToString())
        //            });


        //        }
        //        else
        //        {
        //            obEntity = Mapper.Map<TelMHTherapeuticPlanDTO, TelMHTherapeuticPlan>(obDTO);
        //            obEntity.ObjectState = ObjectState.Added;
        //            _telMHTherapeuticPlanRepository.Insert(obEntity);
        //            _unitOfWorkAsync.SaveChanges();


        //            var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

        //            var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
        //            usersInCase.Add(_MHCase.CreatedBy);


        //            var user = _applicationContext.GetApplicationUserData();
        //            var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FullName).FirstOrDefault();

        //            _userNotificationBLL.PublishNotification(
        //               _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan).Id,
        //               NotificationObjectTypes.TeleMentalHealth,
        //               _MHCase.Id.ToString(),
        //               user.FullName, "Edited",
        //              $"Mental Health Case For Patient {patientFullName }",
        //               user.JobRole,
        //              usersInCase);



        //            _operationLogBLL.Insert(
        //            new OperationLogDTO()
        //            {
        //                EntityType = typeof(TelMHTherapeuticPlan).Name.ToString(),
        //                ObjectId = obEntity.Id,
        //                ObjectRefernceNO = obEntity.Id.ToString(),
        //                OperationType = OperationTypeEnum.Add,
        //                LogDescription = String.Format("{0} is added",
        //                obEntity.Id.ToString())
        //            });
        //        }
        //        _unitOfWorkAsync.SaveChanges();

        //        UpdateTeleMentalHealthCurrentStep(obEntity.TeleMentalHealthId, TeleMentalHealthCurrentSteps.Step5);



        //        _unitOfWorkAsync.Commit();
        //    }
        //    catch { _unitOfWorkAsync.Rollback(); throw; }
        //}

        public int InsertVitalSign(TelMHVitalSignDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelMHVitalSignDTO, TelMHVitalSign>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                _telMHVitalSignRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_MHCase.CreatedBy);


                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.AddVitalSign).Id,
                   NotificationObjectTypes.TeleMentalHealth_VitalSign,
                   _MHCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"Mental Health Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHVitalSign).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public void UpdateVitalSign(TelMHVitalSignDTO obDTO)
        {
            try
            {
                TelMHVitalSign obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _telMHVitalSignRepository.Find(obDTO.Id);
                    Mapper.Map<TelMHVitalSignDTO, TelMHVitalSign>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _telMHVitalSignRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.EditVitalSign).Id,
                       NotificationObjectTypes.TeleMentalHealth_VitalSign,
                       _MHCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"Mental Health Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHVitalSign).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelMHVitalSignDTO, TelMHVitalSign>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _telMHVitalSignRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();


                    var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                    var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_MHCase.CreatedBy);

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.EditVitalSign).Id,
                       NotificationObjectTypes.TeleMentalHealth_VitalSign,
                       _MHCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"Mental Health Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelMHVitalSign).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public IQueryable<TelMHVitalSignDTO> GetAllVitalSignsByMentalHelthId(int mentalHealthId)
        {
            try
            {
                return _telMHVitalSignRepository.Query().SelectQueryable().Where(x => x.TeleMentalHealthId == mentalHealthId).ProjectTo<TelMHVitalSignDTO>();
            }
            catch { throw; }
        }

        public void DeleteVitalSign(int id)
        {

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _telMHVitalSignRepository.Find(id);
                _telMHVitalSignRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();



                var _MHCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                var usersInCase = _MHCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_MHCase.CreatedBy);


                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _MHCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.DeleteVitalSign).Id,
                   NotificationObjectTypes.TeleMentalHealth_VitalSign,
                   _MHCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"Mental Health Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);



                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelMHVitalSign).Name.ToString(),
                      ObjectId = obEntity.Id,
                      ObjectRefernceNO = obEntity.Id.ToString(),
                      OperationType = OperationTypeEnum.Delete,
                      LogDescription = String.Format("{0} is deleted",
                      obEntity.Id.ToString())
                  });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TelMHVitalSignDTO GetVitalSign(int id)
        {
            try
            {
                return _telMHVitalSignRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelMHVitalSignDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void NotifyZoomMeeting(string url, int id)
        {
            try
            {
                var obEntity = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == id).FirstOrDefault();
                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();


                _userNotificationBLL.PublishNotificationForZoomMeeting(
                    _userNotificationBLL.GetByName(NotificationConsistent.Zoom.JoinMeeting).Id,
                    NotificationObjectTypes.Zoom,
                    obEntity.Id.ToString(),
                    user.FullName, "Create Zoom Meeting",
                   $"Mental Health Case For Patient {patientFullName }",
                    user.JobRole,
                    url,
                   usersInCase);
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public void UpdateTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                TelMHTherapeuticPlan obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = _telMHTherapeuticPlanRepository.Query().Include(c => c.Medications).SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<TelMHTherapeuticPlanDTO, TelMHTherapeuticPlan>(obDTO, obEntity);
                obEntity.Medications.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);
                _unitOfWorkAsync.SaveChanges();
                var Medications = Mapper.Map<ICollection<Medication_MHTherapeuticPlanDTO>, ICollection<Medication_MHTherapeuticPlan>>(obDTO.Medications);
                Medications.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var Medication in Medications)
                    obEntity.Medications.Add(Medication);
                obEntity.Medications.ToList().ForEach(c => c.TelMHTherapeuticPlanId = obEntity.Id);


                obEntity.ObjectState = ObjectState.Modified;
                _telMHTherapeuticPlanRepository.InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
               _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan).Id,
               NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan,
               _case.Id.ToString(),
               user.FullName, "Edited",
              $"Mental Health Case For Patient {patientFullName }",
               user.JobRole,
              usersInCase);
                }



                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleMentalHealth).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Update,
                    LogDescription = String.Format("{0} is updated",
                    obEntity.Id.ToString())
                });



                _unitOfWorkAsync.SaveChanges();

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public int InsertTherapeuticPlan(TelMHTherapeuticPlanDTO obDTO)
        {
            try
            {
                TelMHTherapeuticPlan obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = Mapper.Map<TelMHTherapeuticPlanDTO, TelMHTherapeuticPlan>(obDTO);

                obEntity.Medications = Mapper.Map<ICollection<Medication_MHTherapeuticPlanDTO>, ICollection<Medication_MHTherapeuticPlan>>(obDTO.Medications);
                obEntity.Medications.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                obEntity.ObjectState = ObjectState.Added;
                _telMHTherapeuticPlanRepository.InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan).Id,
                   NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan,
                   _case.Id.ToString(),
                   user.FullName, "Edited",
                  $"Mental Health Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleMentalHealth).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("{0} is added",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                UpdateTeleMentalHealthCurrentStep(obEntity.TeleMentalHealthId, TeleMentalHealthCurrentSteps.Step4);

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void DeleteTherapeuticPlan(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _telMHTherapeuticPlanRepository.Find(id);
                var mTPIds = _telMHMedicationTherapeuticPlan.Query().SelectQueryable().Where(x => x.TelMHTherapeuticPlanId == id).Select(x => x.Id).ToList();
                foreach (var mTPId in mTPIds)
                {
                    _telMHMedicationTherapeuticPlan.Delete(mTPId);
                    _unitOfWorkAsync.SaveChanges();
                }
                _telMHTherapeuticPlanRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

//                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleMentalHealthId).FirstOrDefault();

//                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

//                usersInCase.Add(_case.CreatedBy);

//                var user = _applicationContext.GetApplicationUserData();
//                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

//                _userNotificationBLL.PublishNotification(
//                 _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.DeleteTherapeuticPlan).Id,
//                 NotificationObjectTypes.TeleMentalHealth_TherapeuticPlan,
//_case.Id.ToString(),
//                 user.FullName, "Edited",
//                $"Mental Health Case For Patient {patientFullName }",
//                 user.JobRole,
//                usersInCase);

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelMHTherapeuticPlan).Name.ToString(),
                      ObjectId = obEntity.Id,
                      ObjectRefernceNO = obEntity.Id.ToString(),
                      OperationType = OperationTypeEnum.Delete,
                      LogDescription = String.Format("{0} is deleted",
                      obEntity.Id.ToString())
                  });

                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public IQueryable<TelMHTherapeuticPlanDTO> GetTherapeuticPlans(int NICUId)
        {
            try
            {
                return _telMHTherapeuticPlanRepository.Query().SelectQueryable().Where(x => x.TeleMentalHealthId == NICUId).Include(c=>c.Medications).ProjectTo<TelMHTherapeuticPlanDTO>();
            }
            catch { throw; }
        }

        public void Close(CaseClosureDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();

                var obEntity = _repository.Query().SelectQueryable().Where(x => x.Id == obDTO.CaseId).FirstOrDefault();
                obEntity.Status = (int)CaseStatus.Closed;
                obEntity.ObjectState = ObjectState.Modified;
                _unitOfWorkAsync.SaveChanges();
                _caseClosureBLL.Insert(obDTO);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _casePatientName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).FirstOrDefault();
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obDTO.CaseId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();

                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
           _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.Close).Id,
           NotificationObjectTypes.TeleMentalHealth,
           obDTO.CaseId.ToString(),
           user.FullName, "Closed",
          $"Mental Health Case For Patient {_casePatientName.FirstName ?? ""} {_casePatientName.LastName ?? "" }",
           user.JobRole,
          usersInCase);
                }

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleMentalHealth).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Update,
                    LogDescription = String.Format("{0} is Closed",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                _unitOfWorkAsync.Commit();
            }
            catch
            {
                _unitOfWorkAsync.Rollback();
                throw;
            }
        }

        public void ReOpen(int caseId)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();

                var obEntity = _repository.Query().SelectQueryable().Where(x => x.Id == caseId).FirstOrDefault();
                obEntity.Status = (int)CaseStatus.Open;
                obEntity.ObjectState = ObjectState.Modified;
                _unitOfWorkAsync.SaveChanges();

                var _casePatientName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).FirstOrDefault();
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == caseId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();

                _userNotificationBLL.PublishNotification(
                 _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.ReOpen).Id,
                 NotificationObjectTypes.TeleMentalHealth,
                caseId.ToString(),
                 user.FullName, "ReOpened",
                $"Mental Health Case For Patient {_casePatientName.FirstName} {_casePatientName.LastName }",
                 user.JobRole,
                usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleMentalHealth).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Update,
                    LogDescription = String.Format("{0} is ReOpened",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                _unitOfWorkAsync.Commit();
            }
            catch
            {
                _unitOfWorkAsync.Rollback();
                throw;
            }
        }

        public CaseClosureDTO GetCaseClosure(int closureId)
        {
            try
            {
                return _caseClosureRepository.Query().SelectQueryable().Where(f => f.Id == closureId).ProjectTo<CaseClosureDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public IQueryable<CaseClosureDTO> GetCaseClosures(int mHealthId)
        {
            try
            {
                return _caseClosureRepository.Query().SelectQueryable().Where(x => x.CaseId == mHealthId && x.CaseDepartment == (int)DepartmentEnum.MH).ProjectTo<CaseClosureDTO>();
            }
            catch { throw; }
        }

        public int? GetCurrentlyOpendCaseIdForPatient(int patientId)
        {
            try
            {
                if (_repository.Query().SelectQueryable().Where(x => x.PatientId == patientId && x.Status == (int)CaseStatus.Open).Any())
                    return _repository.Query().SelectQueryable().Where(x => x.PatientId == patientId && x.Status == (int)CaseStatus.Open).Select(x => x.Id).FirstOrDefault(); ;


                return null;
            }
            catch { throw; }
        }

        public void UpdateInvolvedUsersColors(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();

                var InvolvedUsers = _repository.Query().SelectQueryable().Where(c => c.Id == id).Include(c => c.InvolvedUsers).Select(c => c.InvolvedUsers).FirstOrDefault();
                foreach (var User in InvolvedUsers)
                {
                    if (User.Color == null)
                    {
                        foreach (string color in Colors)
                        {
                            if (!InvolvedUsers.Select(c => c.Color).Contains(color))
                            {
                                User.Color = color;
                                _telmhUsersRepository.Update(User);
                                break;
                            }
                        }
                    }
                }
                var UsersWithNoColor = InvolvedUsers.Where(c => c.Color == null).ToList();
                for (int i = 0; i < UsersWithNoColor.Count; i++)
                {
                    UsersWithNoColor[i].Color = Colors[i % Colors.Count];
                    _telmhUsersRepository.Update(UsersWithNoColor[i]);

                }
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }

        public void UpdateTherapeuticPlanForAttachment(TelMHTherapeuticPlanDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity =_telMHTherapeuticPlanRepository.Find(obDTO.Id);
                obEntity.NotesAttachmentId = obDTO.NotesAttachmentId;
                obEntity.ObjectState = ObjectState.Modified;
                _telMHTherapeuticPlanRepository.Update(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }
    }
}
