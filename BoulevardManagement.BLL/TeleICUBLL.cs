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

namespace BoulevardManagement.BLL
{

    public class TeleICUBLL : Service<TeleICU>, ITeleICUBLL
    {
        private readonly ILocationBLL _locationBLL;
        readonly IRepositoryAsync<TeleICU> _repository;
        readonly IRepositoryAsync<TelICUClinicalStory> _TelICUClinicalStoryRepository;
        readonly IRepositoryAsync<TelICUMedicationScheduler> _TelICUMedicationScheduleRepository;
        readonly IRepositoryAsync<TelICUDiabetesControl> _TelICUDiabetesControlRepository;
        readonly IRepositoryAsync<TelICUVitalSign> _TelICUVitalSignRepository;
        readonly IRepositoryAsync<TelICUPump> _TelICUPumpRepository;
        readonly IRepositoryAsync<TelICUMedicationDailySchedule> _TelICUMedicationDailyScheduleRepository;
        readonly IRepositoryAsync<TelICULabUnit> _TelICULabUnitRepository;
        readonly IRepositoryAsync<TelICUInternalConsultationForm> _TelICUInternalConsultationFormRepository;
        readonly IRepositoryAsync<TelICUExit> _TelICUExitRepository;
        readonly IRepositoryAsync<Patient> _patientRepository;
        private readonly IUserNotificationBLL _userNotificationBLL;
        readonly IRepositoryAsync<TelICUConsultationForm> _TelICUConsultationFormRepository;
        private readonly ICaseClosureBLL _caseClosureBLL;
        readonly IRepositoryAsync<CaseClosure> _caseClosureRepository;
        readonly IRepositoryAsync<TeleICUUser> _telIcuUsersRepository;


        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;
        List<string> Colors = new List<string>() { "#CFAFAF", "#EDA2A2", "#FCAF94",
            "#FFC388", "#EAEDC9", "#E5F494",
            "#CCF0C8", "#B7E2DB", "#93E4FF",
            "#CEC0EA","#ff373742","#ff379d36","#6837ff1c","#3799ff61","#ffcb372b"

        };
        public TeleICUBLL(
     IRepositoryAsync<TeleICU> repository,
     IRepositoryAsync<TelICUClinicalStory> TelICUClinicalStoryRepository,
     IRepositoryAsync<TelICUMedicationScheduler> TelICUMedicationScheduleRepository,
     IRepositoryAsync<TelICUDiabetesControl> TelICUDiabetesControlepository,
     IRepositoryAsync<TelICUVitalSign> TelICUVitalSignepository,
     IRepositoryAsync<TelICUPump> TelICUPumpRepository,
     IRepositoryAsync<TelICUMedicationDailySchedule> TelICUMedicationDailyScheduleRepository,
     IRepositoryAsync<TelICULabUnit> TelICULabUnitRepository,
     IRepositoryAsync<TelICUInternalConsultationForm> TelICUInternalConsultationFormRepository,
     IRepositoryAsync<TelICUExit> TelICUExitRepository,
     IRepositoryAsync<Patient> patientRepository,
        IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
       IUserNotificationBLL userNotificationBLL,
      IApplicationUserDataContext applicationContext,
      IRepositoryAsync<TelICUConsultationForm> TelICUConsultationFormRepository
, ICaseClosureBLL caseClosureBLL, IRepositoryAsync<CaseClosure> caseClosureRepository,
      IRepositoryAsync<TeleICUUser> telIcuUsersRepository,
            ILocationBLL locationBLL) : base(repository, applicationContext)
        {
            _userNotificationBLL = userNotificationBLL;
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _TelICUMedicationScheduleRepository = TelICUMedicationScheduleRepository;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _TelICUClinicalStoryRepository = TelICUClinicalStoryRepository;
            _TelICUDiabetesControlRepository = TelICUDiabetesControlepository;
            _TelICUVitalSignRepository = TelICUVitalSignepository;
            _TelICUPumpRepository = TelICUPumpRepository;
            _TelICUMedicationDailyScheduleRepository = TelICUMedicationDailyScheduleRepository;
            _TelICULabUnitRepository = TelICULabUnitRepository;
            _TelICUInternalConsultationFormRepository = TelICUInternalConsultationFormRepository;
            _TelICUExitRepository = TelICUExitRepository;
            _patientRepository = patientRepository;
            _TelICUConsultationFormRepository = TelICUConsultationFormRepository;
            _caseClosureBLL = caseClosureBLL;
            _caseClosureRepository = caseClosureRepository;
            _locationBLL = locationBLL;
            _telIcuUsersRepository = telIcuUsersRepository;
            
        }


