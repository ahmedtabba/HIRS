using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using TripleA.Utilities.HashidsNet;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class ErrorLogController : BaseController
    {
        IErrorLogBLL _errorLogBLL;
        public ErrorLogController(IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _errorLogBLL = errorLogBLL;
        }
        // GET: ErrorLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return Json(_errorLogBLL.GetAll().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

        public ActionResult Clean()
        {
            try
            {
                _errorLogBLL.Clean();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorLogBLL.Insert(new ErrorLogDTO
                {
                    Message = ex.Message,
                    Location = "ErrorLogController"
                });
                //throw new KendoException(ex);
                return RedirectToAction("Index");

            }
        }

        public string GetDetails(string encrptedId)
        {
            var id = HashIdsManager.Decrypt(encrptedId);
            var details = _errorLogBLL.GetAll().Where(c => c.Id == id).FirstOrDefault().Details;
            return details;
        }
    }
}