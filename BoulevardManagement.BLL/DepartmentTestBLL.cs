

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
using System.Linq;

namespace BoulevardManagement.BLL
{
    public class DepartmentTestBLL : Service<DepartmentTest>, IDepartmentTestBLL
    {
        private readonly IRepositoryAsync<DepartmentTest> _repository;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public DepartmentTestBLL(IRepositoryAsync<DepartmentTest> repository, IUnitOfWorkAsync unitOfWork) : base(repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public DepartmentTestBLL(IRepositoryAsync<DepartmentTest> repository, IApplicationUserDataContext applicationContext) : base(repository, applicationContext)
        {
        }

        public IQueryable<DepartmentTestDTO> GetAll()
        {
            var departments = _repository.Query().SelectQueryable().ProjectTo<DepartmentTestDTO>();

            return departments;
        }
        
        public DepartmentTestDTO GetById(int id)
        {
            var departmentEntity = _repository.Find(id);
            
            var department = Mapper.Map<DepartmentTest, DepartmentTestDTO>(departmentEntity);
            
            return department;
        }

        public int Insert(DepartmentTestDTO input)
        {
            var departmentEntity = Mapper.Map<DepartmentTestDTO, DepartmentTest>(input);
            departmentEntity.ObjectState = ObjectState.Added;
            Insert(departmentEntity);

            _unitOfWork.SaveChanges();
            
            return departmentEntity.Id;
        }

        public void Update(DepartmentTestDTO input)
        {
            var departmentEntity = Find(input.Id);
            if (departmentEntity is null)
                throw new System.Exception("Department not found");
            
            Mapper.Map(input, departmentEntity);
            departmentEntity.ObjectState = ObjectState.Modified;
            Update(departmentEntity);

            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var departmentEntity = Find(id);
            if (departmentEntity is null)
                throw new System.Exception("Department not found");

            _repository.Delete(id);

            _unitOfWork.SaveChanges();
        }
    }
}