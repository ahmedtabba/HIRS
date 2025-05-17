using Repository.Pattern;
using System.Linq;
using System.Security.Claims;
using BoulevardManagement.Model.Entities;
using BoulevardManagement.WebApplication.Models;
using System.Collections.Generic;
using System;

namespace BoulevardManagement.WebApplication.App_Start
{
    public class ApplicationContext : IApplicationUserDataContext
    {
        private ApplicationUserData applicationUserData;

        public ApplicationContext() { }

        public ApplicationContext(ApplicationUserData _applicationUserData)
        {
            this.applicationUserData = _applicationUserData;

        }

        public ApplicationUserData GetApplicationUserData()
        {
            var user = new ApplicationUserData();
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
            {
                var prinicpal = System.Web.HttpContext.Current.User as ClaimsPrincipal;

                var x = prinicpal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).ToArray();
                if (prinicpal != null)
                    user = new ApplicationUserData
                    {
                        Email = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault(),
                        UserId = prinicpal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault(),
                        FullName = prinicpal.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).SingleOrDefault(),
                        UserName = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(),
                        Roles = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray(),
                        //JobRole = prinicpal.Claims.FirstOrDefault(c => c.Type == "JobRole").ToString()
                        //Roles = customIdentity.Roles,
                        // SessionId = customIdentity.SessionId,
                        //UserName = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()
                    };
                var jobRole = prinicpal.Claims.FirstOrDefault(c => c.Type == "JobRole");
                var DepartmentId = prinicpal.Claims.FirstOrDefault(c => c.Type == "DepartmentId");
                var Department = prinicpal.Claims.FirstOrDefault(c => c.Type == "Department");

                if (jobRole != null)
                    user.JobRole = jobRole.Value.ToString();

                if (Department != null)
                    user.Department = Department.Value.ToString();

                if (DepartmentId != null&&!string.IsNullOrWhiteSpace(DepartmentId.Value))
                    user.DepartmentId =Convert.ToInt32( DepartmentId.Value);

            }
            else if (applicationUserData != null)
                user = applicationUserData;

            return user;
        }

        public bool IsInRole(string roleName)
        {
            if (System.Web.HttpContext.Current.User != null)
            {
                var prinicpal = System.Web.HttpContext.Current.User as ClaimsPrincipal;
                return prinicpal.IsInRole(roleName);
            }
            return false;
        }

        public List<ApplicationUserData> GetUsersBy(string roleName)
        {
            var list = new List<ApplicationUserData>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var users = context.Users.Where(c => c.JobRole.ToString() == roleName).ToList();

                foreach (var user in users)
                {
                    list.Add(new ApplicationUserData()
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        JobRole = user.JobRole.ToString(),
                        UserName = user.UserName,
                        DepartmentId = user.DepartmentId
                    });
                }

            }

            return list;
        }

        public ApplicationUserData GetUsersByUserId(string userId)
        {
            ApplicationUser user;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                user = context.Users.Where(c => c.Id == userId).FirstOrDefault();
            }

            return new ApplicationUserData
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName,
                DepartmentId = user.DepartmentId
            };
        }

        public List<ApplicationUserData> GetUsersByDepartmentId(int departmentId)
        {
            var list = new List<ApplicationUserData>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var users = context.Users.Where(c => c.DepartmentId==departmentId).ToList();

                foreach (var user in users)
                {
                    list.Add(new ApplicationUserData()
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        JobRole = user.JobRole.ToString(),
                        UserName = user.UserName,
                        DepartmentId = user.DepartmentId
                    });
                }

            }

            return list;
        }
    }

    public class FlavourManagementDBContext : BoulevardManagementContext
    {
        public FlavourManagementDBContext() : base(new ApplicationContext())
        {
        }

        
    }
}
