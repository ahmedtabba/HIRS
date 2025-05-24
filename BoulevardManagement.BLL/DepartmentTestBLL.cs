

using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Repository.Pattern;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System.Collections.Generic;
using System.Linq;

namespace BoulevardManagement.BLL
{
    public class DepartmentTestBLL : Service<DepartmentTest>, IDepartmentTestBLL
    {
        #region Ctor
        
        private readonly IRepositoryAsync<DepartmentTest> _repository;
        private readonly IEmployeeTestBLL _employeeTestBll;
        private readonly IApplicationUserDataContext _applicationContext;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public DepartmentTestBLL(IRepositoryAsync<DepartmentTest> repository, IEmployeeTestBLL employeeTestBll, IApplicationUserDataContext applicationContext, IUnitOfWorkAsync unitOfWork) : base(repository)
        {
            _repository = repository;
            _employeeTestBll = employeeTestBll;
            _applicationContext = applicationContext;
            _unitOfWork = unitOfWork;
        }
        
        #endregion
        
        #region Public Methods
        
        public IEnumerable<DepartmentTestDTO> GetAll()
        {
            var departments = _repository.Query()
                .SelectQueryable()
                .ToList()
                .Select(x => new DepartmentTestDTO
            {
                Id = x.Id,
                Name = x.Name,
                OpeningDate = x.OpeningDate,
                ResponsibleItUserId = x.ResponsibleItUserId,
                ResponsibleItUserName = GetUserFullName(x.ResponsibleItUserId)
            });

            return departments;
        }
        
        public DepartmentTestDTO GetById(int id)
        {
            var departmentEntity = _repository.Find(id);
            
            var department = Mapper.Map<DepartmentTest, DepartmentTestDTO>(departmentEntity);
            
            return department;
        }

        public DepartmentTestDTO Insert(DepartmentTestDTO input)
        {
            var departmentEntity = Mapper.Map<DepartmentTestDTO, DepartmentTest>(input);
            departmentEntity.ObjectState = ObjectState.Added;
            Insert(departmentEntity);

            _unitOfWork.SaveChanges();
            
            input.Id = departmentEntity.Id;
            input.ResponsibleItUserName = GetUserFullName(input.ResponsibleItUserId);
            return input;
        }

        public DepartmentTestDTO Update(DepartmentTestDTO input)
        {
            var departmentEntity = Find(input.Id);
            if (departmentEntity is null)
                throw new System.Exception("Department not found");
            
            Mapper.Map(input, departmentEntity);
            departmentEntity.ObjectState = ObjectState.Modified;
            Update(departmentEntity);

            _unitOfWork.SaveChanges();
            
            input.Id = departmentEntity.Id;
            input.ResponsibleItUserName = GetUserFullName(input.ResponsibleItUserId);
                return input;
        }

        public void Delete(int id)
        {
            _unitOfWork.BeginTransaction();
            
            var departmentEntity = Find(id);
            if (departmentEntity is null)
                throw new System.Exception("Department not found");

            DeleteRelatedEmployee(departmentEntity.Id);

            _repository.Delete(id);
            _unitOfWork.Commit();

            _unitOfWork.SaveChanges();
        }

        public List<ApplicationUserData> GetItUsers()
        {
            return _applicationContext.GetUsersBy(nameof(JobRole.IT));
        }

        #endregion

        #region Private Methods

        private string GetUserFullName(string userId) => !string.IsNullOrEmpty(userId) ? _applicationContext.GetUsersByUserId(userId)?.FullName : string.Empty;
        
        private void DeleteRelatedEmployee(int id)
        {
            var employees = _employeeTestBll.GetAll().Where(x => x.DepartmentId == id.ToString());

            foreach (var employee in employees)
            {
                employee.DepartmentId = null;
                _employeeTestBll.Update(employee);
            }
        }

        #endregion
      
    }
}