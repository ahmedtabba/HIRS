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

    public class NPICUBLL : Service<NPICU>, INPICUBLL
    {
        private readonly IUserNotificationBLL _userNotificationBLL;
        readonly IRepositoryAsync<NPICU> _repository;
        readonly IRepositoryAsync<Patient> _patientRepository;
        readonly IRepositoryAsync<NICUConsultationForm> _NICUConsultationFormRepository;
        readonly IRepositoryAsync<PICUConsultationForm> _PICUConsultationFormRepository;
        readonly IRepositoryAsync<NICUConsultationFollowUpForm> _NICUConsultationFollowUpFormRepository;
        readonly IRepositoryAsync<PICUConsultationFollowUpForm> _PICUConsultationFollowUpFormRepository;
        readonly IRepositoryAsync<NPICUInvestigation> _NPICUInvestigationRepository;
        readonly IRepositoryAsync<NPICUConsultantSection> _NPICUConsultantSectionRepository;
        readonly IRepositoryAsync<NPICUUser> _telnpicuUsersRepository;

        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;
        private readonly ICaseClosureBLL _caseClosureBLL;
        readonly IRepositoryAsync<CaseClosure> _caseClosureRepository;
        List<string> Colors = new List<string>() { "#CFAFAF", "#EDA2A2", "#FCAF94",
            "#FFC388", "#EAEDC9", "#E5F494",
            "#CCF0C8", "#B7E2DB", "#93E4FF",
            "#CEC0EA","#ff373742","#ff379d36","#6837ff1c","#3799ff61","#ffcb372b"

        };
        public NPICUBLL(
     IRepositoryAsync<NPICU> repository,
      IRepositoryAsync<Patient> patientRepository,
        IRepositoryAsync<NICUConsultationForm> NICUConsultationFormRepository,
        IRepositoryAsync<PICUConsultationForm> PICUConsultationFormRepository,
        IRepositoryAsync<NICUConsultationFollowUpForm> NICUConsultationFollowUpFormRepositor,
        IRepositoryAsync<PICUConsultationFollowUpForm> PICUConsultationFollowUpFormRepositor,
        IRepositoryAsync<NPICUInvestigation> NPICUInvestigationRepository,
        IRepositoryAsync<NPICUConsultantSection> NPICUConsultantSectionRepository,
        IRepositoryAsync<NPICUUser> telnpicuUsersRepository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IApplicationUserDataContext applicationContext
  ,
            IUserNotificationBLL userNotificationBLL, ICaseClosureBLL caseClosureBLL, IRepositoryAsync<CaseClosure> caseClosureRepository) : base(repository, applicationContext)
        {
            _repository = repository;
            _NICUConsultationFormRepository = NICUConsultationFormRepository;
            _PICUConsultationFormRepository = PICUConsultationFormRepository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _patientRepository = patientRepository;
            _userNotificationBLL = userNotificationBLL;
            _NICUConsultationFollowUpFormRepository = NICUConsultationFollowUpFormRepositor;
            _PICUConsultationFollowUpFormRepository = PICUConsultationFollowUpFormRepositor;
            _NPICUInvestigationRepository = NPICUInvestigationRepository;
            _NPICUConsultantSectionRepository = NPICUConsultantSectionRepository;
            _caseClosureBLL = caseClosureBLL;
            _caseClosureRepository = caseClosureRepository;
            _telnpicuUsersRepository = telnpicuUsersRepository;
        }


        public int Insert(NPICUDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<NPICUDTO, NPICU>(obDTO);
                obEntity.ObjectState = ObjectState.Added;



                obEntity.InvolvedUsers = Mapper.Map<ICollection<NPICUUserDTO>, ICollection<NPICUUser>>(obDTO.InvolvedUsers);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);




                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();


                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.Add).Id,
                   NotificationObjectTypes.NPICU,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Added",
                   $"NPICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);


                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
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
        public void Update(NPICUDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Query()
                                    .Include(x => x.InvolvedUsers)
                                    .SelectQueryable()
                                    .Where(c => c.Id == obDTO.Id).FirstOrDefault();


                obEntity.InvolvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Deleted);
                var involvedUsers = Mapper.Map<ICollection<NPICUUserDTO>, ICollection<NPICUUser>>(obDTO.InvolvedUsers);
                involvedUsers.ToList().ForEach(c => c.ObjectState = ObjectState.Added);
                foreach (var involvedUser in involvedUsers)
                    obEntity.InvolvedUsers.Add(involvedUser);
                obEntity.InvolvedUsers.ToList().ForEach(c => c.NPICUId = obEntity.Id);


                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();


                var usersInCase = obEntity.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(obEntity.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == obEntity.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.Edit).Id,
                   NotificationObjectTypes.NPICU,
                   obEntity.Id.ToString(),
                   user.FullName, "Edited",
                  $"NPICU For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);


                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
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
                var obEntity = _repository.Find(id);
                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(NPICU).Name.ToString(),
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

        public NPICUDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<NPICUDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(NPICUDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";


            return res;
        }

        public IQueryable<NPICUDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<NPICUDTO>();
            }
            catch { throw; }
        }

        public IQueryable<NPICUDTO> GetAllIncludedPatient()
        {
            try
            {
                return _repository.Query().Include(x => x.Patient).Include(x => x.InvolvedUsers).SelectQueryable().ProjectTo<NPICUDTO>();
            }
            catch { throw; }
        }

        public NICUConsultationFormDTO GetNICUConsultationForm(int NICUId)
        {
            try
            {
                return _NICUConsultationFormRepository.Query().SelectQueryable().Where(f => f.NPICUId == NICUId).ProjectTo<NICUConsultationFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public PICUConsultationFormDTO GetPICUConsultationForm(int PICUId)
        {
            try
            {
                return _PICUConsultationFormRepository.Query().SelectQueryable().Where(f => f.NPICUId == PICUId).ProjectTo<PICUConsultationFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateNICUConsultationForm(NICUConsultationFormDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                NICUConsultationForm obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _NICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<NICUConsultationFormDTO, NICUConsultationForm>(obDTO, obEntity);




                    obEntity.ObjectState = ObjectState.Modified;
                    _NICUConsultationFormRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                    var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_case.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                    _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdateNICUConsultationForm).Id,
                                    NotificationObjectTypes.NPICU_ConsultationForm,
                                    _case.Id.ToString(),
                                    user.FullName, "Edited",
                                   $"NPICU Case For Patient {patientFullName }",
                                    user.JobRole,
                                   usersInCase);
                    }
         


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<NICUConsultationFormDTO, NICUConsultationForm>(obDTO);



                    obEntity.ObjectState = ObjectState.Added;
                    _NICUConsultationFormRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    obDTO.Id = obEntity.Id;
                    var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                    var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                    usersInCase.Add(_case.CreatedBy);

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                    _userNotificationBLL.PublishNotification(
                       _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdateNICUConsultationForm).Id,
                       NotificationObjectTypes.NPICU_ConsultationForm,
                       _case.Id.ToString(),
                       user.FullName, "Edited",
                      $"NPICU Case For Patient {patientFullName }",
                       user.JobRole,
                      usersInCase);

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step2);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }


        public void UpdatePICUConsultationForm(PICUConsultationFormDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                PICUConsultationForm obEntity;
                _unitOfWorkAsync.BeginTransaction();
                if (obDTO.Id > 0)
                {
                    obEntity = _PICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                    Mapper.Map<PICUConsultationFormDTO, PICUConsultationForm>(obDTO, obEntity);




                    obEntity.ObjectState = ObjectState.Modified;
                    _PICUConsultationFormRepository.Update(obEntity);
                    _unitOfWorkAsync.SaveChanges();

                    var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                    var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(_case.CreatedBy);



                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                    if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                            _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdatePICUConsultationForm).Id,
                                            NotificationObjectTypes.NPICU_ConsultationForm,
                                            _case.Id.ToString(),
                                            user.FullName, "Edited",
                                           $"NPICU Case For Patient {patientFullName }",
                                            user.JobRole,
                                           usersInCase);
                    }
                 


                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Update,
                        LogDescription = String.Format("{0} is updated",
                        obEntity.Id.ToString())
                    });


                }
                else
                {
                    obEntity = Mapper.Map<PICUConsultationFormDTO, PICUConsultationForm>(obDTO);



                    obEntity.ObjectState = ObjectState.Added;
                    _PICUConsultationFormRepository.Insert(obEntity);
                    _unitOfWorkAsync.SaveChanges();
                    obDTO.Id = obEntity.Id;
                    var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                    var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                    usersInCase.Add(_case.CreatedBy);

                    var user = _applicationContext.GetApplicationUserData();
                    var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

             if (PublishNotification)
                    {
                        _userNotificationBLL.PublishNotification(
                                            _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdatePICUConsultationForm).Id,
                                            NotificationObjectTypes.NPICU_ConsultationForm,
                                            _case.Id.ToString(),
                                            user.FullName, "Edited",
                                           $"NPICU Case For Patient {patientFullName }",
                                            user.JobRole,
                                           usersInCase);
                    }

                    _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(NPICU).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.Id.ToString(),
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });
                }
                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step2);



                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        private void UpdateNPICUCurrentStep(int NPICUId, NPICUCurrentSteps newStep)
        {
            try
            {
                var currentCase = Find(NPICUId);
                currentCase.CurrentStep = currentCase.CurrentStep < (int)newStep ? (int)newStep : currentCase.CurrentStep;
                currentCase.ObjectState = ObjectState.Modified;
                Update(currentCase);
                _unitOfWorkAsync.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public NICUConsultationFollowUpFormDTO GetNICUConsultationFollowUpForm(int id)
        {
            try
            {
                return _NICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<NICUConsultationFollowUpFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public PICUConsultationFollowUpFormDTO GetPICUConsultationFollowUpForm(int id)
        {
            try
            {
                return _PICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<PICUConsultationFollowUpFormDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public NPICUInvestigationDTO GetNPICUInvestigation(int id)
        {
            try
            {
                return _NPICUInvestigationRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<NPICUInvestigationDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public void UpdateNICUConsultationFollowUpForm(NICUConsultationFollowUpFormDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                NICUConsultationFollowUpForm obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = _NICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<NICUConsultationFollowUpFormDTO, NICUConsultationFollowUpForm>(obDTO, obEntity);




                obEntity.ObjectState = ObjectState.Modified;
                _NICUConsultationFollowUpFormRepository.Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
                 _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdateNICUConsultationFollowUpForm).Id,
                 NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
                 _case.Id.ToString(),
                 user.FullName, "Edited",
                $"NPICU Case For Patient {patientFullName }",
                 user.JobRole,
                usersInCase);
                }
              


                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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

        public void UpdatePICUConsultationFollowUpForm(PICUConsultationFollowUpFormDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                PICUConsultationFollowUpForm obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = _PICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<PICUConsultationFollowUpFormDTO, PICUConsultationFollowUpForm>(obDTO, obEntity);




                obEntity.ObjectState = ObjectState.Modified;
                _PICUConsultationFollowUpFormRepository.Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
                _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdatePICUConsultationFollowUpForm).Id,
                NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
                _case.Id.ToString(),
                user.FullName, "Edited",
               $"NPICU Case For Patient {patientFullName }",
                user.JobRole,
               usersInCase);
                }
             


                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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

    

        public int InsertNICUConsultationFollowUpForm(NICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {
                NICUConsultationFollowUpForm obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = Mapper.Map<NICUConsultationFollowUpFormDTO, NICUConsultationFollowUpForm>(obDTO);
                var NICUConsultationForm = _NICUConsultationFormRepository.Query().SelectQueryable().Where(x => x.NPICUId == obDTO.NPICUId)
                .Select(x => new { x.BirthWeight, x.GestationalAge, x.ChronologicalAge }).FirstOrDefault();

                obEntity.BirthWeight = NICUConsultationForm.BirthWeight;
                obEntity.GestationalAge = NICUConsultationForm.GestationalAge;
                obEntity.ChronologicalAge = NICUConsultationForm.ChronologicalAge;

                obEntity.ObjectState = ObjectState.Added;
                _NICUConsultationFollowUpFormRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.AddNICUConsultationFollowUpForm).Id,
                   NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
                   _case.Id.ToString(),
                   user.FullName, "Edited",
                  $"NPICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("{0} is added",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step3);

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        public int InsertPICUConsultationFollowUpForm(PICUConsultationFollowUpFormDTO obDTO)
        {
            try
            {
                PICUConsultationFollowUpForm obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = Mapper.Map<PICUConsultationFollowUpFormDTO, PICUConsultationFollowUpForm>(obDTO);
               

                obEntity.ObjectState = ObjectState.Added;
                _PICUConsultationFollowUpFormRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.AddPICUConsultationFollowUpForm).Id,
                   NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
                   _case.Id.ToString(),
                   user.FullName, "Edited",
                  $"NPICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("{0} is added",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step3);

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }


      

        public void DeleteNICUConsultationFollowUpForm(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _NICUConsultationFollowUpFormRepository.Find(id);
                _NICUConsultationFollowUpFormRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();
                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                _userNotificationBLL.PublishNotification(
               _userNotificationBLL.GetByName(NotificationConsistent.NPICU.DeleteNICUConsultationFollowUpForm).Id,
               NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
               _case.Id.ToString(),
               user.FullName, "Deleted FollowUp Form" ,
              $"NICU Case For Patient {patientFullName }",
               user.JobRole,
              usersInCase);

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(NICUConsultationFollowUpForm).Name.ToString(),
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

        public void DeletePICUConsultationFollowUpForm(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _PICUConsultationFollowUpFormRepository.Find(id);
                _PICUConsultationFollowUpFormRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();
                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                _userNotificationBLL.PublishNotification(
               _userNotificationBLL.GetByName(NotificationConsistent.NPICU.DeletePICUConsultationFollowUpForm).Id,
               NotificationObjectTypes.NPICU_ConsultationFollowUpForm,
               _case.Id.ToString(),
               user.FullName, "Deleted FollowUp Form",
              $"PICU Case For Patient {patientFullName }",
               user.JobRole,
              usersInCase);
                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(PICUConsultationFollowUpForm).Name.ToString(),
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


        

        public IQueryable<NICUConsultationFollowUpFormDTO> GetNICUConsultationFollowUpForms(int NICUId)
        {
            try
            {
                return _NICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(x=>x.NPICUId==NICUId).ProjectTo<NICUConsultationFollowUpFormDTO>();
            }
            catch { throw; }
        }

        public IQueryable<PICUConsultationFollowUpFormDTO> GetPICUConsultationFollowUpForms(int PICUId)
        {
            try
            {
                return _PICUConsultationFollowUpFormRepository.Query().SelectQueryable().Where(x => x.NPICUId == PICUId).ProjectTo<PICUConsultationFollowUpFormDTO>();
            }
            catch { throw; }
        }


        public void UpdateNPICUInvestigation(NPICUInvestigationDTO obDTO, bool PublishNotification = true)
        {
            try
            {
                NPICUInvestigation obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = _NPICUInvestigationRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<NPICUInvestigationDTO, NPICUInvestigation>(obDTO, obEntity);




                obEntity.ObjectState = ObjectState.Modified;
                _NPICUInvestigationRepository.Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
                                     _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdateNPICUInvestigation).Id,
                                     NotificationObjectTypes.NPICU_Investigation,
                                     _case.Id.ToString(),
                                     user.FullName, "Edited",
                                    $"NPICU Case For Patient {patientFullName }",
                                     user.JobRole,
                                    usersInCase);
                }
              


                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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
        public int InsertNPICUInvestigation(NPICUInvestigationDTO obDTO)
        {
            try
            {
                NPICUInvestigation obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = Mapper.Map<NPICUInvestigationDTO, NPICUInvestigation>(obDTO);


                obEntity.ObjectState = ObjectState.Added;
                _NPICUInvestigationRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.AddNPICUInvestigation).Id,
                   NotificationObjectTypes.NPICU_Investigation,
                   _case.Id.ToString(),
                   user.FullName, "Edited",
                  $"NPICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("{0} is added",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step4);

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void DeleteNPICUInvestigation(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _NPICUInvestigationRepository.Find(id);
                _NPICUInvestigationRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(NPICUInvestigation).Name.ToString(),
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
        public IQueryable<NPICUInvestigationDTO> GetNPICUInvestigations(int NICUId)
        {
            try
            {
                return _NPICUInvestigationRepository.Query().SelectQueryable().Where(x => x.NPICUId == NICUId).ProjectTo<NPICUInvestigationDTO>();
            }
            catch { throw; }
        }





        public IQueryable<NPICUConsultantSectionDTO> GetNPICUConsultantSections(int NICUId)
        {
            try
            {
                return _NPICUConsultantSectionRepository.Query().SelectQueryable().Where(x => x.NPICUId == NICUId).ProjectTo<NPICUConsultantSectionDTO>();
            }
            catch { throw; }
        }
        public void DeleteNPICUConsultantSection(int id)
        {
            try
            {

                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _NPICUConsultantSectionRepository.Find(id);
                _NPICUConsultantSectionRepository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(NPICUConsultantSection).Name.ToString(),
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

        public NPICUConsultantSectionDTO GetNPICUConsultantSection(int id)
        {
            try
            {
                return _NPICUConsultantSectionRepository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<NPICUConsultantSectionDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

       

        public void UpdateNPICUConsultantSection(NPICUConsultantSectionDTO obDTO,bool PublishNotification = true)
        {
            try
            {
                NPICUConsultantSection obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = _NPICUConsultantSectionRepository.Query().SelectQueryable().Where(x => x.Id == obDTO.Id).FirstOrDefault();
                Mapper.Map<NPICUConsultantSectionDTO, NPICUConsultantSection>(obDTO, obEntity);




                obEntity.ObjectState = ObjectState.Modified;
                _NPICUConsultantSectionRepository.Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();
                usersInCase.Add(_case.CreatedBy);



                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();
                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
            _userNotificationBLL.GetByName(NotificationConsistent.NPICU.UpdateNPICUConsultantSection).Id,
            NotificationObjectTypes.NPICU_ConsultantSection,
            _case.Id.ToString(),
            user.FullName, "Edited",
           $"NPICU Case For Patient {patientFullName }",
            user.JobRole,
           usersInCase);
                }
         


                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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


        public int InsertNPICUConsultantSection(NPICUConsultantSectionDTO obDTO)
        {
            try
            {
                NPICUConsultantSection obEntity;
                _unitOfWorkAsync.BeginTransaction();

                obEntity = Mapper.Map<NPICUConsultantSectionDTO, NPICUConsultantSection>(obDTO);


                obEntity.ObjectState = ObjectState.Added;
                _NPICUConsultantSectionRepository.Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                obDTO.Id = obEntity.Id;
                var _case = _repository.Query().Include(x => x.InvolvedUsers).SelectQueryable().Where(x => x.Id == obEntity.NPICUId).FirstOrDefault();

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);

                var user = _applicationContext.GetApplicationUserData();
                var patientFullName = _patientRepository.Query().SelectQueryable().Where(x => x.Id == _case.PatientId).Select(x => x.FirstName).FirstOrDefault();

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.NPICU.AddNPICUConsultantSection).Id,
                   NotificationObjectTypes.NPICU_ConsultantSection,
                   _case.Id.ToString(),
                   user.FullName, "Edited",
                  $"NPICU Case For Patient {patientFullName }",
                   user.JobRole,
                  usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.Id.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("{0} is added",
                    obEntity.Id.ToString())
                });


                _unitOfWorkAsync.SaveChanges();


                UpdateNPICUCurrentStep(obEntity.NPICUId, NPICUCurrentSteps.Step4);

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
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
                   $"N/PICU Case For Patient {patientFullName }",
                    user.JobRole,
                    url,
                   usersInCase);
            }
            catch (Exception ex)
            {

                throw;
            }
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

                var usersInCase = _case.InvolvedUsers.Select(x => x.UserId).ToList();

                usersInCase.Add(_case.CreatedBy);
                var user = _applicationContext.GetApplicationUserData();

                if (PublishNotification)
                {
                    _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.NPICU.Close).Id,
              NotificationObjectTypes.NPICU,
              obDTO.CaseId.ToString(),
              user.FullName, "Closed",
             $"Mental Health Case For Patient {_casePatientName.FirstName ?? ""} {_casePatientName.LastName ?? "" }",
              user.JobRole,
             usersInCase);
                }

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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
                 _userNotificationBLL.GetByName(NotificationConsistent.NPICU.ReOpen).Id,
                 NotificationObjectTypes.NPICU,
                caseId.ToString(),
                 user.FullName, "ReOpened",
                $"Mental Health Case For Patient {_casePatientName.FirstName} {_casePatientName.LastName }",
                 user.JobRole,
                usersInCase);

                _operationLogBLL.Insert(
                new OperationLogDTO()
                {
                    EntityType = typeof(NPICU).Name.ToString(),
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

        public IQueryable<CaseClosureDTO> GetCaseClosures(int npicuId)
        {
            try
            {
                return _caseClosureRepository.Query().SelectQueryable().Where(x => x.CaseId == npicuId && x.CaseDepartment == (int)DepartmentEnum.NPICU).ProjectTo<CaseClosureDTO>();
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
                                _telnpicuUsersRepository.Update(User);
                                break;
                            }
                        }
                    }
                }
                var UsersWithNoColor = InvolvedUsers.Where(c => c.Color == null).ToList();
                for (int i = 0; i < UsersWithNoColor.Count; i++)
                {
                    UsersWithNoColor[i].Color = Colors[i % Colors.Count];
                    _telnpicuUsersRepository.Update(UsersWithNoColor[i]);

                }
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }

        }
    }
}