        public int Insert(TeleICUDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TeleICUDTO, TeleICU>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                obEntity.InvolvedUsers = Mapper.Map<ICollection<TeleICUUserDTO>, ICollection<TeleICUUser>>(obDTO.InvolvedUsers);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                _locationBLL.SetStatus(obDTO.LocationId, IsOccupied: true);
                _unitOfWorkAsync.SaveChanges();

                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.Add).Id,
                   NotificationObjectTypes.TeleICU,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Added",
                   $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TeleICU).Name.ToString(),
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
        public void Update(TeleICUDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
            //    var obEntity = Find(obDTO.Id);
                var obEntity = _repository.Query()
                               .Include(x => x.InvolvedUsers)
                               .SelectQueryable()
                               .Where(c => c.Id == obDTO.Id).FirstOrDefault();

                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);
                var involvedUsers = Mapper.Map<ICollection<TeleICUUserDTO>, ICollection<TeleICUUser>>(obDTO.InvolvedUsers);
                involvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var involvedUser in involvedUsers)
                    obEntity.InvolvedUsers.Add(involvedUser);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.TeleICUId = obEntity.Id);

                Mapper.Map<TeleICUDTO, TeleICU>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;

                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.Edit).Id,
                   NotificationObjectTypes.TeleICU,
                   obEntity.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TeleICU).Name.ToString(),
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

               var clinicalStorys= _TelICUClinicalStoryRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var clinicalStory in clinicalStorys)
                {
                    _TelICUClinicalStoryRepository.Delete(clinicalStory.Id);
                    _unitOfWorkAsync.SaveChanges();
                }
                

                var cUMedicationSchedules = _TelICUMedicationScheduleRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var cUMedicationSchedule in cUMedicationSchedules)
                {
                    _TelICUMedicationScheduleRepository.Delete(cUMedicationSchedule.Id);
                    _unitOfWorkAsync.SaveChanges();
                }
               

                var DiabetesControls = _TelICUDiabetesControlRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var DiabetesControl in DiabetesControls)
                {
                    _TelICUDiabetesControlRepository.Delete(DiabetesControl.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var VitalSignes = _TelICUVitalSignRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var VitalSigne in VitalSignes)
                {
                    _TelICUVitalSignRepository.Delete(VitalSigne.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var Pumps = _TelICUPumpRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var Pump in Pumps)
                {
                    _TelICUPumpRepository.Delete(Pump.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var MedicationDailySchedules = _TelICUMedicationDailyScheduleRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var MedicationDailySchedule in MedicationDailySchedules)
                {
                    _TelICUMedicationDailyScheduleRepository.Delete(MedicationDailySchedule.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var LabUnits = _TelICULabUnitRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var LabUnit in LabUnits)
                {
                    _TelICULabUnitRepository.Delete(LabUnit.Id);
                    _unitOfWorkAsync.SaveChanges();
                }
                var ConsultationForms = _TelICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var ConsultationForm in ConsultationForms)
                {
                    _TelICUConsultationFormRepository.Delete(ConsultationForm.Id);
                    _unitOfWorkAsync.SaveChanges();
                }
                var InternalConsultationForms = _TelICUInternalConsultationFormRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var InternalConsultationForm in InternalConsultationForms)
                {
                    _TelICUInternalConsultationFormRepository.Delete(InternalConsultationForm.Id);
                    _unitOfWorkAsync.SaveChanges();
                }

                var Exits = _TelICUExitRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).ToList();
                foreach (var Exit in Exits)
                {
                    _TelICUExitRepository.Delete(Exit.Id);
                    _unitOfWorkAsync.SaveChanges();
                }


                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
               _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.Delete).Id,
               NotificationObjectTypes.TeleICU,
               id.ToString(),
               user.FullName,
               "Deleted",
               $"ICU Case For Patient {patientFullName }",
               user.JobRole, usersInCase);

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TeleICU).Name.ToString(),
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

        public TeleICUDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TeleICUDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(TeleICUDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";


            return res;
        }

        public IQueryable<TeleICUDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<TeleICUDTO>();
            }
            catch { throw; }
        }

        public IQueryable<TeleICUDTO> GetAllIncludedPatient()
        {
            try
            {
                return _repository.Query().Include(x => x.Patient).Include(c => c.InvolvedUsers).SelectQueryable().ProjectTo<TeleICUDTO>();
            }
            catch { throw; }
        }

        public TelICUClinicalStoryDTO GetClinicalStory(int TeleICUId)
        {
            try
            {
                return _TelICUClinicalStoryRepository.Query().SelectQueryable().Where(f => f.TeleICUId == TeleICUId).ProjectTo<TelICUClinicalStoryDTO>().FirstOrDefault();
            }
            catch { throw; }
        }


        public void UpdateClinicalStory(TelICUClinicalStoryDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                TelICUClinicalStory obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUClinicalStoryRepository.Query().SelectQueryable().Where(x=>x.Id==obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICUClinicalStoryDTO, TelICUClinicalStory>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUClinicalStoryRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                           _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateCllinicalStory).Id,
                                           NotificationObjectTypes.TeleICU_ClinicalStory,
                                           _ICUCase.Id.ToString(),
                                           user.FullName, "Edited",
                                          $"ICU Case For Patient {patientFullName }",
                                           user.JobRole,
                                          usersInCase);
                    }
                

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUClinicalStory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUClinicalStoryDTO, TelICUClinicalStory>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUClinicalStoryRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);


                    obDTO.Id = obEntity.Id;
                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                             _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateCllinicalStory).Id,
                                             NotificationObjectTypes.TeleICU_ClinicalStory,
                                             _ICUCase.Id.ToString(),
                                             user.FullName, "Edited",
                                            $"ICU Case For Patient {patientFullName }",
                                             user.JobRole,
                                            usersInCase);
                    }
                  
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUClinicalStory).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateTeleICUCurrentStep(obEntity.TeleICUId, TeleICUCurrentSteps.MedicationDailyScheduleTable);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        private void UpdateTeleICUCurrentStep(int TeleICUId, TeleICUCurrentSteps newStep)
        {
            try
            {
                var TeleICU = Find(TeleICUId);
                TeleICU.CurrentStep = TeleICU.CurrentStep < (int)newStep ? (int)newStep : TeleICU.CurrentStep;
                TeleICU.ObjectState = ObjectState.Modified;
                Update(TeleICU);
                _unitOfWorkAsync.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int InsertMedicationScheduler(TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelICUMedicationSchedulerDTO, TelICUMedicationScheduler>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                _TelICUMedicationScheduleRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();


                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuMedicationScheduler.AddMedicationScheduler).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUMedicationScheduler).Name.ToString(),
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

        public void UpdateMedicationScheduler(TelICUMedicationSchedulerDTO obDTO)
        {
            try
            {
                TelICUMedicationScheduler obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUMedicationScheduleRepository.Find(obDTO.Id);
                    Mapper.Map<TelICUMedicationSchedulerDTO, TelICUMedicationScheduler>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUMedicationScheduleRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();


                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuMedicationScheduler.EditMedicationScheduler).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUMedicationScheduler).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUMedicationSchedulerDTO, TelICUMedicationScheduler>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUMedicationScheduleRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();


                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuMedicationScheduler.EditMedicationScheduler).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUMedicationScheduler).Name.ToString(),
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

        public IQueryable<TelICUMedicationSchedulerDTO> GetAllMedicationSchedulersByICUId(int ICUId)
        {
            try
            {
                return _TelICUMedicationScheduleRepository.Query().SelectQueryable().Where(x => x.TeleICUId == ICUId).ProjectTo<TelICUMedicationSchedulerDTO>();
            }
            catch { throw; }
        }

        public TelICUMedicationSchedulerDTO GetMedicationScheduler(int id)
        {
            try
            {
                return _TelICUMedicationScheduleRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelICUMedicationSchedulerDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void DeleteMedicationScheduler(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _TelICUMedicationScheduleRepository.Find(id);
                _TelICUMedicationScheduleRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuMedicationScheduler.DeleteMedicationScheduler).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelICUMedicationScheduler).Name.ToString(),
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









        public int InsertDiabetesControl(TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelICUDiabetesControlDTO, TelICUDiabetesControl>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                _TelICUDiabetesControlRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuDiabetesControl.AddDiabetesControl).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUDiabetesControl).Name.ToString(),
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

        public void UpdateDiabetesControl(TelICUDiabetesControlDTO obDTO)
        {
            try
            {
                TelICUDiabetesControl obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUDiabetesControlRepository.Find(obDTO.Id);
                    Mapper.Map<TelICUDiabetesControlDTO, TelICUDiabetesControl>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUDiabetesControlRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuDiabetesControl.EditDiabetesControl).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUDiabetesControl).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUDiabetesControlDTO, TelICUDiabetesControl>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUDiabetesControlRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuDiabetesControl.EditDiabetesControl).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUDiabetesControl).Name.ToString(),
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

        public IQueryable<TelICUDiabetesControlDTO> GetAllDiabetesControlsByICUId(int ICUId)
        {
            try
            {
                return _TelICUDiabetesControlRepository.Query().SelectQueryable().Where(x => x.TeleICUId == ICUId).ProjectTo<TelICUDiabetesControlDTO>();
            }
            catch { throw; }
        }

        public TelICUDiabetesControlDTO GetDiabetesControl(int id)
        {
            try
            {
                return _TelICUDiabetesControlRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelICUDiabetesControlDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void DeleteDiabetesControl(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _TelICUDiabetesControlRepository.Find(id);
                _TelICUDiabetesControlRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuDiabetesControl.DeleteDiabetesControl).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelICUDiabetesControl).Name.ToString(),
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







        public int InsertVitalSign(TelICUVitalSignDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelICUVitalSignDTO, TelICUVitalSign>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                _TelICUVitalSignRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuVitalSign.AddVitalSign).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUVitalSign).Name.ToString(),
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

        public void UpdateVitalSign(TelICUVitalSignDTO obDTO)
        {
            try
            {
                TelICUVitalSign obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUVitalSignRepository.Find(obDTO.Id);
                    Mapper.Map<TelICUVitalSignDTO, TelICUVitalSign>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUVitalSignRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuVitalSign.EditVitalSign).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUVitalSign).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUVitalSignDTO, TelICUVitalSign>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUVitalSignRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuVitalSign.EditVitalSign).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUVitalSign).Name.ToString(),
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

        public IQueryable<TelICUVitalSignDTO> GetAllVitalSignsByICUId(int ICUId)
        {
            try
            {
                return _TelICUVitalSignRepository.Query().SelectQueryable().Where(x => x.TeleICUId == ICUId).ProjectTo<TelICUVitalSignDTO>();
            }
            catch { throw; }
        }

        public TelICUVitalSignDTO GetVitalSign(int id)
        {
            try
            {
                return _TelICUVitalSignRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelICUVitalSignDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void DeleteVitalSign(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _TelICUVitalSignRepository.Find(id);
                _TelICUVitalSignRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuVitalSign.DeleteVitalSign).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelICUVitalSign).Name.ToString(),
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








        public int InsertPump(TelICUPumpDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<TelICUPumpDTO, TelICUPump>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                _TelICUPumpRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuPump.AddPump).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUPump).Name.ToString(),
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

        public void UpdatePump(TelICUPumpDTO obDTO)
        {
            try
            {
                TelICUPump obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUPumpRepository.Find(obDTO.Id);
                    Mapper.Map<TelICUPumpDTO, TelICUPump>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUPumpRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuPump.EditPump).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUPump).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUPumpDTO, TelICUPump>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUPumpRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuPump.EditPump).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUPump).Name.ToString(),
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

        public IQueryable<TelICUPumpDTO> GetAllPumpsByICUId(int ICUId)
        {
            try
            {
                return _TelICUPumpRepository.Query().SelectQueryable().Where(x => x.TeleICUId == ICUId).ProjectTo<TelICUPumpDTO>();
            }
            catch { throw; }
        }

        public TelICUPumpDTO GetPump(int id)
        {
            try
            {
                return _TelICUPumpRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<TelICUPumpDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void DeletePump(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _TelICUPumpRepository.Find(id);
                _TelICUPumpRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_ICUCase.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.TeleIcuPump.DeletePump).Id,
                   NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                   _ICUCase.Id.ToString(),
                   user.FullName, "Edited",
                  $"ICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelICUPump).Name.ToString(),
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

        public TelICUMedicationDailyScheduleDTO GetMedicationDailySchedule(int teleICUId)
        {
            try
            {
                return _TelICUMedicationDailyScheduleRepository.Query().SelectQueryable().Where(f => f.TeleICUId == teleICUId).ProjectTo<TelICUMedicationDailyScheduleDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateMedicationDailySchedule(TelICUMedicationDailyScheduleDTO obDTO)
        {
            try
            {
                TelICUMedicationDailySchedule obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUMedicationDailyScheduleRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICUMedicationDailyScheduleDTO, TelICUMedicationDailySchedule>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUMedicationDailyScheduleRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateMedicationDailyScheduleTable).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUMedicationDailySchedule).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUMedicationDailyScheduleDTO, TelICUMedicationDailySchedule>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUMedicationDailyScheduleRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateMedicationDailyScheduleTable).Id,
                       NotificationObjectTypes.TeleICU_MedicationDailyScheduleTable,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUMedicationDailySchedule).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateTeleICUCurrentStep(obEntity.TeleICUId, TeleICUCurrentSteps.LabUnit);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public TelICULabUnitDTO GetLabUnit(int telelabunitId)
        {
            try
            {
                return _TelICULabUnitRepository.Query().SelectQueryable().Where(f => f.Id == telelabunitId).ProjectTo<TelICULabUnitDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateLabUnit(TelICULabUnitDTO obDTO,bool PuplishNotification=true)
        {
            try
            {
                TelICULabUnit obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICULabUnitRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICULabUnitDTO, TelICULabUnit>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICULabUnitRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PuplishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                         _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateLabUnit).Id,
                                         NotificationObjectTypes.TeleICU_LabUnit,
                                         _ICUCase.Id.ToString(),
                                         user.FullName, "Edited",
                                        $"ICU Case For Patient {patientFullName }",
                                         user.JobRole,
                                        usersInCase);
                    }
              

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICULabUnit).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICULabUnitDTO, TelICULabUnit>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICULabUnitRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);

                    obDTO.Id = obEntity.Id;

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PuplishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                    _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateLabUnit).Id,
                    NotificationObjectTypes.TeleICU_LabUnit,
                    _ICUCase.Id.ToString(),
                    user.FullName, "Edited",
                   $"ICU Case For Patient {patientFullName }",
                    user.JobRole,
                   usersInCase);
                    }
                 

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICULabUnit).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateTeleICUCurrentStep(obEntity.TeleICUId, TeleICUCurrentSteps.InternalConsultationForm);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }


        public TelICUInternalConsultationFormDTO GetInternalConsultationForm(int teleICUId)
        {
            try
            {
                return _TelICUInternalConsultationFormRepository.Query().SelectQueryable().Where(f => f.TeleICUId == teleICUId).ProjectTo<TelICUInternalConsultationFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateInternalConsultationForm(TelICUInternalConsultationFormDTO obDTO)
        {
            try
            {
                TelICUInternalConsultationForm obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUInternalConsultationFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICUInternalConsultationFormDTO, TelICUInternalConsultationForm>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUInternalConsultationFormRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateInternalConsultationForm).Id,
                       NotificationObjectTypes.TeleICU_InternalConsultationForm,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUInternalConsultationForm).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUInternalConsultationFormDTO, TelICUInternalConsultationForm>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUInternalConsultationFormRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateInternalConsultationForm).Id,
                       NotificationObjectTypes.TeleICU_InternalConsultationForm,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);



                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUInternalConsultationForm).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateTeleICUCurrentStep(obEntity.TeleICUId, TeleICUCurrentSteps.PatientExitStatusReport);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }





        public TelICUExitDTO GetExit(int teleICUId)
        {
            try
            {
                return _TelICUExitRepository.Query().SelectQueryable().Where(f => f.TeleICUId == teleICUId).ProjectTo<TelICUExitDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateExit(TelICUExitDTO obDTO)
        {
            try
            {
                TelICUExit obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUExitRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICUExitDTO, TelICUExit>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUExitRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();


                    //_locationBLL.SetStatus(_ICUCase.LocationId.Value, IsOccupied: false);
                    //_unitOfWorkAsync.SaveChanges();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdatePatientExitStatusReport).Id,
                       NotificationObjectTypes.TeleICU_PatientExitStatusReport,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUExit).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUExitDTO, TelICUExit>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUExitRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();
                 
                    //_locationBLL.SetStatus(_ICUCase.LocationId.Value, IsOccupied: false);
                    //_unitOfWorkAsync.SaveChanges();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdatePatientExitStatusReport).Id,
                       NotificationObjectTypes.TeleICU_PatientExitStatusReport,
                       _ICUCase.Id.ToString(),
                       user.FullName, "Edited",
                      $"ICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUExit).Name.ToString(),
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
                   $"ICU Case For Patient {patientFullName }",
                    user.JobRole,
                    url,
                   usersInCase);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<TelICULabUnitDTO> GetLabUnits(int telIcuId)
        {
            try
            {
                return _TelICULabUnitRepository.Query().SelectQueryable().Where(x => x.TeleICUId == telIcuId).ProjectTo<TelICULabUnitDTO>();
            }
            catch { throw; }
        }

        public void DeleteLabUnit(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _TelICULabUnitRepository.Find(id);
                _TelICULabUnitRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(TelICULabUnitDTO).Name.ToString(),
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


        public TelICUConsultationFormDTO GetConsultationForm(int teleConsultationForm)
        {
            try
            {
                return _TelICUConsultationFormRepository.Query().SelectQueryable().Where(f => f.Id == teleConsultationForm).ProjectTo<TelICUConsultationFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateConsultationForm(TelICUConsultationFormDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                TelICUConsultationForm obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _TelICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<TelICUConsultationFormDTO, TelICUConsultationForm>(obDTO, obEntity);
                    obEntity.ObjectState = ObjectState.Modified;
                    _TelICUConsultationFormRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                           _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateConsultationForm).Id,
                                           NotificationObjectTypes.TeleICU_ConsultationForm,
                                           _ICUCase.Id.ToString(),
                                           user.FullName, "Edited",
                                          $"ICU Case For Patient {patientFullName }",
                                           user.JobRole,
                                          usersInCase);
                    }
                
                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUConsultationForm).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<TelICUConsultationFormDTO, TelICUConsultationForm>(obDTO);
                    obEntity.ObjectState = ObjectState.Added;
                    _TelICUConsultationFormRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    var _ICUCase = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.TeleICUId).FirstOrDefault();

                    var usersInCase = _ICUCase.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_ICUCase.CreatedBy);


                    obDTO.Id = obEntity.Id;
                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _ICUCase.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                     _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.UpdateConsultationForm).Id,
                     NotificationObjectTypes.TeleICU_ConsultationForm,
                     _ICUCase.Id.ToString(),
                     user.FullName, "Edited",
                    $"ICU Case For Patient {patientFullName }",
                     user.JobRole,
                    usersInCase);
                    }
                  



                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(TelICUConsultationForm).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateTeleICUCurrentStep(obEntity.TeleICUId, TeleICUCurrentSteps.PatientExitStatusReport);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }

        public void Close(CaseClosureDTO obDTO,bool PublishNotification=true)
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
                _locationBLL.SetStatus(_case.LocationId, IsOccupied: false);
                _unitOfWorkAsync.SaveChanges();
                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();

                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.Close).Id,
              NotificationObjectTypes.TeleICU,
              obDTO.CaseId.ToString(),
              user.FullName, "Closed",
             $"Mental Health Case For Patient {_casePatientName.FirstName ?? ""} {_casePatientName.LastName ?? "" }",
              user.JobRole,
             usersInCase);
                }

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleICU).Name.ToString(),
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
                _locationBLL.SetStatus(_case.LocationId, IsOccupied: true);
                _unitOfWorkAsync.SaveChanges();
                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();

                _userNotificationBLL.PublishNotification(
                 _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.ReOpen).Id,
                 NotificationObjectTypes.TeleICU,
                caseId.ToString(),
                 user.FullName, "ReOpened",
                $"Mental Health Case For Patient {_casePatientName.FirstName} {_casePatientName.LastName }",
                 user.JobRole,
                usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(TeleICU).Name.ToString(),
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

        public IQueryable<CaseClosureDTO> GetCaseClosures(int ICUId)
        {
            try
            {
                return _caseClosureRepository.Query().SelectQueryable().Where(x => x.CaseId == ICUId && x.CaseDepartment == (int)DepartmentEnum.ICU).ProjectTo<CaseClosureDTO>();
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

        public string ValidToCloseCase(int id)
        {
            try
            {
                var res = "";
                if (!_TelICUExitRepository.Query().SelectQueryable().Where(x => x.TeleICUId == id).Any())
                    res = "You have to add Exit Report to be able to close the case!!";
                return res;
            }
            catch
            {

                throw;
            }
        }

        public IQueryable<TelICUConsultationFormDTO> GetConsultationForms(int telIcuId)
        {
            try
            {
                return _TelICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.TeleICUId == telIcuId).ProjectTo<TelICUConsultationFormDTO>();
            }
            catch { throw; }
        }

        public void DeleteConsultationForm(int id)
        {
            try { 
            _unitOfWorkAsync.BeginTransaction();
            var obEntity = _TelICUConsultationFormRepository.Find(id);
                _TelICUConsultationFormRepository.Delete(id);
            _unitOfWorkAsync.SaveChanges();
            _operationLogBLL.Insert(
              new OperationLogDTO()
              {
                  EntityType = typeof(TelICUConsultationFormDTO).Name.ToString(),
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

        public void UpdateInvolvedUsersColors(int id)
        {
            try {
                _unitOfWorkAsync.BeginTransaction();

                var InvolvedUsers = _repository.Query().SelectQueryable().Where(c => c.Id == id).Include(c => c.InvolvedUsers).Select(c => c.InvolvedUsers).FirstOrDefault();
                foreach (var User in InvolvedUsers)
                {
                    if (User.Color == null)
                    {
                        foreach (string color in Colors)
                        {
                            if (!InvolvedUsers.Select(c=>c.Color).Contains(color))
                            {
                                User.Color = color;
                                _telIcuUsersRepository.Update(User);
                                break;
                            }
                        }
                    }
                }
                var UsersWithNoColor = InvolvedUsers.Where(c => c.Color == null).ToList();
                for (int i = 0; i < UsersWithNoColor.Count; i++)
                {
                    UsersWithNoColor[i].Color = Colors[i % Colors.Count];
                    _telIcuUsersRepository.Update(UsersWithNoColor[i]);

                }
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }
    }
}
