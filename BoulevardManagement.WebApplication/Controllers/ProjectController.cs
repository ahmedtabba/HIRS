using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectBLL _projectBLL;
        public ProjectController(IErrorLogBLL errorLogBLL, IProjectBLL projectBLL) : base(errorLogBLL)
        {
            _projectBLL = projectBLL;
        }

        // GET: Project
        [Permission(RoleConsistent.Project.Browse)]
        public ActionResult Index()
        {
            return View();
        }


        [Permission(RoleConsistent.Project.Edit)]
        public ActionResult Edit(string projectId = "")
        {
            ProjectDTO obDTO;
            if (String.IsNullOrWhiteSpace(projectId))
            {
                obDTO = new ProjectDTO();

            }
            else
            {
                var id = HashIdsManager.Decrypt(projectId);
                obDTO = _projectBLL.GetAll().Where(x => x.Id == id).FirstOrDefault();

            }
            return View(obDTO);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ProjectDTO obDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    if (obDTO.Id == 0)
                    {
                        _projectBLL.Insert(obDTO);
                        //TODO: Resource
                        ShowSuccessfullyAdded($"Geographical Group {obDTO.Name} {obDTO.Code} ");
                    }
                    else
                    {
                        _projectBLL.Update(obDTO);
                        //TODO: Resource
                        ShowSuccessfullyUpdated($"Geographical Group {obDTO.Name} {obDTO.Code} ");
                    }

               
                    return View(obDTO);

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }

        [Permission(RoleConsistent.Project.Add)]
        public ActionResult Create()
        {
            var obDTO = new ProjectDTO();

            return View("Edit", obDTO);
        }



        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IQueryable<ProjectDTO> projectList;
                projectList = _projectBLL.GetAll();

                return Json(projectList.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ProjectDTO obDTO)
        {
            try
            {

                _projectBLL.Delete(obDTO.Id);
                return Json(new[] { new ProjectDTO() }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }


    }
}