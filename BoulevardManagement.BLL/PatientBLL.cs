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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoulevardManagement.DTO.Resources;

namespace BoulevardManagement.BLL
{

    public class PatientBLL : Service<Patient>, IPatientBLL
    {
        private readonly UserNotificationBLL _userNotificationBLL;
        private readonly ITeleMentalHealthBLL _teleMentalHealthBLL;
        private readonly INPICUBLL _nPICUBLL;
        private readonly ITeleICUBLL _teleICUBLL;
        private readonly IDepartmentBLL _departmentBLL;
        readonly IRepositoryAsync<Patient> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IOperationLogBLL _operationLogBLL;
        readonly IApplicationUserDataContext _applicationContext;

        public PatientBLL(
     IRepositoryAsync<Patient> repository,
      IUnitOfWorkAsync unitOfWorkAsync,
      IOperationLogBLL operationLogBLL,
      IApplicationUserDataContext applicationContext
  ,
            UserNotificationBLL userNotificationBLL,
            ITeleMentalHealthBLL teleMentalHealthBLL,
            INPICUBLL nPICUBLL,
            ITeleICUBLL teleICUBLL,
            IDepartmentBLL departmentBLL) : base(repository, applicationContext)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _operationLogBLL = operationLogBLL;
            _applicationContext = applicationContext;
            _userNotificationBLL = userNotificationBLL;
            _teleMentalHealthBLL = teleMentalHealthBLL;
            _nPICUBLL = nPICUBLL;
            _teleICUBLL = teleICUBLL;
            _departmentBLL = departmentBLL;

        }


        public int Insert(PatientDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Mapper.Map<PatientDTO, Patient>(obDTO);
                obEntity.ObjectState = ObjectState.Added;
                obEntity.Number = GenerateNumber(obEntity);
                obEntity.NumberStr = obEntity.Number.ToString().PadLeft(5, '0');
                Insert(obEntity);
                _unitOfWorkAsync.SaveChanges();
                var validUsers = new List<string>();
                validUsers.AddRange(_applicationContext.GetUsersByDepartmentId(obDTO.DepartmentId).Select(x => x.UserId).ToList());
                validUsers.AddRange(_applicationContext.GetUsersBy(JobRole.IT.ToString()).Select(x=>x.UserId).ToList());

                var user = _applicationContext.GetApplicationUserData();
                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.Patient.Add).Id,
                   NotificationObjectTypes.Patient,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Added",
                   $"Patient {obEntity.FirstName }",
                   user.JobRole,validUsers);


                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(Patient).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.FirstName,
                        OperationType = OperationTypeEnum.Add,
                        LogDescription = String.Format("{0} is added",
                        obEntity.Id.ToString())
                    });

                _unitOfWorkAsync.Commit();
                return obEntity.Id;
            }
            catch { _unitOfWorkAsync.Rollback(); throw; }
        }

        private int GenerateNumber(Patient obEntity)
        {
            try
            {
                var num = 0;
                if (_repository.Query().SelectQueryable().Where(x => x.DepartmentId == obEntity.DepartmentId).Any())
                    num = _repository.Query().SelectQueryable().Where(x => x.DepartmentId == obEntity.DepartmentId).Select(x => x.Number).Max();

                num++;
                return num;
            }
            catch 
            {

                throw;
            }
        }

        public void Update(PatientDTO obDTO)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var obEntity = Find(obDTO.Id);
                Mapper.Map<PatientDTO, Patient>(obDTO, obEntity);
                obEntity.ObjectState = ObjectState.Modified;
                Update(obEntity);
                _unitOfWorkAsync.SaveChanges();

                var user = _applicationContext.GetApplicationUserData();
                var validUsers = new List<string>();
                validUsers.AddRange(_applicationContext.GetUsersByDepartmentId(obDTO.DepartmentId).Select(x => x.UserId).ToList());
                validUsers.AddRange(_applicationContext.GetUsersBy(JobRole.IT.ToString()).Select(x => x.UserId).ToList());

                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.Patient.Edit).Id,
                   NotificationObjectTypes.Patient,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Updated",
                   $"Patient {obEntity.FirstName }",
                   user.JobRole,validUsers);

                _operationLogBLL.Insert(
                    new OperationLogDTO()
                    {
                        EntityType = typeof(Patient).Name.ToString(),
                        ObjectId = obEntity.Id,
                        ObjectRefernceNO = obEntity.FirstName,
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

                var validUsers = new List<string>();
                validUsers.AddRange(_applicationContext.GetUsersByDepartmentId(obEntity.DepartmentId).Select(x => x.UserId).ToList());
                validUsers.AddRange(_applicationContext.GetUsersBy(JobRole.IT.ToString()).Select(x => x.UserId).ToList());

                _repository.Delete(id);
                _unitOfWorkAsync.SaveChanges();

                var user = _applicationContext.GetApplicationUserData();
                
                _userNotificationBLL.PublishNotification(
                   _userNotificationBLL.GetByName(NotificationConsistent.Patient.Edit).Id,
                   NotificationObjectTypes.Patient,
                   obEntity.Id.ToString(),
                   user.FullName,
                   "Deleted",
                   $"Patient {obEntity.FirstName }",
                   user.JobRole,validUsers);

                _operationLogBLL.Insert(
                  new OperationLogDTO()
                  {
                      EntityType = typeof(Patient).Name.ToString(),
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

        public PatientDTO GetById(int id)
        {
            try
            {
                return _repository.Query().SelectQueryable().Where(f => f.Id == id).ProjectTo<PatientDTO>().FirstOrDefault();
            }
            catch { throw; }
        }

        public string IsValid(PatientDTO obDTO)
        {
            var res = "";


            return res;
        }

        public string ValidToBeDeleted(int id)
        {
            var res = "";
            if (_teleMentalHealthBLL.GetAll().Where(x => x.PatientId == id).Any())
                res = PatientResource.PatientCannotbedeletedRelatedWithMentalHealthCases;

            if (_nPICUBLL.GetAll().Where(x => x.PatientId == id).Any())
                res = PatientResource.PatientCannotbedeletedRelatedWithNPICUCases;

            if (_teleICUBLL.GetAll().Where(x => x.PatientId == id).Any())
                res = PatientResource.PatientCannotbedeletedRelatedWithICUCases;

            return res;
        }

        public IQueryable<PatientDTO> GetAll()
        {
            try
            {
                return _repository.Query().SelectQueryable().ProjectTo<PatientDTO>();
            }
            catch { throw; }
        }
    }
}
