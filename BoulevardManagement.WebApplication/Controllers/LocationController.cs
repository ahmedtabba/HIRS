using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{

    public class LocationController : BaseController
    {

        readonly private ILocationBLL _locationBLL;
        public LocationController(
            ILocationBLL locationBLL,
            IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            this._locationBLL = locationBLL;
        }

        // GET: Location
        [Permission(RoleConsistent.Location.Browse)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
            

                IQueryable<LocationDTO> _res;
                _res = _locationBLL.GetAll();
                return new JsonResult() { Data = _res.ToDataSourceResult(request), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = ex.Message
                });
            }

        }


        [Permission(RoleConsistent.Location.Edit, RoleConsistent.Location.Add)]
        public ActionResult Edit(string locationId = "")
        {
            LocationDTO obDTO;
            if (String.IsNullOrWhiteSpace(locationId))
            {
                obDTO = new LocationDTO();
            }
            else
            {
                var id = HashIdsManager.Decrypt(locationId);
                obDTO = _locationBLL.GetAll().Where(x => x.Id == id).FirstOrDefault();
            }
            return View(obDTO);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(LocationDTO obDTO, SubmitFormType FormType = SubmitFormType.SaveAndExist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var errorMessage = _locationBLL.IsValid(obDTO);

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        if (obDTO.Id == 0)
                        {
                            if (!User.IsInRole(RoleConsistent.Location.Add))
                                RedirectToAction("AccessDenied", "Home");

                            obDTO.Id = _locationBLL.Insert(obDTO);
                            ShowSuccessfullyAdded();
                        }
                        else
                        {
                            _locationBLL.Update(obDTO);
                            ShowSuccessfullyUpdated();
                        }
                    }
                    else
                    {

                        ShowErrorMessage(errorMessage);
                        return View(obDTO);
                    }

                    switch (FormType)
                    {
                        case SubmitFormType.SaveAndExist:
                            return RedirectToAction("Index");
                        case SubmitFormType.SaveAndContinue:
                            return RedirectToAction("Edit", new { locationId = obDTO.EncrptedId });
                        case SubmitFormType.SaveAndAddNew:
                            return RedirectToAction("Create");
                        default:
                            break;
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(obDTO);

        }


        [Permission(RoleConsistent.Location.Add)]
        public ActionResult Create()
        {
            var obDTO = new LocationDTO();

            return View("Edit", obDTO);
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [Permission(RoleConsistent.Location.Delete)]

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, LocationDTO obDTO)
        {
            try
            {
              
                var errors = _locationBLL.ValidToBeDeleted(obDTO.Id);
                if (string.IsNullOrWhiteSpace(errors))
                    _locationBLL.Delete(obDTO.Id);
                else
                    ModelState.AddModelError("Id", errors);

                return Json(new[] { obDTO }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }
        public JsonResult GetLocations(string text, int? prevId)
        {
            try
            {
                var Locations = _locationBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(text))
                    Locations = Locations.Where(x => x.Name.Contains(text) || x.Code.Contains(text));

                var resList = Locations.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_locationBLL.GetAll().Where(x => x.Id == prevId && (x.Name.Contains(text) || x.Code.Contains(text))).ToList());
                        else
                            resList.AddRange(_locationBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        public JsonResult GetUnOquppiedLocations(string text, int? prevId)
        {
            try
            {
                var Locations = _locationBLL.GetAll().Where(x => !x.Occupied);
                if (!string.IsNullOrWhiteSpace(text))
                    Locations = Locations.Where(x => x.Name.Contains(text) || x.Code.Contains(text));

                var resList = Locations.Take(20).ToList();
                if (prevId.HasValue)
                {
                    if (!resList.Select(x => x.Id).Contains((int)prevId))
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                            resList.AddRange(_locationBLL.GetAll().Where(x => x.Id == prevId && (x.Name.Contains(text) || x.Code.Contains(text))).ToList());
                        else
                            resList.AddRange(_locationBLL.GetAll().Where(x => x.Id == prevId).ToList());

                    }
                }

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

        public JsonResult GetLocationsForCurrent(string text)
        {
            try
            {
                if (!User.IsInRole(RoleConsistent.Location.Browse))
                    return Json(null, JsonRequestBehavior.AllowGet);

                var DepartmentId = GetCurrentUser.DepartmentId;
                var Locations = _locationBLL.GetAll().Where(c => c.DepartmentId == DepartmentId);
                if (!string.IsNullOrWhiteSpace(text))
                    Locations = Locations.Where(x => x.Name.Contains(text) || x.Code.Contains(text));
                var resList = Locations.Take(20).ToList();


                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

        public JsonResult GetUnOquppiedLocationsForCurrent(string text)
        {
            try
            {
                if (!User.IsInRole(RoleConsistent.Location.Browse))
                    return Json(null, JsonRequestBehavior.AllowGet);

                var DepartmentId = GetCurrentUser.DepartmentId;
                var Locations = _locationBLL.GetAll().Where(c => c.DepartmentId == DepartmentId && !c.Occupied);

                if (!string.IsNullOrWhiteSpace(text))
                    Locations = Locations.Where(x => x.Name.Contains(text) || x.Code.Contains(text));

                var resList = Locations.Take(20).ToList();

                return Json(resList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }
    }

}