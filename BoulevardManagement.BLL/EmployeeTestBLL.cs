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
    public class EmployeeTestBLL : Service<EmployeeTest>, IEmployeeTestBLL
    {
        #region Ctor

        private readonly IRepositoryAsync<EmployeeTest> _repository;
        private readonly IRepositoryAsync<DepartmentTest> _departmentRepository;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public EmployeeTestBLL(IRepositoryAsync<EmployeeTest> repository, IRepositoryAsync<DepartmentTest> departmentRepository, IUnitOfWorkAsync unitOfWork) : base(
            repository)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        
        #endregion


        #region Public Methods

        public IEnumerable<EmployeeTestDTO> GetAll()
        {
            var employees = _repository.Query().SelectQueryable().ProjectTo<EmployeeTestDTO>().ToList();
            
            employees.ForEach(x => x.DepartmentName = GetDepartmentName(x.DepartmentId));

            return employees;
        }

        public EmployeeTestDTO GetById(int id)
        {
            var employeeEntity = _repository.Find(id);

            var employee = Mapper.Map<EmployeeTest, EmployeeTestDTO>(employeeEntity);
            
            employee.DepartmentName = GetDepartmentName(employee.DepartmentId);

            return employee;
        }

        public int Insert(EmployeeTestDTO input)
        {
            var employeeEntity = Mapper.Map<EmployeeTestDTO, EmployeeTest>(input);
            employeeEntity.ObjectState = ObjectState.Added;
            Insert(employeeEntity);

            _unitOfWork.SaveChanges();

            return employeeEntity.Id;
        }

        public void Update(EmployeeTestDTO input)
        {
            var employeeEntity = Find(input.Id);
            if (employeeEntity is null)
                throw new System.Exception("Employee not found");

            Mapper.Map(input, employeeEntity);
            employeeEntity.ObjectState = ObjectState.Modified;
            Update(employeeEntity);

            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var employeeEntity = Find(id);
            if (employeeEntity is null)
                throw new System.Exception("Employee not found");

            _repository.Delete(id);

            _unitOfWork.SaveChanges();
        }

        public IEnumerable<DepartmentTestDTO> GetAllDepartments()
        {
            return _departmentRepository.Query().SelectQueryable().ProjectTo<DepartmentTestDTO>().ToList();
        }

        #endregion

        #region Private Methods

        private string GetDepartmentName(string departmentId) => !string.IsNullOrEmpty(departmentId) ? _departmentRepository.Find(int.Parse(departmentId)).Name : string.Empty;
            
        #endregion
    }
}