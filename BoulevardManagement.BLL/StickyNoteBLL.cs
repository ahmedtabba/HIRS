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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.BLL
{
    public class StickyNoteBLL : Service<StickyNote>, IStickyNoteBLL
    {
        readonly IRepositoryAsync<StickyNote> _repository;
        //readonly IRepositoryAsync<StickyNote_User> _StickyNoteUserRepository;
        readonly IRepositoryAsync<TeleMentalHealth> _teleMentalHealthRepository;
        readonly IRepositoryAsync<TeleICU> _teleIcuRepository;
        readonly IRepositoryAsync<NPICU> _teleNPICURepository;

        private readonly IRepositoryAsync<NotificationGroup> _notificationGroupRepository;

       
        private readonly IRepositoryAsync<UserNotificationGroups> _userNotificationGroupsRepository;

        private readonly IUserNotificationBLL _userNotificationBLL;
        private readonly IOperationLogBLL _operationLogBLL;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
      

        public StickyNoteBLL(
            IRepositoryAsync<StickyNote> repository,
            //IRepositoryAsync<StickyNote_User> StickyNoteUserRepository,
            IRepositoryAsync<UserNotificationGroups> userNotificationGroupsRepository,
            IRepositoryAsync<NotificationGroup> notificationGroupRepository,
            IUserNotificationBLL userNotificationBLL,
            IOperationLogBLL operationLogBLL,
            IUnitOfWorkAsync unitOfWorkAsync,
            IRepositoryAsync<TeleMentalHealth> teleMentalHealthRepository,
            IRepositoryAsync<TeleICU> teleIcuRepository,
            IRepositoryAsync<NPICU> teleNPICURepository,
            IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _userNotificationBLL = userNotificationBLL;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
            _operationLogBLL = operationLogBLL;
            //_StickyNoteUserRepository = StickyNoteUserRepository;
            _userNotificationGroupsRepository = userNotificationGroupsRepository;
            _notificationGroupRepository = notificationGroupRepository;
            _teleMentalHealthRepository = teleMentalHealthRepository;
            _teleIcuRepository = teleIcuRepository;
            _teleNPICURepository = teleNPICURepository;
        }

        public IQueryable<StickyNoteDTO> GetAllbyObject(string objectId, string objectType, int channel)
        {
            try
            {
                int objId = HashIdsManager.Decrypt(objectId);
                return _repository
                    .Query()
                    .SelectQueryable()
                    .Where(c => c.ObjectId == objId && c.ObjectType == objectType && c.Channel == channel).ProjectTo<StickyNoteDTO>();
            }
            catch { throw; }
        }

        public IQueryable<NewsFeedCommentDTO> GetAllbyObject(int objectId, string objectType, int channel)
        {
            try
            {

                return _repository
                    .Query()
                    .SelectQueryable()
                    .Where(c => c.ObjectId == objectId && c.ObjectType == objectType && c.Channel == channel).ProjectTo<NewsFeedCommentDTO>();
            }
            catch { throw; }
        }


        public IQueryable<StickyNoteDTO> GetAll()
        {
            try
            {
                return _repository
                    .Query()
                    .SelectQueryable()
                    .ProjectTo<StickyNoteDTO>();
            }
            catch { throw; }
        }


        public IQueryable<StickyNoteGridDTO> GetAllForGrid()
        {
            try
            {
                var notes = _repository
                    .Query()
                    .SelectQueryable()
                    .ProjectTo<StickyNoteGridDTO>();



                return notes;
            }
            catch { throw; }
        }

        public void Update(StickyNoteDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<StickyNoteDTO, StickyNote>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }
        public void Delete(int id)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = _repository.Query().SelectQueryable().Where(c => c.Id == id).FirstOrDefault();
               
                _unitOfWorkAsync.SaveChanges();
                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception e) { _unitOfWorkAsync.Rollback(); throw; }
        }
        public int Insert(StickyNoteDTO obDTO, List<string> SalesUsers = null)
        {
            try
            {
                var linkParserHttps = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                var rawString = obDTO.Note;
                foreach (Match m in linkParserHttps.Matches(rawString))
                {
                    obDTO.Note = obDTO.Note.Replace(m.Value, "<a target='_blank' href='" + m.Value + "'>" + m.Value + "</a>");
                }
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<StickyNoteDTO, StickyNote>(obDTO);
                _repository.Insert(obEntity);
                obEntity.ObjectState = ObjectState.Added;
                InsertOrUpdateGraph(obEntity);
                _unitOfWorkAsync.SaveChanges();
                string NotificationStr = "";
                NotificationObjectTypes notificationObjectType;

                if (obDTO.ObjectType.Contains(typeof(TeleICU).Name.ToString()))
                {
                    NotificationStr = NotificationConsistent.TeleIcu.ChatOnICU;
                    notificationObjectType = NotificationObjectTypes.TeleICU_Chat;
                }
                else if (obDTO.ObjectType.Contains(typeof(NPICU).Name.ToString()))
                {
                    NotificationStr = NotificationConsistent.NPICU.ChatOnNPICU;
                    notificationObjectType = NotificationObjectTypes.NPICU_Chat;
                }
                else if (obDTO.ObjectType.Contains(typeof(TeleMentalHealth).Name.ToString()))
                {
                    NotificationStr = NotificationConsistent.TeleMentalHealth.ChatOnMentalHealth;
                    notificationObjectType = NotificationObjectTypes.TeleMentalHealth_Chat;
                }
                else
                {
                    notificationObjectType = (NotificationObjectTypes)Enum.Parse(typeof(NotificationObjectTypes), obDTO.ObjectType);
                }

                var user = _applicationContext.GetApplicationUserData();
               

                if (obDTO.ObjectType == typeof(TeleMentalHealth).Name.ToString())
                {
                    var Entity = _teleMentalHealthRepository.Query().SelectQueryable().Where(c => c.Id == obDTO.ObjectId).Include(c => c.InvolvedUsers).Include(c => c.Patient).FirstOrDefault();
                    var usersInCase = Entity.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(Entity.CreatedBy);

                    _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.TeleMentalHealth.ChatOnMentalHealth).Id,
             notificationObjectType,
              Entity.Id.ToString(),
              user.FullName,
              "Added Note on",
              $"Mental Health Case For Patient {Entity.Patient.FirstName+" "+Entity.Patient.LastName}",
              user.JobRole,
             usersInCase);
                }
                else if (obDTO.ObjectType == typeof(TeleICU).Name.ToString())
                {
                    var Entity = _teleIcuRepository.Query().SelectQueryable().Where(c => c.Id == obDTO.ObjectId).Include(c => c.InvolvedUsers).Include(c=>c.Patient).FirstOrDefault();
                    var usersInCase = Entity.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(Entity.CreatedBy);
                   
                    _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.TeleIcu.ChatOnICU).Id,
             notificationObjectType,
              Entity.Id.ToString(),
              user.FullName,
              "Added Note on",
                $"ICU Case For Patient {Entity.Patient.FirstName + " " + Entity.Patient.LastName}",
              user.JobRole,
             usersInCase);
                }
                else if (obDTO.ObjectType == typeof(NPICU).Name.ToString())
                {
                    var Entity = _teleNPICURepository.Query().SelectQueryable().Where(c => c.Id == obDTO.ObjectId).Include(c => c.InvolvedUsers).Include(c => c.Patient).FirstOrDefault();
                    var usersInCase = Entity.InvolvedUsers.Select(x => x.UserId).ToList();
                    usersInCase.Add(Entity.CreatedBy);

                    _userNotificationBLL.PublishNotification(
              _userNotificationBLL.GetByName(NotificationConsistent.NPICU.ChatOnNPICU).Id,
             notificationObjectType,
              Entity.Id.ToString(),
              user.FullName,
              "Added Note on",
                $"N/PICU Case For Patient {Entity.Patient.FirstName + " " + Entity.Patient.LastName}",
              user.JobRole,
             usersInCase);
                }
                _operationLogBLL.Insert(new OperationLogDTO()
                {
                    EntityType = typeof(StickyNote).Name.ToString(),
                    ObjectId = obEntity.Id,
                    ObjectRefernceNO = obEntity.ObjectId.ToString(),
                    OperationType = OperationTypeEnum.Add,
                    LogDescription = String.Format("Sticky Note For {0} is added", obDTO.ObjectType)
                });


                _operationLogBLL.Insert(new OperationLogDTO()
                {
                    EntityType = obDTO.ObjectType,
                    ObjectId = obDTO.ObjectId,
                    ObjectRefernceNO = obDTO.ObjectTag.ToString(),
                    OperationType = OperationTypeEnum.Chat,
                    LogDescription = String.Format("Chat on {0} {1}", obDTO.ObjectType, obDTO.ObjectTag)
                });


                var notesOnSameObjectIds = _repository
                    .Query()
                    .SelectQueryable()
                    .Where(x => x.ObjectId == obDTO.ObjectId && x.ObjectType == obDTO.ObjectType)
                    .Select(x => x.Id);

                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

     
    }
}
